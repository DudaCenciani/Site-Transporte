# Imagem base do .NET SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia arquivos de projeto e restaura dependências
COPY ./*.sln .
COPY ./AgendamentoTransporte/*.csproj ./AgendamentoTransporte/
RUN dotnet restore

# Copia o restante dos arquivos e publica
COPY . ./
WORKDIR /app/AgendamentoTransporte
RUN dotnet publish -c Release -o out

# Imagem base para rodar
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/AgendamentoTransporte/out .

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "AgendamentoTransporte.dll"]
