using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectDEV.DTOs
{
    public class ColaboradorCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public int UnidadeId { get; set; }

        [Required]
        public UsuarioCreateDTO Usuario { get; set; } = null!;
    }

    public class ColaboradorUpdateDTO
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public int UnidadeId { get; set; }
    }

    public class ColaboradorResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UnidadeResponseDTO? Unidade { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UsuarioResponseDTO? Usuario { get; set; }
    }
} 