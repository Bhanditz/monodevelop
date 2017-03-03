#!/bin/sh 

GOTO WIN

dp0=$(cd "$(dirname "$0")"; pwd)
mono $dp0/../Tools/NUnit/3.6/nunit3-console.exe $dp0/../../CorApi.Tests/bin/Debug/CorApi.Tests.dll --inprocess --where:cat==Core --noresult

return

:WIN

%~dp0\..\Tools\Mono\v4.8_win\bin\mono.exe %~dp0\..\Tools\NUnit\3.6\nunit3-console.exe %~dp0\..\..\CorApi.Tests\bin\Debug\CorApi.Tests.dll --inprocess --where:cat==Core --noresult