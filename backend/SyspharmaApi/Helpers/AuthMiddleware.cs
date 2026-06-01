using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SyspharmaApi.Context;
using SyspharmaApi.Models;

namespace SyspharmaApi.Helpers
{

    public class AuthMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        public async Task InvokeAsync(HttpContext context, SyspharmaContext db, IConfiguration config)
        {
            var path = context.Request.Path.Value;

            if (string.IsNullOrWhiteSpace(path))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Boas vindas à API do Syspharma!");
                return;
            }

            List<string> pathWhiteList =
            [
                "/Auth/login",
                "/Auth/refresh",
                "/Auth/register",
                "/Auth/logout",
                "/Auth/me",
                "/swagger",
                "/Pagbank"
            ];

            if (path is not null && pathWhiteList.FirstOrDefault(p => path.Contains(p, StringComparison.InvariantCultureIgnoreCase)) is not null)
            {
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token ausente");
                return;
            }

            var token = authHeader["Bearer ".Length..].Trim();

            try
            {
                var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]!);
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = principal.FindFirst(ClaimTypes.Role)?.Value;

                if (!int.TryParse(userIdClaim, out int id) || string.IsNullOrEmpty(role))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token inválido");
                    return;
                }

                if (role == "Admin")
                {
                    await _next(context);
                    return;
                }

                var cfg = await db.Configs.FirstOrDefaultAsync(c => c.Idconfig == (int)ConfigType.AppOnline);
                if (cfg is null || cfg.Value != 1)
                {
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    await context.Response.WriteAsync("Aplicativo em manutenção");
                    return;
                }

                if (role == "User")
                {
                    var user = await db.Users.FirstOrDefaultAsync(u => u.Iduser == id) ?? throw new Exception("Usuário não encontrado");

                    _ = await db.UserTokens.FirstOrDefaultAsync(t => t.Iduser == user.Iduser && !t.Revoked)
                        ?? throw new Exception("Senha expirada. Por favor, faça o login novamente");

                    if (!user.Active)
                    {
                        string msg = "Cadastro Bloqueado";

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync(msg);
                        var userTokenList = db.UserTokens.Where(u => u.Iduser == id).ForEachAsync(u => db.UserTokens.Remove(u));
                        await db.SaveChangesAsync();

                        return;
                    }

                    context.Items["User"] = user;
                }
            }
            catch (Exception ex)
            {
                var cfg = await db.Configs.FirstOrDefaultAsync(c => c.Idconfig == (int)ConfigType.ProductionMode);
                string details = string.Empty;
                if (cfg is not null)
                {
                    if (cfg.Value != 1)
                        details = $"\n\nDetalhes: {ex}";
                }

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token inválido" + details);
                return;
            }

            await _next(context);
        }
    }
}
