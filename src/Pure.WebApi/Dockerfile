FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["src/Pure.Application/", "Pure.Application/"]
COPY ["src/Pure.Domain/", "Pure.Domain/"]
COPY ["src/Pure.Infrastructure/", "Pure.Infrastructure/"]
COPY ["src/Pure.Persistence/", "Pure.Persistence/"]
COPY ["src/Pure.WebApi/", "Pure.WebApi/"]
RUN dotnet restore "Pure.WebApi/Pure.WebApi.csproj"
WORKDIR /src/Pure.WebApi
RUN dotnet publish "Pure.WebApi.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Pure.WebApi.dll"]