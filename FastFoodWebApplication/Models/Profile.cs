using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace FastFoodWebApplication.Models
{
    public enum Gender
    {
        Male, Female, Other
    }
    public enum Nationality
    {
        English, Vietnamese, Chineses, Korean, Japanese
    }
    public class Profile
    {
        [Key]
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public string Avatar { get; set; }
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public Gender Gender { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Date of birth")]
        public DateTime Dob { get; set; }
        [StringLength(maximumLength: 100, MinimumLength = 20)]
        public string Address { get; set; }
        [RegularExpression("[0-9]{10}")]
        public string Phone { get; set; }
        public Nationality Nationality { get; set; }
      
        public decimal totalPayment { get; set; }
    }
}