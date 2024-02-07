using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal
{

    public class Cliente
    {
        [Key]
        public int codCliente { get; set; }
        [MaxLength(100)]
        [Required]
        public string? nomeCliente { get; set; }
        [MaxLength(50)]
        [Required]
        public string? telefoneCliente { get; set; }
        [MaxLength(50), EmailAddress]
        [Required]
        public string? emailCliente { get; set; }
        [MaxLength(100)]
        [Required]
        public string? enderecoCliente { get; set; }
        [MaxLength(100)]
        public string? descricaoCliente { get; set; }
        [MaxLength(14)]
        public string? PessFCPFCliente { get; set; }//pessoa física
        [MaxLength(18)]
        public string? PessJCNPJCliente { get; set; }//pessoa juridica
        [MaxLength(20)]
        public string? statusCliente { get; set; }
        public virtual ICollection<Projeto> clienteProjetos { get; set; } = null!;

        public Cliente()
        {
            clienteProjetos = new List<Projeto>();
        }

    }
}