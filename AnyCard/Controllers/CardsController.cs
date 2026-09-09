using AnyCard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AnyCard.Application.Extensions;
using AnyCard.DTOs;
using AnyCard.Domain.Model;
using AnyCard.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AnyCard.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class CardsController : ControllerBase
{
    private readonly ICardRepository _cardRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICardProgressRepository _progressRepository;

    public CardsController(ICategoryRepository categoryRepository, ICardRepository cardRepository, ICardProgressRepository cardProgressRepository)
    {
        _cardRepository = cardRepository;
        _categoryRepository = categoryRepository;
        _progressRepository = cardProgressRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CardDto>>> GetCards()
    {
        var userId = User.GetUserId();
        var cards = await _cardRepository.GetAllAsync(userId);

        var cardsDto = cards.Select(c => new CardDto(c.Id, c.Question, c.Answer, c.Category.Name)).ToList();
        return Ok(cardsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CardDto>> GetCardById(int id)
    {
        var userId = User.GetUserId();
        var card = await _cardRepository.GetByIdAsync(id, userId);
        if(card == null)
        {
            return NotFound();
        }

        var cardDto = new CardDto(card.Id, card.Question,card.Answer, card.Category.Name);
        return Ok(cardDto);
    }

    [HttpPost]
    public async Task<ActionResult<CardDto>> CreateCard(CreateCardDto createCardDto)
    {
        var userId = User.GetUserId();
        var category = await _categoryRepository.GetByIdAsync(createCardDto.CategoryId);
        if(category == null)
        {
            return NotFound();
        }

        var card = new Card
        {
            Question = createCardDto.Question,
            Answer = createCardDto.Answer,
            CategoryId = createCardDto.CategoryId,
            UserId = userId
        };
        await _cardRepository.AddAsync(card);
        await _cardRepository.SaveChangesAsync();

        var cardProgress = new CardProgress
        {
            CardId = card.Id,
            UserId = userId,
            NextShowtime = DateTime.UtcNow,
            UserRating = UserRating.Again,
            ViewCounter = 0,
        };
        await _progressRepository.AddAsync(cardProgress);
        await _progressRepository.SaveChangesAsync();

        var cardDto = new CardDto(card.Id, card.Question, card.Answer, category.Name);
        return CreatedAtAction(nameof(GetCardById), new { id = card.Id }, cardDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CardDto>> UpdateCard(int id, CreateCardDto createCardDto)
    {
        var userId = User.GetUserId();

        var card = await _cardRepository.GetByIdAsync(id, userId);
        if (card == null)
        {
            return NotFound("Die Karte wurde nicht gefunden");
        }
        
        var category = await _categoryRepository.GetByIdAsync(createCardDto.CategoryId);
        if (category == null)
        {
            return NotFound("Die Kategorie wurde nicht gefunden");
        }

        card.Question = createCardDto.Question;
        card.Answer = createCardDto.Answer;
        card.CategoryId = createCardDto.CategoryId;
        await _cardRepository.SaveChangesAsync();

        var cardDto = new CardDto(card.Id, card.Question, card.Answer, category.Name);
        return Ok(cardDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCard(int id)
    {
        var userId = User.GetUserId();
        var card = await _cardRepository.GetByIdAsync(id,userId);
        if (card == null)
        {
            return NotFound("Die Karte wurde nicht gefunden");
        }

        _cardRepository.Delete(card);
        await _cardRepository.SaveChangesAsync();
        return NoContent();
    }
}
