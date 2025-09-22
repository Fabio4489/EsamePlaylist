# ESAME FINALE:
## Progetto: Minimal API gestione delle playlist.
### Le due Classi: 
Questo progetto, come richiesto da consegna, nasce con la creazione di:
- ***PLAYLIST***, con i seguenti attiributi:
```csharp
        [Key]
            // "[Key]" Serve per far capire al db che IdPlaylist è chiave primaria
        public int IdPlaylist { get; set; }
        public string? Titolo { get; set; }
        public string Autore { get; set; } = "Utente";
            // Campo che se non viene compilato avrà "Utente"

        public List<Canzone>? Elenco { get; set; } = new();
            // "= new()" serve per inizializzare automaticamente l'elenco, con una nuova lista vuota al momento della creazione dell’oggetto.
```

- ***CANZONE***, con i seguenti attiributi:
```csharp
        [Key]
            // "[Key]" Serve per far capire al db che IdCanzone è chiave primaria
        public int IdCanzone { get; set; }
        public string Titolo { get; set; } = null!;
            // Campo sarà nullo prima della prima compilazione, e subito dopo non potrà essere nullo 
        public int Durata { get; set; }
        public string NomeAutore { get; set; } = "Non riconosciuto";
            // Campo che se non viene compilato avrà "Non riconosciuto"
        public string CognomeAutore { get; set; } = "";
            // Campo che se non viene compilato sarà vuoto 
        public int? PlaylistId { get; set; }
            // Campo che funge da chiave secondaria.
        
```
### Le principali operazioni CRUD 
In seguito il progetto continua con l'implementazione, per ogni classe, delle operazioni CRUD.
- **GET - Read**: 
    - Get tutte le Playlist, con annesse le canzoni inserite negli elenchi;
    - Get tutte le Canzoni;
- **POST - Create**:
    - Post di una Playlist, con (nel caso si voglia) annesse canzoni nell'elenco;
    - Post di una Canzone, con (nel caso si voglia) possibilità di inserirla in una playlisy inserendo l'`id` della playlist nella campo `PlaylistId`;
- **DELETE - Delete**: (Tramite ID)
    - Delete di una playlist, lasciando però tutte le canzoni inserite (così possiamo riassegnarle ad altre playlist senza doverle reiserire);
    - Delete di una canzone;
- **PUT - Update**: (Tramite ID)
    - Put di una playlist, andando ad aggiornare le variabili della playlist e le canzoni al suo interno;
    - Put di una canzone, andando a cambiare i valori di una canzone.

- **GET FILTRO - Read**: (Tramite ID)
    - Get di una Playlist specificata dall'`id`, con annesse le canzoni inserite nel suo elenco;
    - Get di una Canzone specificata dall'`id`;

Ed ho applicato i `DTO`, così da andare a nascondere gli `id` ogni qual volta andiamo ad osservare i dati che ci arrivano dal `DB` (Get, Post, Put) (anche se, ora che non abbiamo
 il fornt end, ci servono per alcune azioni CRUD).

Inoltre ho utilizzato 2 tecnologie: 
- *DATABASE*: 

    Ho usato il db sqlite, con il controllo dei dati gestito da EF (Entity Framework).
```csharp
    builder.Services.AddDbContext<LibreriaContext>(
        options => options.UseSqlite("Data Source=Libreria.db")
    );
    // Abbiamo configurato l'EF con il database sqlite "Libreria.db"
    builder.Services.AddCors();
    // Per permettere le richieste anche se dallo stesso dispositivo
```

```csharp
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<LibreriaContext>();
        // Recupera il context 
        db.Database.EnsureCreated();
        // Controlla se il DB e le tabelle esistono; se non ci sono li crea.
    }
```
Nelle operazioni CRUD: 
```csharp
    // Per esempio una get

    app.MapGet("/api/read/songs", async (LibreriaContext db) =>
    // Passiamo il contesto e poi EF gestirà la comunicazione con il db.
    {
        var elenco = await db.Canzoni.ToListAsync();

        var result = elenco.Select(c => new
        {
            Titolo = c.Titolo,
            Durata = c.Durata,
            Nome = c.NomeAutore,
            Cognome = c.CognomeAutore
        });
        // Noi lavoreremo solo con gli oggetti.
        
        return Results.Ok(result);
    })
    .WithOpenApi();

    // L'operazione deve essere Awayt-Async dato che ci devono arrivare le conferme o gli errori dal DB prima di poter dare delle risposte all'utente.
```


