<?
#============================================================================================================
#	PHPファイルアップロードスクリプト　Sch Uploader
#	Copyright 2010 sitm. All Rights Reserved.
#	URL       : http://www.sanadake.info/sch/
#	Support   : http://www.sanadake.info/bbs/read.php/support/
#	-----------------------------------------------
#	アップロードフォーム・データ一覧画面
#	uploader.php
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

# 設定情報取得
$Set = getSetting();

# ファイル一覧
$List = getDataList($Set["PrintCount"]);
# アップロードフォーム
$Form = getUploadForm($Set);

# ページ出力
PrintPage($Form, $List);

#------------------------------------------------------------------------------------------------------------
#	ファイル一覧生成 - getDataList
#	---------------------------------------
#	引　数：表示件数
#	戻り値：ファイル一覧
#------------------------------------------------------------------------------------------------------------
function getDataList($PrintCount) {
	### 表示件数分データ取得
	# 取得範囲
	$Range = ($_GET["p"] == ""? "0, ".$PrintCount: (($_GET["p"] - 1) * $PrintCount).", ".$PrintCount);
	# 有効データのみ
	$Result = Query('SELECT No, DLKey, Comment, Size, Date, FileName, SourceName, DL_Limit, Time_Limit FROM FileList WHERE Time_Limit >= "'.DATE.'" OR Time_Limit = "" ORDER BY No DESC Limit '.$Range);
	$Data = $Result -> fetchall(PDO::FETCH_ASSOC);
	unset($Result);

	### ファイル一覧生成
	for($i = 0; count($Data) > $i; $i++) {
		# DL可能回数
		$DL_Limit =($Data[$i]["DL_Limit"] == ""? "無制限": "残 ".$Data[$i]["DL_Limit"]);
		# DL期限
		$Time_Limit =($Data[$i]["Time_Limit"] == ""? "無期限": $Data[$i]["Time_Limit"]);

		# DLキー or DL回数制限 or DL期限有り
		if(strrpos($Data[$i]["FileName"], "_") != false) {
			# DLキー有りの場合
			if($Data[$i]["DLKey"] != "") {
				$Data[$i]["Comment"] = '<SPAN ID="Red">[DLKey]</SPAN> '.$Data[$i]["Comment"];
			}
			# ファイル名/リンク編集
			$Data[$i]["FileName"] = substr($Data[$i]["FileName"], 21);
			$Link = '<A href="./download.php?No='.$Data[$i]["No"].'" target="_blank">'.$Data[$i]["FileName"]."</A>";
		# DLキーなし
		} else {
			# リンク編集
			$Link = '<A href="./src/'.$Data[$i]["FileName"].'" target="_blank">'.$Data[$i]["FileName"]."</A>";
		}
		# 1件分
		$FileList .= '<TR class="White24_Center" onMouseOver="MouseOver(this);" onMouseOut="MouseOut(this);"><TD>'.$Link.'</TD><TD ID="Cell_Margin_Right">'.$Data[$i]["Comment"].'</TD><TD ID="Cell_Margin_Right">'.$Data[$i]["SourceName"].'</TD><TD ID="Cell_Margin_Left">'.$Data[$i]["Size"].'</TD><TD ID="Cell_Margin">'.$Data[$i]["Date"]."</TD><TD>".$DL_Limit.'</TD><TD ID="Cell_Margin">'.$Time_Limit.'</TD><TD><INPUT TYPE="Radio" NAME="DelNo" VALUE="'.$Data[$i]["No"].'"></TD></TR>'."\n";
	}
	unset($Data);

	### 1件も存在しない場合
	if($FileList == "") {
		$FileList = '<TR><TD class="White24_Center" colspan="8">データがありません。</TD></TR>'."\n";
	}

	### ページリスト取得
	$Pager = getPager($PrintCount);

	### 一覧テーブル作成
	$List = '<FORM METHOD="post" ACTION="./update.php">'."\n"
			 .$Pager
			 .'<SPAN class="FileList">'."\n"
			 .'<TABLE cellspacing="1" cellpadding="0" class="TABLE_FRAME">'."\n"
			 .'<TBODY class="TABLE_TBODY">'."\n"
			 .'<TR class="Purple18_Bold_Center"><TD ID="Cell_Margin">FILE</TD><TD ID="Cell_Margin">コメント</TD><TD ID="Cell_Margin">元のファイル名</TD><TD ID="Cell_Margin">サイズ</TD><TD width="150">日付</TD><TD ID="Cell_Margin">DL可能回数</TD><TD ID="Cell_Margin">ダウンロード期限</TD><TD ID="Cell_Margin">削除</TD></TR>'."\n"
			 .$FileList
			 ."</TBODY>\n"
			 ."</TABLE>\n"
			 ."</SPAN>\n"
			 ."<HR>\n"
			 .'<INPUT TYPE="hidden" NAME="Action" VALUE="Delete">'."\n"
			 .'ファイル削除：<INPUT TYPE="text" NAME="DelKey" SIZE="10" ID="IME-Mode">　<INPUT TYPE="submit" VALUE="削除" class="S2">'."\n"
			 ."</FORM>\n";

	return $List;
}

