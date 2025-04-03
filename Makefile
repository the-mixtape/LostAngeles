.PHONY: all clean build

all: clean build

clean:
	@echo [CLEAN] Removing previous build directory...
	@if exist Dist rmdir /s /q Dist
	
build: clean
	@echo [BUILD] Building Client project... 
	@cd Client && dotnet publish -c Release
	
	@echo [BUILD] Building Server project... 
	@cd Server && dotnet publish -c Release

	@echo [PACKAGE] Creating distribution directory...
	@mkdir Dist

	@echo [PACKAGE] Copying files...
	@copy /y fxmanifest.lua Dist
	@copy /y FiveMConfig\lostangeles.yml Dist
	@copy /y FiveMConfig\lostangeles.yml.sample Dist
	@xcopy /y /e /I Client\bin\Release\net452\publish Dist\Client\bin\Release\net452\publish
	@xcopy /y /e /I Server\bin\Release\netstandard2.0\publish Dist\Server\bin\Release\netstandard2.0\publish
