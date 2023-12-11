using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace FastFoodWebApplication.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int DishId {  get; set; }
        public Dish Dish { get; set; }
        public int Quantity { get; set; }
        public DishSize DishSize { get; set; }
        public int UserId { get; set; }
        public Profile profile { get; set; }
    }
}
