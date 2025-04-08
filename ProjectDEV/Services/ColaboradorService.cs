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
    public class ColaboradorService
    {
        private readonly AppDbContext _context;
        private readonly UsuarioService _usuarioService;

        public ColaboradorService(AppDbContext context, UsuarioService usuarioService)
        {
            _context = context;
            _usuarioService = usuarioService;
        }

        public async Task<ColaboradorResponseDTO> CriarColaborador(ColaboradorCreateDTO colaboradorDto)
        {
            // Verificar se a unidade existe e está ativa
            var unidade = await _context.Unidades.FindAsync(colaboradorDto.UnidadeId);
            if (unidade == null)
            {
                throw new KeyNotFoundException("Unidade não encontrada");
            }
            
            if (!unidade.Ativa)
            {
                throw new InvalidOperationException("Não é possível cadastrar colaborador em unidade inativa");
            }

            // Criar usuário
            var usuarioDto = colaboradorDto.Usuario;
            var usuarioResponse = await _usuarioService.CriarUsuario(usuarioDto);

            // Criar colaborador
            var colaborador = new Colaborador
            {
                Nome = colaboradorDto.Nome,
                UnidadeId = colaboradorDto.UnidadeId,
                UsuarioId = usuarioResponse.Id
            };

            _context.Colaboradores.Add(colaborador);
            await _context.SaveChangesAsync();

            // Carregar dados para retorno
            await _context.Entry(colaborador)
                .Reference(c => c.Unidade)
                .LoadAsync();

            await _context.Entry(colaborador)
                .Reference(c => c.Usuario)
                .LoadAsync();

            if (colaborador.Unidade == null || colaborador.Usuario == null)
            {
                throw new InvalidOperationException("Erro ao carregar dados relacionados do colaborador");
            }

            return new ColaboradorResponseDTO
            {
                Id = colaborador.Id,
                Nome = colaborador.Nome,
                Unidade = new UnidadeResponseDTO
                {
                    Id = colaborador.Unidade.Id,
                    Codigo = colaborador.Unidade.Codigo,
                    Nome = colaborador.Unidade.Nome,
                    Ativa = colaborador.Unidade.Ativa
                },
                Usuario = new UsuarioResponseDTO
                {
                    Id = colaborador.Usuario.Id,
                    Login = colaborador.Usuario.Login,
                    Ativo = colaborador.Usuario.Ativo
                }
            };
        }

        public async Task<ColaboradorResponseDTO> AtualizarColaborador(int id, ColaboradorUpdateDTO colaboradorDto)
        {
            var colaborador = await _context.Colaboradores
                .Include(c => c.Unidade)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (colaborador == null)
            {
                throw new KeyNotFoundException("Colaborador não encontrado");
            }

            if (colaborador.Usuario == null)
            {
                throw new InvalidOperationException("Colaborador não possui usuário associado");
            }

            // Verificar se a unidade existe e está ativa
            var unidade = await _context.Unidades.FindAsync(colaboradorDto.UnidadeId);
            if (unidade == null)
            {
                throw new KeyNotFoundException("Unidade não encontrada");
            }
            
            if (!unidade.Ativa)
            {
                throw new InvalidOperationException("Não é possível transferir colaborador para unidade inativa");
            }

            colaborador.Nome = colaboradorDto.Nome;
            colaborador.UnidadeId = colaboradorDto.UnidadeId;
            colaborador.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Recarregar dados para retorno
            await _context.Entry(colaborador)
                .Reference(c => c.Unidade)
                .LoadAsync();

            if (colaborador.Unidade == null)
            {
                throw new InvalidOperationException("Erro ao carregar dados da unidade");
            }

            return new ColaboradorResponseDTO
            {
                Id = colaborador.Id,
                Nome = colaborador.Nome,
                Unidade = new UnidadeResponseDTO
                {
                    Id = colaborador.Unidade.Id,
                    Codigo = colaborador.Unidade.Codigo,
                    Nome = colaborador.Unidade.Nome,
                    Ativa = colaborador.Unidade.Ativa
                },
                Usuario = new UsuarioResponseDTO
                {
                    Id = colaborador.Usuario.Id,
                    Login = colaborador.Usuario.Login,
                    Ativo = colaborador.Usuario.Ativo
                }
            };
        }

        public async Task<bool> RemoverColaborador(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);
            if (colaborador == null)
            {
                throw new KeyNotFoundException("Colaborador não encontrado");
            }

            _context.Colaboradores.Remove(colaborador);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ColaboradorResponseDTO>> ListarColaboradores()
        {
            var colaboradores = await _context.Colaboradores
                .Include(c => c.Unidade)
                .Include(c => c.Usuario)
                .ToListAsync();

            return colaboradores
                .Where(c => c.Unidade != null && c.Usuario != null)
                .Select(c => new ColaboradorResponseDTO
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Unidade = new UnidadeResponseDTO
                    {
                        Id = c.Unidade!.Id,
                        Codigo = c.Unidade.Codigo,
                        Nome = c.Unidade.Nome,
                        Ativa = c.Unidade.Ativa
                    },
                    Usuario = new UsuarioResponseDTO
                    {
                        Id = c.Usuario!.Id,
                        Login = c.Usuario.Login,
                        Ativo = c.Usuario.Ativo
                    }
                }).ToList();
        }
    }
} 