$($MyInvocation.MyCommand.Source)

function Script:SetAliases()
{
	Set-Alias ReAlias Global:ReProfile -Scope Global
	Set-Alias ReProf Global:ReProfile -Scope Global
	
	#-Option ReadOnly 
	Set-Alias ips LocalIpTable -Scope Global 
	
	Set-Alias SSHPi PuttyPi -Scope Global 
	
	Set-Alias LogInfo LogInformational -Scope Global 
}