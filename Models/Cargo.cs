using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal
{

    public class Cargo
    {
        [Key]
        public int codCargo { get; set; }
        [MaxLength(50)]
        [Required]
        public string? nomeCargo { get; set; }
        public float salarioBase { get; set; }
        public virtual ICollection<Funcionario> funcionarioCargos { get; set; } = null!;
        public Cargo()
        {
            funcionarioCargos = new List<Funcionario>();
        }
    }
}