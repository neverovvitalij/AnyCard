using AnyCard.Application.Common;
using AnyCard.Application.Extensions;
using AnyCard.Application.Interfaces;
using AnyCard.Domain.Model;
using AnyCard.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Microsoft.AspNetCore.RateLimiting;
using System.Text;

namespace AnyCard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
    private readonly IEmailService _emailService;

    public AuthController(IUserRepository userRepository, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository,
        IOptions<JwtSettings> jwtSettings, IPasswordResetTokenRepository passwordResetTokenRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtSettings = jwtSettings.Value;
        _passwordResetTokenRepository = passwordResetTokenRepository;
        _emailService = emailService;
    }

    [HttpPost("register")]
    [EnableRateLimiting("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        var email = registerDto.Email.NormalizeEmail();

        var isAlreadyRegistered = await _userRepository.GetByEmailAsync(email);
        if (isAlreadyRegistered != null)
        {
            return Conflict("Email ist bereits registriert");
        }
        var user = new User { Email = email };

        var hasher = new PasswordHasher<User>();
        var hashedPassword = hasher.HashPassword(user, registerDto.Password);

        user.PasswordHash = hashedPassword;
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenModel = new RefreshToken
        {
            Token = refreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            User = user,
            IsRevoked = false
        };
        await _refreshTokenRepository.AddNewTokenAsync(refreshTokenModel);
        await _refreshTokenRepository.SaveChangesAsync();

        var authResponseDto = new AuthResponseDto(refreshToken, accessToken);
        return Ok(authResponseDto);
    }

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        var email = loginDto.Email.NormalizeEmail();

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return Unauthorized("Ungültiger Email oder Passwort");
        }

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (result != PasswordVerificationResult.Success)
        {
            return Unauthorized("Ungültiger Email oder Passwort");
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenModel = new RefreshToken
        {
            Token = refreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            User = user,
            IsRevoked = false
        };
        await _refreshTokenRepository.AddNewTokenAsync(refreshTokenModel);
        await _refreshTokenRepository.SaveChangesAsync();

        var authResponseDto = new AuthResponseDto(refreshToken, accessToken);
        return Ok(authResponseDto);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh(RefreshDto refreshDto)
    {
        var existRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshDto.RefreshToken);
        if (existRefreshToken == null || existRefreshToken.ExpirationDate < DateTime.UtcNow || existRefreshToken.IsRevoked == true)
        {
            return Unauthorized();
        }

        var accessToken = _tokenService.GenerateAccessToken(existRefreshToken.User);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenModel = new RefreshToken
        {
            Token = refreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            User = existRefreshToken.User,
            IsRevoked = false
        };
        existRefreshToken.IsRevoked = true;
        await _refreshTokenRepository.AddNewTokenAsync(refreshTokenModel);
        await _refreshTokenRepository.SaveChangesAsync();

        var authResponseDto = new AuthResponseDto(refreshToken, accessToken);
        return Ok(authResponseDto);
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout(RefreshDto refreshDto)
    {
        var existRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshDto.RefreshToken);
        if (existRefreshToken != null)
        {
            existRefreshToken.IsRevoked = true;
            await _refreshTokenRepository.SaveChangesAsync();
        }

        return NoContent();
    }

    [HttpPost("forgot-password")]
    [EnableRateLimiting("forgot")]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var email = forgotPasswordDto.Email.NormalizeEmail();
        var now = DateTime.UtcNow;

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return NoContent();
        }

        var latestResetCode = await _passwordResetTokenRepository.GetLatestUnusedAsync(user.Id);
        if (latestResetCode != null && now - latestResetCode.CreatedAt < PasswordResetPolicy.MinRequestInterval)
        {
            return NoContent();
        }

        if (latestResetCode != null)
        {
            latestResetCode.IsUsed = true;
        }

        var resetCode = new PasswordResetToken
        {
            Code = _tokenService.GenerateResetCode(),
            User = user,
            CreatedAt = now,
            ExpirationDate = now + PasswordResetPolicy.TokenLifetime,
        };

        await _passwordResetTokenRepository.AddAsync(resetCode);
        await _passwordResetTokenRepository.SaveChangesAsync();

        await _emailService.SendEmailAsync(email, "Passwort zurücksetzen", $"Ihr Passwort-Zurücksetzungs-Code lautet: {resetCode.Code}, Sie haben {(int)PasswordResetPolicy.TokenLifetime.TotalMinutes} Minuten Zeit, um Ihr Passwort zurückzusetzen.");
        return NoContent();
    }

    [HttpPost("reset-password")]
    [EnableRateLimiting("reset")]
    public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var email = resetPasswordDto.Email.NormalizeEmail();
        var now = DateTime.UtcNow;

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return BadRequest("Ungültiger Code oder Email");
        }

        var latestResetCode = await _passwordResetTokenRepository.GetLatestUnusedAsync(user.Id);
        if (latestResetCode == null || latestResetCode.ExpirationDate < now || latestResetCode.FailedAttempts >= PasswordResetPolicy.MaxFailedAttempts)
        {
            return BadRequest("Ungültiger Code oder Email");
        }

        var entered = Encoding.UTF8.GetBytes(resetPasswordDto.Code.Trim().ToUpperInvariant());
        var stored = Encoding.UTF8.GetBytes(latestResetCode.Code);
        bool isMatch = CryptographicOperations.FixedTimeEquals(entered, stored);

        if (!isMatch)
        {
            latestResetCode.FailedAttempts++;
            await _passwordResetTokenRepository.SaveChangesAsync();
            return BadRequest("Ungültiger Code oder Email");
        }

        await _refreshTokenRepository.RevokeAllForUserAsync(user.Id);

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, resetPasswordDto.NewPassword);
        latestResetCode.IsUsed = true;
        await _userRepository.SaveChangesAsync();
        return NoContent();
    }
}
