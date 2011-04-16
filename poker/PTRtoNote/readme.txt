--- ツール名 ---

PTRtoNote

--- ファイル説明 ---

PTRtoNote.exe : 本ツールの本体です。
PTRtoNote.exe.config : 

--- 動作概要 ---

notes.xmlを読み込み、PTRからのデータを追加し、条件によるラベリングを実行し、
データを追加したnotes.xmlを出力します。

--- 全体フロー ---

. ラベル設定ルールをapp.configから読み取る -> done
. PTRのアカウント情報をapp.configから読み取る -> done
. XML(更新版)を生成 -> done
. notes.xml(オリジナル)を読み込む -> done
. XML要素を解析 -> done
. ラベル要素をオリジナルから更新版にコピー -> done
. csvからプレイヤー名を読み込む
. オリジナルに存在しないプレイヤーがあれば検索フラグを立てて更新版に追加
. プレイヤー毎にループ
  . noteの内容(オリジナル)を読み込む -> done
  . 形式に合致していた場合 -> done
	. 日付を確認(古いならばPTRから再取得) -> done
	. ラベル設定ルールに沿って、ラベルを更新 -> done
  . 形式に合致していなかった場合 -> done
    . PTRから取得 -> done
	. 以前のnoteの内容を付加 -> done
	. ラベル設定ルールに沿って、ラベルを付加 -> done
  . 生成結果を更新版に書き込み -> done
. 更新版を出力する -> done

--- 個別関数 ---

. ラベル要素を取得する関数
. データとラベル設定ルールからラベルを決定する関数
. csvからプレイヤー名を読み込む関数
. 読み込んだプレイヤー名がXMLに存在するか確認し、存在しないならば検索フラグを立てて追加する関数

--- クラス ---

. PTRとの接続クラス -> done
  . PTRにログインする関数 -> done
  . PTRからログアウトする関数 -> 不要？
  . PTRの残り検索回数を確認する関数 -> done
  . PTRからデータを取得する関数 -> done
  . PTRのページからデータを切り出す関数 -> done
. 個別noteに関するクラス -> done
  . note文字列から内容を切り出す関数 -> done
  . データが古いかどうかを判定する関数 -> 不要？

--- note文字列 ---

a : 内容を削除し、PTRからデータを取得する
error : エラー時に付与される。再実行時にはerror文字列を削除し、PTRからデータを取得する

string summary = "R:" + dr.GetInt32(2).ToString() + ", H:" + dr.GetInt32(3).ToString()
               + ", $:" + dr.GetDouble(5).ToString("f0") + ", BB:" + dr.GetDouble(4).ToString("f2")
               + ", " + dr.GetDateTime(1).ToString("yyyy/MM/dd");

--- ラベル設定ルール ---

app.configのLabel_6_Hand_Max未満のハンド数の場合は"6"
app.configのLabel_0_Min以上のBB/100の場合は"0"
app.configのLabel_1_Min以上のBB/100の場合は"1"
app.configのLabel_2_Min以上のBB/100の場合は"2"
app.configのLabel_3_Min以上のBB/100の場合は"3"
app.configのLabel_4_Min以上のBB/100の場合は"4"
app.configのLabel_4_Min未満のBB/100の場合は"5"

--- app.config ---

Label_0_Min - ラベル"0"になる最小BB/100
Label_1_Min - ラベル"1"になる最小BB/100
Label_2_Min - ラベル"2"になる最小BB/100
Label_3_Min - ラベル"3"になる最小BB/100
Label_4_Min - ラベル"4"になる最小BB/100
Logins - ログインユーザ名のCSV
Passwords - ログインパスワードのCSV
AccountNum - アカウント総数(上記二つの確認用)
Label_6_Hand_Max - ラベル"6"になる最大ハンド数
ReacquisitionSpanDays - 再取得を実行する期間

--- 状態遷移 ---
デフォルト
 buttonOpen = True, buttonCSV = True, buttonExecute = False, buttonSave = False
 notesXMLオリジナル = null, notesXML更新版 = null
XML読み込み
 buttonExecute -> True, buttonSave -> False
 notesXMLオリジナル -> 生成
CSV読み込み
 buttonExecute -> True, buttonSave -> False
Execute
 buttonOpen -> False, buttonCSV -> False, buttonExecute -> False, buttonSave -> True
 notesXML更新版 -> 生成
Save
 buttonOpen -> True, buttonCSV -> True, buttonExecute -> False, buttonSave -> False
 notesXMLオリジナル -> null, notesXML更新版 -> null

--- 注意 ---

Windows XP Professional SP3 + IE8 + .net Framework 4.0の環境でしかテストしていません。他の環境では
動作しないかもしれません。仮に動作しなかった場合、あまり積極的に対処するつもりはありませんので、VMで
XP環境を構築するなどの自助努力をお願いします。
画面UIに関しても、FlowLayoutPanelとAutoScrollに丸投げしていますので、正しく表示されない場合等は、
クライアントのウィンドウサイズを適宜調整するなどをお願いします。
ログ出力にはlog4netを用いています。出力等が気に入らない場合は、log4netのヘルプを参考にして、app.configを
書き換えてください。
不具合や要望は連絡してくれたら対処するかもしれません。しかし、必要であればソースコードを提供しますので、
自分で修正してパッチをこちらまで送付してください。GPLは面倒なので採用しませんが、精神としてはGPLのように
お願いします(つまり、他人に頼らずにできることは自分でやろうってことですねー)。自分では無理だけど、
どうしてもっていう場合は気持ち(単位は$)次第ですねｗｗｗｗｗｗｗｗｗ

--- 変更履歴 ---



--- to do ---

PTRアクセス時にはランダム時間waitする
readmeをプログラムの実態に合わせて更新
エラー処理
ログ出力

--- author ---

bloodysnow

--- date ---



--- version ---
