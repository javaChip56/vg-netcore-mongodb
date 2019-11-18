
Vagrant.configure("2") do |config|

  config.vm.define "netcoremongo_host" do |netcoremongo_host|
    netcoremongo_host.vm.box = "ubuntu/xenial64"

    ################################################################################################
    # Port fowarding rules
    # Change port 27017 to a different port if you already have MongoDb running on your host machine 
    # as this is lkely to cause conflicts.
    ################################################################################################
    # MongoDb Server
    netcoremongo_host.vm.network "forwarded_port", guest: 27017, host: 27020
    # Client API
    netcoremongo_host.vm.network "forwarded_port", guest: 8080, host: 8080
   
    netcoremongo_host.vm.network "private_network", ip: "192.168.3.34" 
    
    # Uncomment the line below if the guest OS requires connection to any local / network resource.
    # netcoremongo_host.vm.network "public_network", use_dhcp_assigned_default_route: true
    
    # This current directory will be synced to /mnt/host folder on the guest VM.
    netcoremongo_host.vm.synced_folder ".", "/mnt/host" 

    netcoremongo_host.vm.provider :virtualbox do |vb|
      vb.name = "netcoremongo_host"
      vb.memory = "4048"
      vb.cpus = 2
    end

    # Add run: "always" argument to force the provisioner to be ran everytime vagrant up is called.
    netcoremongo_host.vm.provision "shell" do |s|
      s.path = "provision.sh"
      # Parameters (VAGRANT_HOST_DIR, BUILD_CLIENT_API, ROOT_USERNAME, ROOT_PASSWORD)
      s.args = "/mnt/host false root D0cker123"
    end

  end

 end