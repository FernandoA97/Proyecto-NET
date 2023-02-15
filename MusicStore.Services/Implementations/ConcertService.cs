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

    public async Task<BaseResponseGeneric<ConcertSingleDtoResponse>> FindByIdAsync(int id)
    {
        var response = new BaseResponseGeneric<ConcertSingleDtoResponse>();

        try
        {
            var concert = await _concertRepository.FindByIdAsync(id);

            if (concert == null)
            {
                response.ErrorMessage = "No se encontró el concierto";
                return response;
            }

            response.Data = new ConcertSingleDtoResponse
            {
                Id = concert.Id,
                Title = concert.Title,
                Description = concert.Description,
                DateEvent = concert.DateEvent.ToString("yyyy-MM-dd"),
                TimeEvent = concert.DateEvent.ToString("HH:mm"),
                ImageUrl = concert.ImageUrl,
                Place = concert.Place,
                TicketsQuantity = concert.TicketsQuantity,
                UnitPrice = concert.UnitPrice,
                Status = concert.Status ? "Activo" : "Inactivo",
                GenreDtoResponse = new GenreDtoResponse
                {
                    Id = concert.Genre.Id,
                    Name = concert.Genre.Name,
                    Status = concert.Genre.Status
                }
            };
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el concierto");
            response.ErrorMessage = "Error al obtener el concierto";
        }

        return response;
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

    public async Task<BaseResponse> UpdateAsync(int id, ConcertDtoRequest request)
    {
        var response = new BaseResponse();

        try
        {
            var concert = await _concertRepository.FindByIdAsync(id);

            if (concert == null)
            {
                response.ErrorMessage = "No se encontró el concierto";
                return response;
            }

            concert.Title = request.Title;
            concert.Description = request.Description;
            concert.DateEvent = Convert.ToDateTime($"{request.DateEvent} {request.TimeEvent}");
            concert.GenreId = request.IdGenre;
            concert.UnitPrice = request.UnitPrice;
            concert.TicketsQuantity = request.TicketsQuantity;
            concert.Place = request.Place;

            await _concertRepository.UpdateAsync();
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el concierto {message}", ex.Message);
            response.ErrorMessage = "Error al actualizar el concierto";
        }

        return response;
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        var response = new BaseResponse();

        try
        {
            await _concertRepository.DeleteAsync(id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el concierto");
            response.ErrorMessage = "Error al eliminar el concierto";
        }

        return response;
    }

    public async Task<BaseResponse> FinalizeAsync(int id)
    {
        var response = new BaseResponse();

        try
        {
            await _concertRepository.FinalizeAsync(id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al finalizar el concierto");
            response.ErrorMessage = "Error al finalizar el concierto";
        }

        return response;
    }
}