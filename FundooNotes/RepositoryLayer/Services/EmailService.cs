using System.Net;
using System.Net.Mail;

namespace RepositoryLayer.Services
{
    public class EmailService
    {
        public static void SendMail(string email, string token)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("banpay0@gmail.com", "payban@23");


                MailMessage msgObj = new MailMessage();
                msgObj.To.Add(email);
                msgObj.From = new MailAddress("banpay0@gmail.com");
                msgObj.Subject = "Password Reset Link";
                msgObj.IsBodyHtml = true;
                msgObj.Body = $"<!DOCTYPE html>" +
                    "<html>" +
                    "<body style=\"background -color:#ff6347;text-align:center;\"> " +
                    "<h1 style=\"color:#800000;\">hello payal</h1>" +
                    "<h2 style=\"color:#ff6347;\">please click on below link to reset password.</h2>" +
                    "</body>" + $"www.FundooNotes.com/reset-password/{token}" +
                    "</html>";
                client.Send(msgObj);
            }
        }
    }
}