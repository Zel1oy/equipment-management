# Step 1: Start with an official base image that already has the .NET 8 SDK and Linux.
# This saves us from having to install .NET manually.
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Step 2: Install Git inside the container.
# The 'RUN' command executes shell commands during the build process.
RUN apt-get update && apt-get install -y git

# Step 3: Set the working directory inside the container.
WORKDIR /app

# Step 4: Clone your project repository into the container's /app directory.
RUN git clone https://github.com/Zel1oy/equipment-management.git .

# Step 5: Set the default command to run when the container starts.
# This will change into your console app's directory and run it.
CMD ["sh", "-c", "cd PstInventory.ConsoleUI && dotnet run"]