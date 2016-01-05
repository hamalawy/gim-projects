# Introduction #

Setting up new .NET projects is a pain.  You need to set up the directory structure, open visual studio, set up the solution structure, copy in commonly used dependencies (xunit.dll, nunit.framework.dll, rhino.mocks) and wait for that horrible "Add Reference" dialog.

This is a powershell script to do all that for you!

Simply type
```
PS c:\code>C:\path-to-script\Create-Project.ps1 NameOfProject
```

# Get It #

Get the script and .NET template at http://gim-projects.googlecode.com/svn/Create-Project/trunk

# Details #

The script works by copying all files from a "template" directory while replacing tokens in file names and file contents.  It should therefore be very easy to simply to make your own modifications to it.  Tokens that it is currently set up to resolve (see the $tokens hash setup in create-project.ps1) are:
  * `#{ProjectName}` - Replaced with NameOfProject
  * `#{GUID}` - Replaced by a GUID (different each time the token is called)
  * `#{GUID[0]}` - Replaced by a specific GUID (up to `#{GUID[9]}`). `#{GUID[0]}` will always be the same GUID over the course of the script run.  Next time the script is run it will be different.

## .NET Template ##
Currently the script only supports one template type (it should be trivial to support multiple) and the type is .NET.
```
PS C:\Temp\projectscript> del Sample -recurse
PS C:\Temp\projectscript> C:\Utilities\create-project\create-project.ps1 Sample
PS C:\Temp\projectscript> dir Sample -recurse | %{$_.FullName}
C:\Temp\projectscript\Sample\app
C:\Temp\projectscript\Sample\lib
C:\Temp\projectscript\Sample\app\Sample
C:\Temp\projectscript\Sample\app\Sample.Tests
C:\Temp\projectscript\Sample\app\Sample.sln
C:\Temp\projectscript\Sample\app\Sample\Properties
C:\Temp\projectscript\Sample\app\Sample\Class1.cs
C:\Temp\projectscript\Sample\app\Sample\Sample.csproj
C:\Temp\projectscript\Sample\app\Sample\Properties\AssemblyInfo.cs
C:\Temp\projectscript\Sample\app\Sample.Tests\Properties
C:\Temp\projectscript\Sample\app\Sample.Tests\Class1Tests.cs
C:\Temp\projectscript\Sample\app\Sample.Tests\Sample.Tests.csproj
C:\Temp\projectscript\Sample\app\Sample.Tests\Properties\AssemblyInfo.cs
C:\Temp\projectscript\Sample\lib\xunit.dll
```

# Caveats #
Currently there is a known bug - when you try to build the project for the first time in visual studio you will get an error.  On the second attempt everything works as it should.