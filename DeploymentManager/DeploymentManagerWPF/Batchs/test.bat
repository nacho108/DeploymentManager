cd C:\Projects\Trueview
@if %errorlevel% neq 0 exit /b %errorlevel%
git checkout master
@if %errorlevel% neq 0 exit /b %errorlevel%
git checkout develop
@if %errorlevel% neq 0 exit /b %errorlevel%






