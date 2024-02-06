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
        //n esta dando certo ,apenas se colocar int?, mas como quero que seja obrigatoria, posso colocar Required e int?
        public int idResponsavel { get; set; }
        [ForeignKey("idResponsavel")]
        public virtual Funcionario? fkResponsavelDepartamento { get; set; }


        public ICollection<Funcionario> funcionariosDepartamento { get; set; }
        public ICollection<Projeto> projetosDepartamento { get; set; }

        public Departamento()
        {
            funcionariosDepartamento = new List<Funcionario>();
            projetosDepartamento = new List<Projeto>();
        }

    }
}