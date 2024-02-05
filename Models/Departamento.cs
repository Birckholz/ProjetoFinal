using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFinal
{

    public class Departamento
    {
        [Key]
        public int codDepartamento { get; set; }
        [MaxLength(50)]
        [Required]
        public string? nomeDepartamento { get; set; }
        [ForeignKey("responsavelDepartamento")]
        public virtual Funcionario? fkresponsavelDepartamento { get; set; }
        public int responsavelDepartamento { get; set; }

        public ICollection<Funcionario> funcionariosDepartamento { get; set; }

        public Departamento()
        {
            funcionariosDepartamento = new List<Funcionario>();
        }

    }
}