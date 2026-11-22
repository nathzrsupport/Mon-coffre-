@echo off
setlocal
cd /d "%~dp0"
echo Creation de MonCoffre.exe ...
set CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe
if not exist "%CSC%" set CSC=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe
if not exist "MonCoffre.cs" (
  echo ERREUR : le fichier MonCoffre.cs doit etre dans le meme dossier que ce script.
  pause
  exit /b 1
)
set ICON=
if exist "MonCoffre.ico" (
  set ICON=/win32icon:MonCoffre.ico
) else (
  echo ATTENTION : MonCoffre.ico est introuvable, l'application n'aura pas de logo.
)
"%CSC%" /nologo /codepage:65001 /target:winexe /optimize+ %ICON% /out:MonCoffre.exe /reference:System.Windows.Forms.dll /reference:System.Drawing.dll MonCoffre.cs
if errorlevel 1 (
  echo.
  echo ERREUR pendant la creation. Ferme Mon Coffre s'il est ouvert, puis reessaie.
  echo Sinon fais une capture de ce message.
  pause
  exit /b 1
)
powershell -NoProfile -Command "$s=(New-Object -ComObject WScript.Shell).CreateShortcut([Environment]::GetFolderPath('Desktop')+'\Mon Coffre.lnk');$s.TargetPath='%~dp0MonCoffre.exe';$s.WorkingDirectory='%~dp0';$s.IconLocation='%~dp0MonCoffre.exe,0';$s.Save()"
echo.
echo C'est pret : MonCoffre.exe cree, et un raccourci "Mon Coffre" est sur ton bureau.
pause
