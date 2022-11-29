FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
#COPY ["src/HexaPokerNet.WebApi/HexaPokerNet.WebApi.csproj", "HexaPokerNet.WebApi/"]
#RUN dotnet restore "src/HexaPokerNet.WebApi/HexaPokerNet.WebApi.csproj"
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HexaPokerNet.WebApi.dll"]
