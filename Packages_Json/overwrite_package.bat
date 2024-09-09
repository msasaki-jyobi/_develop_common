@echo off
:: バッチファイルのディレクトリを取得
set scriptdir=%~dp0

:: コピー元 (Package_Jsonディレクトリ)
set source_dir=%scriptdir%

:: コピー先 (Packagesディレクトリ)
set target_dir=%scriptdir%..\..\Packages\

:: manifest.jsonをコピーして上書き
copy /Y "%source_dir%manifest.json" "%target_dir%manifest.json"

:: packages-lock.jsonをコピーして上書き
copy /Y "%source_dir%packages-lock.json" "%target_dir%packages-lock.json"

:: 完了メッセージ
echo Files have been successfully copied.
pause
