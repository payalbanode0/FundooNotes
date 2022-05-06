using BusinessLayer.Interfaces;
using CommonLayer.Users;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.FundooContext;
using System;
using System.Linq;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        FundooDBContext fundooDBContext;
        IUserBL userBL;
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
                return this.Ok(new { success = true, message = $"login successfull(result)" });

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost("ForgotPassword/{email}")]
        public IActionResult ForgotPassword(string email)
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
    }
}

