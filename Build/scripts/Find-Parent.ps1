Param(
	[string]$Name,
	[int]$Depth = 3
)

$current = Get-Item .
if($current.Name -eq $Name){
	return $current.FullName
}
	
for($i = 0; $i -lt $Depth; $i++)
{
	$current = $current.Parent
	if($current.Name -eq $Name){
		return $current.FullName
	}
}

Write-Error -Message `
@"
Failed to find parent directory 

Input: 
	Directory: $Name 
	Depth: $Depth
	Current: $Current
"@