$outputFile = 'ToanBoCode.md'
$content = '# Toàn bộ mã nguồn dự án' + [Environment]::NewLine

$files = Get-ChildItem -Path '.' -Recurse -Filter *.cs | Where-Object { 
    $_.FullName -notmatch '\\obj\\' -and 
    $_.FullName -notmatch '\\bin\\' -and 
    $_.FullName -notmatch '\\Properties\\' -and 
    $_.Name -notmatch '\.Designer\.cs' -and 
    $_.Name -ne 'Program.cs' -and 
    $_.Name -ne 'Class1.cs' 
}

foreach ($f in $files) {
    $relPath = $f.FullName.Substring((Get-Location).Path.Length + 1)
    $content += '## ' + $relPath + [Environment]::NewLine
    $content += '```csharp' + [Environment]::NewLine
    $content += (Get-Content $f.FullName -Raw) + [Environment]::NewLine
    $content += '```' + [Environment]::NewLine + [Environment]::NewLine
}

Set-Content -Path $outputFile -Value $content -Encoding UTF8
Write-Output "Done exporting to $outputFile"
