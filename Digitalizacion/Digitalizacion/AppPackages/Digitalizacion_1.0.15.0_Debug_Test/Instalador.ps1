#$source = "https://onedrive.live.com/download?cid=14E1473E78273A8A&resid=14E1473E78273A8A%2115525&authkey=AG_VGu3SoZsxryU"
#$destination = "Instalador.zip"

# Invoke-WebRequest $source -OutFile $destination

#Add-Type -AssemblyName System.IO.Compression.FileSystem
#function Unzip
#{
#    param([string]$zipfile, [string]$outpath)
#
#    [System.IO.Compression.ZipFile]::ExtractToDirectory($PSScriptRoot + "\" + $zipfile, $outpath)
#}

#Unzip $destination "Instalador"

Import-Certificate -FilePath '.\Digitalizacion_1.0.15.0_x64_Debug.cer' -CertStoreLocation 'Cert:\LocalMachine\Root'
Add-AppxPackage .\Dependencies\x64\Microsoft.NET.CoreRuntime.1.0.appx
Add-AppxPackage .\Dependencies\x64\Microsoft.VCLibs.x64.Debug.14.00.appx
Add-AppxPackage .\Dependencies\x64\Microsoft.NET.Native.Runtime.1.1.appx
Add-AppxPackage .\Digitalizacion_1.0.15.0_x64_Debug.appxbundle