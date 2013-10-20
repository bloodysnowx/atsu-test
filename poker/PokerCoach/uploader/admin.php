<?
#============================================================================================================
#	PHPファイルアップロードスクリプト　Sch Uploader
#	Copyright 2010 sitm. All Rights Reserved.
#	URL       : http://www.sanadake.info/sch/
#	Support   : http://www.sanadake.info/bbs/read.php/support/
#	-----------------------------------------------
#	管理画面
#	admin.php
#	-----------------------------------------------
#	2013/02/24 Ver1.2
#
#============================================================================================================
header("Content-Type: text/html; charset=UTF-8");

#------------------------------------------------------------------------------------------------------------
#	メイン処理
#------------------------------------------------------------------------------------------------------------
### 共通関数読み込み
include("./Function.php");
$_POST = DeleteStripslashes($_POST);

### 初回ログイン ###
if($_COOKIE["ID"] == "" || $_COOKIE["Pass"] == "") {
	Login();
}

### 処理選択
$Action = $_POST["Action"];
# 管理画面へ戻る
if($Action == "管理画面へ") {
	PrintMenu();
# 選択した処理が存在する
} elseif(function_exists($Action)) {
	$Action();
# 存在しない場合ログイン画面へ
} else {
	Login();
}

#------------------------------------------------------------------------------------------------------------
#	ログインページ - LoginPage
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function LoginPage() {
	#クッキー削除
	setcookie("ID", "", -1);
	setcookie("Pass", "", -1);

	$HTML = '<TABLE border="0">'."\n"
			 .'<TR><TD>ID：</TD><TD><INPUT TYPE="text" class="Text" NAME="ID" SIZE="20"></TD></TR>'."\n"
			 .'<TR><TD>パスワード：</TD><TD><INPUT TYPE="PassWord" class="Text" NAME="Pass" SIZE="20"></TD></TR>'."\n"
			 ."</TABLE><BR><BR>\n"
			 .'<INPUT TYPE="submit" VALUE="ログイン" class="S4">'."\n";

	PrintPage("ログイン", $HTML);
}

#------------------------------------------------------------------------------------------------------------
#	ログイン - Login
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function Login() {
	### ID/Pass取得
	$ID = $_POST["ID"];
	$Pass= $_POST["Pass"];

	### 入力された場合のみ
	if($ID != "" && $Pass != "") {
		# 管理者ID/Passを取得
		$Result = Query("SELECT * FROM Admin");
		$Data = $Result -> fetch(PDO::FETCH_ASSOC);
		# 一致している
		if($Data["ID"] == $ID && $Data["Pass"] == $Pass) {
			setcookie("ID", $ID);
			setcookie("Pass", $Pass);
			PrintMenu();
		# 一致していない
		} else {
			LoginPage();
		}
	### 未入力がある場合
	} else {
		LoginPage();
	}
}

#------------------------------------------------------------------------------------------------------------
#	管理メニュー - PrintMenu
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function PrintMenu() {
	$HTML = '<SELECT SIZE="19" NAME="Action">'."\n"
			 .'<OPTION VALUE="PrintAdminInfo" SELECTED>　管理者情報変更'."\n"
			 .'<OPTION VALUE="PrintSetting">　設定情報変更'."\n"
			 .'<OPTION VALUE="PrintData">　データ管理'."\n"
			 .'<OPTION VALUE="PrintRegulation">　ホスト/IP規制'."\n"
			 .'<OPTION VALUE="DBOptimize">　DB最適化'."\n"
			 ."</SELECT><BR><BR><BR>\n"
			 .'<INPUT TYPE="submit" VALUE="実行" class="S2">'."\n";

	PrintPage("管理メニュー", $HTML);
}

