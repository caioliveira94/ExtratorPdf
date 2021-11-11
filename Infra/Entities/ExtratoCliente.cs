using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("ExtratoCliente")]
    public class ExtratoCliente
    {
        public ExtratoCliente()
        {
            PosicaoCliente = new List<PosicaoCliente> { };
            MovimentacaoCliente = new List<MovimentacaoCliente> { };
        }

        [Key]
        [Column("Id")]
        public int Id { get; set; }
        
        [Column("Agencia")]
        [Display(Description = "Agência")]
        public string Agencia { get; set; }

        [Column("ContaCorrente")]
        [Display(Description = "Conta Corrente")]
        public string ContaCorrente { get; set; }

        [Column("DataExtrato")]
        [Display(Description = "Data do Extrato")]
        public DateTime DataExtrato { get; set; }

        [Column("ModeloExtrato")]
        [Display(Description = "Modelo do Extrato")]
        public string ModeloExtrato { get; set; }

        public ICollection<PosicaoCliente> PosicaoCliente { get; set; }
        public ICollection<MovimentacaoCliente> MovimentacaoCliente { get; set; }
    }
}
