$($MyInvocation.MyCommand.Source)

function Script:SetGameDevelopmentRootDirectories()
{
	$global:UnityProjectsRoot="${global:WorkspaceRoot}\Unity\"
	$global:UnityCommonsRoot="${global:CommonsRoot}\Unity\"
	$global:PamuxUnityLibraryRoot="${global:UnityCommonsRoot}\Pamux\"
	$global:ImportedAssetsRoot="${global:UnityProjectsRoot}\Imports\"

	$global:ImportsOriginalsRoot="${global:ImportedAssetsRoot}\Originals\"
	$global:ImportsAlteredRoot="${global:ImportedAssetsRoot}\Modified\"

	$global:ImportedUnityAssetsRoot="${global:ImportsOriginalsRoot}\Unity\"
	$global:ImportedStandardAssetsRoot="${global:ImportedUnityAssetsRoot}\Standard Assets\"
	$global:Imported3rdPartyAssetsRoot="${global:ImportsOriginalsRoot}\3rdParty\"

	$global:ZodiacRoot="${global:UnityProjectsRoot}\Zodiac\"
	
	$global:UnityAppDataRoot="${env:APPDATA}\Unity"

	$global:UnityAssetStoreRoot="${global:UnityAppDataRoot}\Asset Store\"
}

function Script:SetEngToolsPaths()
{
	$global:EngTools_KBRoot = "${global:EngTools_Root}KB\"
	$global:EngTools_TempRoot = "${global:EngTools_Root}Temp\"
	$global:EngTools_ToolsRoot = "${global:EngTools_Root}Tools\"
	$global:EngTools_ScriptsRoot = "${global:EngTools_Root}Scripts\"
	$global:EngTools_DataRoot = "${global:EngTools_Root}Data\"
	$global:EngTools_ConfigRoot = "${global:EngTools_Root}Config\"
	$global:EngTools_DocsRoot = "${global:EngTools_Root}Docs\"
	$global:EngTools_LogsRoot = "${global:EngTools_Root}Logs\"
	
	$global:EngTools_SysInternalsRoot="${global:EngTools_ToolsRoot}\SysinternalsSuite"
	
	$global:EngTools_AliasesFile = "${global:EngTools_ConfigRoot}Aliases.txt"
	
	$global:EngTools_EnvFile = "${global:EngTools_TempRoot}Env.txt"
	$global:EngTools_ErrFile = "${global:EngTools_TempRoot}Err.txt"
	$global:EngTools_WrnFile = "${global:EngTools_TempRoot}Wrn.txt"
	$global:EngTools_LogFile = "${global:EngTools_TempRoot}Log.txt"
}

function Script:SetRootDirectories()
{
	# $global:WorkspaceRoot should be set at WindowsPowerShell\Microsoft.PowerShell_profile.ps1
	$global:OutputRoot = $global:WorkspaceRoot
	$global:AppsRoot = "c:\Apps"
	
	SetEngToolsPaths
	
	$global:ArduinoProjectsRoot="${global:WorkspaceRoot}\Arduino\"
	$global:ThreeDRoot="${global:WorkspaceRoot}\3D\"
	$global:CommonsRoot="${global:WorkspaceRoot}\Common\"

	SetGameDevelopmentRootDirectories
}

function Script:SetToolPaths()
{
	$global:GitRoot="${env:ProgramFiles(x86)}\Git\"

	
	$global:GitExe = "${global:GitRoot}git.exe"
	$global:NuGetExe = "${global:EngTools_ToolsRoot}nuget.exe"
	$global:PuttyExe = "${global:EngTools_ToolsRoot}putty.exe"
	$global:PscpExe = "${global:EngTools_ToolsRoot}pscp.exe"
	$global:NotepadExe = "${env:ProgramFiles(x86)}\Notepad++\notepad++.exe"
	$global:RdpExe = "${env:windir}\system32\mstsc.exe"
	$global:UnityExe="${env:ProgramFiles}\Unity\Editor\Unity.exe"
	$global:DevEnvExe="${env:ProgramFiles(x86)}\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe"
	$global:BlenderExe="${env:ProgramFiles}\Blender Foundation\Blender\blender.exe"
	$global:SketchUpExe="${env:ProgramFiles}\SketchUp\SketchUp 2015\SketchUp.exe"
	$global:PaintDotNetExe="${env:ProgramFiles}\paint.net\PaintDotNet.exe"
	$global:KDiffExe="${env:ProgramFiles}\KDiff3\kdiff3.exe"
	
	$global:SevenZExe ="${env:ProgramFiles}\7-Zip\7z.exe"
	$global:GzipExe ="${global:AppsRoot}\gzip-1.3.12-1-bin\bin\gzip.exe"
	$global:GzipExe ="${global:AppsRoot}\gnu\bin\gzip.exe"
	$global:TarExe ="${global:AppsRoot}\gnu\bin\tar.exe"
	
}

function Script:SetRaspberryPiGlobals()
{
	$global:RaspberryPiRdpFilePath = "${global:EngTools_ConfigRoot}RaspberryPi.rdp"
	
	
	# All raspberry Pi boards have MAC address starting with "B8-27-EB"
	# B8-27-EB-BB-4F-66	
	$global:RaspberryPi_MacPrefix = "B8-27-EB"
}

function Script:SetGlobals()
{
	SetRootDirectories
	SetToolPaths	
	SetRaspberryPiGlobals
}