#------------------------------------------------------------------------------------------------------------
#	管理者情報表示 - PrintAdminInfo
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function PrintAdminInfo() {
	### 管理者情報取得
	$Result = Query("SELECT * FROM Admin");
	$Data = $Result -> fetch(PDO::FETCH_ASSOC);

	$HTML = ### 現在の情報
			  '<TABLE cellspacing="1" cellpadding="0" class="TABLE_FRAME">'."\n"
			 .'<TBODY class="TABLE_TBODY">'."\n"
			 .'<TR><TD class="Purple18_Bold_Center" ID="Cell_Margin">ID</TD><TD ID="Cell_Margin">'.$Data["ID"]."</TD></TR>\n"
			 .'<TR><TD class="Purple18_Bold_Center" ID="Cell_Margin">パスワード</TD><TD ID="Cell_Margin">'.$Data["Pass"]."</TD></TR>\n"
			 ."</TBODY>\n"
			 ."</TABLE><BR>\n"
			 ."------------------------------<BR><BR>\n"
			 ### 入力フォーム
			 .'<TABLE cellspacing="1" cellpadding="0" class="TABLE_FRAME">'."\n"
			 .'<TBODY class="TABLE_TBODY">'."\n"
			 .'<TR><TD class="Purple18_Bold_Center" ID="Cell_Margin">ID</TD><TD><INPUT TYPE="text" NAME="ID" SIZE="15"></TD></TR>'."\n"
			 .'<TR><TD class="Purple18_Bold_Center" ID="Cell_Margin">パスワード</TD><TD><INPUT TYPE="text" NAME="Pass" SIZE="15"></TD></TR>'."\n"
			 ."</TBODY>\n"
			 ."</TABLE><BR><BR>\n"
			 .'<INPUT TYPE="hidden" NAME="Action" Value="UpdateAdminInfo">'."\n"
			 .'<INPUT TYPE="submit" VALUE="実行" class="S2"><BR><BR><BR>'."\n"
			 .'<INPUT TYPE="submit" NAME="Action" VALUE="管理画面へ" class="S5">';

	PrintPage("管理者情報変更", $HTML);
}

#------------------------------------------------------------------------------------------------------------
#	管理者情報変更 - UpdateAdminInfo
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function UpdateAdminInfo() {
	$ID = $_POST["ID"];
	$Pass = $_POST["Pass"];

	### ID入力有り
	if($ID != "") {
		$Update["ID"] = 'ID = "'.$ID.'"';
		setcookie("ID", $ID);
	}

	### Pass入力有り
	if($Pass != "") {
		$Update["Pass"] = 'Pass = "'.$Pass.'"';
		setcookie("Pass", $Pass);
	}

	### 1つでも入力がある場合DB更新
	if(isset($Update)) {
		$SQL = "UPDATE Admin SET ".implode(",", $Update);
		QueryExec($SQL);
	}

	PrintAdminInfo();
}

