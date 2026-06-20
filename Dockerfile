FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY *.slnx .
COPY src/HireHub.Domain/*.csproj src/HireHub.Domain/
COPY src/HireHub.Application/*.csproj src/HireHub.Application/
COPY src/HireHub.Infrastructure/*.csproj src/HireHub.Infrastructure/
COPY src/HireHub.API/*.csproj src/HireHub.API/
RUN dotnet restore

COPY src/ src/
RUN dotnet build -c Release -o /app/build

RUN dotnet publish src/HireHub.API/HireHub.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HireHub.API.dll"]