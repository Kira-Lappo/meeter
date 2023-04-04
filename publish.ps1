#!/bin/pwsh

param(
    [Parameter()]
    [ValidateSet("linux-x64", "win-x64")]
    $RID = "win-x64"
)

dotnet publish "$PSScriptRoot/src/Meeter.Cli" `
    -c Release  `
    -p:PublishProfile="publish.pubxml"  `
    -p:RuntimeIdentifier="$RID"  `
    -o "$PSScriptRoot/out/$RID"

dotnet publish "$PSScriptRoot/src/Meeter.TerminalGui" `
    -c Release  `
    -p:PublishProfile="publish.pubxml"  `
    -p:RuntimeIdentifier="$RID"  `
    -o "$PSScriptRoot/out/$RID"