#------------------------------------------------------------------------------------------------------------
#	設定情報表示 - PrintSetting
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function PrintSetting() {
	### 設定情報取得
	$Data = getSetting();

	$HTML = ### 現在の情報
			  '<TABLE cellspacing="1" cellpadding="0" class="TABLE_FRAME">'."\n"
			 .'<TBODY class="TABLE_TBODY">'."\n"
			 .'<TR class="Purple18_Bold_Center"><TD width="140">項目</TD><TD width="650">説明</TD><TD>現在の設定</TD></TR>'."\n"
			 .'<TR><TD>　　表示件数</TD><TD>　　ファイル一覧で表示される1ページあたりの件数を指定します。</TD><TD><INPUT TYPE="text" NAME="Data[PrintCount]" VALUE="'.$Data["PrintCount"].'" SIZE="2" ID="IME-MODE">件</TD></TR>'."\n"
			 .'<TR><TD>　　最大サイズ</TD><TD>　　アップロードできるファイルの最大サイズをバイト単位で指定します。</TD><TD><INPUT TYPE="text" NAME="Data[MaxSize]" VALUE="'.$Data["MaxSize"].'" SIZE="10" ID="IME-MODE">バイト</TD></TR>'."\n"
			 .'<TR><TD>　　DL期限1</TD><TD>　　デフォルトのDL期限を指定します。</TD><TD><INPUT TYPE="text" NAME="Data[Default_TimeLimit]" VALUE="'.$Data["Default_TimeLimit"].'" SIZE="2" ID="IME-MODE">分</TD></TR>'."\n"
			 .'<TR><TD>　　DL期限2</TD><TD>　　無期限を許可します。</TD><TD><SELECT NAME="Data[NoLimit]"><OPTION VALUE="on">許可<OPTION VALUE="off"'.($Data["NoLimit"] == "off"? " SELECTED": "").">不許可</SELECT></TD></TR>\n"
			 .'<TR><TD>　　無効データ削除</TD><TD>　　ファイルがアップロードされる度に、期限切れ等になった無効データを削除します。</TD><TD><SELECT NAME="Data[LimitCheck]"><OPTION VALUE="on">有効<OPTION VALUE="off"'.($Data["LimitCheck"] == "off"? " SELECTED": "").">無効</SELECT></TD></TR>\n"
			 .'<TR><TD>　　最大ファイル保持数</TD><TD>　　保持できるファイル数。これを超えるとアップロード不可となります。「0」を入力すると無制限になります。</TD><TD><INPUT TYPE="text" NAME="Data[MaxKeepFile]" VALUE="'.$Data["MaxKeepFile"].'" SIZE="2" ID="IME-MODE"></TD></TR>'."\n"
			 .'<TR><TD>　　古いデータの自動削除</TD><TD>　　アップロード件数が「最大ファイル保持数」に達した場合、それ以降のアップロード時に最も古いデータから削除します。</TD><TD><SELECT NAME="Data[AutoDelete]"><OPTION VALUE="on">有効<OPTION VALUE="off"'.($Data["AutoDelete"] == "off"? " SELECTED": "").">無効</SELECT></TD></TR>\n"
			 .'<TR><TD>　　ファイル圧縮</TD><TD>　　アップロードされたファイルを <B>gzip</B> で強制的に圧縮します。</TD><TD><SELECT NAME="Data[FileArchive]"><OPTION VALUE="on">有効<OPTION VALUE="off"'.($Data["FileArchive"] == "off"? " SELECTED": "").">無効</SELECT></TD></TR>\n"
			 .'<TR><TD>　　連続投稿間隔</TD><TD>　　連続投稿間隔を指定します。「0」を入力すると無効になります。</TD><TD><INPUT TYPE="text" NAME="Data[Interval]" VALUE="'.$Data["Interval"].'" SIZE="2" ID="IME-MODE">秒</TD></TR>'."\n"
			 .'<TR><TD>　　リファラチェック</TD><TD>　　他サイトを経由してきたアップロードを拒否します。</TD><TD><SELECT NAME="Data[RefererCheck]"><OPTION VALUE="on">有効<OPTION VALUE="off"'.($Data["RefererCheck"] == "off"? " SELECTED": "").">無効</SELECT></TD></TR>\n"
			 .'<TR><TD>　　ファイル名リネーム</TD><TD>　　アップロードしたファイルの名前を数字に変更します。※文字化けする場合は有効にして下さい。</TD><TD><SELECT NAME="Data[ReName]"><OPTION VALUE="on">有効<OPTION VALUE="off"'.($Data["ReName"] == "off"? " SELECTED": "").">無効</SELECT></TD></TR>\n"
			 ."</TBODY>\n"
			 ."</TABLE><BR><BR>\n"
			 .'<INPUT TYPE="hidden" NAME="Action" Value="UpdateSetting">'."\n"
			 .'<INPUT TYPE="submit" VALUE="実行" class="S2"><BR><BR><BR>'."\n"
			 .'<INPUT TYPE="submit" NAME="Action" VALUE="管理画面へ" class="S5"><BR><BR><BR><BR><BR>'."\n"
			 ### 最大サイズについて
			 .'<TABLE cellspacing="1" cellpadding="0" class="TABLE_FRAME">'."\n"
			 .'<TBODY class="TABLE_TBODY">'."\n"
			 .'<TR class="Purple18_Bold_Center"><TD width="820">最大サイズについて</TD></TR>'."\n"
			 ."<TR>\n"
			 .'<TD height="60">'."\n"
			 ."　　アップロードできるファイルの最大サイズは、<B>php.ini</B> で設定されている <B>memory_limit</B> と <B>upload_max_filesize</B> と <B>post_max_size</B> の値に依存します。<BR>\n"
			 ."　　上記の項目の関係は、<B>memory_limit >= post_max_size >= upload_max_filesize >= 最大サイズ</B> でなければなりません。<BR>\n"
			 ."　　共用レンタルサーバ等で php.ini の変更が難しい場合は、あらかじめマニュアル、サポート等を参考に設定値をご確認下さい。\n"
			 ."</TD>\n"
			 ."</TR>\n"
			 ."</TBODY>\n"
			 ."</TABLE>\n";

	PrintPage("設定情報", $HTML);
}

