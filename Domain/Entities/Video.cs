using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Video")]
    public class Video
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Titulo")]
        [Display(Description = "Título")]
        public string Titulo { get; set; }

        [Column("Descricao")]
        [Display(Description = "Descrição")]
        public string Descricao { get; set; }

        [Column("Url")]
        [Display(Description = "Url")]
        public string Url { get; set; }
    }
}
