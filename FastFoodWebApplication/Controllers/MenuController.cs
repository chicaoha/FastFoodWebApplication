using FastFoodWebApplication.Data;
using FastFoodWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
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


        public async Task<IActionResult> Index(int? DishTypeId)
        {
            var dishes = await _context.Dish.Include(d => d.DishType).ToListAsync();

            if (DishTypeId != null)
            {
                dishes = dishes.Where(x => x.DishTypeId == DishTypeId).ToList();
            }

            var dishSizes = Enum.GetValues(typeof(DishSize)).Cast<DishSize>();
            ViewData["Dishes"] = dishes;
            ViewData["DishType"] = await _context.DishType.ToListAsync();
            ViewData["active"] = DishTypeId;
            ViewData["DishSizes"] = dishSizes;
            return View();

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
            ViewBag.Price = dish.DishPrice;
            return View(dish);
        }

        //public IActionResult AddToCart(int dishId)
        //{
        //    // Assuming you have a method to get dish details by ID
        //    Dish dish = GetDishById(dishId);

        //    // Set data in TempData
        //    TempData["SelectedDish"] = dish;

        //    // Redirect to CartsController/Index
        //    return RedirectToAction("Index", "Carts");
        //}
        //private Dish GetDishById(int dishId)
        //{
            
        //    return new Dish { DishId = dishId, Name = "Sample Dish", Price = 10.99 };
        //}
    }
}
