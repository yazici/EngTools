$($MyInvocation.MyCommand.Source)

function Script:Log($message, $color = "")
{
	Write-Host $message
}

function Global:Message($message)
{
	Log $message
}

function Global:LogDebug($message)
{
	Log "[DEBUG]: $message"
}

function Global:LogInformational($message)
{
	Log "[INFO]: $message"
}

function Global:LogWarning($message)
{
	Log "[WARNING]: $message"
}

function Global:LogError($message)
{
	Log "[ERROR]: $message"
}

function Global:LogCritical($message)
{
	Log "[CRITICAL]: $message"
	Exit -1
}