#------------------------------------------------------------------------------------------------------------
#	設定情報変更 - UpdateSetting
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function UpdateSetting() {
	$Data = $_POST["Data"];
	$SQL = "";

	### 数値指定項目の入力内容確認
	# 表示件数
	if($Data["PrintCount"] > 0) {
		$Update["PrintCount"] = "PrintCount = ".$Data["PrintCount"];
	}

	# 最大サイズ
	if($Data["MaxSize"] > 0) {
		$Update["MaxSize"] = "MaxSize = ".$Data["MaxSize"];
	}

	# デフォルトDL期限
	# 1以上 or 無期限許可 かつ 期限空白
	if($Data["Default_TimeLimit"] > 0 || ($Data["NoLimit"] == "on" && $Data["Default_TimeLimit"] == "")) {
		$Update["Default_TimeLimit"] = 'Default_TimeLimit = "'.$Data["Default_TimeLimit"].'"';
	# 無期限不許可 and 期限空白
	} elseif($Data["NoLimit"] == "off" && $Data["Default_TimeLimit"] == "") {
		$Update["Default_TimeLimit"] = 'Default_TimeLimit = "60"';
	}

	# 最大ファイル保持数
	if($Data["MaxKeepFile"] >= 0) {
		$Update["MaxKeepFile"] = "MaxKeepFile = ".$Data["MaxKeepFile"];
	}

	# 連続投稿間隔
	if($Data["Interval"] >= 0) {
		### 規制を無効にした場合、一時テーブルのデータを削除
		if($Data["Interval"] == "0") {
			$SQL = "DELETE FROM Temp;";
		}
		$Update["Interval"] = "Interval = ".$Data["Interval"];
	}

	### 数値指定項目が1つでも入力された場合
	if(isset($Update)) {
		$Update = ",".implode(",", $Update);
	}

	### DB更新（プルダウン項目の値は編集なし）
	$SQL .= 'UPDATE Setting SET NoLimit = "'.$Data["NoLimit"].'", LimitCheck = "'.$Data["LimitCheck"].'", FileArchive = "'.$Data["FileArchive"].'", RefererCheck = "'.$Data["RefererCheck"].'", ReName = "'.$Data["ReName"].'", AutoDelete = "'.$Data["AutoDelete"].'"'.$Update;
	QueryExec($SQL);

	PrintSetting();
}