#------------------------------------------------------------------------------------------------------------
#	ページリスト取得 - getPager
#	---------------------------------------
#	引　数：表示件数
#	戻り値：ページリスト
#------------------------------------------------------------------------------------------------------------
function getPager($PrintCount) {
	### 有効データ全件数取得
	$Result = Query('SELECT COUNT(No) AS AllCount FROM FileList WHERE Time_Limit >= "'.DATE.'" OR Time_Limit = ""');
	$Data = $Result -> fetch(PDO::FETCH_ASSOC);
	define("ALLCOUNT", $Data["AllCount"]);
	unset($Result);

	### 全件数が表示件数よりも多い場合
	if(ALLCOUNT > $PrintCount) {
		### ページリンク作成
		# 現在ページ数設定
		$PageCnt = ($_GET["p"] != ""? $_GET["p"]: 1);
		# 表示開始行
		$Start = ($PageCnt > 1? (($PageCnt - 1) * $PrintCount): 0);
		# 表示終了行
		$End = $Start + $PrintCount;
		# パラメータ
		$Param = "./uploader.php?";
		# 前へリンク
		$Last = ($PageCnt > 1? '<TD width="50"><A href="'.$Param.'&p='.($PageCnt - 1).'">< 前へ</A></TD>': "");
		# 次へリンク
		$Next = (ALLCOUNT > ($PageCnt * $PrintCount)? '<TD width="50"><A href="'.$Param.'&p='.($PageCnt + 1).'">次へ ></A></TD>': "");

		### ページ一覧 ###
		# 全ページ数
		$AllPageCnt = ceil(ALLCOUNT / $PrintCount);
		# 開始位置
		$StartPage = (($PageCnt + 5) > $AllPageCnt? ($AllPageCnt - 9): ($PageCnt - 5));
		# 開始位置がマイナスの場合
		$StartPage = (1 > $StartPage? 1: $StartPage);
		# 表示範囲
		$Start = (($PageCnt - 1) * $PrintCount + 1);
		$End = (($PageCnt * $PrintCount) > ALLCOUNT? ALLCOUNT: ($PageCnt * $PrintCount));
		# 2ページ以上ある場合のみ作成
		if($AllPageCnt > 1) {
			for($i = $StartPage; $AllPageCnt >= $i && ($PageCnt + 5) > $i; $i++) {
				if($i != $PageCnt) {
					$PageLink .=  '<TD width="20"><A href="'.$Param.'&p='.$i.'">'.$i.'</A></TD>';
				} else {
					$PageLink .= '<TD width="20" ID="PageSees">'.$i."</TD>";
				}
			}
			# リンクテーブル作成
			$PageLink = '<TABLE cellspacing="1" cellpadding="0" class="TABLE_FRAME">'."\n"
						  .'<TBODY class="TABLE_TBODY">'."\n"
						  .'<TR class="PageLink" align="center">'.$Last.$PageLink.$Next."</TR>\n"
						  ."</TBODY>\n"
						  ."</TABLE><BR>\n";
		}

		### 編集
		$Pager = "<HR>\n"
				  .ALLCOUNT." 件中　".$Start."～".$End." 件目<BR>\n"
				  ."<HR>\n"
				  .$PageLink;

	} else {
		### 編集
		$Pager = "<HR>\n"
				  .ALLCOUNT." 件<BR>\n"
				  ."<HR>\n";
		
	}

	return $Pager;
}

