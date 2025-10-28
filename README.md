# INKECOEX-Webshop
This guide contains the step-by-step instructions for setting up and running the Webshop application locally. We assume you have a clean machine without the necessary software installed.

## 1. Prerequisites

To run this project, you need two main components:

1.  [**.NET 9 SDK**](https://dotnet.microsoft.com/download/dotnet/9.0)**:** Microsoft's software development kit required to build and execute the C# code.

2.  [**Docker Desktop**](https://www.docker.com/products/docker-desktop/)**:** We use Docker to host the PostgreSQL database in a container, ensuring a consistent and easy database environment.

## 2. Installation of Prerequisites

Follow the steps below to install the required software:

### A. Install .NET 9 SDK

1.  Go to the official [.NET 9 SDK download page](https://dotnet.microsoft.com/download/dotnet/9.0).

2.  Download the installer suitable for your operating system (Windows, macOS, or Linux).

3.  Run the installer and follow the instructions. The installation is usually quick and requires no special configurations.

4.  **Verification:** Open your terminal (Command Prompt, PowerShell, or Bash) and type:

    ```
    dotnet --version
    ```

    If the installation was successful, you will now see a version number starting with `9.` (e.g., `9.0.x`).

### B. Install Docker Desktop

1.  Go to the [Docker Desktop download page](https://www.docker.com/products/docker-desktop/).

2.  Download and install Docker Desktop for your system.

3.  Start Docker Desktop after installation. It may take a moment for Docker to fully start. Ensure the Docker icon is stable (this means the Docker Engine is running).

## 3. Local Startup (Recommended: Docker Compose)

The entire project (database, backend, and frontend) can be started with a single command using `docker compose`. This method will build all necessary images, apply database migrations, and start all services in the background.

1.  **Navigate to the Project Root:** Open your terminal and navigate to the root directory of this project. This is the `INKECOEX-Webshop/` directory, where the file `docker-compose.yml` is located.

    ```
    cd /path/to/INKECOEX-Webshop/
    ```

2.  **Start the Entire Project:** Execute the following command. This will download the necessary containers, build the backend and frontend, and start all services (Database, WebshopService, WebshopFrontend).

    ```
    docker compose up -d
    ```

    * The `-d` option stands for "detached" and ensures that the containers continue to run in the background.
    * The Backend service (`WebshopService`) will be running at **`http://localhost:5176`**.
    * The Frontend application (`WebshopFrontend`) will be available in your browser, usually at **`http://localhost:5000`** (or another available port).

3.  **Verification:** You can now check in Docker Desktop that the containers (e.g., `webshopservice-db`, `webshopservice-webshopservice`, and `webshopservice-webshopfrontend`) have been created and are in the "Running" status. You can now open the webshop in your browser via the indicated URL (e.g., `http://localhost:5000`).

**Congratulations!** You have successfully started the entire project locally using Docker Compose.