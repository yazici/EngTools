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


function Global:Putty(){
	& ${global:PuttyExe} 
}


function Global:PuttyPi(){
	$ip = GetRaspberryPiIpAddress
	& ${global:PuttyExe} -ssh -X pi@$ip -pw raspberry
}

function Global:RDPPi()
{
	$ip = GetRaspberryPiIpAddress
	& ${global:RdpExe} $global:RaspberryPiRdpFilePath /v:$ip /admin /f
}