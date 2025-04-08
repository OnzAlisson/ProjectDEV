using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectDEV.DTOs
{
    public class UnidadeCreateDTO
    {
        [Required]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;
    }

    public class UnidadeUpdateDTO
    {
        [Required]
        public bool Ativa { get; set; }
    }

    public class UnidadeResponseDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public bool Ativa { get; set; }
    }

    public class UnidadeColaboradoresDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public bool Ativa { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ColaboradorResponseDTO> Colaboradores { get; set; } = new List<ColaboradorResponseDTO>();
    }
} 