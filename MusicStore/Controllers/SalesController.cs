using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Services.Interfaces;

namespace MusicStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _service;
    private readonly ILogger<SalesController> _logger;

    public SalesController(ISaleService service, ILogger<SalesController> logger)
    {
        _service = service;
        _logger = logger;
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

    [HttpGet("ListSales")]
    public async Task<IActionResult> GetListSales(string email, string? filter, int page = 1, int rows = 10)
    {
        var response = await _service.ListAsync(email, filter, page, rows);

        if (response.Success)
        {
            return Ok(response);
        }

        return NotFound(response);
    }

    [HttpGet("ListSalesByDate")]
    public async Task<IActionResult> GetListSalesByDate(string dateStart, string dateEnd, int page = 1, int rows = 10)
    {
        try
        {
            var response = await _service.ListAsync(DateTime.Parse(dateStart), DateTime.Parse(dateEnd), page, rows);

            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
        catch (FormatException ex)
        {
            _logger.LogWarning(ex, "Error en conversion de formato de fecha {message}", ex.Message);
            return BadRequest(new BaseResponse { ErrorMessage = "Error en conversion de formato de fecha" });
        }
    }

}