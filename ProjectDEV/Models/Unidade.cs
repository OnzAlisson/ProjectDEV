using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectDEV.Models
{
    public class Unidade : BaseEntity
    {
        [Required]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public bool Ativa { get; set; } = true;

        // Propriedade de navegação para Colaboradores
        public List<Colaborador> Colaboradores { get; set; } = new List<Colaborador>();
    }
} 