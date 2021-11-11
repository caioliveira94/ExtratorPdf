using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("ExtratoCliente")]
    public class ExtratoCliente
    {
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

        public virtual ICollection<PosicaoCliente> PosicaoCliente { get; set; }
        public virtual ICollection<MovimentacaoCliente> MovimentacaoCliente { get; set; }
    }
}
