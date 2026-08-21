$ErrorActionPreference = 'Stop'

Import-Module WebAdministration

$pool = 'pindah-website'
$target = 'F:\Applications\pindah-website'
$staging = 'C:\Applications\pindah-publish-staging-clean'
if (-not (Test-Path $staging)) {
    $staging = 'C:\Applications\pindah-publish-staging'
}

# Fallback if site uses a differently named pool
$site = Get-Item 'IIS:\Sites\pindah-website' -ErrorAction SilentlyContinue
if ($site -and $site.applicationPool) {
    $pool = $site.applicationPool
    if ($site.physicalPath) {
        $target = $site.physicalPath
    }
}

if (-not (Test-Path $staging)) {
    throw "Staging folder not found: $staging"
}

Write-Host "Target: $target"
Write-Host "App pool: $pool"
Write-Host 'Stopping app pool...'
$state = (Get-WebAppPoolState -Name $pool).Value
if ($state -ne 'Stopped') {
    Stop-WebAppPool -Name $pool
    $deadline = (Get-Date).AddSeconds(45)
    do {
        Start-Sleep -Seconds 1
        $state = (Get-WebAppPoolState -Name $pool).Value
    } while ($state -ne 'Stopped' -and (Get-Date) -lt $deadline)

    if ($state -ne 'Stopped') {
        throw "App pool did not stop in time. State: $state"
    }
} else {
    Write-Host 'App pool already stopped.'
}

Write-Host 'Removing stale design-time assemblies if present...'
Get-ChildItem (Join-Path $target '*.dll') -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -match 'CodeGeneration|VisualStudio\.SolutionPersistence|dotnet-aspnet-codegenerator' } |
    ForEach-Object {
        Write-Host "  Removing $($_.Name)"
        Remove-Item $_.FullName -Force
    }

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
