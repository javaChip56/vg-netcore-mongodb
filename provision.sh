#!/bin/bash
VAGRANT_HOST_DIR=$1
BUILD_CLIENT_API=$2
ROOT_USERNAME=$3
ROOT_PASSWORD=$4

echo_parameters()
{
    echo "Parameters received from Vagrantfile: $VAGRANT_HOST_DIR $BUILD_CLIENT_API $ROOT_USERNAME $ROOT_PASSWORD"  
}

img_pull_mongodb()
{
    docker rm -f docker-mongo
    echo "Pulling MongoDb from remote Docker registry."
    # Pulling the image from remote docker registry may take a while. Wait for this command to finish.
    docker pull mongo &
    wait $!
    echo "Successfully pulled MongoDb Docker image."
}

### TODO ###
# img_build_mongodb_nonroot()
# {
#     echo "Building Non-root MongoDb."
#     docker build $VAGRANT_HOST_DIR/docker/sql-nonroot -t sqlserver-2017-nonroot &
#     wait $!
# }
### TODO ###

container_run_mongodb()
{
    echo "Running MongoDb container."
    docker run -d --restart unless-stopped -p 27017:27017 -v mongodata:/data/db -e MONGO_INITDB_ROOT_USERNAME=$ROOT_USERNAME -e MONGO_INITDB_ROOT_PASSWORD=$ROOT_PASSWORD --name docker-mongo mongo:latest &
    wait $!
    sudo usermod -aG docker mongo
}

db_setup()
{
    echo "Setting up Client database."
    docker exec docker-mongo /bin/sh -c 'mkdir /var/opt/imported-collections/' &
    wait $!
    # Copy the DB initialization scripts from host to the container scripts directory.
    docker cp /mnt/host/mongo_scripts/client_collection.json docker-mongo:/var/opt/imported-collections/client_collection.json
    # Run the DB initilization script.
    docker exec docker-mongo /bin/sh -c 'mongoimport --db ClientDB --collection client --authenticationDatabase admin --username root --password D0cker123 --drop --file /var/opt/imported-collections/client_collection.json' &
    wait $!
    echo "MongoDB is ready."
}

reset_database_data()
{
    rm -rf $VAGRANT_HOST_DIR/mongodata/
}

########################
# Update OS
########################
update_os()
{
    apt-get update
    apt-get -y install emacs
    apt-get -y install apt-transport-https ca-certificates
    apt-key adv --keyserver  hkp://keyserver.ubuntu.com:80 --recv-keys 58118E89F3A912897C070ADBF76221572C52609D
}

########################
# Docker
########################
install_docker_engine()
{
    echo "deb https://apt.dockerproject.org/repo ubuntu-xenial main" | sudo tee /etc/apt/sources.list.d/docker.list
    sudo apt-get update
    sudo apt-get -y install linux-image-extra-$(uname -r) linux-image-extra-virtual
    sudo apt-get update
    sudo apt-get -y install linux-image-generic-lts-xenial
    sudo apt-get -y install apache2-utils
    # sudo -- sh -c -e "echo '52.84.227.108   subdomain.domain.com' >> /etc/hosts"
    # apt-get -y install docker-engine
    apt-get -y install docker.io
    # required to prevent masking of docker services.
    sudo systemctl unmask docker.service
    sudo systemctl unmask docker.socket
    sudo systemctl start docker.service
    #
    sudo systemctl enable docker
    sudo usermod -aG docker vagrant
    # 
}

build_client_api()
{
    docker rm -f /docker-client-api
    docker build $VAGRANT_HOST_DIR/api/clientapi -t img-client-api
    docker run -d --name docker-client-api -p 8090:8090 img-client-api:latest
}

########################
# Install NGINX
########################
install_nginx()
{
    docker pull nginx
    docker run -d -p 80:8080 --name docker-nginx nginx
    docker start docker-nginx
}

########################
# Start Provisioning
########################

echo_parameters

echo "Updating OS."
update_os
echo "OS successfully updated."

echo "Installing Docker Engine."
install_docker_engine
echo "Docker Engine successfully installed."

img_pull_mongodb
# img_build_mongodb_nonroot
container_run_mongodb
db_setup

# echo "Resetting the database."
# reset_database_data

if ($BUILD_CLIENT_API = true)
    then
        echo "Build Client API."
        build_client_api
        echo "Successfully built client API in Docker."
    else
        echo "User skipped building Client API."
fi
