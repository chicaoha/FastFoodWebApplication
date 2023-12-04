using FastFoodWebApplication.Data;
using FastFoodWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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


        public async Task<IActionResult> Index(int? dishId)
        {
            var dishes = await _context.Dish.Include(d => d.DishType).ToListAsync();
          
            if (dishId != null)
            {
                dishes = dishes.Where(x => x.DishTypeId == dishId).ToList();
            }

            ViewData["Dishes"] = dishes;
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


        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.DishType)
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }
    }
}
