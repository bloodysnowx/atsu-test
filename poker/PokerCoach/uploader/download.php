<?
#============================================================================================================
#	PHPファイルアップロードスクリプト　Sch Uploader
#	Copyright 2010 sitm. All Rights Reserved.
#	URL       : http://www.sanadake.info/sch/
#	Support   : http://www.sanadake.info/bbs/read.php/support/
#	-----------------------------------------------
#	ファイルダウンロード
#	download.php
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

### DLキー入力時
$KeyInput = $_POST["KeyInput"];
$DLKey = $_POST["DLKey"];

### DL対象Noのデータを取得
define("NO", $_GET["No"]);
if(NO != "") {
	$Result = Query("SELECT DLKey, Size, FileName, SourceName, DL_Limit, Time_Limit FROM FileList WHERE No = ".NO);
	$Data = $Result -> fetch(PDO::FETCH_ASSOC);
	unset($Result);
}

### データが存在する場合
if($Data != false) {
	### DL可否判定
	CheckCondition($Data);

	### DLキーなし or DLキーが一致
	if($Data["DLKey"] == "" || ($KeyInput == "ON" && $DLKey == $Data["DLKey"])) {
		### ファイル出力
		SubmitFile($Data["FileName"], substr($Data["FileName"], 21));
		### 後処理
		AfterProcess($Data["FileName"], $Data["DL_Limit"]);
	### DLキーが一致しない
	} elseif($KeyInput == "ON") {
		PrintHTML("DL Error!", "DLキーが違います。");
	### DLキーの入力が必要
	} else {
		$Form = "DLキーを入力して下さい。<BR><BR>\n"
				 .'<TABLE cellspacing="1" cellpadding="0" class="TABLE_FRAME">'."\n"
				 .'<TBODY class="TABLE_TBODY">'."\n"
				 .'<TR class="Purple18_Bold_Center" ID="Cell_Margin"><TD>元のファイル名</TD><TD>サイズ</TD></TR>'."\n"
				 .'<TR class="White24_Center"><TD ID="Cell_Margin">'.$Data["SourceName"].'</TD><TD align="right" ID="Cell_Margin_Left">'.$Data["Size"]."</TD></TR>\n"
				 ."</TBODY>\n"
				 ."</TABLE><BR>\n"
				 .'<FORM METHOD="post" ACTION="./download.php?No='.NO.'">'."\n"
				 .'<INPUT TYPE="text" NAME="DLKey" SIZE="10" ID="IME-Mode">　<INPUT TYPE="submit" VALUE="ダウンロード">'."\n"
				 .'<INPUT TYPE="hidden" NAME="KeyInput" VALUE="ON">'."\n"
				 ."</FORM>";
		PrintHTML("DLキー入力", $Form);
	}
### 存在しない場合
} else {
	PrintHTML("DL Error!", "ファイルが存在しません。");
}

#------------------------------------------------------------------------------------------------------------
#	DL可否判定 - CheckCondition
#	---------------------------------------
#	引　数：ファイル情報
#	戻り値：なし or エラーページ
#------------------------------------------------------------------------------------------------------------
function CheckCondition($Data) {
	### DL可能時間
	# DL期限切れ
	if($Data["Time_Limit"] != "" && DATE > $Data["Time_Limit"]) {
		DeleteExpireFile($Data["FileName"]);
		PrintHTML("DL Error!", "DL可能時間を過ぎました。このファイルはダウンロードできません。");
	}

	### DL可能回数
	if($Data["DL_Limit"] === "0") {
		DeleteExpireFile($Data["FileName"]);
		PrintHTML("DL Error!", "DL可能回数に達しました。このファイルはダウンロードできません。");
	}
}

#------------------------------------------------------------------------------------------------------------
#	ファイル出力 - SubmitFile
#	---------------------------------------
#	引　数：実ファイル名、出力用ファイル名
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function SubmitFile($FileName, $DummyName) {
	header("Content-Type: application/octet-stream");
	header("Content-Length: ".filesize("./src/".$FileName));
	header('Content-Disposition: attachment; filename="'.$DummyName.'"');
	readfile("./src/".$FileName);
}

#------------------------------------------------------------------------------------------------------------
#	出力後処理 - AfterProcess
#	---------------------------------------
#	引　数：ファイル名、DL可能回数
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function AfterProcess($FileName, $DL_Limit) {
	### DL回数制限がある
	if($DL_Limit > 0) {
		# 残1の場合はファイルを削除する
		if($DL_Limit == 1) {
			DeleteExpireFile($FileName);
		# 残2以上の場合はDL可能回数を減らす
		} else {
			# DB更新
			QueryExec("UPDATE FileList SET DL_Limit = (DL_Limit - 1) WHERE No = ".NO);
		}
	}
}

#------------------------------------------------------------------------------------------------------------
#	無効ファイル削除 - DeleteExpireFile
#	---------------------------------------
#	引　数：ファイル名
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function DeleteExpireFile($FileName) {
	# ファイル削除
	@unlink("./src/".$FileName);

	# DB更新
	QueryExec("DELETE FROM FileList WHERE No = ".NO);
}
?>
