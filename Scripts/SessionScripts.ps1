#https://technet.microsoft.com/en-us/library/cc753404.aspx
#Local Group Policy Editor

$scriptDirectory=Split-Path -parent $PSCommandPath
Import-Module "$scriptDirectory\utils.psm1"

Param(
	[Parameter(Mandatory=$True,Position=1)]
	[string]$event
)
#http://docs.unity3d.com/356/Documentation/Manual/CommandLineArguments.html
##http://jpsoft.com/help/mklink.htm
#https://technet.microsoft.com/en-us/library/cc753194.aspx


function Write-Session-Log($path, $msg)
{
	"================================================================================" | Out-File -FilePath $path -Force -Append
	$msg | Out-File -FilePath $path -Append
	Get-Date | Out-File -FilePath $path -Append
	"================================================================================" | Out-File -FilePath $path -Append
	ipconfig | Out-File -FilePath $path -Append
	get-process | Out-File -FilePath $path -Append
	dir env: | Out-File -FilePath $path -Append
	"================================================================================" | Out-File -FilePath $path -Append
}

function Write-Session-Logs($type)
{
	Write-Session-Log "$env:MYLOGS\session.log" $type
	""  | Out-File -FilePath "$env:MYLOGS\last_$type.log" -Force
	Write-Session-Log "$env:MYLOGS\last_$type.log" $type
}

function OnStartup
{
	Write-Session-Logs "STARTUP"
}

function OnLogon
{
	Write-Session-Logs "LOGON"
}

function OnLogoff
{
	Write-Session-Logs "LOGOFF"
}

function OnShutdown
{
	Write-Session-Logs "SHUTDOWN"
}


Invoke-Expression $event