#------------------------------------------------------------------------------------------------------------
#	アップロードフォーム - getUploadForm
#	---------------------------------------
#	引　数：設定情報
#	戻り値：アップロードフォーム
#------------------------------------------------------------------------------------------------------------
function getUploadForm($Set) {
	### アップロード可否
	# ファイル保持数が上限に達したが、古いデータの自動削除をしない場合
	if($Set["MaxKeepFile"] > 0 && ALLCOUNT >= $Set["MaxKeepFile"] && $Set["AutoDelete"] == "off") {
		$Flag = " disabled";
		$Message = '<SPAN ID="Red"><B>アップロードされているファイルが <U>上限数：'.$Set["MaxKeepFile"]." 件</U> に達したためアップロードができません。削除されるか、無効になるまでお待ち下さい。</B></SPAN>\n";
	# ファイル保持数が上限に達し、古いデータの自動削除をする場合
	} elseif($Set["MaxKeepFile"] > 0 && ALLCOUNT >= $Set["MaxKeepFile"] && $Set["AutoDelete"] == "on") {
		$Message = '<SPAN ID="Red"><B>アップロードされているファイルが <U>上限数：'.$Set["MaxKeepFile"]." 件</U> に達しました。これ以降アップロードすると古いデータから削除されます。</B></SPAN>\n";
	# ファイル保持数が上限に達していない場合
	} elseif($Set["MaxKeepFile"] > 0 && $Set["AutoDelete"] == "off") {
		$Message = "アップロードできるファイルは ".$Set["MaxKeepFile"]." 件 です。これ以上になるとアップロードができなくなります。\n";
	}

	$Form = '<FORM METHOD="post" ACTION="./update.php" ENCTYPE="multipart/form-data">'."\n"
			 .'<TABLE cellspacing="1" cellpadding="0" class="TABLE_FRAME">'."\n"
			 .'<TBODY class="TABLE_TBODY">'."\n"
			 .'<TR><TD class="Purple18_Bold_Left" width="100">　ファイル　</TD><TD><INPUT TYPE="file" NAME="File" SIZE="50"><BR>　※サイズは <B>'.floor($Set["MaxSize"] / 1024)."KB</B> まで</TD></TR>\n"
			 .'<TR><TD class="Purple18_Bold_Left">　コメント　</TD><TD><INPUT TYPE="text" NAME="Comment" SIZE="50"></TD></TR>'."\n"
			 .'<TR><TD class="Purple18_Bold_Left">　DLキー　</TD><TD><INPUT TYPE="text" NAME="DLKey" SIZE="10" ID="IME-Mode"></TD></TR>'."\n"
			 .'<TR><TD class="Purple18_Bold_Left">　削除キー　</TD><TD><INPUT TYPE="text" NAME="DelKey" SIZE="10" ID="IME-Mode">　<B>※入力必須です。</B></TD></TR>'."\n"
			 .'<TR><TD class="Purple18_Bold_Left">　DL可能回数　</TD><TD><INPUT TYPE="text" NAME="DL_Limit" SIZE="10" ID="IME-Mode"> 回まで　※空白は無制限</TD></TR>'."\n"
			 .'<TR><TD class="Purple18_Bold_Left">　DL可能時間　</TD><TD><INPUT TYPE="text" NAME="Time_Limit" VALUE="'.$Set["Default_TimeLimit"].'" SIZE="10" ID="IME-Mode"> 分<BR>　※最大1440分(24時間)まで'.($Set["NoLimit"] == "on"? "<BR>　※空白は無制限": "")."</TD></TR>\n"
			 .'<TR><TD class="Purple18_Bold_Left">　アップロード上限数　</TD><TD>　'.($Set["MaxKeepFile"] == "0"? "無制限": $Set["MaxKeepFile"]."件")."</TD></TR>\n"
			 ."</TBODY>\n"
			 ."</TABLE><BR>\n"
			 .'<INPUT TYPE="hidden" NAME="Action" VALUE="New">'."\n"
			 .'<INPUT TYPE="submit" VALUE="アップロード"'.$Flag.'" class="S5">'."\n"
			 ."</FORM>\n"
			 .$Message;

	return $Form;
}

#------------------------------------------------------------------------------------------------------------
#	ページ出力 - PrintPage
#	---------------------------------------
#	引　数：新規フォーム、ファイル一覧
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function PrintPage($Form, $List) {
	print "<HTML>\n"
		  ."<HEAD>\n"
		  .'<META http-equiv="Content-Type" content="text/html; charset=UTF-8">'."\n"
		  .'<META http-equiv="Content-Style-Type" content="text/css">'."\n"
		  .'<META http-equiv="Content-Script-Type" content="text/javascript">'."\n"
		  .'<META NAME="robots" CONTENT="noarchive">'."\n"
		  ."<TITLE>Sch Uploader - ファイル一覧</TITLE>\n"
		  .'<SCRIPT type="text/javascript" src="./jscript.js"></SCRIPT>'."\n"
		  .'<link rel="stylesheet" type="text/css" href="./style.css">'."\n"
		  ."</HEAD>\n"
		  ."<BODY>\n"
	### フォーム
		  .$Form
	### 一覧出力
		  .$List
	### バージョン情報
		  ."<BR><BR>".VERSION.'　by　<A href="http://www.sanadake.info/">Sch</A>'."\n"
		  ."</BODY>\n"
		  ."</HTML>";
}
?>
