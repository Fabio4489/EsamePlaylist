using EsameFinale.Context;
using EsameFinale.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Aggiunge i servizi per Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Configurazione del database SQL Lite
builder.Services.AddDbContext<LibreriaContext>(
        options => options.UseSqlite("Data Source=Libreria.db")
    );

builder.Services.AddCors();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Se non esiste il database, crealo!
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibreriaContext>();
    db.Database.EnsureCreated();
}
#endregion

#region Get tutte: 
app.MapGet("/api/read/playlist", async (LibreriaContext db) =>
{
    var tuttePlaylist = await db.Playlists
        .Include(p => p.Elenco)
        .ToListAsync();

    var result = tuttePlaylist.Select(p => new
    {
        Titolo = p.Titolo,
        Autore = p.Autore,
        Elenco = p.Elenco.Select(c => new
        {
            Titolo = c.Titolo,
            Durata = c.Durata,
            Nome = c.NomeAutore,
            Cognome = c.CognomeAutore
        }).ToList()
    });

return Results.Ok(result);
})
.WithOpenApi();

app.MapGet("/api/read/songs", async (LibreriaContext db) =>
{
    var elenco = await db.Canzoni.ToListAsync();

    var result = elenco.Select(c => new
    {
        Titolo = c.Titolo,
        Durata = c.Durata,
        Nome = c.NomeAutore,
        Cognome = c.CognomeAutore
    });

    return Results.Ok(result);
})
.WithOpenApi();
#endregion

#region Get solo una:

app.MapGet("/api/read/playlist/{id}", async (int id, LibreriaContext db) =>
{
    var playlist = await db.Playlists
            .Include(p => p.Elenco)
            .FirstOrDefaultAsync(p => p.IdPlaylist == id);

    if (playlist != null)
    {
        var result = new
        {
            Titolo = playlist.Titolo,
            Autore = playlist.Autore,
            Elenco = playlist.Elenco.Select(c => new
            {
                Titolo = c.Titolo,
                Durata = c.Durata,
                Nome = c.NomeAutore,
                Cognome = c.CognomeAutore
            }).ToList()
        };

        return Results.Ok(result);
    }
    return Results.NotFound();

})
.WithOpenApi();

app.MapGet("/api/read/song/{id}", async (int id, LibreriaContext db) =>
{
    Canzone? c = await db.Canzoni.FindAsync(id);
    if (c != null)
    {
        var result = new
        {
            Titolo = c.Titolo,
            Durata = c.Durata,
            Nome = c.NomeAutore,
            Cognome = c.CognomeAutore
        };

        return Results.Ok(result);
    }
    return Results.NotFound();
})
.WithOpenApi();

#endregion

#region Post:
app.MapPost("/api/create/playlist", async (LibreriaContext db, Playlist p) =>
{
    db.Playlists.Add(p);
    await db.SaveChangesAsync();
    if (p != null)
    {
        var result = new
        {
            Id = p.IdPlaylist,
            Titolo = p.Titolo,
            Autore = p.Autore,
            Elenco = p.Elenco.Select(c => new
            {
                Titolo = c.Titolo,
                Durata = c.Durata,
                Nome = c.NomeAutore,
                Cognome = c.CognomeAutore
            }).ToList()
        };
        return Results.Created($"/api/read/onePlaylist/{result.Id}", result);
    }

    return Results.NotFound();

})
.WithOpenApi();

app.MapPost("/api/create/song", async (LibreriaContext db, Canzone c)  =>
{
    db.Canzoni.Add(c);
    await db.SaveChangesAsync();
    var result = new
    {
        Id = c.IdCanzone,
        Titolo = c.Titolo,
        Durata = c.Durata,
        Nome = c.NomeAutore,
        Cognome = c.CognomeAutore
    };
    return Results.Created($"/api/read/song/{result.Id}", result);
})
.WithOpenApi();

#endregion

#region Delete:
app.MapDelete("/api/delete/playlist/{id}", async (int id, LibreriaContext db) =>
{
    var playlist = await db.Playlists
            .FirstOrDefaultAsync(p => p.IdPlaylist == id);

    if (playlist == null)
        return Results.NotFound();

    db.Playlists.Remove(playlist);
    await db.SaveChangesAsync();

    return Results.Ok("Playlist cancellata con successo.");
})
.WithOpenApi();

app.MapDelete("/api/delete/song/{id}", async (int id, LibreriaContext db) =>
{
    var canzone = await db.Canzoni
            .FirstOrDefaultAsync(p => p.IdCanzone == id);

    if (canzone == null)
        return Results.NotFound();

    db.Canzoni.Remove(canzone);
    await db.SaveChangesAsync();

    return Results.Ok("Canzone cancellata con successo.");
})
.WithOpenApi();
#endregion

#region Update:

app.MapPut("/api/update/playlist/{id}", async (int id, LibreriaContext db, Playlist pAggiorna) =>
{
    var playlist = await db.Playlists
        .Include(p => p.Elenco)
        .FirstOrDefaultAsync(p => p.IdPlaylist == id);

    if (playlist == null)
        return Results.NotFound();

    playlist.Titolo = pAggiorna.Titolo;
    playlist.Autore = pAggiorna.Autore;

    playlist.Elenco.Clear();

    foreach (var canzone in pAggiorna.Elenco)
    {
        var canzoneEsistente = await db.Canzoni.FindAsync(canzone.IdCanzone);
        if (canzoneEsistente != null)
        {
            playlist.Elenco.Add(canzoneEsistente);
        }
    }

    await db.SaveChangesAsync();

    var result = new
    {
        Titolo = playlist.Titolo,
        Autore = playlist.Autore,
        Elenco = playlist.Elenco.Select(c => new
        {
            Titolo = c.Titolo,
            Durata = c.Durata,
            Nome = c.NomeAutore,
            Cognome = c.CognomeAutore
        }).ToList()
    };

    return Results.Ok(result);
})
.WithOpenApi();

app.MapPut("/api/update/song/{id}", async (int id, LibreriaContext db, Canzone cAggiorna) =>
{
    var c = await db.Canzoni
            .FirstOrDefaultAsync(c => c.IdCanzone == id);

    if (c == null)
        return Results.NotFound();

    c.Durata = cAggiorna.Durata;
    c.Titolo = cAggiorna.Titolo;
    c.NomeAutore = cAggiorna.NomeAutore;
    c.CognomeAutore = cAggiorna.CognomeAutore;
    c.PlaylistId = cAggiorna.PlaylistId;
    await db.SaveChangesAsync();

    var result = new
    {
        Titolo = c.Titolo,
        Durata = c.Durata,
        Nome = c.NomeAutore,
        Cognome = c.CognomeAutore
    };

    return Results.Ok(result);
})
.WithOpenApi();

#endregion

app.UseCors(builder =>
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
    );

app.Run();
