# Use the official .NET SDK as a base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files to the container
COPY . ./

# Build the application
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output from the build image
COPY --from=build /app/out ./

# Expose the port the app will run on
EXPOSE 80

# Set environment variables for the database connection
ENV ConnectionStrings__DefaultConnection "Server=localhost;Initial Catalog=master;User=SA;Password=reallyStrongPwd123;TrustServerCertificate=true"
# Start the application
ENTRYPOINT ["dotnet", "Lab3SD.dll"]
