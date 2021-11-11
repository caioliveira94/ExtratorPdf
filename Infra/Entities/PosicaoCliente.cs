using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("PosicaoCliente")]
    public class PosicaoCliente
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        //[Column("IdExtratoCliente")]
        //public int IdExtratoCliente { get; set; }

        [ForeignKey("IdExtratoCliente")]
        public ExtratoCliente ExtratoCliente { get; set; }

        [Column("Ativo")]
        public string Ativo { get; set; }

        [Column("TipoAtivo")]
        public string TipoAtivo { get; set; }

        [Column("ValorFinanceiroLiq")]
        public decimal ValorFinanceiroLiq { get; set; }

        [Column("ClassificacaoAtivo")]
        public string ClassificacaoAtivo { get; set; }
    }
}
