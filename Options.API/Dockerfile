FROM mcr.microsoft.com/dotnet/aspnet:9.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 5191

ENV ASPNETCORE_URLS=http://+:5191

FROM mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-1809 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Options.API/Options.API.csproj", "Options.API/"]
RUN dotnet restore "Options.API\Options.API.csproj"
COPY . .
WORKDIR "/src/Options.API"
RUN dotnet build "Options.API.csproj" -c $configuration -o /app/build


FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Options.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Options.API.dll"]
