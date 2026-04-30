# Transformer Monitor App

Full-stack aplikacija za monitoring transformatora, pracenje napona u realnom vremenu i organizaciju maintenance/work order procesa.

Aplikacija je napravljena kao showcase modernog enterprise stack-a:

- Backend: .NET 8 Web API, Clean/Onion Architecture, CQRS preko MediatR-a, Entity Framework Core i SQL Server.
- Frontend: Vue 3, TypeScript, Vite, Pinia, SignalR klijent i modularna domain/data/application/presentation struktura.
- Runtime: Docker Compose dize SQL Server, API i frontend zajedno.

## Sta aplikacija radi

Transformer Monitor ima dva glavna dela:

1. Dashboard za live monitoring transformatora.
2. Maintenance dispatcher za kreiranje, dodelu i resavanje tiketa.

Backend seed-uje pocetne transformatore i maintenance timove. Pozadinski servis na backendu na svakih nekoliko sekundi generise nova ocitavanja napona za aktivne transformatore, upisuje ih u bazu i salje frontend-u preko SignalR-a.

Frontend ucitava pocetno stanje preko REST API-ja, a zatim prima live voltage update-e preko WebSocket/SignalR konekcije. Dashboard kartice mogu da se filtriraju, sortiraju i cuvaju lokalno u browseru.

## Tehnologije

### Backend

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- MediatR
- AutoMapper
- FluentValidation
- SignalR
- xUnit i Moq za testove
- Swagger/OpenAPI za API dokumentaciju

### Frontend

- Vue 3
- TypeScript
- Vite
- Pinia
- SignalR JavaScript client
- ECharts
- lucide-vue-next
- Vitest

### Infrastruktura

- Docker
- Docker Compose
- Nginx za serviranje produkcionog frontend build-a
- SQL Server 2022 container

## Struktura projekta

```text
transformatorMonitorApp/
  app-config.json
  docker-compose.yml
  scripts/
    set-ports.ps1
  backend/
    Dockerfile
    TransformerMonitor.sln
    src/
      TransformerMonitor.Api/
      TransformerMonitor.Application/
      TransformerMonitor.Domain/
      TransformerMonitor.Infrastructure/
    tests/
      TransformerMonitor.Tests/
  presentation/
    Dockerfile
    package.json
    vite.config.ts
    src/
      application/
      data/
      domain/
      mock/
      presentation/
      shared/
```

## Kako je buildovana aplikacija

### Backend arhitektura

Backend je podeljen u cetiri projekta.

`TransformerMonitor.Domain` je centralni domain sloj. Tu se nalaze entiteti kao sto su `Transformer`, `VoltageReading`, `Ticket`, `Team` i `Technician`, kao i repository interfejsi. Ovaj sloj nema zavisnost od ASP.NET-a, baze ili UI-ja.

`TransformerMonitor.Application` sadrzi application/business logiku. Koristi CQRS stil kroz MediatR:

- Queries za citanje podataka.
- Commands za izmene.
- Handlers za izvrsavanje use-case-ova.
- DTO objekte za izlaz prema API-ju.
- AutoMapper profile za mapiranje entiteta u DTO.
- FluentValidation pipeline za validaciju command-a.

`TransformerMonitor.Infrastructure` sadrzi tehnicku implementaciju:

- `ApplicationDbContext` za EF Core.
- Migracije za SQL Server bazu.
- Repository implementacije.
- Unit of Work.
- `DataSeeder` za inicijalne transformatore i timove.
- `TransformerSimulationService`, background servis koji generise live voltage ocitavanja.

`TransformerMonitor.Api` je ulazna tacka aplikacije:

- REST kontroleri: `TransformersController`, `TicketsController`, `TeamsController`.
- SignalR hub: `/hubs/transformers`.
- Swagger UI.
- CORS konfiguracija.
- Global exception middleware.
- Automatsko pokretanje migracija i seed-a pri startu.

### Frontend arhitektura

Frontend je Vue 3 aplikacija podeljena po slojevima.

`application/` sadrzi Pinia store-ove i servise:

- `transformers.store.ts` ucitava transformatore i slusa SignalR update-e.
- `maintenance.store.ts` upravlja tiketima i timovima.
- `card.store.ts` cuva dashboard kartice u localStorage.
- `ui.store.ts` drzi navigaciju, temu i quick view stanje.
- `ConfigService.ts` ucitava `app-config.json`.

`domain/` sadrzi TypeScript modele, DTO tipove i mapper-e. Tu se prevodi API oblik podataka u frontend domain model.

