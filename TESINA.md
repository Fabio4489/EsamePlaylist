# Tesina teorica:

## Indice
- [Note](#note)
- [1. Definizione di REST API](#1-definizione-di-rest-api)
- [2. Confronto con altre architetture](#2-confronto-con-altre-architetture)
- [3. Metodi HTTP principali](#3-metodi-http-principali)
- [4. Codici di stato HTTP](#4-codici-di-stato-http-più-comuni)
- [5. Autenticazione e sicurezza](#5-autenticazione-e-sicurezza)
- [6. Documentazione di OpenAPI e Swagger](#6-documentazione-di-openapi-e-swagger)
- [Fonti](#fonti)

## Note
Putroppo ho organizzato male le richieste per la tesina che saranno in 2 file. Mi scuso per il disagio creato.

1. Parte teorica --> <span style="color:green"> In questo file</span>
2. Descrizione del progetto sviluppato --> <span style="color:orange"> Nel file README.md nel progetto </span>
3. Documentazione degli endpoint con esempi di chiamata (Postman o curl) <span style="color:orange"> --> Nel file README.md nel progetto </span>
4. Diagramma UML delle entità. --> <span style="color:orange"> Nel file README.md nel progetto </span>
2. **Repository GitHub** contenente:
    - Codice sorgente del progetto (URL incluso nella tesina). --> https://github.com/Fabio4489/EsamePlaylist


## 1. Definizione di REST API: 
Le API REST (Representational State Transfer) sono uno stile architetturale per la progettazione di servizi web che si basano sul protocollo HTTP.

Le REST API devono rispettare sei vincoli:
- **Architettura client-server**: devono avere un'architettura formata da client e  server ed una gestione delle richieste tramite `protocollo HTTP`.
- **Statelessness**: ogni richiesta è indipendente, il server non ricorda le richieste precedenti. Il client dovrà inviare le informazioni di sessione ogni volta (Token/cookie).
- **Supporto cache**: il caching permette al client di riaccedere alle informazioni chieste senza rifare la richiesta.
- **Sistema a livelli**: le interazioni client-server possono essere mediate da livelli aggiuntivi, offrire altre funzionalità.
- **Codice on demand** (opzionale): oltre ai dati in json, i server possono inviare codice eseguibile dal client, offrire altre funzionalità.
- **Interfaccia uniforme**: è il `VINCOLO PRINCIPALE` e prevede:
    - **Identificazione delle risorse nelle richieste**: le risorse (come gli oggetti) vengono identificate (con un codice univoco) nelle richieste così da essere distinte dalle rappresentazioni (JSON) restituite al client.
    - **Manipolazione delle risorse tramite le rappresentazioni**: i client non lavorano con la risorsa ma con una rappresentano della risorse. Questa rappresentazione devono contenere le informazioni necessarie per consentirne la lettura, modifica o l'eliminazione.
    - **Messaggi autodescrittivi**: ogni messaggio restituito a un client contiene le informazioni necessarie per descrivere come il client deve elaborare l'informazione.
    - **Ipermedia come motore dello stato dell'applicazione**: il client deve poter individuare tutte le altre azioni disponibili, senza dover indovinare gli URL.

## 2. Confronto con altre architetture

| ? | SOAP | REST | GraphQL |
| :- | :-: | :-: | :-: |
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

## 3. Metodi HTTP principali
La comunicazione tra client e server avviene tramite richieste HTTP. Queste utilizzano metodi HTTP che permettono al client di fare le operazioni CRUD, specificando il metodo che vuole usare.

I principali metodi sono: 
- ***GET***: Permettono all'utente di `leggere` le risorse dal server. Queste richieste possono essere ripetute più volte e restituiranno sempre la stessa rappresentazione, senza modificare lo stato del server.
    Possiamo richiedere tutto un elenco di ciò che abbiamo nel DB:
    ```csharp
    // Mappiamo l'indirizzo "/api/read/songs" con il metodo GET
    app.MapGet("/api/read/songs", async (LibreriaContext db) => {
        // Ricerche nel DB
        var elenco = await db.Canzoni.ToListAsync();
        // Ritorna l'elenco che è composto da tutte le canzoni ne DB, con lo status code 200 (OK)
        return Results.Ok(elenco);
    })
    ```
    Oppure possiamo essere più specifici, andando a creare una `ricerca con filtro`.
    
    Per poter filtrare utilizziamo un "Path Parameter" che, l'utente riempirà quando scriverà l'URL. Noi andando a mappare l'URL prendiamo il Path Parameter e lo passiamo dentro al programma. Come in questo esempio:
    ```csharp
    // Mappiamo l'indirizzo (GET) prendiamo l'id e lo passiamo dentro
    app.MapGet("/api/read/song/{id}", async (int id, LibreriaContext db) => {
        // Qui possiamo usare l'id liberamente per fare le nostre ricerche su DB
        Canzone? c = await db.Canzoni.FindAsync(id);
        // Controllo se ho trovato la canzone
        if (c != null){
            // Ritorna la canzone e lo status code 200 (OK)
            return Results.Ok(c);
        }
        // Ritorna lo status code 404 (Not Found)
        return Results.NotFound();
    })
    ```

- ***DELETE***: Permettono all'utente di `eliminare` le risorse. Queste richieste non possono essere ripetute più volte sulla stessa risorsa (dopo la prima volta modifichiamo lo stato della risorsa sul server).
    
    Per poter eliminare utilizziamo anche qui un "Path Parameter". Esempio:
    ```csharp
    // Mappiamo l'indirizzo (DELETE) prendiamo l'id e lo passiamo dentro
    app.MapDelete("/api/delete/song/{id}", async (int id, LibreriaContext db) => {
        // Essendo id univoco, il primo che troveremo (se lo troveremo) sarà la canzone che cerchiamo
        var canzone = await db.Canzoni
                .FirstOrDefaultAsync(p => p.IdCanzone == id);
        // Controllo se l'ho trovata
        if (canzone == null)
            // Se canzone = null ritorno lo status code 404 (Not Found)
            return Results.NotFound();
        // Se canzone != null rimuovo la canzone dal DB 
        db.Canzoni.Remove(canzone);
        // Salva i cambiamenti apportati al DB 
        await db.SaveChangesAsync();
        // Ritorna lo status code 200 (OK) con messaggio
        return Results.Ok("Canzone cancellata con successo.");
    })
    ```
- ***POST***: Sono quelli che permettono all'utente di `creare` delle risorse sul DB. Queste, se ripetute più volte, creeranno diverse risorse tutte uguali ma con ID diversi. 
    
    Per poter creare, qui non utilizziamo un "Path Parameter" per inserire i dati ma andiamo ad usare il BODY delle richieste HTTP.
    
    Questo "campo" verrà riempito dall'utente con del JSON, che passeremo dentro al programma. Esempio:
    ```csharp
    // Mappiamo l'indirizzo (POST) passiamo il body della richiesta dentro alle graffe
    app.MapPost("/api/create/song", async (LibreriaContext db, Canzone c)  => {
        // Qui possiamo usare l'oggetto canzone liberamente 
        // Aggiungiamo la canzone al DB
        db.Canzoni.Add(c);
        // Salviamo i cambiamenti sul DB
        await db.SaveChangesAsync();
        // Ritorniamo lo Status Code 201 (Created) + URL per poter visualizzare l'inserimento + la risorsa inserita.
        return Results.Created($"/api/read/song/{c.IdCanzone}", c);
    })
    ```
- ***PUT***: Permettono all'utente di `modificare` le risorse sul server. Queste possono essere ripetute più volte sulla stessa risorsa, (ma se non cambiamo i dati le modifiche non si percepiranno) la prima volta sarà modificato lo stato della risorsa.
    
    Per poter modificare una risorsa specifica, utilizziamo un "Path Parameter", ma non solo, andiamo anche ad usare il BODY che conterrà la risorsa aggiornata in JSON, e la passeremo dentro al programma, insieme anche al ID del "Path Parameter". Esempio:
    ```csharp
    // Mappiamo l'indirizzo (PUT) passiamo il body e l'id dentro alle graffe
    app.MapPut("/api/update/song/{id}", async (int id, LibreriaContext db, Canzone cAggiorna) => {
        // Possiamo usare sia la risorsa aggiornata sia l'id della vecchia risorsa
        // Poi cerchiamo la prima canzone con ID = all'id del path parameter
        var c = await db.Canzoni
                .FirstOrDefaultAsync(c => c.IdCanzone == id);
        // Controllo che ci sia la canzone
        if (c == null)
            // Se non la trovo ritorno lo Status Code 404 (Not Found)
            return Results.NotFound();
        // Qui aggiorniamo i campi della canzone vecchia (c) con i campi della canzone aggiornata (cAggiorna)
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
    ```
---
## 4. Codici di stato HTTP più comuni.
I `codici di stato HTTP` sono circa 60/70, ma i più usati sono solo circa 30.
Questi codici segnalano la risposta che il server genera a una specifica richiesta HTTP.

Per gli utenti, alcuni codici sono visibili, per i quali si creano in automatico pagine HTML sul browser, mentre altri sono "invisibili".

Questi codici sono divisi in 5 categorie: 
- `1xx`: Sono i codici di **informativa**. 
    
    Il server comunica che la richiesta è stata ricevuta e che è in corso il processo di elaborazione.

- `2xx`: I codici di **successo**. 

    Qui l’operazione è stata completata; il server ha ricevuto, elaborato e accettato la richiesta del client.
    L’utente non vedrà questo codice, ma la sua presenza sarà dimostrata dalla visualizzazione della pagina o della risorsa.

- `3xx`: I codici di **reindirizzamento**. 
    
    Con questi codici il server dice che riceve la richiesta, ma che sono necessarie altre azioni del client per completarla.

- `4xx`: I codici di **errore del client**. 

    Questi segnalano che la richiesta non può essere completata perché ci sono problemi del client. 

- `5xx`: I codici di **errore del server**. 

    Questa tipologia segnala problemi del server che non consentono di portare a termine una richiesta apparentemente valida. L’errore può essere momentaneo o definitivo.

#### I più frequenti: 

| Codice | Nome | Significato |
| :-: | :-: | :-: |
| 200 | OK | Risposta **normale** nel dialogo tra client e server, tutto funziona in modo corretto|
| 201 | Creato | La richiesta è stata **soddisfatta** e ha portato alla **creazione** di una o più risorse |
| 301 | Redirect permanente | La risorsa **è stata spostata** verso una nuova pagina, gli utenti non visualizzano il codice, ma il nuovo **l’URL viene automaticamente modificato** |
| 302 | Redirect temporaneo | Il **trasferimento** della risorsa è **temporaneo**, la vecchia pagina resta valida |
| 404 | Pagina non trovata | I **dati** richiesti **non** sono stati **localizzati** sul server.|
| 410 | Gone | La **pagina non** è più **disponibile**, e non lo sarà più in futuro |
| 500 | Internal Server Error | Il server non riesce a soddisfare una richiesta valida; generici **problemi al server** |
| 503 | Service Unavailable | Il **server** è **down** (temporaneamente non funzionante) |

## 5. Autenticazione e sicurezza 
### API Key
Una `chiave API` è una **stringa alfanumerica** che gli sviluppatori utilizzano per controllare le proprie API.
Possiamo elencare tre tipologie di controlli che avvengono:
- *Monitoraggio dell'utilizzo*: 
    Le key vengono usate per tracciare l'utilizzo e gestire il consumo delle proprie API. 
    Queste possoni limitare l'accesso ai servizi e permette il passaggio **solo** del traffico legittimo. 
    
    Possono consentire anche analisi statistiche riguardo agli utilizzatori dell'API e anche un ottimizzazione delle risorse e la capacità della banda dell'API.

- Risolvere i *problemi di integrazione*: 
    L'uso di API key permette di rilevare (se prendiamo dati da tante API) dei modelli di dati anomali e associarli alle rispettive API.

- *Identificare i progetti*:
    Le chiavi API fungono anche da meccanismo di autorizzazione.
    Per usare un'API, il progetto potrebbe dover presentare le credenziali API corrette.
    
    Queste chiavi non autenticano utenti, identificano l'organizzazione che viene associata alla chiave specifica.

#### Come funzionano?
Associamo una chiave API specifica a un client API specifico. 
Dato che l'utilizzo dell'API è essenzialmente un modulo software che comunica con un altro, le chiavi sono associate a diversi moduli software o applicazioni che vogliono comunicare con l'API. 

La comunicazione segue questi passi:
1. Un'applicazione manda la richiesta;
2. Il server API convalida l'autenticità del client con la chiave API univoca;
3. Se la chiave API non corrisponde a nessuna di quelle consentite, il server rifiuta la chiamata API e invia un messaggio di rifiuto;
4. Se la chiave API corrisponde, il server soddisfa la richiesta e restituisce la risposta prevista.

I provider di API possono utilizzare la chiave API per regolare diversi gradi di accesso ai propri servizi API

### JWT
JWT (JSON Web Token) è uno standard che permette la trasmissione sicura di informazioni tra server e client.
Questi token vengono utilizzati per l'autenticazione stateless, eliminando la necessità di mantenere sessioni sul server.

Sono composti da tre parti:

Un flusso tipico di lavoro di un JWT è:
1. L'utente si autentica con nome utente e password.
2. Il server verifica le credenziali e genera un JWT 
    - *Header*: che contiene il dato cifrato (in base 64) di 2 informazioni: 
        - il tipo di token (JWT);
        - l'algoritmo utilizzato (es. HS256). 
    - *Payload*: che contiene il dato cifrato (in base 64) di diverse informazioni:
        - ciò che noi vogliamo salvare (es. come l'ID e il ruolo);
        - la scadenza del token (che si calcola dall'unix datetime stamp).
    - *Signature*: è una funzione non invertente che permette tramite una password segreta di controllare che gli altri 2 pezzi del JWT non siano stati invalidati da terzi.
3. Il client memorizza il token (solitamente in localStorage o sessionStorage).
4. Ad ogni richiesta protetta, il client lo invia nell'Authorization Header.
5. Il server verifica il JWT e concede o nega l'accesso.
---
## 6. Documentazione di OpenAPI e Swagger
Swagger è uno strumento, indipendente dal linguaggio, per la descrizione delle REST API. Permette di comprendere le funzionalità di un'API REST senza accesso diretto al codice sorgente.
- OpenAPI è una specifica (è un documento, è la parte principale del flusso e viene usata per guidare strumenti).
- Swagger è uno strumento che viene giudato dalla specifica OpenAPI.

Swagger offre un'interfaccia utente basata sul Web che fornisce informazioni, usando la specifica OpenAPI generata. Ogni metodo di azione pubblico nei controller dell'API può essere testato dall'interfaccia utente. 



---
## Fonti:
- https://www.redhat.com/en/topics/api/what-are-application-programming-interfaces#soap-vs-rest
- https://aws.amazon.com/it/compare/the-difference-between-soap-rest/
- https://aws.amazon.com/it/compare/the-difference-between-graphql-and-rest/
- https://openapi.it/blog/5-metodi-http-api-restful
- https://www.seozoom.it/codici-di-stato-http/
- https://aws.amazon.com/it/what-is/api-key/
https://www.html.it/pag/541989/jwt-per-autenticazione-e-protezione-delle-api/#:~:text=Utilizzo%20di%20JWT%20per%20la,concede%20o%20nega%20l'accesso.
- https://www.youtube.com/watch?v=lmpvItN_tcU
- https://www.html.it/pag/541989/jwt-per-autenticazione-e-protezione-delle-api/
- https://learn.microsoft.com/it-it/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-8.0