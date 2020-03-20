#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SpaceCtrl.Front/SpaceCtrl.Front.csproj", "SpaceCtrl.Front/"]
COPY ["SpaceCtrl.Data/SpaceCtrl.Data.csproj", "SpaceCtrl.Data/"]
RUN dotnet restore "SpaceCtrl.Front/SpaceCtrl.Front.csproj"
COPY . .
WORKDIR "/src/SpaceCtrl.Front"
RUN dotnet build "SpaceCtrl.Front.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpaceCtrl.Front.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpaceCtrl.Front.dll"]