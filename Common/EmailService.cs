using GameStore.Models.Orders;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace GameStore.Common
{
    public class EmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, List<Order> orders)
        {
            var smtpSettings = configuration.GetSection("EmailCredential");           

            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(smtpSettings["Adress"]!);
            mail.To.Add(toEmail);
            mail.Subject = "Purchase Notification in GameStore";
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(smtpSettings["Adress"], smtpSettings["Password"]);
            smtpServer.EnableSsl = true;

            string body = "<h2>You have successfully purchased the games:</h2><ul>";
            foreach (var order in orders)
            {
                body += $"<li><h4>{order.Game.Name} - {order.Game.Price}$</h4></li>";
            }
            body += $"</ul><h4>in the amount of {orders.Sum(x => x.Game.Price)}$</h4>";
            mail.Body = body;
            mail.IsBodyHtml = true;

            smtpServer.Send(mail);
            Console.WriteLine("Email sent successfully!");
        }
    }
}
