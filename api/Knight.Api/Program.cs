using Knight.Application.Services;
using Knight.Domain.Interfaces;
using Knight.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(opt => opt.UseNpgsql(builder.Configuration["Data:KnightConnection"]));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IKnightService, KnightService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/knights", async (IKnightService service, [FromQuery(Name = "filter")] string? filter) =>
{
    if (!string.IsNullOrEmpty(filter))
    {
        if (filter == "knightes")
        {
            return Results.Ok(await service.GetKnights(true));
        }
        else
        {
            return Results.NotFound();
        }
    }

    return Results.Ok(await service.GetKnights(false));
})
.Produces<List<Knight.Domain.Models.Dto.KnightDto>>()
.WithOpenApi();

app.MapGet("/knights/{id}", async (IKnightService service, [FromRoute] int id) =>
{
    return Results.Ok(await service.GetKnight(id));
})
.Produces<Knight.Domain.Models.Dto.KnightDto>()
.WithOpenApi();

app.MapPost("/knights", async (IKnightService service, [FromBody] Knight.Domain.Models.Knight knight) =>
{
    await service.AddKnight(knight);
    return Results.Ok();
})
.WithOpenApi();

app.MapPatch("/knights/{id}", async (IKnightService service, [FromRoute] int id, [FromBody] Knight.Domain.Models.Knight knight) =>
{
    await service.UpdateKnight(id, knight);
    return Results.Ok();
})
.WithOpenApi();

app.MapDelete("/knights/{id}", async (IKnightService service, [FromRoute] int id) =>
{
    await service.RemoveKnight(id);
    return Results.Ok();
})
.WithOpenApi();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<AppDbContext>();
        
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();