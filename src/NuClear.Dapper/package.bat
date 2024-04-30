@echo off
cls

echo ####################################################################################################################
echo #####

set version=4.0.0
set nugetSource=http://erpnuget.yijiupidev.com/
set nugetApiKey=yjp-erp-nuget-server-2019


::==========================================================================================================================================================================================
::==== NuClear.Dapper =================================================================================================================================================================
::==========================================================================================================================================================================================



echo =============================== NuClear.Dapper ===============================
rd /s /q .\bin\
rd /s /q .\obj\
rd /s /q .\nupkgs\
dotnet clean
dotnet restore 
dotnet build
dotnet pack -p:PackageVersion=%version% --output nupkgs
dotnet nuget delete NuClear.Dapper %version% -k %nugetApiKey%  -s %nugetSource% --non-interactive
dotnet nuget push -s %nugetSource% -k %nugetApiKey% nupkgs/NuClear.Dapper.%version%.nupkg 
dotnet nuget push -s %nugetSource% -k %nugetApiKey% nupkgs/NuClear.Dapper.%version%.snupkg

PAUSE 





