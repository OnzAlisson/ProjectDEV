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
    public class ColaboradoresController : ControllerBase
    {
        private readonly ColaboradorService _colaboradorService;

        public ColaboradoresController(ColaboradorService colaboradorService)
        {
            _colaboradorService = colaboradorService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(ColaboradorCreateDTO colaboradorDto)
        {
            try
            {
                var colaborador = await _colaboradorService.CriarColaborador(colaboradorDto);
                return CreatedAtAction(nameof(Listar), new { id = colaborador.Id }, colaborador);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, ColaboradorUpdateDTO colaboradorDto)
        {
            try
            {
                var colaborador = await _colaboradorService.AtualizarColaborador(id, colaboradorDto);
                return Ok(colaborador);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                await _colaboradorService.RemoverColaborador(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Colaborador não encontrado" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var colaboradores = await _colaboradorService.ListarColaboradores();
            return Ok(colaboradores);
        }
    }
} 