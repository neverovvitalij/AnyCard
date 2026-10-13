namespace AnyCard.Application.Common;

public record PagedResult<T>(List<T> Items, int TotalCount, int PageNumber, int PageSize);
