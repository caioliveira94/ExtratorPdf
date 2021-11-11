using System.ComponentModel.DataAnnotations;

namespace SmartSATWeb.ViewModels
{
    public class VideoVM
    {
        public int Id { get; set; }

        [Display(Description = "Título")]
        public string Titulo { get; set; }

        [Display(Description = "Descrição")]
        public string Descricao { get; set; }

        [Display(Description = "Categoria")]
        public string Categoria { get; set; }

        [Display(Description = "Url")]
        public string Url { get; set; }

        public string VideoId { get; set; }
    }
}
