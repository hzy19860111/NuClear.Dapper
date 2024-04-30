@echo off
cls

echo ####################################################################################################################
echo #####

set version=4.0.0
set nugetSource=http://erpnuget.yijiupidev.com/
set nugetApiKey=yjp-erp-nuget-server-2019


::==========================================================================================================================================================================================
::==== NuClear.Dapper.MySql ===========================================================================================================================================================
::==========================================================================================================================================================================================

echo =============================== NuClear.Dapper.MySql ===============================

dotnet clean
dotnet add package NuClear.Dapper  --version %version%
dotnet restore 
dotnet build
dotnet pack -p:PackageVersion=%version% --output nupkgs
dotnet nuget delete NuClear.Dapper.MySql %version% -k %nugetApiKey%  -s %nugetSource% --non-interactive
dotnet nuget push -s %nugetSource% -k %nugetApiKey% nupkgs/NuClear.Dapper.MySql.%version%.nupkg 
dotnet nuget push -s %nugetSource% -k %nugetApiKey% nupkgs/NuClear.Dapper.MySql.%version%.snupkg

PAUSE 





