git submodule update --init --recursive
"external\nuget-binary\NuGet.exe" restore Main.sln
"external\nuget-binary\NuGet.exe" restore external\RefactoringEssentials\RefactoringEssentials.sln
cd external\debugger-libs
git remote add jetbrains ssh://git@github.com/JetBrains/debugger-libs.git
git fetch jetbrains
git checkout rider-new
cd ..\..\
gacutil /i  packages\SharpZipLib.0.86.0\lib\20\ICSharpCode.SharpZipLib.dll