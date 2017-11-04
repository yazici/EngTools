$($MyInvocation.MyCommand.Source)

function DeployThisCodeAsProfile() 
{
	# THIS IS NOT A REAL FUNCTION, DO NOT CALL THIS
	# THE CODE OF THIS FUNCTION SHOULD BE PUT INTO THE Location
	# POINTED BY $PROFILE VARIABLE
	# 
	# e.g. : 
	# $PROFILE usually is: [Environment]::GetFolderPath("MyDocuments") + "\WindowsPowerShell\Microsoft.PowerShell_profile.ps1"
	
	# CUT FROM HERE
	$($MyInvocation.MyCommand.Source)

	$global:MyDocumentsRoot = [Environment]::GetFolderPath("MyDocuments") + "\"

	$global:WorkspaceRoot = "${global:MyDocumentsRoot}\Workspace\"

	$global:EngTools_Root = "${global:WorkspaceRoot}EngTools\"
	$global:EngTools_Profile = "${global:EngTools_Root}Microsoft.PowerShell_profile.ps1"

	. $global:EngTools_Profile
	# CUT UNTIL HERE
}
