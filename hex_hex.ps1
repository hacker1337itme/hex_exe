$filePath = "hex_exe.ps1"
$outputFile = "hex_exe.hex"

# Read all bytes from the file
$bytes = [System.IO.File]::ReadAllBytes($filePath)
$totalBytes = $bytes.Length

# Initialize an empty string for the hex output
$hex = ""

# Process each byte and build the hex string with a progress bar
for ($i = 0; $i -lt $totalBytes; $i++) {
    $hex += "{0:X2}" -f $bytes[$i]
    
    # Calculate the percentage of completion
    $percentComplete = [math]::Round(($i + 1) / $totalBytes * 100)

    # Update the progress bar
    Write-Progress -PercentComplete $percentComplete -Status "Processing..." -CurrentOperation "Converting byte $($i + 1) of $totalBytes"
}

# Write the hex string to the output file
Set-Content -Path $outputFile -Value $hex
