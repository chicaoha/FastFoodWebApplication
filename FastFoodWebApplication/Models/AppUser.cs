using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FastFoodWebApplication.Models
{
    public class AppUser: IdentityUser<int>
    {
        public Profile Profile { get; set; }
    }
}
