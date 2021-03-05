[CmdletBinding()]
Param ( 
    [string]$NewVersion = $(throw "NewVersion is required")
)

# ------------------------------------------------------------------------------
$ErrorActionPreference = "Stop"
Push-Location $PSScriptRoot.Parent

try {
    $mainBranchName = "main"
    $releaseNotesFile = Resolve-Path release_notes.md
    $releaseBranchName = "release/$NewVersion"

    Write-Host "Releasing version $NewVersion"

    git checkout $mainBranchName
    git pull --ff-only origin $mainBranchName
    git checkout --quiet -b $releaseBranchName $mainBranchName

    $releaseNotesContent = [System.IO.File]::ReadAllText($releaseNotesFile)
    $releaseNotesContent = ("## $NewVersion`r`n`r`n" + $releaseNotesContent)
    [System.IO.File]::WriteAllText($releaseNotesFile, $releaseNotesContent)

    Write-Host "`r`nReleasing version $NewVersion. Changing $releaseNotesFile like so:`r`n"
    git diff $releaseNotesFile
    $response = Read-Host "`r`n  Proceed (y/N)?"
    Switch ($response) {
        y { }
        n { Write-Host "Update cancelled. Clean up yourself."; return }
        Default { Write-Host "Unknown response '$response'. Aborting."; return }
    }

    git commit --quiet --message "Set version to $NewVersion" $releaseNotesFile
    git checkout --quiet $mainBranchName
    git merge --quiet --no-ff $releaseBranchName
    git branch -D $releaseBranchName

    git tag $NewVersion
    git push origin $NewVersion $mainBranchName
}
finally {
    Pop-Location
}
