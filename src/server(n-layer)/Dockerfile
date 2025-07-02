FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY ["server/TranslationService.Api/TranslationService.Api.csproj", "server/TranslationService.Api/"]
COPY ["server/TranslationService.Application/TranslationService.Application.csproj", "server/TranslationService.Application/"]
COPY ["server/TranslationService.Persistence/TranslationService.Persistence.csproj", "server/TranslationService.Persistence/"]
COPY ["server/SharedKernel/Database/Database.csproj", "server/SharedKernel/Database/"]
COPY ["server/SharedKernel/Domain/Domain.csproj", "server/SharedKernel/Domain/"]
COPY ["server/SharedKernel/Common/Common.csproj", "server/SharedKernel/Common/"]

RUN dotnet restore "server/TranslationService.Api/TranslationService.Api.csproj"

COPY . . 

RUN dotnet publish "server/TranslationService.Api/TranslationService.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
EXPOSE 7000

COPY --from=build /app/publish . 

ENTRYPOINT ["dotnet", "TranslationService.Api.dll"]