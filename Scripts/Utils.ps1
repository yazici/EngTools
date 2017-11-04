$($MyInvocation.MyCommand.Source)

function ConfigureGit() 
{
	git config --global user.name "Baris Yazici"
	git config --global user.email barisinus@hotmail.com
	git config --global core.editor $env:NPP_EXE
	git config --global merge.tool $env:KDIFF_EXE
	git config --list
}

function SetupForProject
{
	$env:START_DIR=${env:WORK_ROOT}

	Message "Welcome to ${env:WORK_TITLE} ${env:WORK_TYPE} PowerShell Window."
	Message ""
	
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

