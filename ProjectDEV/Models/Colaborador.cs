using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectDEV.Models
{
    public class Colaborador : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public int UnidadeId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        // Propriedades de navegação
        [ForeignKey("UnidadeId")]
        public Unidade? Unidade { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
    }
} 