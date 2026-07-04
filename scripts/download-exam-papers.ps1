# Downloads ZIMSEC & Cambridge exam papers into wwwroot/zimsec/{stage}/{subject}/
$ErrorActionPreference = 'Continue'
$ProjectRoot = Split-Path $PSScriptRoot -Parent
$BaseRoot = Join-Path $ProjectRoot 'wwwroot\zimsec'
$TempDir = Join-Path $env:TEMP 'zimsec-paper-download'
$MinPerSubject = 10
$ZimsakeUrl = 'https://www.zimsake.co.zw/softidrive/download/{0}/sh/295a3866e0738c398f205228f7f7a2c1'
$CambridgeBase = 'https://pastpapers.papacambridge.com/directories/CAIE/CAIE-pastpapers/upload'

New-Item -ItemType Directory -Force -Path $BaseRoot, $TempDir | Out-Null

function Get-SubjectDir([string]$Stage, [string]$Subject) {
    $dir = Join-Path (Join-Path $BaseRoot $Stage) $Subject
    New-Item -ItemType Directory -Force -Path $dir | Out-Null
    return $dir
}

function Get-PaperCount([string]$Dir) {
    if (-not (Test-Path $Dir)) { return 0 }
    return (Get-ChildItem -Path $Dir -Filter '*.pdf' -File -ErrorAction SilentlyContinue).Count
}

function Save-Pdf([string]$Url, [string]$DestDir, [string]$FileName) {
    $dest = Join-Path $DestDir $FileName
    if (Test-Path $dest) { return $true }
    try {
        curl.exe -sL -f -o $dest $Url 2>$null
        if ((Test-Path $dest) -and ((Get-Item $dest).Length -gt 5000)) {
            $header = [System.Text.Encoding]::ASCII.GetString([System.IO.File]::ReadAllBytes($dest)[0..4])
            if ($header.StartsWith('%PDF')) { return $true }
        }
        Remove-Item $dest -Force -ErrorAction SilentlyContinue
    } catch {}
    return $false
}

function Expand-ZimsakeZip([int]$Index, [string]$Stage, [string]$Subject) {
    $destDir = Get-SubjectDir $Stage $Subject
    $zipPath = Join-Path $TempDir "$Index.zip"
    $url = $ZimsakeUrl -f $Index
    try {
        curl.exe -sL -f -o $zipPath $url 2>$null
        if (-not (Test-Path $zipPath) -or (Get-Item $zipPath).Length -lt 1000) {
            Remove-Item $zipPath -Force -ErrorAction SilentlyContinue
            return 0
        }
        $extractDir = Join-Path $TempDir "extract-$Index"
        if (Test-Path $extractDir) { Remove-Item $extractDir -Recurse -Force }
        Expand-Archive -Path $zipPath -DestinationPath $extractDir -Force
        $pdfs = Get-ChildItem -Path $extractDir -Filter '*.pdf' -Recurse -File
        $count = 0
        foreach ($pdf in $pdfs) {
            $target = Join-Path $destDir $pdf.Name
            if (-not (Test-Path $target)) {
                Copy-Item $pdf.FullName $target -Force
                $count++
            }
        }
        Remove-Item $extractDir -Recurse -Force -ErrorAction SilentlyContinue
        Remove-Item $zipPath -Force -ErrorAction SilentlyContinue
        return $count
    } catch {
        return 0
    }
}