`data/` sadrzi repository klase koje komuniciraju sa backend API-jem:

- `TransformerApiRepository`
- `TeamApiRepository`
- `TicketApiRepository`

`presentation/` sadrzi Vue komponente, modale i view-ove:

- `MainPage.vue` za dashboard.
- `MaintenancePage.vue` za maintenance dispatcher.
- kartice, tabele, chart komponente i modalne forme.

`shared/` sadrzi pomocne delove kao sto je cross-tab sync.

## Tok podataka

1. Frontend pri startu poziva `loadConfig()` i cita `/app-config.json`.
2. Vue aplikacija ucitava pocetne transformatore preko `GET /api/transformers`.
3. Backend cita podatke iz SQL Server baze preko EF Core repository sloja.
4. `TransformerSimulationService` na backendu periodicki generise nova voltage ocitavanja.
5. Backend preko SignalR-a emituje `ReceiveVoltageUpdate`.
6. Frontend SignalR data source prima update i azurira Pinia store.
7. Dashboard kartice i grafikoni reaktivno prikazuju nove podatke.

Maintenance tok:

1. Frontend cita timove i tikete preko `GET /api/teams`, `GET /api/teams/active` i `GET /api/tickets`.
2. Novi work order se kreira preko `POST /api/tickets`.
3. Tiket se dodeljuje timu preko `POST /api/tickets/assign`.
4. Tiket se resava preko `POST /api/tickets/{id}/resolve`.

## Konfiguracija

Glavna konfiguracija portova je u root fajlu:

```json
{
  "Backend": {
    "Port": 61471,
    "Url": "http://localhost:61471"
  },
  "Frontend": {
    "Port": 5173,
    "Url": "http://localhost:5173"
  }
}
```

U rucnom development modu backend i frontend citaju `app-config.json`.

U Docker modu portovi se citaju iz environment promenljivih:

- `FRONTEND_PORT`
- `BACKEND_PORT`
- `DB_PORT`

Ako promenljive nisu zadate, Docker koristi default vrednosti:

```text
FRONTEND_PORT=5173
BACKEND_PORT=61471
DB_PORT=1433
```

Frontend Docker container pri startu sam generise `/usr/share/nginx/html/app-config.json` na osnovu tih environment vrednosti. Zato za Docker nije potrebno rucno menjati lokalni `app-config.json`.

## Pokretanje preko Docker-a

Preporuceni nacin pokretanja je Docker Compose.

Preduslovi:

- Docker Desktop instaliran i pokrenut.

Komande:

```powershell
cd I:\transformatorMonitorApp
docker compose up --build -d
```

Ako zelis da odredis portove pri startu, koristi:

```powershell
cd I:\transformatorMonitorApp
$env:FRONTEND_PORT=5180
$env:BACKEND_PORT=61500
$env:DB_PORT=1434
docker compose up --build -d
```

U tom slucaju aplikacija radi na:

- Frontend: `http://localhost:5180`
- Swagger/API: `http://localhost:61500/swagger`
- SQL Server: `localhost,1434`

Ova komanda:

- build-uje .NET API image,
- build-uje Vue frontend image,
- pokrece SQL Server 2022,
- pokrece API,
- pokrece frontend preko Nginx-a,
- backend automatski radi migracije i seed podataka.

Adrese:

- Frontend: http://localhost:5173
- Swagger/API: http://localhost:61471/swagger
- SQL Server: `localhost,1433`

Ako si koristio custom portove, koristi portove koje si zadao kroz environment promenljive.

SQL Server kredencijali iz Docker Compose fajla:

- User: `sa`
- Password: `YourStrong@Password123`
- Database: `TransformerTestDataBase`

Gasanje:

```powershell
cd I:\transformatorMonitorApp
docker compose down
```

Gasanje sa brisanjem Docker volume podataka:

```powershell
cd I:\transformatorMonitorApp
docker compose down -v
```

## Rucno pokretanje za development

Rucno pokretanje je korisno kada hoces live reload i laksi debugging.

Preduslovi:

- .NET 8 SDK
- Node.js LTS
- SQL Server LocalDB ili lokalni SQL Server

### Backend

```powershell
cd I:\transformatorMonitorApp\backend\src\TransformerMonitor.Api
dotnet run
```

Backend po default-u koristi connection string iz `appsettings.json`:

```text
Server=(localdb)\mssqllocaldb;Database=TransformerTestDataBase;Trusted_Connection=True;MultipleActiveResultSets=true
```

Ako zelis drugi SQL Server, podesi environment variable `DB_CONNECTION_STRING`.

