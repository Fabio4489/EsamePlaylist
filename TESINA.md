# Tesina teorica:

## 1. Definizione di REST API: 
Le API REST (Representational State Transfer) sono uno stile architetturale per la progettazione di servizi web che si basano sul protocollo HTTP.

Per poter essere chiamate REST devono rispettare sei vincoli:
- **Architettura client-server**: le api REST devono avere un'architettura formata da client e  server ed una gestione delle richieste tramite `protocollo HTTP`.
- **Statelessness**: ogni richiesta è indipendente, il server non ricorda le richieste precedenti. Quindi il client dovrà inviare tutte le informazioni di sessione ogni volta (Token/cookie).
- **Supporto cache**: il caching permette al client di riaccedere alle informazioni chieste precedentemente senza rifare la richiesta.
- **Sistema a livelli**: le interazioni client-server possono essere mediate da livelli aggiuntivi, che possono offrire funzionalità.
- **Codice on demand** (opzionale): oltre ai dati in json, i server possono inviare codice eseguibile dal client per avere altre funzionalità.
- **Interfaccia uniforme**: è il `VINCOLO PRINCIPALE` per la progettazione di API RESTful e prevede:
    - **Identificazione delle risorse nelle richieste**: le risorse (come gli oggetti) vengono identificate (con un codice univoco) nelle richieste così da essere distinte dalle rappresentazioni (JSON) restituite al client.
    - **Manipolazione delle risorse tramite le rappresentazioni**: i client non lavorano con la risorsa ma con una rappresentano della risorse. Questa rappresentazione devono contenere le informazioni necessarie per consentirne la lettura, modifica o l'eliminazione.
    - **Messaggi autodescrittivi**: ogni messaggio restituito a un client contiene le informazioni necessarie per descrivere come il client deve elaborare l'informazione.
    - **Ipermedia come motore dello stato dell'applicazione**: il client deve poter individuare tutte le altre azioni disponibili, senza dover indovinare gli URL.

## 2. Confronto tra REST e altre architetture (SOAP, GraphQL).

| ?                   |                      SOAP                      |                          REST                         |                            GraphQL                            |
| :------------------ | :--------------------------------------------: | :---------------------------------------------------: | :-----------------------------------------------------------: |
| **Cos'è?**          | Protocollo + restrittivo                       | Stile architetturale + flessibile                     | Linguaggio di query, stile architetturale, set di strumenti   |
| **Messaggi**        | XML                                            | JSON, XML, binario, HTML, CSV…                        | JSON (quasi esclusivo)                                        |
| **Risposte**        | Sempre complete                                | Rigide e complete (anche se non serve tutto)          | Solo i dati specificati nella query                           |
| **Progettazione**   | Basata su operazioni (WSDL)                    | Basata su risorse                                     | Basata su schema                                              |
| **Errori**          | Gestione errori integrata                      | Richiede riprovare o gestioni personalizzate          | Messaggi di errore strutturati automaticamente                |
| **Stato**           | Può mantenere stato (stateful)                 | Stateless                                             | Stateless                                                     |
| **Prestazioni**     | Più lento → messaggi grandi e complessi        | Più veloce ed efficiente, caching                     | Molto efficiente lato client, ma può sovraccaricare il server |
| **Sicurezza**       | Supporta WS-Security, firma e crittografia XML | HTTPS di base (JWT per autenticazione)                | HTTPS + meccanismi standard (JWT per autenticazione)          |
| **Invio richieste** | E' uguale (HTTP, SMTP, TCP…)                   | HTTP/HTTPS                                            | Solo HTTP/HTTPS                                               |
| **I dati**          | Fortemente tipizzati                           | Debolmente tipizzati                                  | Fortemente tipizzati (schema GraphQL)                         |
| **Età**             | Più vecchia                                    | Intermedia                                            | Più giovane                                                   |

### SOAP
SOAP(Simple Object Access Protocol) è un protocollo che ha diversi standard associati che controllano ogni aspetto dello scambio di dati, questo può rendere più complicato (rispetto alle REST) lo sviluppo. 

Le API SOAP utilizzano solo XML come formato dei messaggi.

Quando si invia una richiesta, è necessario racchiudere la richiesta HTTP in una busta SOAP (una struttura di dati che modifica il contenuto HTTP con i requisiti di richiesta SOAP); è poi possibile inviare queste richieste anche con TCP o ICMP.
### GraphQL
GraphQL è un linguaggio di query e un runtime per API, che permette ai client di richiedere esattamente i dati di cui hanno bisogno, diminuendo le risposte inutilmente grandi delle REST.

