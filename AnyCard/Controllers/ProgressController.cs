using AnyCard.Application.Extensions;
using AnyCard.Application.Interfaces;
using AnyCard.Application.Services;
using AnyCard.Domain.Enums;
using AnyCard.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnyCard.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class ProgressController : ControllerBase
{
    private readonly ICardProgressRepository _progressRepository;
    public ProgressController(ICardProgressRepository cardProgressRepository)
    {
        _progressRepository = cardProgressRepository;
    }

    [HttpPut]
    public async Task<ActionResult> ReviewCard(ReviewCardDto reviewCardDto)
    {
        var userId = User.GetUserId();
        var cardProgress = await _progressRepository.GetByUserAndCardAsync(userId, reviewCardDto.CardId);
        if(cardProgress == null)
        {
            return NotFound("Die Karte wurde nicht gefunden");
        }

        TimeSpan interval;
        try
        {
            interval = SpacedRepetitionCalculator.CalculateNextReview(reviewCardDto.UserRating, cardProgress.ViewCounter);
        }
        catch(ArgumentOutOfRangeException)
        {
            return BadRequest("Falsche Parameter");
        }
            
        cardProgress.UserRating = reviewCardDto.UserRating;
        cardProgress.NextShowtime = DateTime.UtcNow + interval;
        cardProgress.ViewCounter = reviewCardDto.UserRating == UserRating.Again ? 0 : cardProgress.ViewCounter +1;

        await _progressRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CardDto>>> GetDueForReview([FromQuery] int? categoryId)
    {
        var userId = User.GetUserId();
        var cardProgresses = await _progressRepository.GetDueForReviewAsync(userId, categoryId);
        var cardDtos = cardProgresses.Select(c => new CardDto(c.Card.Id, c.Card.Question, c.Card.Answer, c.Card.Category.Name)).ToList();

        return Ok(cardDtos);
    }
}
