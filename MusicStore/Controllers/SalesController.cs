using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Services.Interfaces;

namespace MusicStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _service;

    public SalesController(ISaleService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSaleAsync(string email, [FromBody] SaleDtoRequest request)
    {
        var response = await _service.CreateSaleAsync(email, request);

        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
}