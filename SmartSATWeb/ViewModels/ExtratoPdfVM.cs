using Microsoft.AspNetCore.Http;

namespace SmartSATWeb.ViewModels
{
    public class ExtratoPdfVM
    {
        public IFormFile File { get; set; }
        public string LayoutPdf { get; set; }
    }
}
