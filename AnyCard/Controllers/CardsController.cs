using AnyCard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AnyCard.Application.Extensions;

namespace AnyCard.Controllers;

public class CardsController : ControllerBase
{
    private readonly ICardRepository _cardRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CardsController(ICategoryRepository categoryRepository, ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
        _categoryRepository = categoryRepository;
    }


}
