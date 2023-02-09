using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Dto.Request;
using MusicStore.Repositories;
using MusicStore.Services.Implementations;
using MusicStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MusicStoreDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IGenreRepository, GenreRepository>();
builder.Services.AddTransient<IGenreService, GenreService>();

builder.Services.AddTransient<IConcertRepository, ConcertRepository>();
builder.Services.AddTransient<IConcertService, ConcertService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("api/Genres", async (IGenreService service) => await service.ListAsync());


// GET api/Genres/5
app.MapGet("api/Genres/{id:int}", async (IGenreService service, int id) =>
{
    var response = await service.FindByIdAsync(id);

    return response.Success ? Results.Ok(response) : Results.NotFound(response);

});

app.MapPost("api/Genres", async (IGenreService service, GenreDtoRequest request) =>
{
    var response = await service.AddAsync(request);

    return response.Success ? Results.Ok(response) : Results.BadRequest(response);
});

app.MapPut("api/Genres/{id:int}", async (IGenreService service, int id, GenreDtoRequest request) =>
{
    var response = await service.UpdateAsync(id, request);

    return response.Success ? Results.Ok(response) : Results.BadRequest(response);
});

app.MapDelete("api/Genres/{id:int}", async (IGenreService service, int id) =>
{
    var response = await service.DeleteAsync(id);

    return response.Success ? Results.Ok(response) : Results.NotFound(response);
});

app.MapPost("api/Concerts", async (IConcertService service, ConcertDtoRequest request) =>
{
    var response = await service.AddAsync(request);

    return response.Success ? Results.Ok(response) : Results.BadRequest(response);
});

app.MapGet("api/Concerts", async (IConcertService service, string? filter, int page, int rows) =>
{
    var response = await service.ListAsync(filter, page, rows);

    return response.Success ? Results.Ok(response) : Results.NotFound(response);
});


app.Run();



