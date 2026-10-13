using AnyCard.Application.Common;
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
    private readonly ICardRepository _cardRepository;
    private readonly ICategoryRepository _categoryRepository;
    private const int PageSize = 9;
    public ProgressController(ICardProgressRepository cardProgressRepository, ICardRepository cardRepository, ICategoryRepository categoryRepository)
    {
        _progressRepository = cardProgressRepository;
        _cardRepository = cardRepository;
        _categoryRepository = categoryRepository;
    }

    [HttpPut]
    public async Task<ActionResult> ReviewCard(ReviewCardDto reviewCardDto)
    {
        var userId = User.GetUserId();
        var cardProgress = await _progressRepository.GetByUserAndCardAsync(userId, reviewCardDto.CardId);
        if (cardProgress == null)
        {
            return NotFound("Die Karte wurde nicht gefunden");
        }

        TimeSpan interval;
        try
        {
            interval = SpacedRepetitionCalculator.CalculateNextReview(reviewCardDto.UserRating, cardProgress.ViewCounter);
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Falsche Parameter");
        }

        cardProgress.UserRating = reviewCardDto.UserRating;
        cardProgress.NextShowtime = DateTime.UtcNow + interval;
        cardProgress.ViewCounter = reviewCardDto.UserRating == UserRating.Again ? 0 : cardProgress.ViewCounter + 1;

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

    [HttpGet("summary")]
    public async Task<ActionResult<ProgressSummaryDto>> GetSummary()
    {
        var userId = User.GetUserId();
        var cardCount = await _cardRepository.GetCardsCountAsync(userId);
        var categoryCount = await _categoryRepository.GetCategoriesCountAsync(userId);
        var cardForReviewCount = await _progressRepository.GetCountForReviewAsync(userId);

        var progressSummaryDto = new ProgressSummaryDto(cardCount, categoryCount, cardForReviewCount);
        return Ok(progressSummaryDto);
    }

    [HttpGet("overview")]
    public async Task<ActionResult<PagedResult<CardOverviewDto>>> GetOverview([FromQuery] int? categoryId, [FromQuery] UserRating? userRating, [FromQuery] int pageNumber = 1)
    {
        var userId = User.GetUserId();
        var result = await _progressRepository.GetPagedByUserAsync(userId, categoryId, userRating, pageNumber, PageSize);
        var resultDto = result.Items.Select(cr => new CardOverviewDto(cr.CardId, cr.Card.Category.Name, cr.Card.Question, cr.UserRating,
            cr.ViewCounter, cr.NextShowtime)).ToList();
        return new PagedResult<CardOverviewDto>(resultDto, result.TotalCount, result.PageNumber, result.PageSize);
    }
}