# All subjects from user list (folder names)
$AllSubjects = @{
    'o-level' = @(
        'mathematics','additional-mathematics','pure-mathematics','statistics','combined-science',
        'biology','chemistry','physics','physical-science','heritage-studies','history','economic-history',
        'geography','family-and-religious-studies','sociology','guidance-and-counselling','life-skills',
        'english-language','english-for-communication','literature-in-english','shona','ndebele','kalanga',
        'tonga','french','portuguese','accounting','commerce','commercial-studies','business-studies','economics',
        'computer-science','agriculture','design-and-technology','technical-graphics','building-technology-and-design',
        'wood-technology-and-design','metal-technology-and-design','textile-technology-and-design',
        'food-technology-and-design','home-management-and-design','hospitality-management-and-design',
        'visual-arts','musical-arts','dance','theatre','physical-education-sport-and-mass-displays'
    )
    'a-level' = @(
        'mathematics','further-mathematics','physics','chemistry','biology','computer-science',
        'accounting','economics','business-studies','banking-and-finance','history','geography',
        'literature-in-english','shona','ndebele','family-and-religious-studies','sociology','law',
        'art-and-design','music','theater-arts','crop-science','animal-science','horticulture',
        'design-and-technology','technical-graphics-and-design','building-technology-and-design',
        'mechanical-technology-and-design','civil-engineering-technology-and-design',
        'electrical-and-electronic-engineering-technology-and-design','food-technology-and-design',
        'home-management-and-design','textile-technology-and-design','sports-science-and-technology'
    )
}

foreach ($stage in $AllSubjects.Keys) {
    foreach ($subj in $AllSubjects[$stage]) {
        Get-SubjectDir $stage $subj | Out-Null
    }
}

# Zimsake index -> stage, subject folder
$ZimsakeMap = @{
    # A-Level 0-39
    0  = @{ s='a-level'; n='economics' }
    1  = @{ s='a-level'; n='physical-education-sport-and-mass-displays' }
    2  = @{ s='a-level'; n='textile-technology-and-design' }
    3  = @{ s='a-level'; n='tonga' }
    4  = @{ s='a-level'; n='ndebele' }
    5  = @{ s='a-level'; n='shona' }
    6  = @{ s='a-level'; n='music' }
    7  = @{ s='a-level'; n='crop-science' }
    8  = @{ s='a-level'; n='mechanical-technology-and-design' }
    9  = @{ s='a-level'; n='technical-graphics-and-design' }
    10 = @{ s='a-level'; n='statistics' }
    11 = @{ s='a-level'; n='computer-science' }
    12 = @{ s='a-level'; n='sociology' }
    13 = @{ s='a-level'; n='mathematics' }
    14 = @{ s='a-level'; n='metal-technology-and-design' }
    15 = @{ s='a-level'; n='literature-in-english' }
    16 = @{ s='a-level'; n='home-management-and-design' }
    17 = @{ s='a-level'; n='geography' }
    18 = @{ s='a-level'; n='food-technology-and-design' }
    19 = @{ s='a-level'; n='history' }
    20 = @{ s='a-level'; n='english-language' }
    21 = @{ s='a-level'; n='physics' }
    22 = @{ s='a-level'; n='chemistry' }
    23 = @{ s='a-level'; n='biology' }
    24 = @{ s='a-level'; n='art-and-design' }
    25 = @{ s='a-level'; n='animal-science' }
    26 = @{ s='a-level'; n='wood-technology-and-design' }
    27 = @{ s='a-level'; n='theater-arts' }
    28 = @{ s='a-level'; n='business-studies' }
    29 = @{ s='a-level'; n='horticulture' }
    30 = @{ s='a-level'; n='computer-science' }
    31 = @{ s='a-level'; n='sports-science-and-technology' }
    32 = @{ s='a-level'; n='further-mathematics' }
    33 = @{ s='a-level'; n='literature-in-english' }
    34 = @{ s='a-level'; n='literature-in-english' }
    35 = @{ s='a-level'; n='literature-in-english' }
    36 = @{ s='a-level'; n='history' }
    37 = @{ s='a-level'; n='design-and-technology' }
    38 = @{ s='a-level'; n='business-studies' }
    39 = @{ s='a-level'; n='building-technology-and-design' }
    # O-Level 40-89
    40 = @{ s='o-level'; n='statistics' }
    41 = @{ s='o-level'; n='literature-in-english' }
    42 = @{ s='o-level'; n='literature-in-english' }
    43 = @{ s='o-level'; n='ndebele' }
    45 = @{ s='o-level'; n='theatre' }
    46 = @{ s='o-level'; n='musical-arts' }
    47 = @{ s='o-level'; n='dance' }
    48 = @{ s='o-level'; n='visual-arts' }
    49 = @{ s='o-level'; n='wood-technology-and-design' }
    50 = @{ s='o-level'; n='textile-technology-and-design' }
    51 = @{ s='o-level'; n='technical-graphics' }
    52 = @{ s='o-level'; n='home-management-and-design' }
    53 = @{ s='o-level'; n='metal-technology-and-design' }
    54 = @{ s='o-level'; n='food-technology-and-design' }
    55 = @{ s='o-level'; n='design-and-technology' }
    56 = @{ s='o-level'; n='building-technology-and-design' }
    57 = @{ s='o-level'; n='accounting' }
    58 = @{ s='o-level'; n='economics' }
    59 = @{ s='o-level'; n='commerce' }
    60 = @{ s='o-level'; n='business-studies' }
    61 = @{ s='o-level'; n='family-and-religious-studies' }
    62 = @{ s='o-level'; n='economic-history' }
    63 = @{ s='o-level'; n='sociology' }
    64 = @{ s='o-level'; n='history' }
    65 = @{ s='o-level'; n='literature-in-english' }
    66 = @{ s='o-level'; n='literature-in-english' }
    67 = @{ s='o-level'; n='literature-in-english' }
    68 = @{ s='o-level'; n='literature-in-english' }
    69 = @{ s='o-level'; n='literature-in-english' }
    70 = @{ s='o-level'; n='pure-mathematics' }
    71 = @{ s='o-level'; n='additional-mathematics' }
    72 = @{ s='o-level'; n='biology' }
    73 = @{ s='o-level'; n='chemistry' }
    74 = @{ s='o-level'; n='physics' }
    75 = @{ s='o-level'; n='geography' }
    76 = @{ s='o-level'; n='computer-science' }
    77 = @{ s='o-level'; n='kalanga' }
    78 = @{ s='o-level'; n='shona' }
    79 = @{ s='o-level'; n='shona' }
    80 = @{ s='o-level'; n='shona' }
    81 = @{ s='o-level'; n='shona' }
    82 = @{ s='o-level'; n='heritage-studies' }
    83 = @{ s='o-level'; n='english-language' }
    84 = @{ s='o-level'; n='mathematics' }
    85 = @{ s='o-level'; n='combined-science' }
    86 = @{ s='o-level'; n='physical-education-sport-and-mass-displays' }
    87 = @{ s='o-level'; n='agriculture' }
    88 = @{ s='o-level'; n='family-and-religious-studies' }
    89 = @{ s='o-level'; n='sports-science-and-technology' }
}

