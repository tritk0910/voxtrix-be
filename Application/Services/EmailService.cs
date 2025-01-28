using Microsoft.Extensions.Configuration;
using Application.Interfaces;
using System.Net.Mail;
using System.Net;

namespace Application.Services;

public class EmailService(IConfiguration _config) : IEmailService
{

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var emailSettings = _config.GetSection("EmailSettings");

        MailMessage mailMessage = new()
        {
            From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        using var client = new SmtpClient();
        client.Host = emailSettings["SmtpServer"];
        client.Port = int.Parse(emailSettings["SmtpPort"]);
        client.Credentials = new NetworkCredential(emailSettings["SenderEmail"], emailSettings["Password"]);
        client.EnableSsl = true;

        await client.SendMailAsync(mailMessage);
    }
}