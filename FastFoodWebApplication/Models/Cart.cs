using System.Drawing;

namespace FastFoodWebApplication.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int DishId {  get; set; }
        public Dish Dish { get; set; }
        public int Quantity { get; set; }

        public DishSize DishSize { get; set; }

    }
}
