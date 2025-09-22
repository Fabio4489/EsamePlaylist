# ESAME FINALE:
## Progetto: Minimal API gestione delle playlist.
Questo progetto, come richiesto da consegna, nasce con la creazione di due classi: 
- Playlist, con i seguenti attiributi:
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

- Canzone, con i seguenti attiributi:
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
Abbiamo poi implementato le principali operazioni CRUD per ogni classe:
- **GET**: 
    - Get tutte le Playlist, con annesse le canzoni inserite negli elenchi;
    - Get tutte le Canzoni;
    - Get di una Playlist, con annesse le canzoni inserite nell'elenco;
    - Get di una Canzone;
- **POST**:
    - Post di una Playlist, con (nel caso si voglia) annesse canzoni nell'elenco;
    - Post di una Canzone, con (nel caso si voglia) possibilità di inserirla in 1 playlisy inserendo l'id della playlist nella campo PlaylistId;
- **DELETE**: 
    - Delete di una playlist, lasciando però tutte le canzoni inserite (così possiamo riassegnarle ad altre playlist senza doverle reiserire);
    - Delete di una canzone;
- **PUT**: 
    - Put di una playlist, andando ad aggiornare le variabili della playlist e le canzoni al suo interno;
    - Put di una canzone, andando a cambiare i valori di una canzone.

