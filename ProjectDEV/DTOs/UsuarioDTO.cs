using System.ComponentModel.DataAnnotations;

namespace ProjectDEV.DTOs
{
    public class UsuarioCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Senha { get; set; } = string.Empty;
    }

    public class UsuarioUpdateDTO
    {
        [Required]
        [StringLength(100)]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public bool Ativo { get; set; }
    }

    public class UsuarioResponseDTO
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }

    public class LoginDTO
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
} 