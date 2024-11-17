$scriptDirectory = Get-Location

Set-Location "MySite"
dotnet publish --os linux --arch arm64 /t:PublishContainer -c Release -p:ContainerFamily=jammy-chiseled
Push-Container("mysite")

Set-Location $scriptDirectory

Set-Location "MySite.API"
dotnet publish --os linux --arch arm64 /t:PublishContainer -c Release -p:ContainerFamily=jammy-chiseled
Push-Container("mysite-api")

ssh pi 'cd ~/MySite && docker compose pull && docker-compose up -d --no-build'
