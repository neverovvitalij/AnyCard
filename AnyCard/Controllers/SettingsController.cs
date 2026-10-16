using AnyCard.Application.Extensions;
using AnyCard.Application.Interfaces;
using AnyCard.Domain.Common;
using AnyCard.Domain.Model;
using AnyCard.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnyCard.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly IUserSettingsRepository _userSettingsRepository;
    public SettingsController(IUserSettingsRepository userSettingsRepository)
    {
        _userSettingsRepository = userSettingsRepository;
    }

    [HttpGet]
    public async Task<ActionResult<UserSettingsDto>> GetSettings()
    {
        var userId = User.GetUserId();
        var response = await _userSettingsRepository.GetByUserIdAsync(userId);

        return Ok(new UserSettingsDto(
            response?.AgainInterval ?? ReviewIntervalDefaults.Again,
            response?.HardInterval ?? ReviewIntervalDefaults.Hard,
            response?.GoodInterval ?? ReviewIntervalDefaults.Good,
            response?.EasyInterval ?? ReviewIntervalDefaults.Easy
            ));
    }

    [HttpPut]
    public async Task<ActionResult<UserSettingsDto>> ChangeUserSettings(UserSettingsDto userSettingsDto)
    {
        var userId = User.GetUserId();
        var userSettings = await _userSettingsRepository.GetByUserIdAsync(userId);

        if (userSettings != null)
        {
            userSettings.AgainInterval = userSettingsDto.Again;
            userSettings.HardInterval = userSettingsDto.Hard;
            userSettings.GoodInterval = userSettingsDto.Good;
            userSettings.EasyInterval = userSettingsDto.Easy;
        }
        else
        {
            await _userSettingsRepository.AddAsync(new UserSettings
            {
                UserId = userId,
                AgainInterval = userSettingsDto.Again,
                HardInterval = userSettingsDto.Hard,
                GoodInterval = userSettingsDto.Good,
                EasyInterval = userSettingsDto.Easy
            });
        }

        await _userSettingsRepository.SaveChangesAsync();

        return Ok(userSettingsDto);
    }
}
