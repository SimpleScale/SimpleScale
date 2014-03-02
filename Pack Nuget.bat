C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe /t:Clean,Build HeadNode/HeadNode.csproj /p:Configuration=Release
nuget pack HeadNode\HeadNode.csproj -IncludeReferencedProjects -Prop Configuration=Release
pause