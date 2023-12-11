using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FastFoodWebApplication.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateAndTime OderDate { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 3)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal TotalPrice { get; set; }
        public string shipping_status { get; set; }
        public int UserId { get; set; }        
        public Profile Profile { get; set; }
    }
}
