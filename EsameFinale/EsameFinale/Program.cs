using EsameFinale.Context;
using EsameFinale.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configurazione del database SQL Lite
builder.Services.AddDbContext<LibreriaContext>(
        options => options.UseSqlite("Data Source=Libreria.db")
    );

builder.Services.AddCors();
#endregion

var app = builder.Build();

#region Se non esiste il database, crealo!
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibreriaContext>();
    db.Database.EnsureCreated();
}
#endregion

#region Get tutte: 
app.MapGet("/api/read/allPlaylist", async (LibreriaContext db) =>
{
    var elenco = await db.Playlists.ToListAsync();
    return Results.Ok(elenco);
})
.WithOpenApi();

app.MapGet("/api/read/allSongs", async (LibreriaContext db) =>
{
    var elenco = await db.Canzoni.ToListAsync();
    return Results.Ok(elenco);
})
.WithOpenApi();
#endregion

#region Get solo una:

app.MapGet("/api/read/playlist/{id}", async (int id, LibreriaContext db) =>
{
    var playlist = await db.Playlists
            .Include(p => p.Elenco)
            .FirstOrDefaultAsync(p => p.IdPlaylist == id);

    if (playlist == null)
        return Results.NotFound();

    return Results.Ok(playlist);

})
.WithOpenApi();

app.MapGet("/api/read/song/{id}", async (int id, LibreriaContext db) =>
{
    Canzone? c = await db.Canzoni.FindAsync(id);
    if (c == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(c);
})
.WithOpenApi();

#endregion

#region Post:
app.MapPost("/api/create/playlist", async (LibreriaContext db, Playlist p) =>
{
    db.Playlists.Add(p);
    await db.SaveChangesAsync();
    return Results.Created($"/api/read/onePlaylist/{p.IdPlaylist}", p);
})
.WithOpenApi();

app.MapPost("/api/create/song", async (LibreriaContext db, Canzone c)  =>
{
    if (c.PlaylistId != null)
    {
        if (!await db.Playlists.AnyAsync(p => p.IdPlaylist == c.PlaylistId))
            return Results.BadRequest("Playlist inesistente");
    }

    db.Canzoni.Add(c);
    await db.SaveChangesAsync();

    return Results.Created($"/api/read/song/{c.IdCanzone}", c);
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



app.UseCors(builder =>
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
    );

app.Run();
