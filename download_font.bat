@echo off
REM バッチファイルの一つ上の階層を取得
set parent_dir=%~dp0..
set font_dir=%parent_dir%\Font

REM Fontフォルダが存在しない場合は作成
if not exist "%font_dir%" (
    mkdir "%font_dir%"
)


REM ダウンロード先のURLを指定
set url=https://downloadx.getuploader.com/g/66f37ad0-e648-495a-aa1f-36f0a010e467/jyobi0508/1/MochiyPopOne-OTF-ExtraBold%20SDF.asset

REM 保存するファイル名を指定
set filename=MochiyPopOne-OTF-ExtraBold SDF.asset

REM curlを使用してファイルをダウンロードし、Fontフォルダに保存
curl -L -o "%font_dir%\%filename%" "%url%"

REM ダウンロード完了メッセージ
echo ファイルのダウンロードが完了しました: %font_dir%\%filename%
pause