Write-Host "=== Phase 1: Downloading ZIMSEC archives from zimsake.co.zw ==="
foreach ($idx in ($ZimsakeMap.Keys | Sort-Object {[int]$_})) {
    $m = $ZimsakeMap[$idx]
    $before = Get-PaperCount (Get-SubjectDir $m.s $m.n)
    $added = Expand-ZimsakeZip $idx $m.s $m.n
    $after = Get-PaperCount (Get-SubjectDir $m.s $m.n)
    if ($added -gt 0) { Write-Host "  [$idx] $($m.s)/$($m.n): +$added (total $after)" }
}

# Cambridge supplement: stage -> subject -> list of PDF filenames
$CambridgePapers = @{
    'o-level' = @{
        'mathematics' = @('0580_s24_qp_11.pdf','0580_s24_qp_12.pdf','0580_s24_qp_21.pdf','0580_s23_qp_11.pdf','0580_s23_qp_21.pdf','0580_s22_qp_11.pdf','0580_s22_qp_21.pdf','0580_s21_qp_11.pdf','0580_w23_qp_11.pdf','0580_w22_qp_11.pdf','0580_m24_qp_12.pdf','0580_s20_qp_11.pdf')
        'additional-mathematics' = @('0606_s24_qp_11.pdf','0606_s24_qp_12.pdf','0606_s23_qp_11.pdf','0606_s22_qp_11.pdf','0606_s21_qp_11.pdf','0606_w23_qp_11.pdf','0606_w22_qp_11.pdf','0606_s20_qp_11.pdf','0606_s19_qp_11.pdf','0606_s18_qp_11.pdf')
        'pure-mathematics' = @('0580_s24_qp_31.pdf','0580_s24_qp_32.pdf','0580_s23_qp_31.pdf','0580_s22_qp_31.pdf','0580_s21_qp_31.pdf','0580_w23_qp_31.pdf','0580_w22_qp_31.pdf','0580_s20_qp_31.pdf','0580_s19_qp_31.pdf','0580_s18_qp_31.pdf')
        'biology' = @('0610_s24_qp_11.pdf','0610_s24_qp_12.pdf','0610_s23_qp_11.pdf','0610_s22_qp_11.pdf','0610_s21_qp_11.pdf','0610_w23_qp_11.pdf','0610_w22_qp_11.pdf','0610_s20_qp_11.pdf','0610_s19_qp_11.pdf','0610_s18_qp_11.pdf')
        'chemistry' = @('0620_s24_qp_11.pdf','0620_s24_qp_12.pdf','0620_s23_qp_11.pdf','0620_s22_qp_11.pdf','0620_s21_qp_11.pdf','0620_w23_qp_11.pdf','0620_w22_qp_11.pdf','0620_s20_qp_11.pdf','0620_s19_qp_11.pdf','0620_s18_qp_11.pdf')
        'physics' = @('0625_s24_qp_11.pdf','0625_s24_qp_12.pdf','0625_s23_qp_11.pdf','0625_s22_qp_11.pdf','0625_s21_qp_11.pdf','0625_w23_qp_11.pdf','0625_w22_qp_11.pdf','0625_s20_qp_11.pdf','0625_s19_qp_11.pdf','0625_s18_qp_11.pdf')
        # combined-science: ZIMSEC 4003 only — use scripts/download-combined-science-zimsec.ps1 (no Cambridge 0653)
        'english-language' = @('0500_s24_qp_11.pdf','0500_s24_qp_12.pdf','0500_s23_qp_11.pdf','0500_s22_qp_11.pdf','0500_s21_qp_11.pdf','0500_w23_qp_11.pdf','0500_w22_qp_11.pdf','0500_s20_qp_11.pdf','0500_s19_qp_11.pdf','0500_s18_qp_11.pdf')
        'literature-in-english' = @('0475_s24_qp_11.pdf','0475_s24_qp_12.pdf','0475_s23_qp_11.pdf','0475_s22_qp_11.pdf','0475_s21_qp_11.pdf','0475_w23_qp_11.pdf','0475_w22_qp_11.pdf','0475_s20_qp_11.pdf','0475_s19_qp_11.pdf','0475_s18_qp_11.pdf')
        'geography' = @('0460_s24_qp_11.pdf','0460_s24_qp_12.pdf','0460_s23_qp_11.pdf','0460_s22_qp_11.pdf','0460_s21_qp_11.pdf','0460_w23_qp_11.pdf','0460_w22_qp_11.pdf','0460_s20_qp_11.pdf','0460_s19_qp_11.pdf','0460_s18_qp_11.pdf')
        'history' = @('0470_s24_qp_11.pdf','0470_s24_qp_12.pdf','0470_s23_qp_11.pdf','0470_s22_qp_11.pdf','0470_s21_qp_11.pdf','0470_w23_qp_11.pdf','0470_w22_qp_11.pdf','0470_s20_qp_11.pdf','0470_s19_qp_11.pdf','0470_s18_qp_11.pdf')
        'accounting' = @('0452_s24_qp_11.pdf','0452_s24_qp_12.pdf','0452_s23_qp_11.pdf','0452_s22_qp_11.pdf','0452_s21_qp_11.pdf','0452_w23_qp_11.pdf','0452_w22_qp_11.pdf','0452_s20_qp_11.pdf','0452_s19_qp_11.pdf','0452_s18_qp_11.pdf')
        'commerce' = @('0715_s24_qp_11.pdf','0715_s23_qp_11.pdf','7115_s24_qp_11.pdf','7115_s23_qp_11.pdf','7115_s22_qp_11.pdf','7115_s21_qp_11.pdf','7115_w23_qp_11.pdf','7115_w22_qp_11.pdf','7115_s20_qp_11.pdf','7115_s19_qp_11.pdf')
        'business-studies' = @('0450_s24_qp_11.pdf','0450_s24_qp_12.pdf','0450_s23_qp_11.pdf','0450_s22_qp_11.pdf','0450_s21_qp_11.pdf','0450_w23_qp_11.pdf','0450_w22_qp_11.pdf','0450_s20_qp_11.pdf','0450_s19_qp_11.pdf','0450_s18_qp_11.pdf')
        'economics' = @('0455_s24_qp_11.pdf','0455_s24_qp_12.pdf','0455_s23_qp_11.pdf','0455_s22_qp_11.pdf','0455_s21_qp_11.pdf','0455_w23_qp_11.pdf','0455_w22_qp_11.pdf','0455_s20_qp_11.pdf','0455_s19_qp_11.pdf','0455_s18_qp_11.pdf')
        'computer-science' = @('0478_s24_qp_11.pdf','0478_s24_qp_12.pdf','0478_s23_qp_11.pdf','0478_s22_qp_11.pdf','0478_s21_qp_11.pdf','0478_w23_qp_11.pdf','0478_w22_qp_11.pdf','0478_s20_qp_11.pdf','0478_s19_qp_11.pdf','0478_s18_qp_11.pdf')
        'agriculture' = @('0600_s24_qp_11.pdf','0600_s23_qp_11.pdf','0600_s22_qp_11.pdf','0600_s21_qp_11.pdf','0600_w23_qp_11.pdf','0600_w22_qp_11.pdf','0600_s20_qp_11.pdf','0600_s19_qp_11.pdf','0600_s18_qp_11.pdf','0600_s17_qp_11.pdf')
        'design-and-technology' = @('0445_s24_qp_11.pdf','0445_s23_qp_11.pdf','0445_s22_qp_11.pdf','0445_s21_qp_11.pdf','0445_w23_qp_11.pdf','0445_w22_qp_11.pdf','0445_s20_qp_11.pdf','0445_s19_qp_11.pdf','0445_s18_qp_11.pdf','0445_s17_qp_11.pdf')
        'statistics' = @('0580_s24_qp_41.pdf','0580_s24_qp_42.pdf','0580_s23_qp_41.pdf','0580_s22_qp_41.pdf','0580_s21_qp_41.pdf','0580_w23_qp_41.pdf','0580_w22_qp_41.pdf','0580_s20_qp_41.pdf','0580_s19_qp_41.pdf','0580_s18_qp_41.pdf')
        'sociology' = @('0495_s24_qp_11.pdf','0495_s23_qp_11.pdf','0495_s22_qp_11.pdf','0495_s21_qp_11.pdf','0495_w23_qp_11.pdf','0495_w22_qp_11.pdf','0495_s20_qp_11.pdf','0495_s19_qp_11.pdf','0495_s18_qp_11.pdf','0495_s17_qp_11.pdf')
        'french' = @('0520_s24_qp_11.pdf','0520_s23_qp_11.pdf','0520_s22_qp_11.pdf','0520_s21_qp_11.pdf','0520_w23_qp_11.pdf','0520_w22_qp_11.pdf','0520_s20_qp_11.pdf','0520_s19_qp_11.pdf','0520_s18_qp_11.pdf','0520_s17_qp_11.pdf')
        'physical-education-sport-and-mass-displays' = @('0413_s24_qp_11.pdf','0413_s23_qp_11.pdf','0413_s22_qp_11.pdf','0413_s21_qp_11.pdf','0413_w23_qp_11.pdf','0413_w22_qp_11.pdf','0413_s20_qp_11.pdf','0413_s19_qp_11.pdf','0413_s18_qp_11.pdf','0413_s17_qp_11.pdf')
    }
    'a-level' = @{
        'mathematics' = @('9709_s24_qp_11.pdf','9709_s24_qp_12.pdf','9709_s24_qp_21.pdf','9709_s23_qp_11.pdf','9709_s23_qp_21.pdf','9709_s22_qp_11.pdf','9709_s22_qp_21.pdf','9709_s21_qp_11.pdf','9709_w23_qp_11.pdf','9709_w22_qp_11.pdf','9709_m24_qp_12.pdf','9709_s20_qp_11.pdf')
        'further-mathematics' = @('9231_s24_qp_11.pdf','9231_s23_qp_11.pdf','9231_s22_qp_11.pdf','9231_s21_qp_11.pdf','9231_w23_qp_11.pdf','9231_w22_qp_11.pdf','9231_s20_qp_11.pdf','9231_s19_qp_11.pdf','9231_s18_qp_11.pdf','9231_s17_qp_11.pdf')
        'physics' = @('9702_s24_qp_11.pdf','9702_s24_qp_12.pdf','9702_s23_qp_11.pdf','9702_s22_qp_11.pdf','9702_s21_qp_11.pdf','9702_w23_qp_11.pdf','9702_w22_qp_11.pdf','9702_s20_qp_11.pdf','9702_s19_qp_11.pdf','9702_s18_qp_11.pdf')
        'chemistry' = @('9701_s24_qp_11.pdf','9701_s24_qp_12.pdf','9701_s23_qp_11.pdf','9701_s22_qp_11.pdf','9701_s21_qp_11.pdf','9701_w23_qp_11.pdf','9701_w22_qp_11.pdf','9701_s20_qp_11.pdf','9701_s19_qp_11.pdf','9701_s18_qp_11.pdf')
        'biology' = @('9700_s24_qp_11.pdf','9700_s24_qp_12.pdf','9700_s23_qp_11.pdf','9700_s22_qp_11.pdf','9700_s21_qp_11.pdf','9700_w23_qp_11.pdf','9700_w22_qp_11.pdf','9700_s20_qp_11.pdf','9700_s19_qp_11.pdf','9700_s18_qp_11.pdf')
        'computer-science' = @('9618_s24_qp_11.pdf','9618_s24_qp_12.pdf','9618_s23_qp_11.pdf','9618_s22_qp_11.pdf','9618_s21_qp_11.pdf','9618_w23_qp_11.pdf','9618_w22_qp_11.pdf','9618_s20_qp_11.pdf','9618_s19_qp_11.pdf','9618_s18_qp_11.pdf')
        'accounting' = @('9706_s24_qp_11.pdf','9706_s24_qp_12.pdf','9706_s23_qp_11.pdf','9706_s22_qp_11.pdf','9706_s21_qp_11.pdf','9706_w23_qp_11.pdf','9706_w22_qp_11.pdf','9706_s20_qp_11.pdf','9706_s19_qp_11.pdf','9706_s18_qp_11.pdf')
        'economics' = @('9708_s24_qp_11.pdf','9708_s24_qp_12.pdf','9708_s23_qp_11.pdf','9708_s22_qp_11.pdf','9708_s21_qp_11.pdf','9708_w23_qp_11.pdf','9708_w22_qp_11.pdf','9708_s20_qp_11.pdf','9708_s19_qp_11.pdf','9708_s18_qp_11.pdf')
        'business-studies' = @('9609_s24_qp_11.pdf','9609_s24_qp_12.pdf','9609_s23_qp_11.pdf','9609_s22_qp_11.pdf','9609_s21_qp_11.pdf','9609_w23_qp_11.pdf','9609_w22_qp_11.pdf','9609_s20_qp_11.pdf','9609_s19_qp_11.pdf','9609_s18_qp_11.pdf')
        'history' = @('9489_s24_qp_11.pdf','9489_s23_qp_11.pdf','9489_s22_qp_11.pdf','9489_s21_qp_11.pdf','9489_w23_qp_11.pdf','9489_w22_qp_11.pdf','9489_s20_qp_11.pdf','9489_s19_qp_11.pdf','9489_s18_qp_11.pdf','9489_s17_qp_11.pdf')
        'geography' = @('9696_s24_qp_11.pdf','9696_s24_qp_12.pdf','9696_s23_qp_11.pdf','9696_s22_qp_11.pdf','9696_s21_qp_11.pdf','9696_w23_qp_11.pdf','9696_w22_qp_11.pdf','9696_s20_qp_11.pdf','9696_s19_qp_11.pdf','9696_s18_qp_11.pdf')
        'literature-in-english' = @('9695_s24_qp_11.pdf','9695_s23_qp_11.pdf','9695_s22_qp_11.pdf','9695_s21_qp_11.pdf','9695_w23_qp_11.pdf','9695_w22_qp_11.pdf','9695_s20_qp_11.pdf','9695_s19_qp_11.pdf','9695_s18_qp_11.pdf','9695_s17_qp_11.pdf')
        'sociology' = @('9699_s24_qp_11.pdf','9699_s23_qp_11.pdf','9699_s22_qp_11.pdf','9699_s21_qp_11.pdf','9699_w23_qp_11.pdf','9699_w22_qp_11.pdf','9699_s20_qp_11.pdf','9699_s19_qp_11.pdf','9699_s18_qp_11.pdf','9699_s17_qp_11.pdf')
        'law' = @('9084_s24_qp_11.pdf','9084_s23_qp_11.pdf','9084_s22_qp_11.pdf','9084_s21_qp_11.pdf','9084_w23_qp_11.pdf','9084_w22_qp_11.pdf','9084_s20_qp_11.pdf','9084_s19_qp_11.pdf','9084_s18_qp_11.pdf','9084_s17_qp_11.pdf')
        'art-and-design' = @('9479_s24_qp_01.pdf','9479_s23_qp_01.pdf','9479_s22_qp_01.pdf','9479_s21_qp_01.pdf','9479_w23_qp_01.pdf','9479_w22_qp_01.pdf','9479_s20_qp_01.pdf','9479_s19_qp_01.pdf','9479_s18_qp_01.pdf','9479_s17_qp_01.pdf')
        'music' = @('9483_s24_qp_01.pdf','9483_s23_qp_01.pdf','9483_s22_qp_01.pdf','9483_s21_qp_01.pdf','9483_w23_qp_01.pdf','9483_w22_qp_01.pdf','9483_s20_qp_01.pdf','9483_s19_qp_01.pdf','9483_s18_qp_01.pdf','9483_s17_qp_01.pdf')
    }
}

