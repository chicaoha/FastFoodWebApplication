using FastFoodWebApplication.Data;
using FastFoodWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFoodWebApplication.Controllers
{
    public class MenuController : Controller
    {
        public readonly FastFoodWebApplicationContext _context;
        public MenuController(FastFoodWebApplicationContext context)
        {

            _context = context;
        }


        public async Task<IActionResult> Index(int? dishType, string searchString)
        {

         
            ViewData["Dishes"] = await _context.Dish.ToListAsync();
            ViewData["DishType"] = await _context.DishType.ToListAsync();
            return View();

        }
    }
}
