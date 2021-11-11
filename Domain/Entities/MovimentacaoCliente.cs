using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MovimentacaoCliente")]
    public class MovimentacaoCliente
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("IdExtratoCliente")]
        public int IdExtratoCliente { get; set; }

        [Column("Ativo")]
        public string Ativo { get; set; }

        [Column("TipoAtivo")]
        public string TipoAtivo { get; set; }

        [Column("ValorFinanceiroLiq")]
        public decimal ValorFinanceiroLiq { get; set; }
    }
}
