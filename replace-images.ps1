$replacements = @{
    "https://images.unsplash.com/photo-1554224155-6726b3ff858f?w=800&q=80" = "~/images/vitrum/glass-dollar-symbol.webp"
    "https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=800&q=80" = "~/images/vitrum/glass-pie-chart.webp"
    "https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=700&q=80" = "~/images/vitrum/glass-pie-chart.webp"
    "https://images.unsplash.com/photo-1504307651254-35680f356dfd?w=800&q=80" = "~/images/vitrum/glass-geotag-sign.webp"
    "https://images.unsplash.com/photo-1541888946425-d81bb19240f5?w=800&q=80" = "~/images/vitrum/glass-shield.webp"
    "https://images.unsplash.com/photo-1552664730-d307ca884978?w=800&q=80" = "~/images/vitrum/blue-purple-glass-heart.webp"
    "https://images.unsplash.com/photo-1600880292203-757bb62b4baf?w=800&q=80" = "~/images/vitrum/glass-megaphone.webp"
    "https://images.unsplash.com/photo-1568667256549-094345857637?w=800&q=80" = "~/images/vitrum/glass-cloud-icon.webp"
    "https://images.unsplash.com/photo-1507925921958-8a62f3d1a50d?w=800&q=80" = "~/images/vitrum/glass-cloud-with-synchronization-arrows.webp"
    "https://images.unsplash.com/photo-1586528116311-ad8dd3c8310d?w=800&q=80" = "~/images/vitrum/glass-gift-box.webp"
    "https://images.unsplash.com/photo-1450101499163-c8848c66ca85?w=800&q=80" = "~/images/vitrum/glass-check-mark.webp"
    "https://images.unsplash.com/photo-1519494026892-80bbd2d6fd0d?w=800&q=80" = "~/images/vitrum/yellow-pink-glass-heart.webp"
    "https://images.unsplash.com/photo-1581093458791-9d42e3c7e117?w=800&q=80" = "~/images/vitrum/glass-magnifying-glass.webp"
    "https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?w=800&q=80" = "~/images/vitrum/glass-shield.webp"
    "https://images.unsplash.com/photo-1521737604893-d14cc237f11d?w=800&q=80" = "~/images/vitrum/glass-star.webp"
    "https://images.unsplash.com/photo-1522071820081-009f0129c71c?w=800&q=80" = "~/images/vitrum/yellow-blue-glass-heart.webp"
    "https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=800&q=80" = "~/images/vitrum/glass-check-mark.webp"
    "https://images.unsplash.com/photo-1601584115197-04ecc0da31d7?w=800&q=80" = "~/images/vitrum/glass-rocket.webp"
    "https://images.unsplash.com/photo-1581091226825-a6a2a5aee158?w=800&q=80" = "~/images/vitrum/glass-rocket-1.webp"
    "https://images.unsplash.com/photo-1565043666747-69f6646db940?w=800&q=80" = "~/images/vitrum/glass-check-mark.webp"
    "https://images.unsplash.com/photo-1553413077-190dd305871c?w=800&q=80" = "~/images/vitrum/glass-earth-globe.webp"
    "https://images.unsplash.com/photo-1578575437130-527eed3abbec?w=800&q=80" = "~/images/vitrum/glass-cloud-with-upload-arrow.webp"
    "https://images.unsplash.com/photo-1523050854058-8df90110c9f1?w=800&q=80" = "~/images/vitrum/glass-rocket.webp"
    "https://images.unsplash.com/photo-1427504494785-3a9ca7044f45?w=800&q=80" = "~/images/vitrum/glass-star.webp"
    "https://images.unsplash.com/photo-1434030216411-0b793f4b4173?w=800&q=80" = "~/images/vitrum/glass-check-mark.webp"
}

$files = Get-ChildItem -Path "f:\Users\Administrator\Documents\GitHub\PindahWebsite3\Views" -Filter "*.cshtml" -Recurse

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $modified = $false

    foreach ($key in $replacements.Keys) {
        if ($content -match \[regex]::Escape($key)) {
            $content = $content -replace \[regex]::Escape($key), $replacements[$key]
            $modified = $true
        }
    }

    if ($modified) {
        Set-Content -Path $file.FullName -Value $content -NoNewline
        Write-Host "Updated $($file.FullName)"
    }
}
