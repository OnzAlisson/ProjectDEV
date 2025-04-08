using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectDEV.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectDEV.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly string _jwtKey;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("A chave JWT não está configurada");
        }

        public string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Login)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 