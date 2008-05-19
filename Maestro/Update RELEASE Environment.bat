xcopy /Y EditorMap.xml bin\release\
xcopy /Y ProviderMap.xml bin\release\
xcopy /Y /E images\stdicons bin\release\stdicons\
xcopy /Y /E images\webstudio bin\release\webstudio\

xcopy /Y /E resourceeditors\templates bin\release\templates\
xcopy /Y /E ..\maestroapi\Schemas bin\release\Schemas\

xcopy /Y /E Localization bin\release\Localization\
pause