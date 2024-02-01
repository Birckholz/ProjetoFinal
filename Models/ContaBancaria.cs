using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFinal
{

    public class ContaBancaria
    {
        [Key]
        public int codContaB { get; set; }

        [ForeignKey("codFuncionario")]
        public int codFuncionario { get; set; }
        public virtual Funcionario? fkCodFuncionario { get; set; }

        [MaxLength(50)]
        [Required]
        public string? agenciaContaB { get; set; }
        [MaxLength(60)]
        [Required]
        public string? numeroContaB { get; set; }
        [MaxLength(60)]
        [Required]
        public string? tipoContaB { get; set; }
    }
}