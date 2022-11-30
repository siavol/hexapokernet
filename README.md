# hexapokernet

## Quick start

### Pre-requisites

- [.NET SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks)

### Run tests

```sh
dotnet test
```

### Build

```sh
dotnet build
```

## Run with Docker

1. Build Web API Docker image
    ```sh
    docker build . --tag hexapokernet-webapi:dev
    ```
2. Run Docker container
    ```sh
    docker run --rm --name HexaPokerNet-WebApi --publish 8080:80 hexapokernet-webapi:dev
    ```
3. Open http://localhost:8080/swagger in browser and play with API.