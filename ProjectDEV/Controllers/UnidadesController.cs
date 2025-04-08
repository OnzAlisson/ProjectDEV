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
    public class UnidadesController : ControllerBase
    {
        private readonly UnidadeService _unidadeService;

        public UnidadesController(UnidadeService unidadeService)
        {
            _unidadeService = unidadeService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(UnidadeCreateDTO unidadeDto)
        {
            try
            {
                var unidade = await _unidadeService.CriarUnidade(unidadeDto);
                return CreatedAtAction(nameof(Listar), new { id = unidade.Id }, unidade);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, UnidadeUpdateDTO unidadeDto)
        {
            try
            {
                var unidade = await _unidadeService.AtualizarUnidade(id, unidadeDto);
                return Ok(unidade);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Unidade não encontrada" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var unidades = await _unidadeService.ListarUnidades();
            return Ok(unidades);
        }

        [HttpGet("colaboradores")]
        public async Task<IActionResult> ListarComColaboradores()
        {
            var unidades = await _unidadeService.ListarUnidadesComColaboradores();
            return Ok(unidades);
        }
    }
} 