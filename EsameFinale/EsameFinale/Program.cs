using EsameFinale.Models;
using System.Diagnostics.Eventing.Reader;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var canzone1 = new Canzone { 
    IdCanzone = 1, 
    Durata = 210, 
    Titolo = "La sera dei miracoli", 
    NomeAutore = "Lucio", 
    CognomeAutore = "Battisti" };
var canzone2 = new Canzone { 
    IdCanzone = 2, 
    Durata = 185, 
    Titolo = "Bocca di rosa", 
    NomeAutore = "Fabrizio", 
    CognomeAutore = "De André" };
var canzone3 = new Canzone { 
    IdCanzone = 3, 
    Durata = 240, 
    Titolo = "Blu" };

// Creazione di un autore con la sua lista di canzoni
var playlist = new Playlist
{
    IdPlaylist = 100,
    Titolo = "I classici italiani",
    Autore = "Mario Rossi",
    Elenco = new List<Canzone> { canzone1, canzone2, canzone3 }
};


app.MapGet("/api/vedi/playlist", () =>
{
    return Results.Ok(playlist);
})
.WithOpenApi();

app.MapPost("/api/aggiungi/canzone", (Canzone c) =>
{
    if (string.IsNullOrWhiteSpace(c.Titolo) || c.Durata == 0)
        return Results.BadRequest();
    c.IdCanzone = playlist.Elenco.Count + 1;
    playlist.Elenco.Add(c);
    return Results.Ok(playlist);

})
.WithOpenApi();


app.Run();
