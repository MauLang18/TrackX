#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y tzdata
RUN mkdir -p /app/wwwroot && \
    chmod 755 /app/wwwroot
WORKDIR /app
EXPOSE 8080
ENV TZ=America/Costa_Rica

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TrackX.Api/TrackX.Api.csproj", "TrackX.Api/"]
COPY ["TrackX.Application/TrackX.Application.csproj", "TrackX.Application/"]
COPY ["TrackX.Domain/TrackX.Domain.csproj", "TrackX.Domain/"]
COPY ["TrackX.Infrastructure/TrackX.Infrastructure.csproj", "TrackX.Infrastructure/"]
COPY ["TrackX.Utilities/TrackX.Utilities.csproj", "TrackX.Utilities/"]
RUN dotnet restore "./TrackX.Api/TrackX.Api.csproj"
COPY . .
WORKDIR "/src/TrackX.Api"
RUN dotnet build "./TrackX.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TrackX.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN chmod 755 /app
EXPOSE 8080
ENTRYPOINT ["dotnet", "TrackX.Api.dll"]