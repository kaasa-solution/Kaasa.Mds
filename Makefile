VS_SOLUTION=.\Kaasa.Mds.sln
VS_ROOT=.\Kaasa.Mds

all:
	msbuild $(VS_SOLUTION) -t:Clean -r -t:Rebuild -p:Configuration=Release

ifeq ($(OS),Windows_NT)
	move "$(VS_ROOT)\bin\Release\**.nupkg"
	rmdir /s /q $(VS_ROOT)\bin
	rmdir /s /q $(VS_ROOT)\obj
else
	mv $(VS_ROOT)\bin\Release\Kaasa.Mds.* .
	rm -rf $(VS_ROOT)\bin
	rm -rf $(VS_ROOT)\obj
endif