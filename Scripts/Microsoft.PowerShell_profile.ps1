$($MyInvocation.MyCommand.Source)

$global:MyDocumentsRoot = [Environment]::GetFolderPath("MyDocuments")


$global:WorkspaceRoot = "${global:MyDocumentsRoot}\Workspace"
$global:WorkspaceRoot = "D:\Workspace"
$global:EngTools_Root = "${global:WorkspaceRoot}\EngTools"
$global:EngTools_Profile = "${global:EngTools_Root}\Pamux.EngTools_profile.ps1"

. $global:EngTools_Profile