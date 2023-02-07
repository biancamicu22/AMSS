using DataLibrary.Models;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace DataLibrary.SMTPServer
{
    public class SMTP
    {
        public SmtpClient smtpClient = new SmtpClient();
        public SMTP()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
            var config = builder.Build();
            smtpClient = new SmtpClient(config["Smtp:Host"])
            {
                Port = int.Parse(config["Smtp:Port"]),
                Credentials = new NetworkCredential("hmpproject22@gmail.com", config["Smtp:Username"]),
                EnableSsl = true,
            };
        }
        public MailMessage ToDoMessage(AppTask task)
        {
            MailAddress from = new MailAddress("hmpproject22@gmail.com");
            MailAddress to = new MailAddress(task.User.Email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "You'll be busy";
            message.Body = $"The task named {task.Name} has been assigned to you ";
            return message;
        }

        public MailMessage InProgressMessage(AppTask task)
        {
            MailAddress from = new MailAddress("hmpproject22@gmail.com");
            MailAddress to = new MailAddress(task.User.Email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Good luck, you've just started a task!";
            message.Body = $"You started working on the task named {task.Name}!";
            return message;
        }
        public MailMessage DoneMessage(AppTask task)
        {
            MailAddress from = new MailAddress("hmpproject22@gmail.com");
            MailAddress to = new MailAddress(task.User.Email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Congratulations, you have completed the task!";
            message.Body = $"You have completed the task named {task.Name}!";
            return message;
        }
    }

}
