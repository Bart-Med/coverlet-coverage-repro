using Demo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TextController : ControllerBase
{
    private readonly ITextService _textService;

    public TextController(ITextService textService)
    {
        _textService = textService;
    }

    [HttpGet("reverse")]
    public IActionResult Reverse([FromQuery] string? value)
    {
        var result = _textService.Reverse(value);
        return Ok(result);
    }

    [HttpGet("vowels")]
    public IActionResult CountVowels([FromQuery] string? value)
    {
        var result = _textService.CountVowels(value);
        return Ok(result);
    }
}

