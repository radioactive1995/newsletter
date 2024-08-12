using Application.Interfaces.Services;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services;
public class EmailService : IEmailService
{
    public async Task SendSubscriberConfirmationMail()
    {
        var user = "dzjumajev95@gmail.com";
        var password = "";

        using var client = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(user, password),
            EnableSsl = true,
        };


        var htmlBody = @"
<html>
<head>
    <style>
        body { 
            font-family: 'Helvetica Neue', sans-serif;
            margin: 0;
            padding: 0;
        }
        .container { 
            padding: 20px; 
        }
        .header { 
            font-size: 26px; 
            font-weight: bold;
            margin-bottom: 15px;
        }
        .content { 
            font-size: 18px; 
            line-height: 1.5;
        }
    </style>
</head>
<body>
    <div class='container'>
        <div class='content'>
            <p>You’ve successfully subscribed to my personal newsletter!</p>
            <p>Welcome to our community! We appreciate your interest and look forward to sharing the latest updates and news with you.</p>
        </div>
    </div>
</body>
</html>
";

        var message = new MailMessage(
            from: "newsletter-notreply@gmail.com",
            to: user,
            subject: "Subscription Confirmation",
            body: htmlBody
        )
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}
