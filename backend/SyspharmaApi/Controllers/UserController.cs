using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SyspharmaApi.Context;
using SyspharmaApi.Contracts.User;
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
        public async Task<IActionResult> GetUsers(int page = 0, int pagesize = 10)
        {
            var users = await _context.Users
                .Skip(page * pagesize)
                .Take(pagesize)
                .ToListAsync();

            if (User.IsInRole(UserRole.Admin.ToString()))
            {
                return Ok(users.Select(u => new UserDto(
                    Iduser: u.Iduser,
                    Idstore: u.Idstore,
                    Role: u.Role,
                    Name: u.Name,
                    Email: u.Email,
                    Cpf: u.Cpf,
                    Phone: u.Phone,
                    Active: u.Active,
                    Createdat: u.Createdat,
                    Profilephotopath: u.Profilephotopath)));
            }

            return Ok(users.Select(u => new BaseUserDTO(
                Iduser: u.Iduser, 
                Name: u.Name, 
                ProfilePhotoPath: u.Profilephotopath, 
                Active: u.Active)));
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
            bool existsEmail = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (existsEmail)
                return BadRequest(new { message = "O email informado já está cadastrado" });

            var existsCpf = await _context.Users.FirstOrDefaultAsync(u => u.Cpf == user.Cpf);
            if (existsCpf is not null)
            {
                string status = existsCpf.Active ? "ATIVO" : "INATIVO";

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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
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


        [HttpPatch("{id}/photo")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ChangeProfilePhoto(int id, [FromForm] ChangeProfilePhotoRequest request)
        {
            var image = request.Image;

            if (image == null || image.Length == 0)
                return BadRequest("Não enviado nenhum arquivo");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Iduser == id);

            if (user is null)
                return NotFound("Não foi encontrado o usuário para a alteração");

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!validExtensions.Contains(extension))
                return BadRequest("Formato de imagem inválido. Os formatos permitidos são: jpg, jpeg, png e webp");

            var imgfolder = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "imgs");

            if (!Directory.Exists(imgfolder))
                Directory.CreateDirectory(imgfolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filepath = Path.Combine(imgfolder, fileName);


            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            if (!string.IsNullOrWhiteSpace(user.Profilephotopath))
            {
                var oldProfilePhoto = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "..", "..",
                    user.Profilephotopath);

                if (System.IO.File.Exists(oldProfilePhoto))
                    System.IO.File.Delete(oldProfilePhoto);
            }

            user.Profilephotopath = Path.Combine("imgs", fileName).Replace("\\", "/");
            await _context.SaveChangesAsync();
            return Ok(user.Profilephotopath);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
