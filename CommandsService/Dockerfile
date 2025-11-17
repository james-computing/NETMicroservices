# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
#FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
#USER $APP_UID
#WORKDIR /app
#EXPOSE 8180
#EXPOSE 8181


# This stage is used to build the service project
#FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["CommandsService/CommandsService.csproj", "CommandsService/"]
#RUN dotnet restore "./CommandsService/CommandsService.csproj"
#COPY . .
#WORKDIR "/src/CommandsService"
#RUN dotnet build "./CommandsService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./CommandsService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "CommandsService.dll"]

# From .NET Microservices course.
# Start from the .NET sdk, which is needed for building
# the project.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
# Set the working directory as /app in the container
WORKDIR /app

# Copy the project file to /app
COPY *.csproj ./
# Run dotnet restore, which restores the dependencies and tools
# for the project, such as NuGet packages
RUN dotnet restore

# Copy the project folder to /app
COPY . ./
# Publish the project inside the container, in /app/out.
# This compiles the code for production.
RUN dotnet publish -c Release -o out

# Now that the project is already built, we extract from it
# what we need to just run the program.
# We don't need the entire sdk anymore, so start from the
# runtime for ASP.NET.
FROM mcr.microsoft.com/dotnet/aspnet:10.0
# Set again the working directory as /app
WORKDIR /app
# Copy from build-env, from the /app/out directory,
# the published project and write it in /app of the final
# container.
COPY --from=build-env /app/out .
# Configure what the final container will do when it runs.
# It will use dotnet to run the PlatformService.dll.
ENTRYPOINT ["dotnet", "CommandsService.dll"]