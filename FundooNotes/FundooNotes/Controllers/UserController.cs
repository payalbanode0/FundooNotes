using BusinessLayer.Interfaces;
using CommonLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.FundooContext;
using System;
using System.Linq;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [ApiController] //handle the client error bind the incoming data
    [Route("[controller]")]
    public class UserController : ControllerBase //provide many method and proprties to handle http req
    {
        FundooDBContext fundooDBContext;
        IUserBL userBL;
        //constructor
        public UserController(FundooDBContext fundooDBContext, IUserBL userBL)
        {
            this.fundooDBContext = fundooDBContext;
            this.userBL = userBL;
        }
        [HttpPost("register")]
        public IActionResult AddUser(UserPostModel user)
        {
            try
            {
                this.userBL.AddUser(user);
                return this.Ok(new { success = true, message = $"user Added sucessfully" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("login/{email}/{password}")]
        public IActionResult LoginUser(string email, string password)
        {
            try
            {
                var userdata = fundooDBContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
                if (userdata == null)
                {
                    return this.BadRequest(new { success = false, message = $"email and password is invalid" });

                }
                var result = this.userBL.LoginUser(email, password);
                return this.Ok(new { success = true, message = $"login successfull {result}"});

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost("ForgotPassword/{email}")]
        public ActionResult ForgotPassword(string email)
        {
            try
            {
                var Result = this.userBL.ForgotPassword(email);
                if (Result != false)
                {
                    return this.Ok(new

                    {
                        success = true,
                        message = $"mail sent sucessfully" + $"token: {Result}"
                    });
                }
                return this.BadRequest(new { success = false, message = $"mail not sent" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [Authorize]
        [HttpPut("ChangePassword")]
           
       public IActionResult ChangePassword(ChangePasswordModel changePassword) 
        {
            try
            {
                string email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                bool result = userBL.ChangePassword(changePassword, email);
                if (result==false)
                {
                    return this.BadRequest(new { success = false, message = $"changepasword not sucessful" });
                }
                return this.Ok(new { success = true, message = $"changepasword  sucessful" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}

