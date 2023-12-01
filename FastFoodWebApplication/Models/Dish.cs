using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FastFoodWebApplication.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Required]
        public string Name { get; set; }

        public DishSize DishSize { get; set; }

        public string Description { get; set; }
        public int DishStatus { get; set; }
        [ForeignKey(nameof(DishType))]
        [Display(Name = "DishType")]
        public int DishTypeId { get; set; }
        public DishType DishType { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 3)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal DishPrice { get; set; }
        public string DishImage { get; set; }

    }
}
