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
        public int? idResponsavel { get; set; }
        [ForeignKey("idResponsavel")]
        public virtual Funcionario? fkResponsavelDepartamento { get; set; }


        public ICollection<Funcionario> funcionariosDepartamento { get; set; }

        public Departamento()
        {
            funcionariosDepartamento = new List<Funcionario>();
        }

    }
}