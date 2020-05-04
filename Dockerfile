#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
RUN apt-get update
RUN apt-get install -y libgdiplus libc6-dev
RUN apt-get install -y libicu-dev libharfbuzz0b libfontconfig1 libfreetype6
RUN apt-get install fontconfig
RUN mkdir -p /usr/share/fonts/truetype/jh
COPY ["Fonts/*.ttc", "/usr/share/fonts/truetype/jh/"] 
RUN chmod 644 /usr/share/fonts/truetype/jh/*  
RUN fc-cache -fv
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["XGGrid_Docker.csproj", ""]
RUN dotnet restore "./XGGrid_Docker.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "XGGrid_Docker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XGGrid_Docker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XGGrid_Docker.dll"]