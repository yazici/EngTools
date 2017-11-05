$($MyInvocation.MyCommand.Source)

function Global:N($path)
{
	Shell ${global:NotepadExe} $path
}

function Global:NErr()
{
	N $global:EngTools_ErrFile
}

function Global:NWrn()
{
	N $global:EngTools_WrnFile
}

function Global:NLog()
{
	N $global:EngTools_LogFile
}


function Global:Putty()
{
	& ${global:PuttyExe} 
}


function Global:PuttyPi()
{
	$ip = GetRaspberryPiIpAddress
	& ${global:PuttyExe} -ssh -X pi@$ip -pw raspberry
}

function Global:RDPPi()
{
	$ip = GetRaspberryPiIpAddress
	& ${global:RdpExe} $global:RaspberryPiRdpFilePath /v:$ip /admin /f
}



function Global:UnpackUnity()
{
	$unityPackagePath = "C:\Users\Baris\AppData\Roaming\Unity\Asset Store-5.x\Agile Reaction\3D ModelsVegetationTrees\Low Poly Style - Tree Pack.unitypackage"
	$outputDirectory = "D:\Workspace\EngTools\Temp\Content"
	New-Item -ItemType Directory -Force -Path "${outputDirectory}"	
	
	& ${global:TarExe} -xvzf "${unityPackagePath}" "${outputDirectory}"
}
function Global:SevenZ($arguments)
{
$commands = @"
	a Add
	b Benchmark
	d Delete
	e Extract
	h Hash
	i Show information about supported formats
	l List
	rn Rename
	t Test
	u Update
	x eXtract with full paths
	
	https://sevenzip.osdn.jp/chm/cmdline/syntax.htm
	
	https://sevenzip.osdn.jp/chm/cmdline/commands/index.htm	
	https://sevenzip.osdn.jp/chm/cmdline/switches/index.htm
"@
	
	$unityPackagePath = "C:\Users\Baris\AppData\Roaming\Unity\Asset Store-5.x\Agile Reaction\3D ModelsVegetationTrees\Low Poly Style - Tree Pack.unitypackage"
	$outputDirectory = "D:\Workspace\EngTools\Temp\Content"
	
	
	
	GzipExe
	
	& ${global:SevenZExe} x "${unityPackagePath}" -o"${outputDirectory}\ar"
	& ${global:SevenZExe} x "${outputDirectory}\ar\*" -o"${outputDirectory}"
}