Le API GraphQL usano principalmente JSON come formato per i messaggi.

Invece di avere molti endpoint, un server GraphQL espone un singolo endpoint attraverso il quale il client può effettuare query.

Con lo schema fortemente tipizzato, GraphQL fornisce al client una descrizione completa delle risorse disponibili e delle operazioni consentite.

## 3. Metodi HTTP principali: GET, POST, PUT, DELETE.

La comunicazione tra client e server avviene tramite richieste HTTP. Queste utilizzano metodi HTTP che permettono al client di specificare l'azione da eseguire.

Questi metodi permettono alle API di eseguire le principali operazioni CRUD sulle risorse in modo standardizzato ed ordinato. 

I principali metodi sono: 
- ***GET***: 

    I metodi `GET` permettono all'utente di leggere le risorse dal server. Questi metodi richiedono al server una vista dei dati. Queste richieste possono essere ripetute più volte e restituiranno sempre la stessa rappresentazione, senza modificare lo stato del server.
    
    Possiamo richiedere tutto un elenco di ciò che abbiamo nel DB, come in questo esempio:
    ```csharp
    // Mappiamo l'indirizzo "/api/read/songs" con il metodo GET
    app.MapGet("/api/read/songs", async (LibreriaContext db) =>
    {
        // Ricerche nel DB
        var elenco = await db.Canzoni.ToListAsync();
        // Ritorna l'elenco che è composto da tutte le canzoni ne DB, con lo status code 200 (OK)
        return Results.Ok(elenco);
    })
    .WithOpenApi();
    ```
    Oppure possiamo essere anche più specifici, andando a creare una vera e propria ricerca con filtro.
    
    Per poter filtrare secondo le volontà dell'utente utilizziamo un "Path Parameter" che, l'utente riempirà quando scrive l'URL. Così che noi andando a mappare l'URL prendiamo il Path Parameter e lo passiamo dentro alla parte attiva del programma.  
    
    Come in quest'altro esempio:
    ```csharp
    // Mappiamo l'indirizzo "/api/read/songs/{id}" con il metodo GET
    // Prendiamo l'id e lo passiamo dentro alle graffe
    app.MapGet("/api/read/song/{id}", async (int id, LibreriaContext db) =>
    {
        // Qui possiamo usare l'id liberamente per fare le nostre ricerche su DB
        Canzone? c = await db.Canzoni.FindAsync(id);
        // Controllo se ho trovato la canzone (se è vuoto non abbiamo trovato, se è pieno abbiamo trovato la nostra canzone)
        if (c != null)
        {
            // Ritorna la canzone, con lo status code 200 (OK)
            return Results.Ok(c);
        }
        // Ritorna lo status code 404 (Not Found)
        return Results.NotFound();
    })
    .WithOpenApi();
    ```

- ***DELETE***: 

    I metodi `DELETE` permettono all'utente di eliminare le risorse dal server. Queste richieste non possono essere ripetute più volte sulla stessa risorsa, dato che dopo la prima volta sarà modificato lo stato della risorsa sul server.
    
    Per poter eliminare secondo le volontà dell'utente, utilizziamo anche qui un "Path Parameter", che l'utente riempirà quando scrive l'URL. Così che noi andando a mappare l'URL prendiamo il Path Parameter e lo passiamo dentro alla parte attiva del programma.

    Esempio:
    ```csharp
    // Mappiamo l'indirizzo "/api/delete/song/{id}" con il metodo DELETE
    // Prendiamo l'id e lo passiamo dentro alle graffe
    app.MapDelete("/api/delete/song/{id}", async (int id, LibreriaContext db) =>
    {
        // Essendo l'id univoco ed assegnato da DB il primo che troveremo (se lo troveremo) sarà la canzone che cercavamo
        var canzone = await db.Canzoni
                .FirstOrDefaultAsync(p => p.IdCanzone == id);
        // Controllo se l'ho trovata
        if (canzone == null)
            // Se canzone è null ritorna lo status code 404 (Not Found)
            return Results.NotFound();

        // Se canzone non è null rimuove la canzone dal DB 
        db.Canzoni.Remove(canzone);
        // Salva i cambiamenti apportati al DB 
        await db.SaveChangesAsync();
        // Ritorna lo status code 200 (OK) con messaggio
        return Results.Ok("Canzone cancellata con successo.");
    })
    .WithOpenApi();
    ```
