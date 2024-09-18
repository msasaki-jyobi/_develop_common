@echo off
:: バッチファイルのディレクトリを取得
set scriptdir=%~dp0

:: 目的の配置先フォルダ（_develop_commonフォルダ）
set target_dir=%scriptdir%_develop_common

:: GitリポジトリのURL
set repo_url=https://github.com/msasaki-jyobi/_develop_common.git

:: 配置先フォルダがすでに存在する場合、削除
if exist "%target_dir%" (
    echo Removing existing _develop_common folder...
    rmdir /S /Q "%target_dir%"
)

:: Gitからリポジトリを直接クローン
echo Cloning repository directly to _develop_common folder...
git clone "%repo_url%" "%target_dir%"

:: 完了メッセージ
echo Repository has been successfully cloned with .git folder.
pause
