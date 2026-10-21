namespace AnyCard.Application.Common;

public record EmailMessage
(
    string To, 
    string Subject, 
    string Body
);
