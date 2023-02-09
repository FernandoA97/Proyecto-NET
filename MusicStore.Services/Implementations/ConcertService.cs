using Microsoft.Extensions.Logging;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Services.Interfaces;

namespace MusicStore.Services.Implementations;

public class ConcertService : IConcertService
{
    private readonly IConcertRepository _concertRepository;
    private readonly ILogger<ConcertService> _logger;

    public ConcertService(IConcertRepository concertRepository, ILogger<ConcertService> logger)
    {
        _concertRepository = concertRepository;
        _logger = logger;
    }

    public async Task<BaseResponseGeneric<ICollection<ConcertDtoResponse>>> ListAsync(string? filter, int page, int rows)
    {
        var response = new BaseResponseGeneric<ICollection<ConcertDtoResponse>>();
        try
        {
            var concerts = await _concertRepository.ListAsync(filter, page, rows);
            response.Data = concerts.Select(p => new ConcertDtoResponse
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                DateEvent = p.DateEvent,
                TimeEvent = p.TimeEvent,
                Genre = p.Genre,
                ImageUrl = p.ImageUrl,
                UnitPrice = p.UnitPrice,
                TicketsQuantity = p.TicketsQuantity
            }).ToList();
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar los conciertos {message}", ex.Message);
            response.ErrorMessage = "Error al listar los conciertos";
        }

        return response;
    }

    public Task<BaseResponseGeneric<ConcertSingleDtoResponse>> FindByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(ConcertDtoRequest request)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            var concert = new Concert
            {
                Title = request.Title,
                Description = request.Description,
                DateEvent = Convert.ToDateTime($"{request.DateEvent} {request.TimeEvent}"),
                GenreId = request.IdGenre,
                UnitPrice = request.UnitPrice,
                TicketsQuantity = request.TicketsQuantity,
                Place = request.Place,
            };
            await _concertRepository.AddAsync(concert);
            response.Data = concert.Id;
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar el concierto {message}", ex.Message);
            response.ErrorMessage = "Error al agregar el concierto";
        }

        return response;
    }

    public Task<BaseResponse> UpdateAsync(int id, ConcertDtoRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse> FinalizeAsync(int id)
    {
        throw new NotImplementedException();
    }
}