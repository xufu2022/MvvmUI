using GloboTicket.Admin.API.DTO;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

#region Data

var events = new List<EventDto>
{
    new()
    {
        Id = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
        Name = "John Egbert Live",
        Price = 65,
        Date = DateTime.Now.AddMonths(6),
        Artists = new List<string> { "John Egbert", "Jane Egbert" },
        Status = EventStatus.OnSale,
        Description = "Join John for his farewell tour across 15 continents. John really needs no introduction since he has already mesmerized the world with his banjo.",
        ImageUrl = "https://lindseybroospluralsight.blob.core.windows.net/globoticket/images/banjo.jpg",
        Category = new CategoryDto
        {
            Id = Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"),
            Name = "Concert"
        }
    },
    new()
    {
        Id = Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}"),
        Name = "The State of Affairs: Michael Live!",
        Price = 85,
        Artists = new List<string> { "Michael Johnson" },
        Status = EventStatus.OnSale,
        Date = DateTime.Now.AddMonths(9),
        Description = "Michael Johnson doesn't need an introduction. His 25 concert across the globe last year were seen by thousands. Can we add you to the list?",
        ImageUrl = "https://lindseybroospluralsight.blob.core.windows.net/globoticket/images/michael.jpg",
        Category = new CategoryDto
        {
            Id = Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"),
            Name = "Concert"
        }
    },
    new()
    {
        Id = Guid.Parse("{B419A7CA-3321-4F38-BE8E-4D7B6A529319}"),
        Name = "Clash of the DJs",
        Price = 85,
        Artists = new List<string> { "DJ 'The Mike'" },
        Status = EventStatus.SalesClosed,
        Date = DateTime.Now.AddMonths(4),
        Description = "DJs from all over the world will compete in this epic battle for eternal fame.",
        ImageUrl = "https://lindseybroospluralsight.blob.core.windows.net/globoticket/images/dj.jpg",
        Category = new CategoryDto
        {
            Id = Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"),
            Name = "Concert"
        }
    },
    new()
    {
        Id = Guid.Parse("{62787623-4C52-43FE-B0C9-B7044FB5929B}"),
        Name = "Spanish guitar hits with Manuel",
        Price = 25,
        Artists = new List<string> { "Manuel Santinonisi" },
        Status = EventStatus.OnSale,
        Date = DateTime.Now.AddMonths(4),
        Description = "Get on the hype of Spanish Guitar concerts with Manuel.",
        ImageUrl = "https://lindseybroospluralsight.blob.core.windows.net/globoticket/images/guitar.jpg",
        Category = new CategoryDto
        {
            Id = Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"),
            Name = "Concert"
        }
    },
    new()
    {
        Id = Guid.Parse("{1BABD057-E980-4CB3-9CD2-7FDD9E525668}"),
        Name = "Techorama",
        Price = 500,
        Artists = new List<string> { "Mister Maui", "Lady C#", "many more" },
        Status = EventStatus.AlmostSoldOut,
        Date = DateTime.Now.AddMonths(10),
        Description = "The best tech conference in the world",
        ImageUrl = "https://lindseybroospluralsight.blob.core.windows.net/globoticket/images/conf.jpg",
        Category = new CategoryDto()
        {
            Id = Guid.Parse("FE98F549-E790-4E9F-AA16-18C2292A2EE9"),
            Name = "Conference"
        }
    },
    new()
    {
        Id = Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"),
        Name = "To the Moon and Back",
        Price = 135,
        Artists = new List<string> { "Nick Sailor" },
        Date = DateTime.Now.AddMonths(8),
        Description = "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
        ImageUrl = "https://lindseybroospluralsight.blob.core.windows.net/globoticket/images/musical.jpg",
        Category = new CategoryDto
        {
            Id = Guid.Parse("6313179F-7837-473A-A4D5-A5571B43E6A6"),
            Name = "Musical"
        }
    }
};

var categories = new List<CategoryDto>
{
    new()
    {
        Id = Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"),
        Name = "Concert"
    },
    new()
    {
        Id = Guid.Parse("FE98F549-E790-4E9F-AA16-18C2292A2EE9"),
        Name = "Conference"
    },
    new()
    {
        Id = Guid.Parse("6313179F-7837-473A-A4D5-A5571B43E6A6"),
        Name = "Musical"
    }
};

#endregion

app.MapGet("/events", () 
    => Results.Ok(events));

app.MapGet("/categories", ()
    => Results.Ok(categories));

app.MapGet("/events/{id}", (Guid id) =>
{
    var @event = events.Find(e => e.Id == id);
    return @event is null
        ? Results.NotFound()
        : Results.Ok(@event);
});

app.MapPost("/events", (EventDto @event) =>
{
    if (!MiniValidator.TryValidate(@event, out var errors))
        return Results.ValidationProblem(errors);

    if (!events.All(e => e.Name != @event.Name && e.Date != @event.Date))
        return Results.BadRequest($"An event {@event.Name} on {@event.Date.ToShortDateString()} already exists.");

    @event.Id = Guid.NewGuid();
    events.Add(@event);
    return Results.Created($"/events/{@event.Id}", @event);
});

app.MapPut("/events/{id}", (Guid id, EventDto @event) =>
{
    if (!MiniValidator.TryValidate(@event, out var errors))
        return Results.ValidationProblem(errors);

    var eventToUpdate = events.Find(e => e.Id == @event.Id);

    if (eventToUpdate is null) return Results.NotFound();

    eventToUpdate.Name = @event.Name;
    eventToUpdate.Category = @event.Category;
    eventToUpdate.Date = @event.Date;
    eventToUpdate.Artists = @event.Artists;
    eventToUpdate.Description = @event.Description;
    eventToUpdate.ImageUrl = @event.ImageUrl;
    eventToUpdate.Price = @event.Price;
    eventToUpdate.Status = @event.Status;

    return Results.NoContent();
});

app.MapPatch("/events/{id}/status", (Guid id, [FromBody] EventStatus status) =>
{
    var eventToUpdate = events.Find(e => e.Id == id);

    if (eventToUpdate is null) return Results.NotFound();

    eventToUpdate.Status = status;

    return Results.NoContent();
});

app.MapDelete("/events/{id}", (Guid id) =>
{
    if (events.Find(e => e.Id == id) is not EventDto @event)
        return Results.NotFound();

    events.Remove(@event);
    return Results.NoContent();
});

app.Run();
