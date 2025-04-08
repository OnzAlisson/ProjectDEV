using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectDEV.Models
{
    public class Usuario : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [JsonIgnore]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public bool Ativo { get; set; } = true;

        // Propriedade de navegação para Colaborador
        [JsonIgnore]
        public Colaborador? Colaborador { get; set; }
    }
} 