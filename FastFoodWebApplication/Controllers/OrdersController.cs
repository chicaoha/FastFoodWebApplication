using FastFoodWebApplication.Data;
using FastFoodWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Enumerable = System.Linq.Enumerable;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using String = System.String;

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
            if (String.IsNullOrEmpty(OrderShippingStatus))
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


        public async Task<IActionResult> ManageOrder()
        {
            //string userName = User.Identity.Name;
            //var user = _context.Users.Include(u => u.Profile).SingleOrDefault(u => u.UserName == userName);
            var order = await _context.Order.ToListAsync();



            return View(order);

        }
        public async Task<IActionResult> UpdateShippingStatus(int? id, string shipping_status)
        {

            var order = await _context.Order.FirstOrDefaultAsync(c => c.Id == id);
            order.shipping_status = shipping_status;
            _context.Order.Update(order);
            _context.SaveChanges();



            return View(order);

        }
        public async Task<IActionResult> ViewOrderDetail(int orderId)
        {
            string userName = User.Identity.Name;
            var user = _context.Users.Include(u => u.Profile).SingleOrDefault(u => u.UserName == userName);

            List<OrderDetail> orderDatail = new List<OrderDetail>();
            var order = await _context.Order.SingleOrDefaultAsync(c => c.UserId == user.Id && c.Id == orderId);

            if (order != null)
            {
                orderDatail = await _context.OrderDetail.Include(c => c.Dish).
                    Where(c => c.OrderId == orderId).ToListAsync();
            }

            ViewData["Order"] = await _context.Order.SingleOrDefaultAsync(c => c.Id == orderId);

            return View(orderDatail);

        }
        public async Task<IActionResult> ViewChart()
        {

            var listOrder = await _context.Order
                .Where(c => c.shipping_status == "Completed" && c.OderDate.Date == DateTime.Now.Date)
                .ToListAsync();
            decimal total = listOrder.Sum(item => item.TotalPrice);
            List<Dictionary<object, object>> list = new List<Dictionary<object, object>>();
            var dish  = await _context.Dish.ToListAsync();

            for (int i = 0; i < dish.Count; i++)
            {
                Dictionary<object, object> map = new Dictionary<object, object>();

                var orderDetail = await _context.OrderDetail.Where(c=>c.DishId == dish[i].DishId).ToListAsync();
                decimal revenue = 0;
                if (!map.ContainsKey(dish[i].DishId))
                {
                    map.Add("label", dish[i].Name);
                    revenue = orderDetail.Sum(item => item.Price);
                    map.Add("y", revenue);
                }
                list.Add(map);
            }

            ViewData["revenue"] = total;
            ViewData["count"] = listOrder.Count;
            ViewData["list"] = list;
            return View();
        }



        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Placeorder([Bind("Id,OrderDate,TotalPrice,shipping_status,UserId,Name,Address,PhoneNumber,voucherCode")] Order order, string name, string address, string phone, string voucherCode)
        {

            string userName = User.Identity.Name;
            var user = _context.Users.Include(u => u.Profile).SingleOrDefault(u => u.UserName == userName);
            var listOrder = await _context.Cart.Where(c => c.UserId == user.Id).ToListAsync();

            order.UserId = user.Id;
            order.shipping_status = "Pending";
            decimal total = listOrder.Sum(item => item.Price);
            order.TotalPrice = total;
            order.Address = address;
            order.voucherCode = voucherCode;
            order.PhoneNumber = phone;
            order.Name = name;
            order.OderDate = DateTime.Now;


            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            int id = order.Id;

            if (id != 0)
            {
                foreach (var item in listOrder)
                {

                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderId = item.DishId;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.Price = item.Price;
                    orderDetail.size = item.size;

                    var sql = $"INSERT INTO OrderDetail (OrderId , DishId, Quantity, Price, size) VALUES ({id},{item.DishId}, {item.Quantity}, {item.Price}, '{item.size}')";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    _context.Cart.Remove(item);
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
       

        public async Task<IActionResult> Checkout()
        {

            string userName = User.Identity.Name;
            var user = _context.Users.Include(u => u.Profile).SingleOrDefault(u => u.UserName == userName);
            var listOrder = await _context.Cart.Where(c => c.UserId == user.Id).ToListAsync();

            var cart = _context.Cart.Include(c => c.Dish).Where(c => c.UserId == user.Id).ToList();

            var vouchers = await _context.UserVoucher.Include(c => c.Voucher).Include(c => c.User).Where(c => c.UserId == user.Id).ToListAsync();
            decimal total = listOrder.Sum(item => item.Price);
            ViewData["vouchers"] = vouchers;
            ViewData["subTotal"] = total;

            return View(cart);
        }


    }

}
