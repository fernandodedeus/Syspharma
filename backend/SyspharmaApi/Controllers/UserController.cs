using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyspharmaApi.Context;
using SyspharmaApi.Helpers;
using SyspharmaApi.Models;
using System.Net;

namespace SyspharmaApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController(SyspharmaContext context) : ControllerBase
    {
        private readonly SyspharmaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                bool existsEmail = await _context.Users.AnyAsync(u => u.Email == user.Email);
                if (existsEmail)
                    return BadRequest(new { message = "O email informado já está cadastrado" });

                var existsCpf = await _context.Users.FirstOrDefaultAsync(u => u.Cpf == user.Cpf);
                if (existsCpf is not null)
                {
                    string status = existsCpf.Active ?? false ? "ATIVO" : "INATIVO";

                    return BadRequest($"Já existe um cadastro para o CPF {existsCpf.Cpf} e o tipo de usuário escolhido.\nEmail registrado: {existsCpf.Email}." +
                        $"\nSTATUS do cadastro: {status}");
                }
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                user.Pass = user.Pass.GetHashCode().ToString();

                _context.Entry(user).Property(u => u.Pass).IsModified = true;
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUser), new { id = user.Iduser }, user);
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"UserController/PostUser", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            try
            {
                if (id != user.Iduser)
                    return BadRequest();

                bool existsEmail = await _context.Users.AnyAsync(u => u.Email == user.Email && u.Iduser != user.Iduser);
                if (existsEmail)
                    return BadRequest("O email informado já está cadastrado.");

                bool existsCpf = await _context.Users.AnyAsync(u => u.Cpf == user.Cpf && u.Iduser != user.Iduser);
                if (existsCpf)
                    return BadRequest("Já existe o CPF informado para outro usuário com o mesmo tipo de acesso que o seu.");

                var currentUser = _context.Users.FirstOrDefault(u => u.Iduser == id);
                if (currentUser is null)
                    return NotFound();                

                currentUser.Email = user.Email;
                currentUser.Cpf = user.Cpf;
                currentUser.Phone = user.Phone;
                currentUser.Name = user.Name;
                currentUser.Active = user.Active;
                currentUser.Idstore = user.Idstore;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"UserController/PutUser/id={id}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound();

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"UserController/DeleteUser/id={id}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
