using AnyCard.Application.Interfaces;
using AnyCard.Infrastructure.Services;

namespace AnyCard.BackgroundServices;

public class EmailBackgroundService : BackgroundService
{
    private readonly EmailQueue _emailQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EmailBackgroundService> _logger;
    public EmailBackgroundService(EmailQueue emailQueue, IServiceScopeFactory scopeFactory, ILogger<EmailBackgroundService> logger)
    {
        _emailQueue = emailQueue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _emailQueue.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                await emailService.SendEmailAsync(message.To, message.Subject, message.Body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process queued email to {Recipient}", message.To);
            }
        }
    }
}
