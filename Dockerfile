#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/localtour.Web.Host/localtour.Web.Host.csproj", "src/localtour.Web.Host/"]
COPY ["src/localtour.Web.Core/localtour.Web.Core.csproj", "src/localtour.Web.Core/"]
COPY ["src/localtour.Application/localtour.Application.csproj", "src/localtour.Application/"]
COPY ["src/localtour.Core/localtour.Core.csproj", "src/localtour.Core/"]
COPY ["src/localtour.EntityFrameworkCore/localtour.EntityFrameworkCore.csproj", "src/localtour.EntityFrameworkCore/"]
RUN dotnet restore "src/localtour.Web.Host/localtour.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/localtour.Web.Host"
RUN dotnet build "localtour.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_12.x | bash \
    && apt-get install nodejs -yq
RUN npm install -g yarn
RUN dotnet publish "localtour.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update -yq \
    && apt-get install -yq libc6-dev \
    && apt-get install -yq libgdiplus
ENTRYPOINT ["dotnet", "localtour.Web.Host.dll"]
