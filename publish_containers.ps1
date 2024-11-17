$scriptDirectory = Get-Location

Set-Location "MySite"
Publish-Container "mysite"

Set-Location $scriptDirectory

Set-Location "MySite.API"
Publish-Container "mysite-api"

ssh pi 'cd ~/MySite && docker compose pull && docker-compose up -d --no-build'
