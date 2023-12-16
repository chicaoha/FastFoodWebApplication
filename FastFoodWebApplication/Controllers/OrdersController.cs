using FastFoodWebApplication.Data;
using FastFoodWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Enumerable = System.Linq.Enumerable;
namespace FastFoodWebApplication.Controllers
{
    public class OrdersController : Controller
    {
        private readonly FastFoodWebApplicationContext _context;

        public OrdersController(FastFoodWebApplicationContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string OrderShippingStatus)
        {


            string userName = User.Identity.Name;
            var user = _context.Users.Include(u => u.Profile).SingleOrDefault(u => u.UserName == userName);
             var order = await _context.Order.Where
            (c => c.UserId == user.Id)
             .ToListAsync();
            if(String.IsNullOrEmpty(OrderShippingStatus))
            {

                order = order;
            }
            else
            {
                order = await _context.Order.Where
            (c => c.UserId == user.Id && c.shipping_status == OrderShippingStatus).ToListAsync();
            }
            ViewData["order"] = order;
            ViewData["act"] = OrderShippingStatus;
            
            return View();
          
        }
        public async Task<IActionResult> ViewOrderDetail(int orderId)
        {
            string userName = User.Identity.Name;
            var user = _context.Users.Include(u => u.Profile).SingleOrDefault(u => u.UserName == userName);

            List<OrderDetail>orderDatail = new List<OrderDetail>();
            var order = await _context.Order.SingleOrDefaultAsync(c => c.UserId == user.Id && c.Id == orderId);
           
            if(order != null)
            {
                orderDatail = await _context.OrderDetail.Include(c=>c.Dish).
                    Where(c=> c.OrderId ==orderId).ToListAsync();
            }

            ViewData["Order"] = await _context.Order.SingleOrDefaultAsync(c => c.Id == orderId);

            return View(orderDatail);

        }
        public async Task<IActionResult> Placeorder([Bind("Id,OrderDate,TotalPrice,shipping_status,UserId")] Order order)
        {

            string userName = User.Identity.Name;
            var user = _context.Users.Include(u => u.Profile).SingleOrDefault(u => u.UserName == userName);
            var listOrder =  await _context.Cart.Where(c => c.UserId == user.Id).ToListAsync();
            
            order.UserId = user.Id;
          //  order.OderDate = localDate;
            order.shipping_status = "Pending";
            decimal total = listOrder.Sum(item => item.Price);
            order.TotalPrice = total ;

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            int id = order.Id;
            
            if(id != 0)
            {
                foreach(var item in listOrder)
                {
                  
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderId = item.DishId;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.Price = item.Price;
                    orderDetail.size = item.size;

                    var sql = $"INSERT INTO OrderDetail (OrderId , DishId, Quantity, Price, size) VALUES ({id},{item.DishId}, {item.Quantity}, {item.Price}, '{item.size}')";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    
                    await _context.SaveChangesAsync();
                   
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _context.Order.Remove(order);
            }
            
            return View();
        }
    }

}
