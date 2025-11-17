using Demo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MathController : ControllerBase
{
    private readonly IMathService _mathService;

    public MathController(IMathService mathService)
    {
        _mathService = mathService;
    }

    [HttpGet("add")]
    public IActionResult Add([FromQuery] int a, [FromQuery] int b)
    {
        var result = _mathService.Add(a, b);
        return Ok(result);
    }

    [HttpGet("factorial")]
    public IActionResult Factorial([FromQuery] int n)
    {
        try
        {
            var result = _mathService.Factorial(n);
            return Ok(result);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