Ako zelis drugi backend port, prvo sinhronizuj config:

```powershell
cd I:\transformatorMonitorApp
.\scripts\set-ports.ps1 -FrontendPort 5180 -BackendPort 61500
```

Zatim pokreni backend:

```powershell
cd I:\transformatorMonitorApp\backend\src\TransformerMonitor.Api
dotnet run
```

### Frontend

```powershell
cd I:\transformatorMonitorApp\presentation
npm install
npm run dev
```

Vite cita port iz `app-config.json`, pa frontend ide na:

```text
http://localhost:5173
```

Ako si pre toga promenio port kroz `set-ports.ps1`, Vite ce citati novi frontend port iz `app-config.json`.

## Build komande

### Docker build cele aplikacije

```powershell
cd I:\transformatorMonitorApp
docker compose build
```

### Backend build

```powershell
cd I:\transformatorMonitorApp\backend
dotnet build TransformerMonitor.sln
```

### Backend publish

```powershell
cd I:\transformatorMonitorApp\backend
dotnet publish src\TransformerMonitor.Api\TransformerMonitor.Api.csproj -c Release -o publish
```

### Frontend build

```powershell
cd I:\transformatorMonitorApp\presentation
npm install
npm run build
```

Frontend build generise staticne fajlove u `presentation/dist`. U Docker produkcionom image-u ti fajlovi se serviraju preko Nginx-a.

## Testovi

### Backend testovi

```powershell
cd I:\transformatorMonitorApp\backend
dotnet test TransformerMonitor.sln
```

Postojeci backend testovi pokrivaju deo application sloja, MediatR handler-e i FluentValidation pravila za transformere.

### Frontend testovi

U `package.json` trenutno ne postoji eksplicitan `test` script, ali projekat ima Vitest dependency i primer mapper testa. Ako se doda script, tipicna komanda bi bila:

```json
"test": "vitest"
```

Zatim:

```powershell
cd I:\transformatorMonitorApp\presentation
npm run test
```

## API pregled

Glavni REST endpoint-i:

```text
GET    /api/transformers
GET    /api/transformers/{id}
POST   /api/transformers
PUT    /api/transformers/{id}
DELETE /api/transformers/{id}

GET    /api/tickets
GET    /api/tickets/open
POST   /api/tickets
POST   /api/tickets/assign
POST   /api/tickets/{id}/resolve

GET    /api/teams
GET    /api/teams/active
```

SignalR hub:

```text
/hubs/transformers
```

Frontend slusa event:

```text
ReceiveVoltageUpdate
```

## Docker detalji

`docker-compose.yml` ima tri servisa:

- `db`: SQL Server 2022 container.
- `api`: build iz `backend/Dockerfile`, po default-u radi na portu `61471`, ili na `BACKEND_PORT`.
- `ui`: build iz `presentation/Dockerfile`, po default-u radi na portu `5173`, ili na `FRONTEND_PORT`, i mapira se na Nginx port `80` u container-u.

Backend Dockerfile:

1. koristi .NET SDK 8 image za restore/build/publish,
2. kopira solution i source projekte,
3. radi `dotnet restore`,
4. radi `dotnet publish`,
5. finalni runtime image koristi ASP.NET 8 runtime,
6. kopira publish output i `app-config.json`,
7. startuje `TransformerMonitor.Api.dll`.

Frontend Dockerfile:

1. koristi Node LTS Alpine image za build,
2. radi `npm install`,
3. kopira frontend source i build-time `app-config.json` u public folder,
4. radi `npm run build`,
5. finalni image koristi Nginx,
6. kopira `dist` u `/usr/share/nginx/html`,
7. pri startu container-a generise runtime `app-config.json` iz `BACKEND_PORT`, `FRONTEND_PORT`, `BACKEND_URL` i `FRONTEND_URL`.

## Napomene

- Root `app-config.json` je vazan za oba dela aplikacije.
- Za Docker ne moras rucno da menjas `app-config.json`; custom portove zadas kroz environment promenljive pre `docker compose up`.
- Za rucni development koristi `scripts\set-ports.ps1` da lokalni `app-config.json` ostane uskladjen.
- Ako Docker build ne vidi izmenu konfiguracije, pokreni `docker compose up --build -d`.
- Ako baza ostane u starom stanju, koristi `docker compose down -v`, pa ponovo `docker compose up --build -d`.
- U repozitorijumu postoje `bin`, `obj`, `node_modules` i slicni generisani folderi; oni nisu deo rucnog citanja arhitekture i normalno se ignorisu.
