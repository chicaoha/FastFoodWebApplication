using Microsoft.AspNetCore.Mvc;

namespace FastFoodWebApplication.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
