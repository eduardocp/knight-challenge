using Knight.Application.Services;
using Knight.Domain.Interfaces;
using Knight.Domain.Models.Dto.Results;
using Knight.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(opt => opt.UseMongoDB(builder.Configuration["MONGODB_URI"] ?? "", builder.Configuration["MONGODB_DATABASE"] ?? ""));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IKnightService, KnightService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Policy", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

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
        if (filter == "heroes")
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
.WithOrder(1)
.Produces<List<KnightResult>>()
.WithOpenApi();

app.MapGet("/knights/{id}", async (IKnightService service, [FromRoute] string id) =>
{
    return Results.Ok(await service.GetKnight(id));
})
.Produces<KnightResult>()
.WithOpenApi();

app.MapPost("/knights", async (IKnightService service, [FromBody] Knight.Domain.Models.Dto.Datas.KnightData knight) =>
{
    try
    {
        return Results.Ok(await service.AddKnight(knight));
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
})
.WithOrder(2)
.WithOpenApi();

app.MapPatch("/knights/{id}", async (IKnightService service, [FromRoute] string id, [FromBody] Knight.Domain.Models.Dto.Datas.KnightData knight) =>
{
    try
    {
        return Results.Ok(await service.UpdateKnight(id, knight));
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
})
.WithOrder(3)
.WithOpenApi();

app.MapDelete("/knights/{id}", async (IKnightService service, [FromRoute] string id) =>
{
    await service.RemoveKnight(id);
    return Results.Ok();
})
.WithOrder(4)
.WithOpenApi();

app.UseCors("Policy");

app.Run();