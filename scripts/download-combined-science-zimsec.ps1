# Downloads ZIMSEC O Level Combined Science (4003) question papers — NOT Cambridge.
# Sources: official ZIMSEC (zimsec.co.zw), Sytbay Academy (sytbay.co.zw), Zimsake specimen archive (index 85).
$ErrorActionPreference = 'Continue'
$ProjectRoot = Split-Path $PSScriptRoot -Parent
$Dest = Join-Path $ProjectRoot 'wwwroot\zimsec\o-level\combined-science'
New-Item -ItemType Directory -Force -Path $Dest | Out-Null

function Save-Pdf([string]$Url, [string]$FileName) {
    $dest = Join-Path $Dest $FileName
    if ((Test-Path $dest) -and ((Get-Item $dest).Length -gt 50000)) { return $true }
    try {
        curl.exe -sL --max-time 180 -f -o $dest $Url 2>$null
        if ((Test-Path $dest) -and ((Get-Item $dest).Length -gt 5000)) {
            $header = [System.Text.Encoding]::ASCII.GetString([System.IO.File]::ReadAllBytes($dest)[0..4])
            if ($header.StartsWith('%PDF')) { return $true }
        }
        Remove-Item $dest -Force -ErrorAction SilentlyContinue
    } catch {}
    return $false
}

function Save-OfficialZimsec([string]$PageUrl, [string]$FileName) {
    $html = curl.exe -sL --max-time 120 $PageUrl
    $m = [regex]::Match($html, 'data-downloadurl="([^"]+)"')
    if (-not $m.Success) { return $false }
    $url = $m.Groups[1].Value -replace '&amp;','&'
    return Save-Pdf $url $FileName
}

function Expand-ZimsakeSpecimen([int]$Index = 85) {
    $url = "https://www.zimsake.co.zw/softidrive/download/$Index/sh/295a3866e0738c398f205228f7f7a2c1"
    $zipPath = Join-Path $env:TEMP "zimsec-cs-$Index.zip"
    curl.exe -sL -f -o $zipPath $url 2>$null
    if (-not (Test-Path $zipPath) -or (Get-Item $zipPath).Length -lt 1000) { return 0 }
    $extractDir = Join-Path $env:TEMP "zimsec-cs-$Index"
    if (Test-Path $extractDir) { Remove-Item $extractDir -Recurse -Force }
    Expand-Archive -Path $zipPath -DestinationPath $extractDir -Force
    $count = 0
    foreach ($pdf in Get-ChildItem $extractDir -Filter '*.pdf' -Recurse -File) {
        $target = Join-Path $Dest $pdf.Name
        if (-not (Test-Path $target)) {
            Copy-Item $pdf.FullName $target -Force
            $count++
        }
    }
    Remove-Item $extractDir -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item $zipPath -Force -ErrorAction SilentlyContinue
    return $count
}

Write-Host '=== Official ZIMSEC specimen papers (4003/01–03) ==='
$official = @(
    @{ page = 'https://www5.zimsec.co.zw/download/combined-science-4003-01/'; file = '4003q1-specimen-official.pdf' }
    @{ page = 'https://www5.zimsec.co.zw/download/combined-science-4003-02/'; file = '4003q2-specimen-official.pdf' }
    @{ page = 'https://www5.zimsec.co.zw/download/combined-science-4003-03/'; file = '4003q3-specimen-official.pdf' }
    @{ page = 'https://www5.zimsec.co.zw/download/combined-science-advanced-instructions-4003-03/'; file = '4003q3-advanced-instructions-official.pdf' }
)
foreach ($o in $official) {
    if (Save-OfficialZimsec $o.page $o.file) { Write-Host "  + $($o.file)" }
}

Write-Host '=== Zimsake specimen archive (index 85) ==='
$added = Expand-ZimsakeSpecimen 85
Write-Host "  +$added specimen files"

Write-Host '=== Ernbooks ZIMSEC past papers ==='
$ernbooks = @(
    @{ url = 'https://www.ernbook.com/wp-content/uploads/2024/07/566962273-ZIMSEC-N2021.pdf'; file = '4003q2-nov2021.pdf' }
)
foreach ($e in $ernbooks) {
    if (Save-Pdf $e.url $e.file) { Write-Host "  + $($e.file)" }
}

Write-Host '=== Sytbay ZIMSEC past papers (not specimen) ==='
$sytbayPages = @(
    @{ page = 'https://sytbay.co.zw/download/zimsec-o-level-combined-science-paper-1-november-2019-pdf/'; file = '4003q1-nov2019.pdf' }
    @{ page = 'https://sytbay.co.zw/download/zimsec-o-level-combined-science-paper-2-november-2019-pdf/'; file = '4003q2-nov2019.pdf' }
    @{ page = 'https://sytbay.co.zw/download/zimsec-o-level-combined-science-paper-1-november-2020-pdf/'; file = '4003q1-nov2020.pdf' }
    @{ page = 'https://sytbay.co.zw/download/zimsec-o-level-combined-science-paper-2-november-2020-pdf/'; file = '4003q2-nov2020.pdf' }
    @{ page = 'https://sytbay.co.zw/download/zimsec-o-level-combined-science-paper-1-june-2020-pdf/'; file = '4003q1-june2020.pdf' }
    @{ page = 'https://sytbay.co.zw/download/zimsec-o-level-combined-science-paper-2-june-2020-pdf/'; file = '4003q2-june2020.pdf' }
    @{ page = 'https://sytbay.co.zw/download/zimsec-o-level-combined-science-paper-1-june-2019-pdf/'; file = '4003q1-june2019.pdf' }
    @{ page = 'https://sytbay.co.zw/download/zimsec-o-level-combined-science-paper-2-june-2019-pdf/'; file = '4003q2-june2019.pdf' }
)
foreach ($s in $sytbayPages) {
    $html = curl.exe -sL --max-time 120 $s.page
    $m = [regex]::Match($html, 'data-downloadurl="([^"]+)"')
    if ($m.Success -and (Save-Pdf ($m.Groups[1].Value -replace '&amp;','&') $s.file)) {
        Write-Host "  + $($s.file)"
    }
}

# Remove any Cambridge fillers if present
Get-ChildItem $Dest -Filter 'cambridge*.pdf' -ErrorAction SilentlyContinue | Remove-Item -Force

Write-Host "`n=== combined-science library ==="
Get-ChildItem $Dest -Filter '*.pdf' | Sort-Object Name | Format-Table Name, Length -AutoSize
Write-Host "Total: $((Get-ChildItem $Dest -Filter '*.pdf').Count) ZIMSEC PDFs"
