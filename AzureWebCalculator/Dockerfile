FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["AzureWebCalculator/AzureWebCalculator.csproj", "AzureWebCalculator/"]
RUN dotnet restore "AzureWebCalculator/AzureWebCalculator.csproj"
COPY . .
WORKDIR "/src/AzureWebCalculator"
RUN dotnet build "AzureWebCalculator.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AzureWebCalculator.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AzureWebCalculator.dll"]