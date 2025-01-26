$scriptDirectory = Get-Location

Set-Location "MySite"
Publish-Container "mysite"

Set-Location $scriptDirectory

Set-Location "MySite.API"
Publish-Container "mysite-api"

ssh $env:SERVER_NAME 'cd ~/MySite && docker compose pull && docker compose up -d'

Set-Location $scriptDirectory
