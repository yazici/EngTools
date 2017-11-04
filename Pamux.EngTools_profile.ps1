$($MyInvocation.MyCommand.Source)

$global:EngTools_ScriptsRoot = "${global:EngTools_Root}\Scripts"

. "${global:EngTools_ScriptsRoot}\Debug.ps1"
. "${global:EngTools_ScriptsRoot}\Globals.ps1"
. "${global:EngTools_ScriptsRoot}\Aliases.ps1"
. "${global:EngTools_ScriptsRoot}\System.ps1"
. "${global:EngTools_ScriptsRoot}\Utils.ps1"
. "${global:EngTools_ScriptsRoot}\ExternalTools.ps1"

function DeployThisCodeAsProfile() 
{
	# THIS IS NOT A REAL FUNCTION, DO NOT CALL THIS
	# THE CODE OF THIS FUNCTION SHOULD BE PUT INTO THE Location
	# POINTED BY $PROFILE VARIABLE
	# 
	# $PROFILE usually is: [Environment]::GetFolderPath("MyDocuments") + "\WindowsPowerShell\Microsoft.PowerShell_profile.ps1"
	# e.g. : 
	# c:\Users\Baris\EngTools\Scripts\Microsoft.PowerShell_profile.ps1
	
	# CUT FROM HERE
	$($MyInvocation.MyCommand.Source)

	$global:MyDocumentsRoot = [Environment]::GetFolderPath("MyDocuments")

	$global:WorkspaceRoot = "${global:MyDocumentsRoot}\Workspace"
	# Override as needed
	$global:WorkspaceRoot = "D:\Workspace"
	$global:EngTools_Root = "${global:WorkspaceRoot}\EngTools"
	$global:EngTools_Profile = "${global:EngTools_Root}\Pamux.EngTools_profile.ps1"

	. $global:EngTools_Profile
	# CUT UNTIL HERE
}

function Main([string[]] $args) 
{
	SetGlobals
	SetAliases
	
	$env:PATH="${env:PATH};${env:SYSINTERNALS_ROOT}"
	
	Set-Location ${global:WorkspaceRoot}
}

Main