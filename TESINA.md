# Tesina teorica:

### 1. Definizione di REST API: 
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

---

- Metodi HTTP principali: GET, POST, PUT, DELETE (con esempi d’uso).
- Codici di stato HTTP più comuni.
- Autenticazione e sicurezza (API Key, JWT – anche solo a livello introduttivo).
- Documentazione delle API: OpenAPI/Swagger.


Fonti:
- https://www.redhat.com/en/topics/api/what-are-application-programming-interfaces#soap-vs-rest