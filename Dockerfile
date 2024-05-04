﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LibraryService.csproj", "./"]
RUN dotnet restore "LibraryService.csproj"
COPY . .
WORKDIR "/src/"

RUN dotnet tool restore
RUN dotnet build "LibraryService.csproj" -c $BUILD_CONFIGURATION