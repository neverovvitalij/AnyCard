using AnyCard.Application.Common;

namespace AnyCard.Application.Interfaces;

public interface IEmailQueue
{
    ValueTask EnqueueAsync(EmailMessage emailMessage);
}
