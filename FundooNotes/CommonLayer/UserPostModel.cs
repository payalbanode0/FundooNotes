using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Users
{


    public class UserPostModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
    }

}
