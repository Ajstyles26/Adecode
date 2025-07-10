#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ACUnified.Portal/ACUnified.Portal.csproj", "ACUnified.Portal/"]
COPY ["ACUnified.Business/ACUnified.Business.csproj", "ACUnified.Business/"]
COPY ["ACUnified.Data/ACUnified.Data.csproj", "ACUnified.Data/"]
RUN dotnet restore "./ACUnified.Portal/ACUnified.Portal.csproj"
COPY . .
WORKDIR "/src/ACUnified.Portal"
RUN dotnet build "./ACUnified.Portal.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ACUnified.Portal.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY --from=publish /app/publish/Templates ./Templates
ENTRYPOINT ["dotnet", "ACUnified.Portal.dll"]