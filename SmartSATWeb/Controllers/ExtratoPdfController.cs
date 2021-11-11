using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interfaces;
using SmartSATWeb.ViewModels;

namespace SmartSATWeb.Controllers
{
    public class ExtratoPdfController : Controller
    {
        private readonly IExtratoClienteService _extratoClienteService;

        public ExtratoPdfController(IExtratoClienteService extratoClienteService)
        {
            _extratoClienteService = extratoClienteService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ExtratoPdfVM pdfViewModel)
        {
            var fileStream = pdfViewModel.File.OpenReadStream();
            var dto = new ExtratoPdfDto { File = fileStream, LayoutPdf = pdfViewModel.LayoutPdf };
            TempData["status"] = _extratoClienteService.ProcessaPdf(dto) ? "sucesso" : "falha"; 

            return View();
        }
    }
}