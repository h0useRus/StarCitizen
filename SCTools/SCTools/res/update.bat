@echo off

set workpath=%~dp0
set updatepath=%workpath%updates\
set latestpath=%updatepath%latest\

timeout 1
xcopy "%latestpath%*.*" "%workpath%" /s /k /h /y
if not errorlevel 0 goto update_error

del "%updatepath%latest.json"
del "%updatepath%latest.zip"
del /q "%latestpath%*"
for /d %%p in ("%latestpath%*.*") do rmdir /s /q "%%p"
rmdir /s /q "%latestpath%

start %workpath%SCTools.exe update_status 0
exit

:update_error

start %workpath%SCTools.exe update_status 1
exit