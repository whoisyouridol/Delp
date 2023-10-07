using Delp.Application.ConfigurationModels;
using Delp.Application.InfrastructureAbstractions;
using Delp.Infrastructure.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Delp.Infrastructure.Implementations;

public class MailService : IMailService
{
    private EmailConfiguration _emailConfiguration;

    public MailService(EmailConfiguration emailConfiguration)
    {
        _emailConfiguration = emailConfiguration;
    }

    public async Task SendMail(IEnumerable<string> to, string subject, string content)
    {
        var message = new Message(to, subject, content);
        var emailMessage = CreateEmailMessage(message);
        await Send(emailMessage);
    }
    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(default, _emailConfiguration.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
        return emailMessage;
    }
    private async Task Send(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
            client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);
            await client.SendAsync(mailMessage);
        }
        catch
        {
            //Log
            throw;
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }
}