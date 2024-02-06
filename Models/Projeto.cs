using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFinal
{

    public class Projeto
    {
        [Key]
        public int codProjeto { get; set; }
        [ForeignKey("codDepartamento")]
        public int codDepartamento { get; set; }
        public virtual Departamento? fkCodDepartamento { get; set; }
        public int idCliente { get; set; }
        [ForeignKey("idCliente")]
        public virtual Cliente? fkCliente { get; set; }
        [MaxLength(100)]
        [Required]
        public string? nomeProjeto { get; set; }
        [MaxLength(200)]
        public string? descricaoProjeto { get; set; }
        [MaxLength(50)]
        [Required]
        public string? statusProjeto { get; set; }
        public float valorProjeto { get; set; }
        public DateOnly dataEntregaProjeto { get; set; }

        public virtual ICollection<ProjetoFuncionario> funcionariosProj { get; set; } = null!;

        public Projeto()
        {
            funcionariosProj = new List<ProjetoFuncionario>();
        }
    }
}