using Microsoft.Extensions.Logging;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Services.Interfaces;

namespace MusicStore.Services.Implementations;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _repository;
    private readonly ILogger<GenreService> _logger;

    public GenreService(IGenreRepository repository, ILogger<GenreService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<BaseResponseGeneric<IEnumerable<GenreDtoResponse>>> ListAsync()
    {
        var response = new BaseResponseGeneric<IEnumerable<GenreDtoResponse>>();

        try
        {
            var collection = await _repository.ListAsync();

            response.Data = collection.Select(x => new GenreDtoResponse
            {
                Id = x.Id,
                Name = x.Name,
                Status = x.Status
            });
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in GenreService.ListAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Ocurrio un error al listar Generos";
        }

        return response;
    }

    public async Task<BaseResponseGeneric<GenreDtoResponse>> FindByIdAsync(int id)
    {
        var response = new BaseResponseGeneric<GenreDtoResponse>();

        try
        {
            var entity = await _repository.FindByIdAsync(id);

            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el Genero";
                return response;
            }

            response.Data = new GenreDtoResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status
            };
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in GenreService.FindByIdAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Ocurrio un error al buscar el Genero";
        }

        return response;
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(GenreDtoRequest request)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            var entity = new Genre
            {
                Name = request.Name,
                Status = request.Status
            };

            var id = await _repository.AddAsync(entity);

            response.Data = id;
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in GenreService.AddAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Ocurrio un error al agregar el Genero";
        }

        return response;
    }

    public async Task<BaseResponse> UpdateAsync(int id, GenreDtoRequest request)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _repository.FindByIdAsync(id);

            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el Genero";
                return response;
            }

            entity.Name = request.Name;
            entity.Status = request.Status;

            await _repository.UpdateAsync(entity);

            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in GenreService.UpdateAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Ocurrio un error al actualizar el Genero";
        }

        return response;
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _repository.FindByIdAsync(id);

            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el Genero";
                return response;
            }

            await _repository.DeleteAsync(entity.Id);

            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in GenreService.DeleteAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Ocurrio un error al eliminar el Genero";
        }

        return response;
    }
}