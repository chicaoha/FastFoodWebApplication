using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FastFoodWebApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FastFoodWebApplication.Data
{
    public class FastFoodWebApplicationContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public FastFoodWebApplicationContext (DbContextOptions<FastFoodWebApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<FastFoodWebApplication.Models.DishType> DishType { get; set; } = default!;

        public DbSet<FastFoodWebApplication.Models.Dish> Dish { get; set; }
    }
}
