@echo off

set PROJECT_NAME=Kaasa.Mds
set BUILD_FOLDER=%PROJECT_NAME%/build
set NET_PROJECT=%PROJECT_NAME%.NET.csproj
set XAMARIN_PROJECT=%PROJECT_NAME%.Xamarin.csproj

if exist "%BUILD_FOLDER%" rmdir /s /q "%BUILD_FOLDER%"
if exist "%PROJECT_NAME%/obj" rmdir /s /q "%PROJECT_NAME%/obj"

dotnet build %PROJECT_NAME%/%NET_PROJECT% -c Release

if exist "%PROJECT_NAME%/obj" rmdir /s /q "%PROJECT_NAME%/obj"

msbuild %PROJECT_NAME%/%XAMARIN_PROJECT% -t:Rebuild -restore:True -p:Configuration=Release

nuget pack %PROJECT_NAME.nuspec%