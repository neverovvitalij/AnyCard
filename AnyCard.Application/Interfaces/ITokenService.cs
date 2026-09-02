
using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;
public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
