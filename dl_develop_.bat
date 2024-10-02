@echo off
:: バッチファイルのディレクトリを取得
set scriptdir=%~dp0

:: ユーザーに入力を求める
set /p input=Please enter the target suffix (e.g., shooter, common, etc.): 

:: 入力に基づいてターゲットディレクトリ名を設定
set target_dir=%scriptdir%..\_develop_%input%

:: GitリポジトリのURL
set repo_url=https://github.com/msasaki-jyobi/_develop_%input%.git

:: クローン先がすでに存在する場合、削除
if exist "%target_dir%" (
    echo Removing existing _develop_%input% folder...
    rmdir /S /Q "%target_dir%"
)

:: Gitからリポジトリをクローン
echo Cloning repository into _develop_%input% folder...
git clone "%repo_url%" "%target_dir%"

:: 完了メッセージ
echo Repository has been successfully cloned into _develop_%input% folder.
pause
