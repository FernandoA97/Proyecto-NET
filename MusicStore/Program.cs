using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.DataAccess.Repositories;
using MusicStore.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MusicStoreDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapGet("api/Genres", (MusicStoreDbContext context) => {
    var repository = new GenreRepository(context);

    return repository.List();
});

app.MapPost("api/Genres", (MusicStoreDbContext context, Genre entity) => {

    var repository = new GenreRepository(context);

    repository.Add(entity);

    return Results.Ok();
});


app.Run();



