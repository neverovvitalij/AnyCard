using AnyCard.Application.Common;
using AnyCard.Application.Interfaces;
using System.Threading.Channels;

namespace AnyCard.Infrastructure.Services;

public class EmailQueue : IEmailQueue
{
    private readonly Channel<EmailMessage> _channel = Channel.CreateUnbounded<EmailMessage>();

    public ValueTask EnqueueAsync(EmailMessage emailMessage) => _channel.Writer.WriteAsync(emailMessage);

    public IAsyncEnumerable<EmailMessage> ReadAllAsync(CancellationToken cancellationToken) => _channel.Reader.ReadAllAsync(cancellationToken);
}
