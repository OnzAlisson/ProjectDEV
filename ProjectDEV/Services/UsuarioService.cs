using Microsoft.EntityFrameworkCore;
using ProjectDEV.Data;
using ProjectDEV.DTOs;
using ProjectDEV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDEV.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public UsuarioService(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<UsuarioResponseDTO> CriarUsuario(UsuarioCreateDTO usuarioDto)
        {
            // Verificar se já existe usuário com o mesmo login
            if (await _context.Usuarios.AnyAsync(u => u.Login == usuarioDto.Login))
            {
                throw new InvalidOperationException("Já existe um usuário com este login");
            }

            var usuario = new Usuario
            {
                Login = usuarioDto.Login,
                Senha = HashSenha(usuarioDto.Senha),
                Ativo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Login = usuario.Login,
                Ativo = usuario.Ativo
            };
        }

        public async Task<UsuarioResponseDTO> AtualizarUsuario(int id, UsuarioUpdateDTO usuarioDto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado");
            }

            usuario.Senha = HashSenha(usuarioDto.Senha);
            usuario.Ativo = usuarioDto.Ativo;
            usuario.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Login = usuario.Login,
                Ativo = usuario.Ativo
            };
        }

        public async Task<List<UsuarioResponseDTO>> ListarUsuarios(bool? ativo = null)
        {
            IQueryable<Usuario> query = _context.Usuarios;
            
            if (ativo.HasValue)
            {
                query = query.Where(u => u.Ativo == ativo.Value);
            }

            var usuarios = await query.ToListAsync();

            return usuarios.Select(u => new UsuarioResponseDTO
            {
                Id = u.Id,
                Login = u.Login,
                Ativo = u.Ativo
            }).ToList();
        }

        public async Task<string?> Autenticar(LoginDTO loginDto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login == loginDto.Login && u.Ativo);

            if (usuario == null || usuario.Senha != HashSenha(loginDto.Senha))
            {
                return null;
            }

            return _tokenService.GenerateToken(usuario);
        }

        private string HashSenha(string senha)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
} 