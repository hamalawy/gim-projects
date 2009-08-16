## create-project v1.0
####################################################################################################
## Used to create all the basic files necessary for starting a new project.  By running this script
## you will create a directory with all the files necessary to get going.
## 
## The application works by copying over all files in a templates directory to the new project
## directory while replacing tokens in the file content and file name.  Currently this script will set
## up a C# .NET project - it should be fairly trivial to add any other project however.
## Tokens are set in the $tokens hash (see below) and are replaced by the results of a script block.
####################################################################################################
## Version History
## 1.0   First Release
####################################################################################################
## Usage:
##   Create-Project ProjectName
####################################################################################################

# Tokens that will be replaced in file names and contents
$tokens = @{
  '#{ProjectName}'={$name};
  '#{GUID}'={ [Guid]::NewGuid().ToString().ToUpperInvariant() };
}
for ($i=0; $i -lt 10; $i++) {
  $tokens.Add("#{GUID[$i]}", {[Guid]::NewGuid().ToString().ToUpperInvariant()})
}
####################################################################################################

$name = $args[0]
if(!$name) {
	echo "Please provide a valid project name as the first argument"
	exit
}

function Replace-Tokens { param($str, $token_dictionary)
	foreach($tkn in $token_dictionary.Keys) {
		$val = $token_dictionary.Get_Item($tkn)
		$calcVal = & $val
		$str = $str.Replace($tkn, $calcVal)
  }
	return $str
}

echo "Creating Project $name"
$scriptpath = [System.IO.Path]::GetDirectoryName($myinvocation.mycommand.path)
$templatespath = "$scriptpath\template"
echo "Using templates at: $templatespath"

# Copy and rename directories
mkdir $name 
$directories =  dir $templatespath -recurse | where {$_.PsIsContainer} | %{$_.FullName} | sort
foreach($dirpath in $directories) { 
	$newdirpath = Replace-Tokens $dirpath @{$templatespath={$name}}
	$newdirpath = Replace-Tokens $newdirpath $tokens
	$exec = "cp '$dirpath' '$newdirpath'"
	echo $exec
	iex $exec
}

# Copy and rename, and replace tokens in binary files
$files =  dir $templatespath -recurse -include *.dll,*.exe | where {!$_.PsIsContainer} | %{$_.FullName} | sort
foreach($filepath in $files) { 
	$newfilepath = Replace-Tokens $filepath @{$templatespath={$name}}
	$newfilepath = Replace-Tokens $newfilepath $tokens
	$exec = "cp '$filepath' '$newfilepath'"
	echo $exec
	iex $exec
}

# Copy, rename, and replace tokens in text files
$files =  dir $templatespath -recurse -exclude *.dll,*.exe | where {!$_.PsIsContainer} | %{$_.FullName} | sort
foreach($filepath in $files) { 
  $contents = [String]::join([Environment]::Newline, (Get-Content $filepath))
	$contents = Replace-Tokens $contents $tokens
	$newfilepath = Replace-Tokens $filepath @{$templatespath={$name}}
	$newfilepath = Replace-Tokens $newfilepath $tokens
	echo "Creating $newfilepath from $filepath"
	Set-Content $newfilepath $contents -Encoding UTF8
}

