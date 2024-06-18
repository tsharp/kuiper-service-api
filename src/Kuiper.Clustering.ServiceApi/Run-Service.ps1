docker volume create kuiperdb-data
docker build -t helpless .
docker run --rm -p 8080:80 -v kuiperdb-data:/var/lib/kuiperdb helpless:latest