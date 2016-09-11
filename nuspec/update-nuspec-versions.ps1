$newVersion = "4.2.3"

$versionPattern = "(<version)>(\d\.\d\.\d)(</version>)"
$dependencyPattern = "(<dependency\s+id=`"MvvmCross[^`"]+`"\s+version=)`"(\d\.\d\.\d)(`"\s*/>)"

$files = Get-ChildItem ".\" -Filter *.nuspec

for($i=0; $i -lt $files.Count; $i++) {
    $content = Get-Content $files[$i].FullName
    $newContent = ""
    ForEach ($line in $content) {
        $line = $line -Replace $versionPattern, ('$1>' + $newVersion + '$3')
        $newContent += ($line -Replace $dependencyPattern, ('$1"' + $newVersion + '$3')) + "`n"
    }
    Set-Content -Force ($files[$i].FullName) $newContent
}

pause