using Demo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrimeController : ControllerBase
{
    private readonly IPrimeService _primeService;

    public PrimeController(IPrimeService primeService)
    {
        _primeService = primeService;
    }

    [HttpGet("isprime")]
    public IActionResult IsPrime([FromQuery] int n)
    {
        var result = _primeService.IsPrime(n);
        return Ok(result);
    }

    [HttpGet("next")]
    public IActionResult NextPrime([FromQuery] int n)
    {
        var result = _primeService.NextPrime(n);
        return Ok(result);
    }
}