#------------------------------------------------------------------------------------------------------------
#	データ管理表示 - PrintData
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function PrintData() {
	### データ一覧取得
	$Result = Query("SELECT No, Comment, Size, Date, FileName, SourceName, DL_Limit, Time_Limit, Host FROM FileList ORDER BY No DESC");
	$Data = $Result -> fetchall(PDO::FETCH_ASSOC);

	### データが存在する場合
	if($Data != false) {
		for($i = 0; count($Data) > $i; $i++) {
			# 背景色（有効/白、無効/薄灰）
			$Class = "";
			$Class = ($Data[$i]["DL_Limit"] == "0" || ($Data[$i]["Time_Limit"] != "" && DATE > $Data[$i]["Time_Limit"])? "U_Pink24_Center": "White24_Center");
			# DL可能回数
			$Data[$i]["DL_Limit"] = ($Data[$i]["DL_Limit"] == ""? "無制限": "残 ".$Data[$i]["DL_Limit"]);
			# DL期限
			$Data[$i]["Time_Limit"] = ($Data[$i]["Time_Limit"] == ""? "無期限": $Data[$i]["Time_Limit"]);
			# 1件分
			$List .= '<TR class="'.$Class.'" onMouseOver="MouseOver(this);" onMouseOut="MouseOut(this);"><TD ID="Cell_Margin">'.$Data[$i]["No"].'</TD><TD><INPUT TYPE="hidden" NAME="FileName[]" VALUE="'.$Data[$i]["FileName"].'"><A href="./src/'.$Data[$i]["FileName"].'" target="_blank">'.$Data[$i]["FileName"].'</A></TD><TD ID="Cell_Margin_Right">'.$Data[$i]["Comment"].'</TD><TD ID="Cell_Margin_Right">'.$Data[$i]["SourceName"].'</TD><TD ID="Cell_Margin_Left">'.$Data[$i]["Size"].'</TD><TD>'.$Data[$i]["Date"].'</TD><TD>'.$Data[$i]["DL_Limit"].'</TD><TD>'.$Data[$i]["Time_Limit"].'</TD><TD ID="Cell_Margin">'.$Data[$i]["Host"].'</TD><TD><INPUT TYPE="CheckBox" NAME="DelNo[]" VALUE="'.$Data[$i]["No"].'"></TD></TR>'."\n";
		}
	}

	### 1件も存在しない場合
	if($List == "") {
		$List = '<TR><TD class="White24_Center" colspan="10">データがありません。</TD></TR>'."\n";
	}		

	$HTML = ### 一括削除等
			  '<INPUT TYPE="submit" NAME="Process" VALUE="無効データをすべて削除" class="S10">　　　　<INPUT TYPE="submit" NAME="Process" VALUE="存在するすべてのデータを削除" class="S13">'."\n"
			 .'<INPUT TYPE="hidden" NAME="Action" VALUE="DeleteConfirm">'."\n"
			 ."</FORM>\n"
			 ."※ 背景色が桃色のデータは無効データです。<BR><BR>\n"
			 .'<FORM METHOD="post" ACTION="./admin.php">'."\n"
			 ### 現在の情報
			 .'<SPAN class="FileList">'."\n"
			 .'<TABLE cellspacing="1" cellpadding="0" class="TABLE_FRAME">'."\n"
			 .'<TBODY class="TABLE_TBODY">'."\n"
			 .'<TR class="Purple18_Bold_Center"><TD ID="Cell_Margin">No</TD><TD ID="Cell_Margin">ファイル名</TD><TD ID="Cell_Margin">コメント</TD><TD ID="Cell_Margin">元のファイル名</TD><TD ID="Cell_Margin">サイズ</TD><TD width="150">日付</TD><TD ID="Cell_Margin">DL可能回数</TD><TD width="150">ダウンロード期限</TD><TD ID="Cell_Margin">アップロード者のホスト</TD><TD ID="Cell_Margin">選択</TD></TR>'."\n"
			 .$List
			 ."</TBODY>\n"
			 ."</TABLE>\n"
			 ."</SPAN><BR><BR>\n"
			 .'<INPUT TYPE="hidden" NAME="Action" Value="DeleteData">'."\n"
			 .'<INPUT TYPE="submit" VALUE="削除" class="S2"><BR><BR><BR>'."\n"
			 .'<INPUT TYPE="submit" NAME="Action" VALUE="管理画面へ" class="S5">'."\n";

	PrintPage("データ管理", $HTML);
}

#------------------------------------------------------------------------------------------------------------
#	データ一括削除確認画面 - DeleteConfirm
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function DeleteConfirm() {
	$Process = $_POST["Process"];

	### 無効データ削除
	if($Process == "無効データをすべて削除") {
		$Message = "以下の条件に当てはまるデータをすべて削除します。実行しますか？<BR><BR>\n"
					 .'<TABLE border="0">'."\n"
					 ."<TR><TD>\n"
					 ."<UL>\n"
					 ."<li>DL期限切れ\n"
					 ."<li>DL可能回数が0になった\n"
					 ."<li>有効データ数が「最大ファイル保持数」を超えている場合<BR>→ 「最大ファイル保持数」が100件で有効データ数が150件の場合、超えた分の50件が日付の古い順に削除されます。\n"
					 ."</UL>\n"
					 ."</TD></TR>\n"
					 ."</TABLE><BR>\n"
					 .'<INPUT TYPE="hidden" NAME="Action" Value="DeleteExpireData">'."\n";
	### 全データ削除
	} elseif($Process = "存在するすべてのデータを削除") {
		$Message = "<B>存在する 有効/無効 を含むすべてのデータ</B> を削除します。実行しますか？<BR><BR><BR>\n"
					 .'<INPUT TYPE="hidden" NAME="Action" Value="DeleteAllData">'."\n";
		
	}

	$HTML = $Message
			 .'<INPUT TYPE="submit" VALUE="実行" class="S2"><BR><BR><BR>'."\n"
			 .'<INPUT TYPE="submit" NAME="Action" VALUE="管理画面へ" class="S5">'."\n";

	PrintPage("データ管理", $HTML);
}

