using System;
using System.ComponentModel.DataAnnotations;

namespace CommonLayer
{
    public class UserPostModel
    {
        [Required]
       // [RegularExpression("^[A-Z]{1}[a-z]{4,}$", ErrorMessage = "name starts with Cap and has minimum 4 characters")]
        public string FirstName { get; set; }

        [Required]
      //  [RegularExpression("^[A-Z]{1}[a-z]{4,}$", ErrorMessage = "name starts with Cap and has minimum 4 characters")]
        public string LastName { get; set; }

        [Required]
        //[RegularExpression("^[a-z]{3,}[1-9]{1,4}[@][a-z]{4,}[.][a-z]{3,}$", ErrorMessage = "Please Enter Valid Email")]
        public string Email { get; set; }

        [Required]
       // [RegularExpression("^[A-Z]{1}[a-z]{3,}[@][0-9]{1,}$", ErrorMessage = "Please Enter Valid Password")]
        public string Password { get; set; }
        public string Address { get; set; }

    }
}