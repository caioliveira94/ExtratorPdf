using System.IO;

namespace Service.Dto
{
    public class ExtratoPdfDto
    {
        public Stream File { get; set; }
        public string LayoutPdf { get; set; }
    }
}
