FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ProjectDEV.csproj", "./"]
RUN dotnet restore "ProjectDEV.csproj"
COPY . .
RUN dotnet build "ProjectDEV.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectDEV.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectDEV.dll"] 