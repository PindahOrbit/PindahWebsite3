$ErrorActionPreference = 'Stop'

Import-Module WebAdministration

$pool = 'DefaultAppPool'
$target = 'C:\Applications\pindah'
$staging = 'C:\Applications\pindah-publish-staging'

if (-not (Test-Path $staging)) {
    throw "Staging folder not found: $staging"
}

Write-Host 'Stopping app pool...'
Stop-WebAppPool -Name $pool
Start-Sleep -Seconds 3

Write-Host 'Copying published files...'
robocopy $staging $target /E /XD logs /XF PindahWebsite3.db PindahWebsite3.db-shm PindahWebsite3.db-wal /R:2 /W:2 | Out-Host
if ($LASTEXITCODE -gt 7) {
    throw "Robocopy failed with exit code $LASTEXITCODE"
}

$webConfig = @'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <security>
      <requestFiltering>
        <requestLimits maxAllowedContentLength="209715200" />
      </requestFiltering>
    </security>
    <handlers>
      <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
    </handlers>
    <aspNetCore processPath=".\PindahWebsite3.exe" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess">
      <environmentVariables>
        <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
      </environmentVariables>
    </aspNetCore>
  </system.webServer>
</configuration>
'@

Set-Content -Path (Join-Path $target 'web.config') -Value $webConfig -Encoding UTF8

Write-Host 'Starting app pool...'
Start-WebAppPool -Name $pool

$state = (Get-WebAppPoolState -Name $pool).Value
Write-Host "App pool state: $state"
Write-Host 'Deploy complete.'
