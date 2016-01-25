function Dnu-Wrap{
    param([String]$path, [String]$projectName)
    cd $path
	Write-Host "dnu wrap "$projectName
    dnu wrap $projectName
}

try{
    Write-Host "Wrapping started at " (Get-Date).ToString()
    dnvm upgrade

    Write-Host "Attempting to wrap QiTwitterDemo.Helper..."
    Dnu-Wrap "..\..\..\Common\OSIsoft.Qi.Helper\OSIsoft.Qi.Helper\" ".\OSIsoft.Qi.Helper.csproj"
    Write-Host "Qi.Helper wrapped successfully"

    Write-Host "Attempting to wrap Qi.Fabric.Monitor.Client..."
    Dnu-Wrap "..\..\OSIsoft.Qi.Fabric.Monitor\OSIsoft.Qi.Fabric.Monitor.Client\" ".\OSIsoft.Qi.Fabric.Monitor.Client.csproj"
    Write-Host "Qi.Fabric.Monitor.Client wrapped successfully"

    Write-Host "Attempting to wrap Qi.Fabric.Monitor.Contract..."
    Dnu-Wrap "..\OSIsoft.Qi.Fabric.Monitor.Contract\" ".\OSIsoft.Qi.Fabric.Monitor.Contract.csproj"
    Write-Host "Qi.Fabric.Monitor.Contract wrapped successfully"

	Write-Host "Attempting to wrap Qi.Fabric.Metrics.Contract..."
    Dnu-Wrap "..\..\OSIsoft.Qi.Fabric.Metrics\OSIsoft.Qi.Fabric.Metrics.Contract\" ".\OSIsoft.Qi.Fabric.Metrics.Contract.csproj"
    Write-Host "Qi.Fabric.Metrics.Contract wrapped successfully"

    cd "..\..\..\"

	if(Test-Path ".\QiOpsPortal\wrap"){
		Remove-Item ".\QiOpsPortal\wrap\*" -Recurse -Force
		Write-Host "Old wraps deleted"
	}
    
	Copy-Item ".\wrap" ".\QiOpsPortal" -Force -Recurse
    Copy-Item ".\Common\OSIsoft.Qi.Helper\wrap" ".\QiOpsPortal" -Force -Recurse

    if(Test-Path ".\wrap"){
        Remove-Item ".\wrap" -Recurse -Force
		Write-Host "Main wraps deleted"
    }
    if(Test-Path ".\global.json"){
        Remove-Item ".\global.json" -Force
		Write-Host "Main global.json deleted"
    }
    if(Test-Path ".\Common\OSIsoft.Qi.Helper\wrap"){
		Write-Host "Helper wrap path exists"
        Remove-Item ".\Common\OSIsoft.Qi.Helper\wrap" -Recurse -Force
		Write-Host "Helper wraps deleted"
    }
    if(Test-Path ".\Common\OSIsoft.Qi.Helper\global.json"){
		Write-Host "Helper global.json path exists"
        Remove-Item ".\Common\OSIsoft.Qi.Helper\global.json" -Force
		Write-Host "Helper global.json deleted"
    }
	
    Write-Host "Wrapping finished at " (Get-Date).ToString()
}
catch [System.Exception]{
    Write-Host "Wrapping process caught an exception"
}
finally{
    if(Test-Path ".\wrap"){
        Remove-Item ".\wrap" -Recurse -Force
    }
    if(Test-Path ".\global.json"){
        Remove-Item ".\global.json"
    }
    if(Test-Path ".\Common\OSIsoft.Qi.Helper\wrap"){
        Remove-Item ".\Common\OSIsoft.Qi.Helper\wrap" -Recurse -Force
    }
    if(Test-Path ".\Common\OSIsoft.Qi.Helper\global.json"){
        Remove-Item ".\Common\OSIsoft.Qi.Helper\global.json"
    }
}