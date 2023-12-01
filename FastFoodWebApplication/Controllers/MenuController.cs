using FastFoodWebApplication.Data;
using FastFoodWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFoodWebApplication.Controllers
{
    public class MenuController : Controller
    {
        public readonly FastFoodWebApplicationContext _context;
        public MenuController( FastFoodWebApplicationContext context)
        {
 
            _context = context;
        }


        public async Task<IActionResult> Index()
        {

            List<Dish> dishes = await _context.Dish.ToListAsync();

            ViewData["Dishes"] = dishes;

            List<DishType> types = await _context.DishType.ToListAsync();

            ViewData["DishType"] = types;
            return View();
        }
    }
}
