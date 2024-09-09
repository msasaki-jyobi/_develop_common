@echo off
:: バッチファイルのディレクトリを取得
set scriptdir=%~dp0

:: リポジトリのクローン先を指定
set target_dir=%scriptdir%..\

:: 一時クローン先（_tempフォルダ）
set temp_dir=%target_dir%_temp

:: GitリポジトリのURL
set repo_url=https://github.com/msasaki-jyobi/_shooter_develop.git

:: 一時クローン先がすでに存在する場合、削除
if exist "%temp_dir%" (
    echo Removing existing temporary folder...
    rmdir /S /Q "%temp_dir%"
)

:: Gitからリポジトリをクローン（_tempフォルダに）
echo Cloning repository...
git clone "%repo_url%" "%temp_dir%"

:: クローンしたリポジトリから内容をAssetsフォルダにコピー（_tempフォルダの中身のみ）
echo Copying files...
xcopy /E /I /Y "%temp_dir%\*" "%target_dir%"

:: 一時フォルダを削除
echo Cleaning up temporary folder...
rmdir /S /Q "%temp_dir%"

:: 完了メッセージ
echo Repository has been successfully cloned and copied.
pause
