## C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe -NoExit -command "& 'C:\Users\Baris\Documents\MyTools\StartDev_Shooter.ps1' -MyArguments blah"
## https://technet.microsoft.com/en-us/library/hh847736.aspx
## From Administrator command line : c:\windows\syswow64\WindowsPowerShell\v1.0\powershell.exe -command set-executionpolicy unrestricted
##http://www.codeproject.com/Articles/3666/Diff-tool

$scriptDirectory=Split-Path -parent $PSCommandPath
Import-Module -force "${scriptDirectory}\utils.psm1"
SetupPowershellAliases
#SetupForUnityProject "Shooter" "The Zodiac Alliance"

CreateProjectSymbolicLinks ${env:SHOOTER_ROOT}




