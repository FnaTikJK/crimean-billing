﻿using System.Net;
using System.Net.Mail;
using API.Infrastructure;
using API.Infrastructure.Config;

namespace API.Modules.NotificationModule;

public interface INotificationService
{
    void SendEmail(string title, string message, params string[] recipients);
}

public class NotificationService : INotificationService
{
    private readonly SmtpClient smtpClient;
    
    public NotificationService(ILog log)
    {
        smtpClient = new SmtpClient
        {
            Host = "smtp.mail.ru",
            Port = 587,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(Config.MailBoxLogin, Config.MailBoxPassword),
            EnableSsl = true,
        };

        smtpClient.SendCompleted += (e, a) =>
        {
            var msg = a.Error?.Message;
            if (msg != null)
            {
                log.Error(msg);
            }
        };
    }


    public void SendEmail(string title, string message, params string[] recipients)
    {
        var mailMessage = new MailMessage
        {
            Subject = title,
            Body = message,
            IsBodyHtml = true,
            From = new MailAddress(Config.MailBoxLogin),
        };
        foreach (var recipient in recipients)
            mailMessage.To.Add(recipient);
        
        smtpClient.SendAsync(mailMessage, null);
    }
}