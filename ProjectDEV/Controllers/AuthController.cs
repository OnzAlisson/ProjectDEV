using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectDEV.DTOs;
using ProjectDEV.Services;
using System.Threading.Tasks;

namespace ProjectDEV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public AuthController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var token = await _usuarioService.Autenticar(loginDto);

            if (token == null)
            {
                return Unauthorized(new { message = "Credenciais inválidas" });
            }

            return Ok(new { token, message = "Autenticação realizada com sucesso" });
        }
    }
} 