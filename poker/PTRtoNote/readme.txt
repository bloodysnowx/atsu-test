--- ツール名 ---

PTRtoNote

--- ファイル説明 ---

PTRtoNote.exe : 本ツールの本体です。
PTRtoNote.exe.config : 設定ファイル
log4net.dll : ログ出力用dll
readme.txt : 本説明ファイル

--- 動作概要 ---

notes.xmlを読み込み、PTRからのデータを追加し、条件によるラベリングを実行し、
データを追加したnotes.xmlを出力します。

--- 使い方 ---

1. 設定作業
  a. app.config(PTRtoNote.exe.config)を開く
  b. 下記ラベル設定ルールを参考に、Label_0_Min, Label_1_Min, Label_2_Min, Label_3_Min, Label_6_Hand_Maxを設定する
  c. 何日以上古いデータであれば更新するかを、ReacquisitionSpanDaysに設定する
  d. LoginsにPTRのログイン名を区切り文字","のみで記入する(余計なスペースがあった場合、スペースもログイン名の一部と認識します)
    ex. user1,user2,user3,...
  e. PasswordsにPTRのパスワードを区切り文字","のみで記入する(余計なスペースがあった場合、スペースもパスワードの一部と認識します)
    ex. pass1,pass2,pass3,...(pass1はuser1のパスワード、pass2はuser2のパスワード、。。。)
  f. LoginsとPasswordsに設定したアカウント数をAccountNumに記載する
2. 下準備
  a. Pokerstarsを起動する
  b. PTRからデータを取得したいプレイヤーでラベリングされていないプレイヤーのnoteに"a"と記入していく
  c. Pokerstarsを終了する(本ソフト実行時は必ず終了させておいてください)
3. 実行
  a. 本ソフトを起動する
  b. openXMLボタンを押して、Pokerstarsのnotes.xmlを開く
  c. Executeボタンを押して、出力ファイル名を指定し、実行
  (検索回数(無料アカウントの場合はアカウント数×10)×3秒程度待つ)
  d. Executeボタンの下のラベルが変更されたことを確認して、終了する
  (ラベル4またはラベル5に相当するカモが発見された場合はテキストボックスに表示されます。必要に応じてKGSやスカイプにコピペして
  共有するといいでしょう)
4. 結果確認(結果出力のnotes.xmlに不具合があった場合)
  a. 実行フォルダの直下にlog出力用フォルダが生成されているはずです。実行した日付のファイルを確認してみてください。

--- 全体フロー(β) ---

. ラベル設定ルールをapp.configから読み取る -> done
. PTRのアカウント情報をapp.configから読み取る -> done
. XML(更新版)を生成 -> done
. notes.xml(オリジナル)を読み込む -> done
. XML要素を解析 -> done
. ラベル要素をオリジナルから更新版にコピー -> done
. csvからプレイヤー名を読み込む -> 未実装
. オリジナルに存在しないプレイヤーがあれば検索フラグを立てて更新版に追加 -> 未実装
. プレイヤー毎にループ -> done
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

--- 個別関数(β) ---

. ラベル要素を取得する関数
. データとラベル設定ルールからラベルを決定する関数
. csvからプレイヤー名を読み込む関数
. 読み込んだプレイヤー名がXMLに存在するか確認し、存在しないならば検索フラグを立てて追加する関数

--- クラス(β) ---

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

--- 状態遷移(β) ---
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
クライアントのウィンドウサイズを適宜調整するなどをお願いします。 -> 更に面倒になったので、FlowLayoutは
廃止しました。ウィンドウサイズをデフォルトから変更した場合の動作は知りません。
ログ出力にはlog4netを用いています。出力等が気に入らない場合は、log4netのヘルプを参考にして、app.configを
書き換えてください。
不具合や要望は連絡してくれたら対処するかもしれません。しかし、必要であればソースコードを提供しますので、
自分で修正してパッチをこちらまで送付してください。GPLは面倒なので採用しませんが、精神としてはGPLのように
お願いします(つまり、他人に頼らずにできることは自分でやろうってことですねー)。自分では無理だけど、
どうしてもっていう場合は気持ち(単位は$)次第ですねｗｗｗｗｗｗｗｗｗ
コード実装に関してはYAGNIの原則に則っています。CSVを読み込む機能や他のnotes.xmlをマージする機能などを
いつかは実装しようとは思っていますが、現状では必要性がありませんの実装していません(つまり、実装したい、
と思える動機を提供してください。あったら便利程度でしたら、自分でコード書いて送付してください)。

--- 変更履歴 ---

2011/04/20 version 0.1.0 初回リリース版

--- to do ---

1. readmeをプログラムの実態に合わせて更新
2. エラー処理
3. ログ出力
4. notesXMLのマージ処理
5. プレイヤー名をCSVから読み込む
6. ニューカマーリストに表示する条件の検討
7. 新規プレイヤーを自動で取得する仕組み

--- 問題点 ---

1. 未サーチ状態の"a"ラベルの人とサーチ済みのLabe_0_Minを超えるBB/100のプレイヤーが
同じラベルになっている。

--- author ---

bloodysnow

--- date ---

2011/04/20

--- version ---

2011/04/20 version 0.1.0 初回リリース版