#------------------------------------------------------------------------------------------------------------
#	無効データ一括削除 - DeleteExpireData
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function DeleteExpireData() {
	# 無効データ削除
	DeleteFile("on", "");
	# 古いデータ削除
	DeleteFile("", "all");

	PrintData();
}

#------------------------------------------------------------------------------------------------------------
#	全データ一括削除 - DeleteAllData
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function DeleteAllData() {
	##### ファイル削除
	### ディレクトリオープン
	$Dir = opendir("./src/");

	### ディレクトリ内すべて
	while(($FileName = readdir($Dir)) != false) {
		# 存在するファイルのみ
		if(is_file("./src/".$FileName)) {
			@unlink("./src/".$FileName);
		}
	}

	### DBから削除
	QueryExec("DELETE FROM FileList");

	PrintData();
}

#------------------------------------------------------------------------------------------------------------
#	選択データ削除 - DeleteData
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function DeleteData() {
	### 削除対象No配列
	$DelNo = $_POST["DelNo"];

	### Noが選択された場合のみ
	if(count($DelNo) > 0) {
		# WHERE条件生成
		for($i = 0; count($DelNo) > $i; $i++) {
			$DelNo[$i] = "No = ".$DelNo[$i];
		}
		# 削除対象ファイル名取得
		$Result = Query("SELECT FileName FROM FileList WHERE ".implode(" OR ", $DelNo));
		$Data = $Result -> fetchall(PDO::FETCH_ASSOC);
		# ファイル削除
		for($i = 0; count($Data) > $i; $i++) {
			@unlink("./src/".$Data[$i]["FileName"]);
		}

		# DBから削除
		QueryExec("DELETE FROM FileList WHERE ".implode(" OR ", $DelNo));
	}

	PrintData();
}

#------------------------------------------------------------------------------------------------------------
#	ホスト/IP規制表示 - PrintRegulation
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function PrintRegulation() {
	### 設定情報取得
	$Data = explode("\n", file_get_contents("./.htaccess"));
	$Start = (array_search("#Regulation", $Data) + 1);

	# 現在の設定一覧作成
	for($i = $Start; count($Data) > $i; $i++) {
		if($Data[$i] != "") {
			$Reg .= '<TR class="White20_Center"><TD ID="Cell_Margin_Right">'.substr($Data[$i], 10).'</TD><TD><INPUT TYPE="CheckBox" NAME="DelNo[]" VALUE="'.substr($Data[$i], 10).'"></TD></TR>'."\n";
		}
	}

	# 未設定の場合
	if($Reg == "") {
		$Reg = '<TR><TD class="White20_Center" colspan="2">未設定です</TD></TR>'."\n";
	}

	# 一覧
	$List = '<TABLE class="TABLE_FRAME" cellspacing="1" cellpadding="0">'."\n"
			 .'<TBODY class="TABLE_TBODY">'."\n"
			 .'<TR class="Purple18_Bold_Center"><TD colspan="2">現在の設定</TD></TR>'."\n"
			 .'<TR class="U_Gray18_Bold_Center"><TD height="20" ID="Cell_Margin">IP/ホスト</TD><TD width="50">削除</TD></TR>'."\n"
			 .$Reg
			 ."</TBODY>\n"
			 ."</TABLE>\n";

	$HTML = ### 現在の情報
			  $List
			 ."<BR><BR>\n"
			 .'<INPUT TYPE="hidden" NAME="Action" Value="DeleteRegulation">'."\n"
			 .'<INPUT TYPE="submit" VALUE="削除" class="S2">'."\n"
			 ."</FORM>\n"
			 ."------------------------------------------------------------<BR><BR>\n"
			 ### 新規追加
			 .'<FORM METHOD="post" ACTION="./admin.php">'."\n"
			 .'<TABLE class="TABLE_FRAME" cellspacing="1" cellpadding="0">'."\n"
			 .'<TBODY class="TABLE_TBODY">'."\n"
			 .'<TR class="Purple18_Bold_Center"><TD>新規追加</TD></TR>'."\n"
			 ."<TR><TD>\n"
			 ."<BR>　　* ホスト名、IPに以下の文字列が含まれる場合に規制対象となります。\n"
			 ."<BR>　　* 閲覧自体できなくなります。\n"
			 ."<BR>　　* 192.168.0.0/24 等の指定も可能です。\n"
			 ."<BR>　　* <B>\</B> は削除されます。<BR><BR>\n"
			 ."</TD></TR>\n"
			 .'<TR><TD><INPUT TYPE="text" NAME="Host" SIZE="80" ID="IME-Mode"></TD></TR>'."\n"
			 ."</TBODY>\n"
			 ."</TABLE><BR><BR>\n"
			 .'<INPUT TYPE="hidden" NAME="Action" Value="NewRegulation">'."\n"
			 .'<INPUT TYPE="submit" VALUE="追加" class="S2"><BR><BR><BR>'."\n"
			 .'<INPUT TYPE="submit" NAME="Action" VALUE="管理画面へ" class="S5">'."\n";

	PrintPage("ホスト/IP規制", $HTML);
}

