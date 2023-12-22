using FastFoodWebApplication.Data;
using FastFoodWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
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

        //[HttpPost]
        //public async Task<IActionResult> UpdateCartBySize(
        //            int dishId, string size)
        //{
        //    // Retrieve the cart item based on dish ID and user
        //    string userName = User.Identity.Name;
        //    var user = _context.Users.SingleOrDefault(u => u.UserName == userName);

        //    var cartItem = await _context.Cart
        //        .Include(c => c.Dish)
        //        .FirstOrDefaultAsync(c => c.DishId == dishId && c.UserId == user.Id);

        //    if (cartItem != null)
        //    {
        //        var quantity = cartItem.Quantity;
        //        cartItem.size = size;

        //        // Recalculate the price based on the updated quantity and size
        //        var dish = await _context.Dish.SingleOrDefaultAsync(x => x.DishId == dishId);
        //        cartItem.Price = CalculatePrice(dish.DishPrice, size, quantity);


        //        await _context.SaveChangesAsync();
        //        // You can return a response if needed
        //        return Json(new { Success = true, UpdatedPrice = cartItem.Price });

        //    }

        //    // Return an error response if the cart item is not found
        //    //return Json(new { Success = false, UpdatedPrice = cartItem.Price });
        //}
        private decimal CalculatePrice(decimal basePrice, string size, int quantity)
        {
            decimal sizePrice = 0;

            if (size == "M")
            {
                sizePrice = basePrice * 0.4m;
            }
            else if (size == "L")
            {
                sizePrice = basePrice * 0.8m;
            }
            return (basePrice + sizePrice) * quantity;
        }
    }
}
