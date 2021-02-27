@pushd %~dp0
@dotnet run --project ".\tools\MakeItEasy.Build\MakeItEasy.Build.csproj" -- %*
@popd