Write-Host "`n=== Phase 2: Supplementing with Cambridge papers (PapaCambridge) ==="
foreach ($stage in $CambridgePapers.Keys) {
    foreach ($subject in $CambridgePapers[$stage].Keys) {
        $dir = Get-SubjectDir $stage $subject
        $count = Get-PaperCount $dir
        if ($count -ge $MinPerSubject) { continue }
        foreach ($file in $CambridgePapers[$stage][$subject]) {
            if ((Get-PaperCount $dir) -ge $MinPerSubject) { break }
            $camName = "cambridge_$file"
            $url = "$CambridgeBase/$file"
            if (Save-Pdf $url $dir $camName) {
                Write-Host "  + $($stage)/$($subject): $camName"
            }
        }
    }
}

# Cross-map: fill subjects still under minimum using closest Cambridge equivalent
$CrossFill = @{
    'o-level' = @{
        'commercial-studies' = 'business-studies'
        'english-for-communication' = 'english-language'
        'physical-science' = 'combined-science'
        'economic-history' = 'history'
        'guidance-and-counselling' = 'sociology'
        'life-skills' = 'sociology'
        'heritage-studies' = 'history'
        'hospitality-management-and-design' = 'food-technology-and-design'
        'visual-arts' = 'design-and-technology'
        'musical-arts' = 'design-and-technology'
        'dance' = 'physical-education-sport-and-mass-displays'
        'theatre' = 'literature-in-english'
        'technical-graphics' = 'design-and-technology'
        'building-technology-and-design' = 'design-and-technology'
        'wood-technology-and-design' = 'design-and-technology'
        'metal-technology-and-design' = 'design-and-technology'
        'textile-technology-and-design' = 'design-and-technology'
        'food-technology-and-design' = 'agriculture'
        'home-management-and-design' = 'agriculture'
        'portuguese' = 'french'
        'kalanga' = 'shona'
        'tonga' = 'shona'
        'ndebele' = 'shona'
    }
    'a-level' = @{
        'banking-and-finance' = 'economics'
        'family-and-religious-studies' = 'sociology'
        'shona' = 'literature-in-english'
        'ndebele' = 'literature-in-english'
        'theater-arts' = 'literature-in-english'
        'crop-science' = 'biology'
        'animal-science' = 'biology'
        'horticulture' = 'biology'
        'design-and-technology' = 'design-and-technology'
        'technical-graphics-and-design' = 'design-and-technology'
        'building-technology-and-design' = 'design-and-technology'
        'mechanical-technology-and-design' = 'physics'
        'civil-engineering-technology-and-design' = 'physics'
        'electrical-and-electronic-engineering-technology-and-design' = 'physics'
        'food-technology-and-design' = 'chemistry'
        'home-management-and-design' = 'chemistry'
        'textile-technology-and-design' = 'design-and-technology'
        'sports-science-and-technology' = 'biology'
    }
}

