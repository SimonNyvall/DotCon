# Use the appropriate base image for building .NET applications on Linux
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the .csproj file(s) and restore the NuGet packages
COPY DotCon/DotCon.csproj .

RUN dotnet restore "DotCon.csproj"

# Copy the entire project directory to the container
COPY . .

# Publish the application
RUN dotnet publish "DotCon/DotCon.csproj" -c Release -o /publish

# Create a new image with the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final

# Set the working directory inside the container
WORKDIR /app

# Copy the published output from the previous stage
COPY --from=build /publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "DotCon.dll"]
