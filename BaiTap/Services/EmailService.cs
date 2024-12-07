using System;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    public void SendEmail(string toEmail, string subject, string body)
    {
        var fromEmail = "nguuentoanbs2k4@gmail.com"; // Thay thế bằng email của bạn
        var fromPassword = "Toanbg2k4"; // Thay thế bằng mật khẩu của bạn

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromEmail, fromPassword),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        smtpClient.Send(mailMessage);
    }
}
