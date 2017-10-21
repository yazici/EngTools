$env:WORKSPACE_ROOT="C:\Workspace"
$env:OUTPUT_ROOT="C:\Workspace"

$env:TOOLS_ROOT="${env:WORKSPACE_ROOT}\Tools"
$env:DATA_ROOT="${env:WORKSPACE_ROOT}\Data"
$env:CONFIG_ROOT="${env:DATA_ROOT}\Config"
$env:DOCS_ROOT="${env:WORKSPACE_ROOT}\Docs"
$env:LOGS_ROOT="${env:OUTPUT_ROOT}\Logs"

$env:UNITY_PROJECTS="${env:WORKSPACE_ROOT}\Unity"
$env:ARDUINO_PROJECTS="${env:WORKSPACE_ROOT}\Arduino"
$env:THREE_D="${env:WORKSPACE_ROOT}\3D"
$env:COMMONS_ROOT="${env:WORKSPACE_ROOT}\Common"
$env:UNITY_COMMONS_ROOT="${env:COMMONS_ROOT}\Unity"
$env:PAMUX_UNITY_LIBRARY="${env:UNITY_COMMONS_ROOT}\Pamux"

$env:NPP_EXE="${env:ProgramFiles(x86)}\Notepad++\notepad++.exe"

$env:UNITY_APPDATA="${env:APPDATA}\Unity"

$env:UNITY4_ASSET_STORE="${env:UNITY_APPDATA}\Asset Store"
$env:UNITY5_ASSET_STORE="${env:UNITY_APPDATA}\Asset Store-5.x"

$env:UNITY4_EXE="${env:ProgramFiles(x86)}\Unity\Editor\Unity.exe"
$env:UNITY5_EXE="${env:ProgramFiles}\Unity\Editor\Unity.exe"

$env:UNITY_ASSET_STORE=$env:UNITY5_ASSET_STORE
$env:UNITY_EXE=${env:UNITY5_EXE}

$env:DEVENV_EXE="${env:ProgramFiles(x86)}\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe"
$env:BLENDER_EXE="${env:ProgramFiles}\Blender Foundation\Blender\blender.exe"
$env:SKETCHUP_EXE="${env:ProgramFiles}\SketchUp\SketchUp 2015\SketchUp.exe"
$env:PAINTNET_EXE="${env:ProgramFiles}\paint.net\PaintDotNet.exe"
$env:KDIFF_EXE="${env:ProgramFiles}\KDiff3\kdiff3.exe"

$env:GIT_ROOT="${env:ProgramFiles(x86)}\Git"

$env:IMPORTED_ASSETS="${env:UNITY_PROJECTS}\Imports"

$env:IMPORTS_ORIGINAL="${env:IMPORTED_ASSETS}\Originals"
$env:IMPORTS_MODIFIED="${env:IMPORTED_ASSETS}\Modified"

$env:IMPORTED_UNITY_ASSETS="${env:IMPORTS_ORIGINAL}\Unity"
$env:IMPORTED_STANDARD_ASSETS="${env:IMPORTED_UNITY_ASSETS}\Standard Assets"
$env:IMPORTED_3RDPARTY_ASSETS="${env:IMPORTS_ORIGINAL}\3rdParty"

$env:SHOOTER_ROOT="${env:UNITY_PROJECTS}\Shooter"

$env:SYSINTERNALS_ROOT="${env:TOOLS_ROOT}\SysinternalsSuite"

git config --global user.name "Baris Yazici"
git config --global user.email barisinus@hotmail.com
git config --global core.editor $env:NPP_EXE
git config --global merge.tool $env:KDIFF_EXE
git config --list

Set-Location ${env:WORKSPACE_ROOT}
$env:PATH="${env:PATH};${env:SYSINTERNALS_ROOT}"
function SetupPowershellAliases
{
	doskey /exename=powershell.exe /MACROFILE="${env:CONFIG_ROOT}\aliases.txt"
}

function LaunchUnity
{
	& $env:UNITY_EXE
}

function LaunchVisualStudio
{
	& $env:DEVENV_EXE
}

function LaunchBlender
{
	& $env:BLENDER_EXE
}

function LaunchSketchUp
{
	& $env:SKETCHUP_EXE
}

function LaunchPaintNET
{
	& $env:PAINTNET_EXE
}

function LaunchTextEditor($path)
{
	& $env:NPP_EXE $path
}

function EditPowershellAliases
{
	LaunchTextEditor "${env:CONFIG_ROOT}\aliases.txt"
}

function SetupForProject
{
	$env:START_DIR=${env:WORK_ROOT}

	Write-Host "Welcome to ${env:WORK_TITLE} ${env:WORK_TYPE} PowerShell Window."
	Write-Host ""
	
	Set-Location -Path ${env:START_DIR}

	SetupPowershellAliases
}

function SetupForUnityProject($name, $title)
{
	$env:WORK_TYPE="Unity"
	$env:WORK_ROOT="${env:UNITY_PROJECTS}\${name}"
	$env:WORK_TITLE=$title

	SetupForProject
}

Function New-SymLink ($link, $target)
{
#write-host $link
#write-host $target
    if (test-path -pathtype container $target)
    {
        $command = "cmd /c mklink /d"
    }
    else
    {
        $command = "cmd /c mklink"
    }

    invoke-expression "$command `"$link`" `"$target`""
}


Function Remove-SymLink ($link)
{
    if (test-path -pathtype container $link)
    {
        $command = "cmd /c rmdir"
    }
    else
    {
        $command = "cmd /c del"
    }

    invoke-expression "$command $link"
}



function CreateProjectSymbolicLinks($projectRoot)
{
	$assetsDir 	= "${projectRoot}\Assets"
	$scriptsDir	= "${assetsDir}\Scripts"
	
	New-SymLink "${env:IMPORTED_3RDPARTY_ASSETS}" 	"C:\Users\Baris\Documents\Unity\Shooter\Assets\3rdParty"
	New-SymLink "${assetsDir}\UnityVS" 			"C:\Users\Baris\Documents\Unity\Shooter\Assets\UnityVS"
	
	New-SymLink "${assetsDir}\3rdParty" 			"${env:IMPORTED_3RDPARTY_ASSETS}"
	New-SymLink "${assetsDir}\Standard Assets" "${env:IMPORTED_STANDARD_ASSETS}"
	
	New-SymLink "${scriptsDir}\Pamux" 				"${env:PAMUX_UNITY_LIBRARY}"

	#& $env:UNITY_EXE -batchMode -quit -importPackage $packagePath
}

