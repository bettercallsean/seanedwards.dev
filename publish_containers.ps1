$scriptDirectory = Get-Location

Set-Location "MySite"
dotnet publish --os linux --arch arm64 /t:PublishContainer -c Release -p:ContainerFamily=jammy-chiseled
docker tag mysite raspberrypi.local:9298/mysite
docker push raspberrypi.local:9298/mysite

Set-Location $scriptDirectory

Set-Location "MySite.API"
dotnet publish --os linux --arch arm64 /t:PublishContainer -c Release -p:ContainerFamily=jammy-chiseled
docker tag mysite-api raspberrypi.local:9298/mysite-api
docker push raspberrypi.local:9298/mysite-api

ssh pi 'cd ~/MySite && docker compose pull && docker-compose up -d --no-build'
