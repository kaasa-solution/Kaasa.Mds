#! /bin/bash

PROJECT_NAME=Kaasa.Mds
BUILD_FOLDER=$PROJECT_NAME/build
NET_PROJECT=$PROJECT_NAME.NET.csproj
XAMARIN_PROJECT=$PROJECT_NAME.Xamarin.csproj

rm -rf $BUILD_FOLDER
rm -rf $PROJECT_NAME/obj

dotnet build $PROJECT_NAME/$NET_PROJECT -c Release

rm -rf $PROJECT_NAME/obj

msbuild $PROJECT_NAME/$XAMARIN_PROJECT -t:Rebuild -restore:True -p:Configuration=Release

nuget pack $PROJECT_NAME.nuspec