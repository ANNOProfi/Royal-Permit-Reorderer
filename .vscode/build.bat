echo off

REM remove unnecessary assemblies
DEL .\*\Assemblies\*.*

REM build dll
dotnet restore Source/RoyalPermitReorderer.sln
dotnet build Source/RoyalPermitReorderer.sln /p:Configuration=Debug