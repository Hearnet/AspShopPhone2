using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

public class OrderConfirmationEmail
{
    public static void SendEmail(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("hearnet1971@gmail.com", "colm cjod xrbn cqvc"),
            EnableSsl = true,
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress("hearnet1971@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true, // Có thể sử dụng HTML trong nội dung email
        };

        mailMessage.To.Add(toEmail);

        // Gửi email
        smtpClient.Send(mailMessage);
    }
}