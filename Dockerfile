# Use the .NET 8.0 SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /src

# Copy the csproj and restore the dependencies
COPY ["WebApplication3/WebApplication3.csproj", "WebApplication3/"]
RUN dotnet restore "WebApplication3/WebApplication3.csproj"

# Copy the rest of the application
COPY . .

# Publish the application
RUN dotnet publish "WebApplication3/WebApplication3.csproj" -c Release -o /app/publish

# Use the .NET 8.0 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "WebApplication3.dll"]
