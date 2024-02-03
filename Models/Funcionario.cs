using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFinal
{

    public class Funcionario
    {
        [Key]
        public int codFuncionario { get; set; }


        public int? idCargo { get; set; }
        [ForeignKey("idCargo")]

        public virtual Cargo? fkCodCargo { get; set; }

        public int? idDepartamento { get; set; }

        public virtual Departamento? fkCodDepartamento { get; set; }
        [MaxLength(100)]
        [Required]
        public string? nomeFuncionario { get; set; }
        [MaxLength(50)]
        [Required]
        public string? telefoneFuncionario { get; set; }
        [MaxLength(50)]
        [Required]
        public string? emailFuncionario { get; set; }
        [MaxLength(100)]
        public string? enderecoFuncionario { get; set; }
        [MaxLength(30)]
        [Required]
        public string? CPFFuncionario { get; set; }
        [MaxLength(50)]
        public string? tipoContrFuncionario { get; set; } //tipo de contrato, temporario etc
        [MaxLength(100)]
        public string? modoTrabFuncionario { get; set; } //modo de trabalho: remoto ,...
        [MaxLength(100)]
        public string? formacaoRelevanteFuncionario { get; set; } //formação mais relevante
        [MaxLength(20)]
        [Required]
        public string? statusFuncionario { get; set; }


        public virtual ICollection<ProjetoFuncionario> funcionariosProj { get; set; } = null!;
    }
}