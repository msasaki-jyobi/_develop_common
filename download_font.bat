
@echo off
REM 指定したURLにブラウザでアクセス
start "" "https://firestorage.jp/download/0b8cadf143c7f17ca2d563a3340599a592fd2cdc"




@REM @echo off
@REM REM バッチファイルの一つ上の階層を取得
@REM set parent_dir=%~dp0..
@REM set font_dir=%parent_dir%\Font

@REM REM Fontフォルダが存在しない場合は作成
@REM if not exist "%font_dir%" (
@REM     mkdir "%font_dir%"
@REM )


@REM REM ダウンロード先のURLを指定
@REM set url=https://downloadx.getuploader.com/g/66f37ad0-e648-495a-aa1f-36f0a010e467/jyobi0508/1/MochiyPopOne-OTF-ExtraBold%20SDF.asset

@REM REM 保存するファイル名を指定
@REM set filename=MochiyPopOne-OTF-ExtraBold SDF.asset

@REM REM curlを使用してファイルをダウンロードし、Fontフォルダに保存
@REM curl -L -o "%font_dir%\%filename%" "%url%"

@REM REM ダウンロード完了メッセージ
@REM echo ファイルのダウンロードが完了しました: %font_dir%\%filename%
@REM pause
