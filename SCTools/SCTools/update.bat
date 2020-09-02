@echo off

SET workpath=%~dp0
SET updatepath=%workpath%updates\
SET latestpath=%updatepath%latest\

timeout 1
xcopy "%latestpath%*.*" "%workpath%" /s /k /h /y
if not errorlevel 0 goto contiunue

del "%updatepath%latest.json"
del "%updatepath%latest.zip"
del /q "%latestpath%*"
for /d %%p in ("%latestpath%*.*") do rmdir /s /q "%%p"
rmdir /s /q "%latestpath%

:contiunue

start %workpath%SCTools.exe
exit