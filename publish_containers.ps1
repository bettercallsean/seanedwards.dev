$scriptDirectory = Get-Location

Set-Location "MySite"
dotnet publish --os linux --arch arm64 /t:PublishContainer -c Release -p:ContainerFamily=jammy-chiseled
docker tag mysite 192.168.1.75:9298/mysite
docker push 192.168.1.75:9298/mysite

Set-Location $scriptDirectory

Set-Location "MySite.API"
dotnet publish --os linux --arch arm64 /t:PublishContainer -c Release -p:ContainerFamily=jammy-chiseled
docker tag mysite-api 192.168.1.75:9298/mysite-api
docker push 192.168.1.75:9298/mysite-api

ssh pi 'cd ~/MySite && docker compose pull && docker-compose up -d --no-build'
