param($buildConfig)
try{
    Write-Host "Building wrapped projects: Started at " (Get-Date).ToString()
	Write-Host "Build Configuration: "$buildConfig
    dnvm upgrade

	cd "..\..\wrap\"

	Get-ChildItem | ForEach-Object{
		Write-Host $_.Name
		cd $_.Name
		dnu restore
		dnu build --configuration $buildConfig
		ls
		cd ..
	}

    Write-Host "Building Wrapped projects finished at " (Get-Date).ToString()
}
catch [System.Exception]{
    Write-Host "Wrapping process caught an exception"
}