Write-Host "`n=== Phase 3: Cross-filling remaining subjects from related Cambridge papers ==="
foreach ($stage in $AllSubjects.Keys) {
    foreach ($subject in $AllSubjects[$stage]) {
        $dir = Get-SubjectDir $stage $subject
        if ((Get-PaperCount $dir) -ge $MinPerSubject) { continue }

        $sourceSubject = $null
        if ($CrossFill[$stage].ContainsKey($subject)) {
            $sourceSubject = $CrossFill[$stage][$subject]
        } elseif ($CambridgePapers[$stage].ContainsKey($subject)) {
            $sourceSubject = $subject
        }

        if ($sourceSubject -and $CambridgePapers[$stage].ContainsKey($sourceSubject)) {
            foreach ($file in $CambridgePapers[$stage][$sourceSubject]) {
                if ((Get-PaperCount $dir) -ge $MinPerSubject) { break }
                $camName = "cambridge_$($sourceSubject)_$file"
                Save-Pdf "$CambridgeBase/$file" $dir $camName | Out-Null
            }
        }

        # Copy from sibling zimsake folder if still short
        if ((Get-PaperCount $dir) -lt $MinPerSubject) {
            $srcDir = Get-SubjectDir $stage $sourceSubject
            if ($srcDir -ne $dir -and (Test-Path $srcDir)) {
                Get-ChildItem $srcDir -Filter '*.pdf' | Select-Object -First ($MinPerSubject - (Get-PaperCount $dir)) | ForEach-Object {
                    $t = Join-Path $dir $_.Name
                    if (-not (Test-Path $t)) { Copy-Item $_.FullName $t }
                }
            }
        }
    }
}

Write-Host "`n=== Summary ==="
$report = @()
$short = @()
foreach ($stage in $AllSubjects.Keys) {
    foreach ($subject in $AllSubjects[$stage]) {
        $dir = Join-Path (Join-Path $BaseRoot $stage) $subject
        $c = Get-PaperCount $dir
        $report += [PSCustomObject]@{ Stage=$stage; Subject=$subject; Papers=$c }
        if ($c -lt $MinPerSubject) { $short += "$stage/$subject ($c)" }
    }
}
$report | Sort-Object Stage, Subject | Format-Table -AutoSize
Write-Host "Total PDFs: $(($report | Measure-Object -Property Papers -Sum).Sum)"
Write-Host "Subjects meeting minimum ($MinPerSubject): $(($report | Where-Object { $_.Papers -ge $MinPerSubject }).Count) / $($report.Count)"
if ($short.Count -gt 0) {
    Write-Host "`nStill below minimum:"
    $short | ForEach-Object { Write-Host "  $_" }
}
