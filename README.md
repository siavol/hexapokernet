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

## WebApi app configuration

Web API application can be configured with the following environment variables:
- `HPN_WRITABLE_REPO` - _Optional._ Defines the repository kind. Can 
   be `"InMemory"` or `"Kafka"`. Default is `"InMemory"`.
- `HPN_KAFKA_SERVER` - _Optional._ Kafka server URL. It is used only when
  Kafka repository is used. Default is `"localhost:9092"`.

## Run with Docker

### Run with in-memory repository

This is the easiest way to run the app. Follow the steps:

1. Build Web API Docker image
    ```sh
    docker build . --tag hexapokernet-webapi:dev
    ```
2. Run Docker container
    ```sh
    docker run --rm --name HexaPokerNet-WebApi --publish 8080:80 hexapokernet-webapi:dev
    ```
3. Open http://localhost:8080/swagger in browser and play with API.

### Run with Kafka repository

1. Run Web API with Kafka storage:
    ```sh
   docker compose -f ./docker-compose.kafka.yml up -d --build --force-recreate hexapokernet-webapi 
    ```
2. Open http://localhost:8080/swagger in browser and play with API.
