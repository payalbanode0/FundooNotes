using CommonLayer;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entities;
using RepositoryLayer.FundooContext;
using RepositoryLayer.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace RepositoryLayer.Services
{
    
    public class UserRL : IUserRL
    {
        //instances of fundoocontext message
        FundooDBContext fundooDBContext;

        public IConfiguration configuration { get; }
        public UserRL(FundooDBContext fundooDBContext, IConfiguration configuration)
        {
            this.fundooDBContext = fundooDBContext;
            this.configuration = configuration;

        }
        //method to user register detail
        public void AddUser(UserPostModel user)
        {
            try
            {
                User userdata = new User();
                userdata.FirstName = user.FirstName;
                userdata.LastName = user.LastName;
                userdata.Email = user.Email;
                
                userdata.Password = EncryptPassword(user.Password);
                //userdata.Password = decryptedpassword(user.Password);
                userdata.RegisteredDate = DateTime.Now;
                fundooDBContext.Add(userdata);
                fundooDBContext.SaveChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //validating email address
        public string LoginUser(string email, string password)
        {
            try
            {
                var result = fundooDBContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
                if (result == null)
                {
                    return null;
                }
                return GetJWTToken(email, result.UserId);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private static string GetJWTToken(string email, int userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", email),
                    new Claim("userID",userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ForgotPassword(string email)
        {
            try
            {
                var userdata = fundooDBContext.Users.FirstOrDefault(u => u.Email == email);
                if (userdata == null)
                {
                    return false;
                }

                MessageQueue queue;
                //Add Message to queue
                if (MessageQueue.Exists(@".\Private$\FundooQueue"))
                {
                    queue = new MessageQueue(@".\Private$\FundooQueue");
                }
                else
                {
                    queue = MessageQueue.Create(@".\Private$\FundooQueue");
                }
                Message MyMessage = new Message();
                MyMessage.Formatter = new BinaryMessageFormatter();
                MyMessage.Body = GetJWTToken(email, userdata.UserId);
                MyMessage.Label = "Forgot Password Email";
                queue.Send(MyMessage);

                Message msg = queue.Receive();
                msg.Formatter = new BinaryMessageFormatter();
                EmailService.SendMail(email, msg.Body.ToString());
                queue.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueue_RecieveCompleted);


                queue.BeginReceive();
                queue.Close();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void msmqQueue_RecieveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                EmailService.SendMail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
                queue.BeginReceive();
            }
            catch (MessageQueueException ex)
            {

                if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access denied." + "Queue might be a system queue");

                }
                //handle other sources of messagequeueexception
            }
        }

        private string GenerateToken(string email)
        {
            if (email == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Email", email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static string EncryptPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    return null;

                }
                else
                {
                    byte[] b = Encoding.ASCII.GetBytes(password);
                    string encrypted = Convert.ToBase64String(b);
                    return encrypted;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        //public static string decryptedpassword(string encryptedpassword)
        //{
        //    byte[] b;
        //    string decrypted;
        //    try
        //    {
        //        if (string.IsNullOrEmpty(encryptedpassword))
        //        {
        //            return null;

        //        }
        //        else
        //        {
        //            b = Convert.FromBase64String(encryptedpassword);
        //            decrypted = Encoding.ASCII.GetString(b);
        //            return decrypted;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public bool ChangePassword(ChangePasswordModel changepassword,string email)
        {
            try
            {    
                var user = fundooDBContext.Users.FirstOrDefault(u => u.Email == email);
                if (changepassword.Password.Equals(changepassword.ConfirmPassword))
                {
                   
                    
                    user.Password = EncryptPassword(changepassword.Password) ;
                    fundooDBContext.SaveChanges();
                    return true;
                }
                return false; 
                
            }
            
            catch (Exception ex)
            {

                throw ex;
            }
        }

        
    }


}
