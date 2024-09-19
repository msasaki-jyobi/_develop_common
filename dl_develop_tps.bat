@echo off
:: バッチファイルのディレクトリを取得
set scriptdir=%~dp0

:: クローン先のディレクトリを指定
set target_dir=%scriptdir%..\_develop_tps

:: GitリポジトリのURL
set repo_url=https://github.com/msasaki-jyobi/_develop_tps.git

:: クローン先がすでに存在する場合、削除
if exist "%target_dir%" (
    echo Removing existing _develop_tps folder...
    rmdir /S /Q "%target_dir%"
)

:: Gitからリポジトリをクローン
echo Cloning repository into _develop_tps folder...
git clone "%repo_url%" "%target_dir%"

:: 完了メッセージ
echo Repository has been successfully cloned into _develop_shooter folder.
pause
