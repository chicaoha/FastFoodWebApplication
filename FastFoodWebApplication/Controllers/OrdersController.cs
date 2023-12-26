﻿using FastFoodWebApplication.Data;
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
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

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

            if (shipping_status.Equals("Completed"))
            {
                string userName = User.Identity.Name;
                var user = _context.Users.Include(u => u.Profile).SingleOrDefault(u => u.UserName == userName);

                var list = await _context.Order.Where(c => c.UserId == user.Id && shipping_status == "Completed").ToListAsync();
                decimal userSpend = list.Sum(item => item.TotalPrice);
                user.Profile.UserSpend = userSpend;

                _context.Update(user);
                _context.SaveChanges();
            }




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
            var dish = await _context.Dish.ToListAsync();

            for (int i = 0; i < dish.Count; i++)
            {
                Dictionary<object, object> map = new Dictionary<object, object>();

                var orderDetail = await _context.OrderDetail.Where(c => c.DishId == dish[i].DishId).ToListAsync();
                decimal revenue = 0;
                if (!map.ContainsKey(dish[i].DishId))
                {
                    map.Add("label", dish[i].Name);
                    revenue = orderDetail.Sum(item => item.Price);
                    map.Add("y", revenue);
                }
                list.Add(map);
            }
            string date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
            ViewData["revenue"] = total;
            ViewData["count"] = listOrder.Count;
            ViewData["list"] = list;
            ViewData["date"] = date;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewChart([FromForm] string date, string hihi)
        {
            DateTime dateValue = DateTime.Now;
            if (date == null)
            {
                date = DateTime.Now.ToString();
            }

            try
            {
                dateValue = DateTime.Parse(date);
                Console.WriteLine("'{0}' converted to {1}.", date, dateValue);
            }
            catch (FormatException)
            {
                Console.WriteLine("Unable to convert '{0}'.", date);
            }



            var listOrder = await _context.Order
                    .Where(c => c.shipping_status == "Completed" && c.OderDate.Date == dateValue.Date)
                    .ToListAsync();
            decimal total = listOrder.Sum(item => item.TotalPrice);
            List<Dictionary<object, object>> list = new List<Dictionary<object, object>>();
            var dish = await _context.Dish.ToListAsync();
            var detail = await _context.OrderDetail.ToListAsync();
            decimal revenue = detail.Sum(item => item.Price);
            for (int i = 0; i < dish.Count; i++)
            {
                Dictionary<object, object> map = new Dictionary<object, object>();

                var orderDetail = await _context.OrderDetail.Where(c => c.DishId == dish[i].DishId).ToListAsync();
                decimal subRevenue = 0;
                if (!map.ContainsKey(dish[i].DishId))
                {
                    map.Add("label", dish[i].Name);
                    subRevenue = orderDetail.Sum(item => item.Price);
                    decimal percentage = (subRevenue / revenue) * 100;
                    map.Add("y", percentage);
                }
                list.Add(map);
            }

            string selectedDate = dateValue.Year + "-" + dateValue.Month + "-" + dateValue.Day;
            ViewData["revenue"] = total;
            ViewData["count"] = listOrder.Count;
            ViewData["list"] = list;
            ViewData["date"] = selectedDate;
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
            //if(voucherCode!= "None")
            //{
            //    removeByVoucherCode(voucherCode);
            //}
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            int id = order.Id;

            if (id != 0)
            {
                foreach (var item in listOrder)
                {

                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderId = id;
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

        public async Task<IActionResult> Report()
        {

            var listOrder = await _context.Order
                    .Where(c => c.shipping_status == "Completed" && c.OderDate.Date == DateTime.Now.Date)
                    .ToListAsync();
            decimal total = listOrder.Sum(item => item.TotalPrice);
            if (total == 0)
            {
                total = 1;
            }
            List<Dictionary<object, object>> list = new List<Dictionary<object, object>>();
            List<Dictionary<object, object>> listPercentage = new List<Dictionary<object, object>>();
            var dish = await _context.Dish.ToListAsync();
            var dishType = await _context.DishType.ToListAsync();

            for (int i = 0; i < dish.Count; i++)
            {

                Dictionary<object, object> map1 = new Dictionary<object, object>();

                var orderDetail = await _context.OrderDetail.Include(c => c.Order).Where(c => c.DishId == dish[i].DishId && c.Order.shipping_status == "Completed" && c.Order.OderDate.Date == DateTime.Now.Date).ToListAsync();

                decimal revenue = 0;
                if (!map1.ContainsKey(dish[i].DishId))
                {

                    revenue = orderDetail.Sum(item => item.Price);
                    decimal percentage = (revenue / total) * 100;
                    map1.Add("label", dish[i].Name);
                    map1.Add("y", percentage);
                }

                listPercentage.Add(map1);
            }
            for (int i = 0; i < dishType.Count; i++)
            {
                
                Dictionary<object, object> map = new Dictionary<object, object>();
                var orderDetail = await _context.OrderDetail.Include(c => c.Order).
                    Include(c => c.Dish).ThenInclude(c => c.DishType).
                    Where(c => c.Order.shipping_status == "Completed"
                    && c.Order.OderDate.Date == DateTime.Now.Date && c.Dish.DishType.Id == dishType[i].Id).ToListAsync();

                
                if (!map.ContainsKey(dishType[i].Id))
                {
                    map.Add("label", dishType[i].Name);
                    decimal revenue = (orderDetail.Sum(item => item.Price));
                    map.Add("y", revenue);
                }
                list.Add(map);
            }


            string date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
            ViewData["listSale"] = list;
            ViewData["listPercentage"] = listPercentage;
            ViewData["date"] = date;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report([FromForm] string fromDate, string toDate)
        {
            DateTime fromValue = DateTime.Now;
            DateTime toValue = DateTime.Now;
            if (fromDate == null && toDate == null)
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            try
            {
                fromValue = DateTime.Parse(fromDate);
                toValue = DateTime.Parse(toDate);
                //Console.WriteLine("'{0}' converted to {1}.", date, dateValue);
            }
            catch (FormatException)
            {
                Console.WriteLine("Unable to convert '{0}'.", fromDate);
            }



            var listOrder = await _context.Order
                    .Where(c => c.shipping_status == "Completed" && c.OderDate.Date >= fromValue.Date && c.OderDate.Date <= toValue.Date)
                    .ToListAsync();
            decimal total = listOrder.Sum(item => item.TotalPrice);
            List<Dictionary<object, object>> list = new List<Dictionary<object, object>>();
            var dish = await _context.Dish.ToListAsync();
            var detail = await _context.OrderDetail.ToListAsync();
            decimal revenue = detail.Sum(item => item.Price);
            for (int i = 0; i < dish.Count; i++)
            {
                Dictionary<object, object> map = new Dictionary<object, object>();

                var orderDetail = await _context.OrderDetail.Where(c => c.DishId == dish[i].DishId).ToListAsync();
                decimal subRevenue = 0;
                if (!map.ContainsKey(dish[i].DishId))
                {
                    map.Add("label", dish[i].Name);
                    subRevenue = orderDetail.Sum(item => item.Price);
                    decimal percentage = (subRevenue / revenue) * 100;
                    map.Add("y", percentage);
                }
                list.Add(map);
            }

            string FromDate = fromValue.Year + "-" + fromValue.Month + "-" + fromValue.Day;
            string ToDate = toValue.Year + "-" + toValue.Month + "-" + toValue.Day;
            ViewData["revenue"] = total;
            ViewData["count"] = listOrder.Count;
            ViewData["list"] = list;
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;
            return View();
        }

    }

}
