FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CertificateAuthenticationTest/CertificateAuthenticationTest.csproj", "CertificateAuthenticationTest/"]
RUN dotnet restore "CertificateAuthenticationTest/CertificateAuthenticationTest.csproj"
COPY . .
WORKDIR "/src/CertificateAuthenticationTest"
RUN dotnet build "CertificateAuthenticationTest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CertificateAuthenticationTest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY ["CertificateAuthenticationTest/cert.pem", "./cert.pem"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CertificateAuthenticationTest.dll"]