- ***POST***: 
    I metodi `POST` sono quelli che permettono all'utente di creare delle risorse sul DB. Queste richieste se ripetute più volte creeranno diverse risorse tutte uguali ma con ID diversi. 
    
    Per poter creare secondo le volontà dell'utente, qui non utilizziamo un "Path Parameter" che sarebbe troppo disordinato.
    
    Quindi andiamo ad usare il BODY delle richieste HTTP.
    Questo "campo" della richiesta l'utente lo andrà a riempire con del JSON, che passeremo dentro alla parte attiva del programma.

    Esempio:
    ```csharp
    // Mappiamo l'indirizzo "/api/create/song" con il metodo POST
    // Passiamo il body della richiesta dentro alle graffe
    app.MapPost("/api/create/song", async (LibreriaContext db, Canzone c)  =>
    {
        // Qui possiamo usare l'oggetto canzone liberamente per fare le nostre azioni
        // Aggiungiamo la canzone al DB
        db.Canzoni.Add(c);
        // Salviamo i cambiamenti sul DB
        await db.SaveChangesAsync();
        // Ritorniamo lo Status Code 201 (Created) e il link per poter visualizzare l'inserimento appena fatto + la canzone inserita.
        return Results.Created($"/api/read/song/{c.IdCanzone}", c);
    })
    .WithOpenApi();
    ```

- ***PUT***: 
    I metodi `PUT` permettono all'utente di modificare le risorse sul server. Queste richieste non possono essere ripetute più volte sulla stessa risorsa, (ma se non cambiamo i dati le modifiche non si percepiranno) dato che la prima volta sarà modificato lo stato della risorsa sul server.
    
    Per poter modificare la risorsa che vuole l'utente, utilizziamo anche qui un "Path Parameter", che l'utente riempirà quando scrive l'URL. 
    
    Ma non solo, andiamo anche ad usare il BODY delle richieste HTTP.
    Questo "campo" della richiesta l'utente lo andrà a riempire con del JSON, e lo passeremo dentro alla parte attiva del programma, insieme anche al ID del "Path Parameter".


    Esempio:
    ```csharp
    // Mappiamo l'indirizzo "/api/update/song/{id}" con il metodo PUT
    // Passiamo il body e l'id della richiesta dentro alle graffe
    app.MapPut("/api/update/song/{id}", async (int id, LibreriaContext db, Canzone cAggiorna) =>
    {
        // Qui possiamo usare sia la canzone aggiornata sia l'id per arrivare alla posizione da aggiornare
        // Qui cerchiamo la prima canzone con ID = all'id del path parameter
        var c = await db.Canzoni
                .FirstOrDefaultAsync(c => c.IdCanzone == id);

        // Controllo che ci sia la canzone con l'id cercato
        if (c == null)
            // Se non la trovo ritorno lo Status Code 404 (Not Found)
            return Results.NotFound();

        // Qui aggiorniamo i campi della canzone vecchia (c) con i campi della canzone nuova (cAggiorna)
        c.Durata = cAggiorna.Durata;
        c.Titolo = cAggiorna.Titolo;
        c.NomeAutore = cAggiorna.NomeAutore;
        c.CognomeAutore = cAggiorna.CognomeAutore;
        c.PlaylistId = cAggiorna.PlaylistId;
        // Salviamo i cambiamenti sul DB
        await db.SaveChangesAsync();
        // Ritorno lo Status Code 200 (OK) e la canzone aggiornata
        return Results.Ok(c);
    })
    .WithOpenApi();
    ```
---
## 4. Codici di stato HTTP più comuni.
I codici di stato HTTP sono circa 60/70, ma i più usati sono solo circa 30.





---
- Autenticazione e sicurezza (API Key, JWT – anche solo a livello introduttivo).
- Documentazione delle API: OpenAPI/Swagger.


Fonti:
- https://www.redhat.com/en/topics/api/what-are-application-programming-interfaces#soap-vs-rest
- https://aws.amazon.com/it/compare/the-difference-between-soap-rest/
- https://aws.amazon.com/it/compare/the-difference-between-graphql-and-rest/
- https://openapi.it/blog/5-metodi-http-api-restful