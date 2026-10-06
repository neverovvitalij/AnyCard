using AnyCard.Application.Common;
using AnyCard.Application.Interfaces;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace AnyCard.Infrastructure.Services;

public class MailjetEmailService : IEmailService
{
    private readonly IMailjetClient _mailjetClient;
    private readonly MailjetSettings _settings;
    private readonly ILogger<MailjetEmailService> _logger;

    public MailjetEmailService(
        IMailjetClient mailjetClient,
        IOptions<MailjetSettings> options,
        ILogger<MailjetEmailService> logger)
    {
        _mailjetClient = mailjetClient;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var request = new MailjetRequest { Resource = Send.Resource }
                .Property(Send.FromEmail, _settings.SenderEmail)
                .Property(Send.FromName, _settings.SenderName)
                .Property(Send.Subject, subject)
                .Property(Send.TextPart, body)
                .Property(Send.Recipients, new JArray { new JObject { { "Email", to } } });

            var response = await _mailjetClient.PostAsync(request);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email sent to {Recipient}", to);
                return true;
            }

            _logger.LogError(
                "Failed to send email to {Recipient}. Status: {StatusCode}, Info: {ErrorInfo}, Message: {ErrorMessage}",
                to, response.StatusCode, response.GetErrorInfo(), response.GetErrorMessage());
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while sending email to {Recipient}", to);
            return false;
        }
    }
}