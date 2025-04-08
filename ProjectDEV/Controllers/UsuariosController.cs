using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectDEV.DTOs;
using ProjectDEV.Services;
using System;
using System.Threading.Tasks;

namespace ProjectDEV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Criar(UsuarioCreateDTO usuarioDto)
        {
            try
            {
                var usuario = await _usuarioService.CriarUsuario(usuarioDto);
                return CreatedAtAction(nameof(Listar), new { id = usuario.Id }, usuario);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, UsuarioUpdateDTO usuarioDto)
        {
            try
            {
                var usuario = await _usuarioService.AtualizarUsuario(id, usuarioDto);
                return Ok(usuario);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] bool? ativo = null)
        {
            var usuarios = await _usuarioService.ListarUsuarios(ativo);
            return Ok(usuarios);
        }
    }
} 