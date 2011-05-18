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

 1. ラベル設定ルールをapp.configから読み取る -> done
 2. PTRのアカウント情報をapp.configから読み取る -> done
 3. notes.xml(オリジナル)を読み込む -> done
 4. XML(更新版)を生成 -> done
 5. XML要素を解析 -> done
 6. ラベル要素をオリジナルから更新版にコピー -> done
 7. csvからプレイヤー名を読み込む -> 未実装
 8. オリジナルに存在しないプレイヤーがあれば検索フラグを立てて更新版に追加 -> 未実装
 9. プレイヤー毎にループ -> done
  a. 現状のラベル番号を読み込む -> done
  b. updateを読み込む -> done
  c. noteの内容(オリジナル)を読み込む -> done
  d1. 取得指示があった場合 -> done
    1. PTRから取得 -> done
    2. Label 3以上のカモを発見した場合は新着リストに表示
  d2. 形式に合致していなかった場合 -> done
    1. PTRから取得 -> done
	2. 以前のnoteの内容を付加 -> done
    3. Label 3以上のカモを発見した場合は更新リストに表示
  d3. 形式に合致していた場合 -> done
	1. 日付を確認(古いならばPTRから再取得) -> done
	2. 以前のnoteの内容を付加 -> done
    3. Label 3以上のカモを発見した場合は更新リストに表示
  e. ラベル設定ルールに沿って、ラベルを付加 -> done
  f. 生成結果を更新版に書き込み -> done
  g. 使用したアカウント数/検索した回数/処理したプレイヤー数/検索できなかった数を表示
10. 更新版を出力する -> done

--- 個別関数(β) ---

. ラベル要素を取得する関数
. データとラベル設定ルールからラベルを決定する関数
. csvからプレイヤー名を読み込む関数
. 読み込んだプレイヤー名がXMLに存在するか確認し、存在しないならば検索フラグを立てて追加する関数

--- クラス(β) ---

1. PTRとの接続クラス -> done
  a. PTRにログインする関数 -> done
  b. PTRからログアウトする関数 -> 不要？
  c. PTRの残り検索回数を確認する関数 -> done
  d. PTRからデータを取得する関数 -> done
  e. PTRのページからデータを切り出す関数 -> done
  f. EXサマリー計算時に対象となるデータかを判定する関数 -> done
2. 個別noteに関するクラス -> 不要？
  a. note文字列から内容を切り出す関数 -> 不要？
  b. データが古いかどうかを判定する関数 -> 不要？
3. PTRから取得したデータのクラス -> done
  a. note文字列から内容を切り出す関数 -> done
  b. PTRのデータからnote文字列を生成する関数 -> done

--- note文字列 ---

a : 内容を削除し、PTRからデータを取得する
error : エラー時に付与される。再実行時にはerror文字列を削除し、PTRからデータを取得する

string summary = "R:" + dr.GetInt32(2).ToString() + ", H:" + dr.GetInt32(3).ToString()
               + ", $:" + dr.GetDouble(5).ToString("f0") + ", BB:" + dr.GetDouble(4).ToString("f2")
               + ", " + dr.GetDateTime(1).ToString("yyyy/MM/dd");

--- ラベル設定ルール ---

ラベル7の場合はラベル7のまま
app.configのLabel_0_Min以上のBB/100の場合は"0"
app.configのLabel_1_Min以上のBB/100の場合は"1"
app.configのLabel_2_Min以上のBB/100の場合は"2"
app.configのLabel_6_Hand_Max未満のハンド数の場合は"6"
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

2011/05/17 rev 096 ラベル6用の再取得処理を追加
2011/05/14 rev 094 HUのデータを追加
2011/05/09 rev 088 csvからプレイヤー名読み込み処理を追加
2011/05/09 version 0.2.0
2011/05/08 rev 085 noteの編集日付を保持するように変更
                   dataがnullの場合にdataにアクセスする可能性があった不具合を修正
2011/04/27 rev 083 新着のカモとアップデートされたカモを分けて表示するように変更
2011/04/25 rev 082 検索終了時にラベルアップデート忘れ修正
2011/04/25 rev 081 長さが0のnote読込時に落ちる不具合を修正
                  ラベル7を勝手に上書きしないように修正
                  定期的にラベルをアップデートするように変更
2011/04/25 rev 078 プレイヤーサーチ毎に画面をリフレッシュするように変更
                  検索結果をログ出力するように変更
2011/04/23 rev 077 note文字列が不正な場合に対応
2011/04/20 rev 076 未検索人数の表示を追加
                  出力する数字の数え間違いを修正
2011/04/20 version 0.1.0 初回リリース版

--- to do ---

1. readmeをプログラムの実態に合わせて更新
2. エラー処理 -> done?
3. ログ出力 -> done?
4. notesXMLのマージ処理
5. プレイヤー名をCSVから読み込む
6. ニューカマーリストに表示する条件の検討 -> done?
7. 新規プレイヤーを自動で取得する仕組み -> AutoNoteによるMacro?

--- 問題点 ---

1. 未サーチ状態の"a"ラベルの人とサーチ済みのLabe_0_Minを超えるBB/100のプレイヤーが
同じラベルになっている。

--- author ---

bloodysnow

--- date ---

2011/04/20

--- version ---

2011/05/09 version 0.2.0

C-M-% \([^>]\)a+< \1<