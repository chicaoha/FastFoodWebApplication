using Microsoft.AspNetCore.Mvc;

namespace FastFoodWebApplication.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
