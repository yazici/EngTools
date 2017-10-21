$($MyInvocation.MyCommand.Source)

$global:MyDocumentsRoot = [Environment]::GetFolderPath("MyDocuments") + "\"

$global:WorkspaceRoot = "${global:MyDocumentsRoot}\Workspace\"

$global:EngTools_Root = "${global:WorkspaceRoot}EngTools\"
$global:EngTools_Profile = "${global:EngTools_Root}Microsoft.PowerShell_profile.ps1"

. $global:EngTools_Profile