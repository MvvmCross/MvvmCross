nuget setapikey ---

for /r %%i in (*1.nupkg) do nuget push %%i
pause
