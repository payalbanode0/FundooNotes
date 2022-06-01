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
                    $"<html lang=\"en\">" +
                    $"<head>" +
                    $"<meta charset=\"UTF-8\">" +
                    $"</head>" +
                    $"<body>" +
                    $"<h2> Dear Fundoo User, </h2>\n" +
                    $"<h3>please click on the below link to reset password</h3>" +
                    $"<a href='http://localhost:4200/reset-password/{token}'>clickhere </a>\n" +
                    $"<h3 style =\"color:#blue\">\n the link is valid for 1 hour</h3>" +
                    $"</body>" +
                    $"</html>";

                //msgObj.Body = $"<!DOCTYPE html>" +
                //                "<html> " +
                //                    "<body style=\"background -color:#ff7f26;text-align:center;\"> " +
                //                    "<h1 style=\"color:#051a80;\">Hello Payal</h1> " +
                //                    "<h2 style=\"color:#800000;\">Please Click on the below link to Recover Password.</h2>" +
                //                    "</body> " + $"http://localhost:4200/reset-password/{token}" +
                //                "</html>";
                client.Send(msgObj);
            }
        }
    }
}