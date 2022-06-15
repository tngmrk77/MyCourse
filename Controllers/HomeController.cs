using Microsoft.AspNetCore.Mvc;

namespace MyCourse.Controllers
{
   // Se inserito qui l'attributo avrebbe effetto su tutte le actione del Controller
   // [ResponseCache(CacheProfileName = "Home")]
    public class HomeController : Controller
    {
        [ResponseCache(CacheProfileName = "Home")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Benvenuto su MyCourse!";
            return View();
        }
    }
}