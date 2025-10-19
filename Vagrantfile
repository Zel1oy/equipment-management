# -*- mode: ruby -*-
# vi: set ft=ruby :

# This is the main configuration block for Vagrant.
Vagrant.configure("2") do |config|
  # 1. Define the Virtual Machine
  # We'll use a generic Ubuntu 22.04 box that is compatible with ARM64.
  # Vagrant will automatically download the correct version for Parallels.
  config.vm.box = "generic/ubuntu2204"

  # Configure the provider (Parallels) with some resources.
  config.vm.provider "parallels" do |prl|
    prl.memory = "2048" # Allocate 2 GB of RAM
    prl.cpus = 2         # Allocate 2 CPU cores
  end

  # 2. Provision the Virtual Machine
  # This shell script runs automatically the first time you run "vagrant up".
  # It sets up the entire environment from scratch.
  config.vm.provision "shell", inline: <<-SHELL
    echo "========= 🚀 Starting VM Provisioning ========="

    # Update the package list to get the latest versions.
    sudo apt-get update -y

    # Install the .NET 8 SDK.
    # Microsoft's official script automatically detects the ARM64 architecture.
    echo "=========  dotnet Installing .NET 8 SDK ========="
    wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    sudo apt-get update -y
    sudo apt-get install -y dotnet-sdk-8.0

    # Install Git.
    echo "========= git Installing Git ========="
    sudo apt-get install -y git

    # Clone your Lab 1 project from GitHub.
    # This ensures we're deploying the latest version of your code.
    echo "========= 📂 Cloning the Project Repository ========="
    git clone https://github.com/Zel1oy/equipment-management.git /home/vagrant/app

    # Run the application.
    echo "========= ✅ Building and Running the Application ========="
    cd /home/vagrant/app/PstInventory.ConsoleApp
    dotnet run
  SHELL
end