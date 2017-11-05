$($MyInvocation.MyCommand.Source)

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

function Global:Shell($commandPath, $parameters)
{
	LogInformational "$commandPath $parameters"
	& $commandPath $parameters
}

function Global:Env()
{
	Get-ChildItem env: >  ${global:EngTools_EnvFile}
	N ${global:EngTools_EnvFile}
}

function Global:ReProfile()
{
	. $PROFILE
}


function Global:ArpShow()
{
  arp -a
}

function Global:GetHostNames()
{

	# https://docs.microsoft.com/en-us/powershell/module/nettcpip/get-netipaddress?view=win10-ps
	$ipConfigs = Get-NetNeighbor -State Reachable
	
	foreach ($ipConfig in $ipConfigs)
	{
		Write-Host $ipConfig.IPAddress

		$hostName = [System.Net.Dns]::GetHostByAddress($ipConfig.IPAddress).Hostname

		Write-Host $hostName
	}
	
	#([system.net.dns]::GetHostByAddress($_)).hostname
}

function Global:LocalIpTable()
{
	Get-NetIPAddress | Format-Table
}

function Global:ReachableNeighbors() {
	Get-NetNeighbor -State Reachable
}


function Global:GetRaspberryPiNetworkInfo()
{
	return $(GetMyNetworkDevices | Select-String $global:RaspberryPi_MacPrefix)
}

function Global:GetRaspberryPiIpAddress()
{
	GetRaspberryPiNetworkInfo |% { $_.ToString().Trim().Split(" ")[0] }
}


function Global:GetWiFiDhcpServerAddress()
{
	$adapterInfo = GetMyWiFiAdapterInfo
	$ipConfig = $(IPConfigAll | Where {  $_.InterfaceIndex -eq $adapterInfo.InterfaceIndex -and $_.DHCPEnabled -eq $true})
	return $ipConfig.DHCPServer
}

function Global:IPConfigAll()
{
	# https://msdn.microsoft.com/en-us/library/aa394217(v=vs.85).aspx
	gwmi -Class Win32_NetworkAdapterConfiguration -ComputerName "LocalHost" | Where {  $_.IPEnabled -eq $true } 
	# | Format-List @{ Label="Computer Name"; Expression= { $_.__SERVER }}, InterfaceIndex, IPEnabled, Description, MACAddress, IPAddress, IPSubnet, DefaultIPGateway, DHCPEnabled, DHCPServer, @{ Label="DHCP Lease Expires"; Expression= { [dateTime]$_.DHCPLeaseExpires }}, @{ Label="DHCP Lease Obtained"; Expression= { [dateTime]$_.DHCPLeaseObtained }}
}


function Global:GetMyWiFiAdapterInfo()
{
	return Get-NetAdapter | ? Name -ne Ethernet
}	
function Global:GetMyWiFiIPv4Info()
{
	return GetMyWiFiAdapterInfo | Get-NetIPAddress | ? AddressFamily -eq IPv4
}	
function Global:GetMyWiFiNetworkDevices()
{
	arp -a -N $(GetMyWiFiIPv4Info).IPAddress
}	
function Global:GetMyEthernetNetworkDevices()
{
	arp -a -N $(GetMyEthernetIPv4Info).IPAddress
}	
function Global:GetMyNetworkDevices()
{
	arp -a
}

function Global:GetMyEthernetAdapterInfo()
{
	return $(Get-NetAdapter Ethernet)
}
function Global:GetMyEthernetIPv4Info()
{
	return GetMyEthernetAdapterInfo |  Get-NetIPAddress | ? AddressFamily -eq IPv4
}

function EnableLinuxSubsystem()
{
# Need elevated shell
	Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux
}