#------------------------------------------------------------------------------------------------------------
#	ホスト/IP規制削除 - DeleteRegulation
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function DeleteRegulation() {
	### 削除対象No配列
	$DelNo = $_POST["DelNo"];

	### Noが選択された場合のみ
	if(count($DelNo) > 0) {
		# 現在の設定を取得
		$Data = explode("\n", file_get_contents("./.htaccess"));
		# 選択した分
		for($i = 0; count($DelNo) > $i; $i++) {
			# 削除対象データ取得
			if(($No = array_search("deny from ".$DelNo[$i], $Data)) != false) {
				# 削除
				unset($Data[$No]);
			}
		}
		# .htaccessファイル更新
		$FP = fopen("./.htaccess", "w");
		fwrite($FP, implode("\n", $Data));
		fclose($FP);
	}

	PrintRegulation();
}

#------------------------------------------------------------------------------------------------------------
#	ホスト/IP規制追加 - NewRegulation
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function NewRegulation() {
	### \ は削除
	$Host = str_replace("\\", "", $_POST["Host"]);

	### 入力された場合のみ
	if($Host != "") {
		# .htaccessファイル更新
		$FP = fopen("./.htaccess", "a");
		fwrite($FP, "deny from ".$Host."\n");
		fclose($FP);
	}

	PrintRegulation();
}

#------------------------------------------------------------------------------------------------------------
#	DBファイル最適化 - DBOptimize
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function DBOptimize() {
	QueryExec("VACUUM");

	PrintMenu();
}

#------------------------------------------------------------------------------------------------------------
#	ページ出力 - PrintPage
#	---------------------------------------
#	引　数：タイトル、メッセージ
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function PrintPage($Title, $Message) {
	print "<HTML>\n"
		  ."<HEAD>\n"
		  .'<META http-equiv="Content-Type" content="text/html; charset=UTF-8">'."\n"
		  .'<META http-equiv="Content-Script-Type" content="text/javascript">'."\n"
		  .'<META http-equiv="Content-Style-Type" content="text/css">'."\n"
		  ."<TITLE>".$Title."</TITLE>\n"
		  .'<SCRIPT type="text/javascript" src="./jscript.js"></SCRIPT>'."\n"
		  .'<link rel="stylesheet" type="text/css" href="./style.css">'."\n"
		  ."</HEAD>\n"
		  ."<BODY>\n"
		  ."<CENTER>\n"
		  .$Title."<BR><BR><BR>\n"
		  .'<FORM METHOD="post" ACTION="./admin.php">'."\n"
		  .$Message
		  ."</FORM>\n"
		  ."</CENTER>\n"
		  ."</BODY>\n"
		  ."</HTML>";
		  exit;
}
?>
