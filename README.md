# INKECOEX-Webshop
Deze gids bevat de stap-voor-stap instructies voor het lokaal instellen en draaien van de Webshop Service-applicatie. We gaan ervan uit dat je een schone machine hebt zonder de benodigde software geïnstalleerd.

## 1\. Vereisten

Om dit project te kunnen draaien, heb je twee belangrijke componenten nodig:

1.  [**.NET 9 SDK**](https://dotnet.microsoft.com/download/dotnet/9.0)**:** De software-ontwikkelingskit van Microsoft die nodig is om de C\# code te bouwen en uit te voeren.

2.  [**Docker Desktop**](https://www.docker.com/products/docker-desktop/)**:** We gebruiken Docker om de PostgreSQL-database te hosten in een container, wat zorgt voor een consistente en gemakkelijke database-omgeving.

## 2\. Installatie van de Vereisten

Volg de stappen hieronder om de benodigde software te installeren:

### A. Installeer .NET 9 SDK

1.  Ga naar de officiële [.NET 9 SDK downloadpagina](https://dotnet.microsoft.com/download/dotnet/9.0).

2.  Download het installatieprogramma dat geschikt is voor jouw besturingssysteem (Windows, macOS, of Linux).

3.  Voer het installatieprogramma uit en volg de instructies. De installatie is meestal snel en vereist geen speciale configuraties.

4.  **Verificatie:** Open je terminal (Command Prompt, PowerShell, of Bash) en typ:

    ```
    dotnet --version
    ```

    Als de installatie succesvol was, zie je nu een versie nummer dat begint met `9.` (bijv. `9.0.x`).

### B. Installeer Docker Desktop

1.  Ga naar de [Docker Desktop downloadpagina](https://www.docker.com/products/docker-desktop/).

2.  Download en installeer Docker Desktop voor jouw systeem.

3.  Start Docker Desktop na de installatie. Het kan even duren voordat Docker volledig is opgestart. Zorg ervoor dat het Docker-icoon stabiel is (dit betekent dat de Docker Engine draait).

## 3\. Database Setup (Docker Compose)

Het project maakt gebruik van een `docker-compose.yml` bestand om de PostgreSQL-database te definiëren. Dit is de snelste manier om de database lokaal te starten.

1.  **Navigeer naar de Project Root:** Open je terminal en navigeer naar de hoofdmap van dit project. Deze map is de `INKECOEX-Webshop/` directory, waar het bestand `docker-compose.yml` zich bevindt.

    ```
    cd /pad/naar/INKECOEX-Webshop/
    ```

2.  **Start de Database Container:** Voer het volgende commando uit. Dit zal de Docker-container downloaden, opstarten en het netwerk aanmaken.

    ```
    docker compose up -d
    ```

    * De optie `-d` staat voor "detached" en zorgt ervoor dat de container op de achtergrond blijft draaien.

3.  **Verifieer de Database:** Je kunt nu in Docker Desktop zien dat een nieuwe container (bijvoorbeeld `webshopservice-db`) is aangemaakt en de status "Running" heeft.

## 4\. De Backend Service Draaien (`WebshopService`)

Dit is de Web API (de backend) die de data levert. Deze moet eerst draaien voordat de frontend kan communiceren.

1.  **Navigeer naar de Service Map:** Ga in je terminal naar de map van het backend-project (`WebshopService`).

    ```
    cd WebshopService
    ```

2.  **Voer de Applicatie uit:** Gebruik het `dotnet run` commando. Dit commando bouwt de applicatie, past alle Entity Framework Core Migraties toe op de zojuist gestarte database, zaait de testdata en start de service op, meestal op **`http://localhost:5176`**.

    ```
    dotnet run
    ```

    * **Laat deze terminal open staan en actief\!** De service moet blijven draaien.

## 5\. De Frontend Applicatie Draaien (`WebshopFrontend`)

Nu de backend draait, kunnen we de Blazor-frontend opstarten.

1.  **Open een Tweede Terminal:** Dit is essentieel omdat de backend-terminal nog actief moet zijn.

2.  **Navigeer naar de Frontend Map:** Ga in de nieuwe terminal naar de map van het frontend-project (`WebshopFrontend`).

    ```
    cd ..
    cd WebshopFrontend
    ```

3.  **Voer de Applicatie uit:** Gebruik het `dotnet run` commando. Dit start de Blazor-applicatie op, meestal op **`http://localhost:5000`** (of een andere beschikbare poort).

    ```
    dotnet run
    ```

4.  **Verificatie:** De terminal zal berichten tonen over het opstarten van de frontend. Je kunt nu de webshop openen in je browser via de aangegeven URL (bijv. `http://localhost:5000`).

**Gefeliciteerd\!** Je hebt zowel de backend (`WebshopService`) als de frontend (`WebshopFrontend`) nu succesvol lokaal draaiende