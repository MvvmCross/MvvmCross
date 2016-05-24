$newVersion = "4.1.6"

$versionPattern = "(<version)>(\d\.\d\.\d(-alpha|beta\d)?)(</version>)"
$dependencyPattern = "(<dependency\s+id=`"MvvmCross[^`"]+`"\s+version=)`"(\d\.\d\.\d(-alpha|beta\d)?)(`"\s*/>)"

$files = Get-ChildItem ".\" -Filter *.nuspec

for($i=0; $i -lt $files.Count; $i++) {
    $content = Get-Content $files[$i].FullName
    $newContent = ""
    ForEach ($line in $content) {
        $line = $line -Replace $versionPattern, ('$1>' + $newVersion + '$4')
        $newContent += ($line -Replace $dependencyPattern, ('$1"' + $newVersion + '$4')) + "`n"
    }
    Set-Content -Force ($files[$i].FullName) $newContent
}

pause