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
        [MaxLength(100)]
        [Required]
        public string? responsavelDepartamento { get; set; }

    }
}