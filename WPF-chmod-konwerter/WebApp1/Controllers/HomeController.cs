using ChmodConverterLib;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string conversion_type, string conversion_data)
        {
            if(conversion_data != null && conversion_data.Length > 0)
            {
                string data = "Coœ posz³o nie tak!";
                try
                {
                    if (conversion_type == "Symbolic")
                    {
                        data = ChmodConverter.NumericToSymbolic(conversion_data);
                    }
                    else
                    {
                        data = ChmodConverter.SymbolicToNumeric(conversion_data).ToString();
                    }
                    ViewBag.FormOverwriteData = $"{conversion_type}|{conversion_data}";
                    var typ = conversion_type == "Symbolic" ? "symboliczny" : "numeryczny";
                    ViewBag.FormData = $"{conversion_data} na typ {typ} => {data}";
                }
                catch (ArgumentException ex)
                {
                    ViewBag.FormData = $"{ex.Message}";
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
