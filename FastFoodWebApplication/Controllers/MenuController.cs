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
            //IQueryable<Dish> dishes = _context.Dish
            //     .Include(m => m.DishType);
            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    dishes = dishes.Where(s => s.Name!.Contains(searchString));
            //}

            //if (dishType != null)
            //{
            //    dishes = dishes.Where(x => x.DishTypeId == dishType);
            //}
            //ViewData["Dishes"] = dishes;
            //ViewData["DishType"] = await _context.DishType.ToListAsync();

            //var menuVM = new MenuViewModel
            //{
            //    DishTypes = new SelectList(await _context.DishType.ToListAsync(), nameof(DishType.Id), nameof(DishType.Name)),
            //    Dishes = await dishes.ToListAsync()
            //};

            //return View(menuVM);
        }
    }
}
