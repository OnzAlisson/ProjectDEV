using Microsoft.EntityFrameworkCore;
using ProjectDEV.Data;
using ProjectDEV.DTOs;
using ProjectDEV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDEV.Services
{
    public class UnidadeService
    {
        private readonly AppDbContext _context;

        public UnidadeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UnidadeResponseDTO> CriarUnidade(UnidadeCreateDTO unidadeDto)
        {
            // Verificar se já existe unidade com o mesmo código
            if (await _context.Unidades.AnyAsync(u => u.Codigo == unidadeDto.Codigo))
            {
                throw new InvalidOperationException("Já existe uma unidade com este código");
            }

            var unidade = new Unidade
            {
                Codigo = unidadeDto.Codigo,
                Nome = unidadeDto.Nome,
                Ativa = true
            };

            _context.Unidades.Add(unidade);
            await _context.SaveChangesAsync();

            return new UnidadeResponseDTO
            {
                Id = unidade.Id,
                Codigo = unidade.Codigo,
                Nome = unidade.Nome,
                Ativa = unidade.Ativa
            };
        }

        public async Task<UnidadeResponseDTO> AtualizarUnidade(int id, UnidadeUpdateDTO unidadeDto)
        {
            var unidade = await _context.Unidades.FindAsync(id);
            if (unidade == null)
            {
                throw new KeyNotFoundException("Unidade não encontrada");
            }

            unidade.Ativa = unidadeDto.Ativa;
            unidade.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UnidadeResponseDTO
            {
                Id = unidade.Id,
                Codigo = unidade.Codigo,
                Nome = unidade.Nome,
                Ativa = unidade.Ativa
            };
        }

        public async Task<List<UnidadeResponseDTO>> ListarUnidades()
        {
            var unidades = await _context.Unidades.ToListAsync();

            return unidades.Select(u => new UnidadeResponseDTO
            {
                Id = u.Id,
                Codigo = u.Codigo,
                Nome = u.Nome,
                Ativa = u.Ativa
            }).ToList();
        }

        public async Task<List<UnidadeColaboradoresDTO>> ListarUnidadesComColaboradores()
        {
            var unidades = await _context.Unidades
                .Include(u => u.Colaboradores)
                .ThenInclude(c => c.Usuario)
                .ToListAsync();

            return unidades.Select(u => new UnidadeColaboradoresDTO
            {
                Id = u.Id,
                Codigo = u.Codigo,
                Nome = u.Nome,
                Ativa = u.Ativa,
                Colaboradores = u.Colaboradores
                    .Where(c => c.Usuario != null) // Filtrar colaboradores sem usuário
                    .Select(c => new ColaboradorResponseDTO
                    {
                        Id = c.Id,
                        Nome = c.Nome,
                        Unidade = new UnidadeResponseDTO
                        {
                            Id = u.Id,
                            Codigo = u.Codigo,
                            Nome = u.Nome,
                            Ativa = u.Ativa
                        },
                        Usuario = c.Usuario != null ? new UsuarioResponseDTO
                        {
                            Id = c.Usuario.Id,
                            Login = c.Usuario.Login,
                            Ativo = c.Usuario.Ativo
                        } : null
                    }).ToList()
            }).ToList();
        }
    }
} 