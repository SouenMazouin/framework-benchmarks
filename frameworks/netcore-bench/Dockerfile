
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY NetCoreBench/*.csproj ./NetCoreBench/
RUN dotnet restore

# copy everything else and build app
COPY NetCoreBench/. ./NetCoreBench/
WORKDIR /app/NetCoreBench
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime

EXPOSE 6565
EXPOSE 5000

WORKDIR /app
COPY --from=build /app/NetCoreBench/out ./
ENTRYPOINT ["dotnet", "NetCoreBench.dll"]
