echo off

REM remove unnecessary assemblies
DEL .\*\Assemblies\*.*

REM build dll
dotnet restore 1.5/Source/RoyalPermitReorderer.sln
dotnet build 1.5/Source/RoyalPermitReorderer.sln /p:Configuration=Debug