<?
#------------------------------------------------------------------------------------------------------------
#	定数設定
#------------------------------------------------------------------------------------------------------------
### バージョン
define("VERSION", "Sch Uploader Ver1.2 - 2013/02/24");
### 日付
define("DATE", date("Y/m/d H:i:s"));

#------------------------------------------------------------------------------------------------------------
#	ページ出力 - PrintHTML
#	---------------------------------------
#	引　数：タイトル、メッセージ
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function PrintHTML($Title, $Message) {
	print "<HTML>\n"
		  ."<HEAD>\n"
		  .'<META http-equiv="Content-Type" content="text/html; charset=UTF-8">'."\n"
		  .'<META http-equiv="Content-Style-Type" content="text/css">'."\n"
		  ."<TITLE>".$Title."</TITLE>\n"
		  .'<link rel="stylesheet" type="text/css" href="./style.css">'."\n"
		  ."</HEAD>\n"
		  ."<BODY>\n"
		  ."<CENTER>\n"
		  .$Message."\n"
		  .'<BR><BR><BR><A href="./uploader.php">もどる</A>'."\n"
		  ."</CENTER>\n"
		  ."</BODY>\n"
		  ."</HTML>";
		  exit;
}

#------------------------------------------------------------------------------------------------------------
#	無効データ・古いデータの削除 - DeleteFile
#	---------------------------------------
#	引　数：無効データ削除(有効/無効)、古いデータの自動削除(有効/無効)
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function DeleteFile($LimitCheck, $AutoDelete) {
	### Singular：1件削除/Plural：複数削除
	$Number = "";

	### 無効データ削除が有効の場合
	if($LimitCheck == "on") {
		# DL期限切れ・DL可能回数に達したデータを取得
		$SQL = 'SELECT No, FileName FROM FileList WHERE (Time_Limit != "" AND "'.DATE.'" > Time_Limit) OR DL_Limit = "0"';
		$Result = Query($SQL);
		$Data = $Result -> fetchall(PDO::FETCH_ASSOC);
		unset($Result);

		# データが存在する場合のみ
		if($Data != false) {
			$Number = "Plural";
		}
	}

	### 古いデータの自動削除が有効の場合
	if($AutoDelete == "on") {
		# 有効データのうち、最も古いデータ1件を取得
		$Result = Query('SELECT No, FileName FROM FileList WHERE (Time_Limit > "'.DATE.'" OR Time_Limit = "") AND DL_Limit != 0 ORDER BY Date ASC Limit 1');
		$Data = $Result -> fetch(PDO::FETCH_ASSOC);
		unset($Result);

		# データが存在する場合のみ
		if($Data != false) {
			$Number = "Singular";
		}
	}

	### 古いデータの自動削除が有効の時に最大ファイル保持数を変更した場合の一括削除
	if($AutoDelete == "all") {
		### 設定情報取得
		$Set = getSetting();

		### 最大ファイル保持数と古いデータの自動削除が有効の場合
		if($Set["MaxKeepFile"] > 0 && $Set["AutoDelete"] == "on") {
			# 有効データ数取得
			$ValidDataCount = getValidDataCount();
			# 最大ファイル保持数と有効データ数の差
			$Diff = ($ValidDataCount - $Set["MaxKeepFile"]);

			# 差が1以上の場合、その分だけデータを削除する
			if($Diff > 0) {
				# 削除対象となる古いデータを取得
				$Result = Query('SELECT No, FileName FROM FileList WHERE (Time_Limit > "'.DATE.'" OR Time_Limit = "") AND DL_Limit != 0 ORDER BY Date ASC Limit '.$Diff);
				$Data = $Result -> fetchall(PDO::FETCH_ASSOC);
				unset($Result);
				# データが存在する場合のみ
				if($Data != false) {
					$Number = "Plural";
				}
			}
		}
	}

	### データ削除処理
	# 1件削除
	if($Number == "Singular") {
		# ファイル削除
		@unlink("./src/".$Data["FileName"]);
		# DBから削除
		$SQL = "DELETE FROM FileList WHERE No = ".$Data["No"];
		QueryExec($SQL);
	# 複数件削除
	} elseif($Number == "Plural") {
		$SQL = "";
		# データが複数ある場合はすべて削除する
		for($i = 0; count($Data) > $i; $i++) {
			$NoRange .= "No = ".$Data[$i]["No"]." OR ";
			# ファイル削除
			@unlink("./src/".$Data[$i]["FileName"]);
		}
		# DBから削除
		$NoRange = substr($NoRange, 0, -4);
		$SQL = "DELETE FROM FileList WHERE ".$NoRange;
		QueryExec($SQL);
	}
}

#------------------------------------------------------------------------------------------------------------
#       有効データ数取得 - getValidDataCount
#       ---------------------------------------
#       引　数：なし
#       戻り値：有効データ数
#------------------------------------------------------------------------------------------------------------
function getValidDataCount() {
	$Result = Query('SELECT COUNT(No) AS Count FROM FileList WHERE (Time_Limit > "'.DATE.'" OR Time_Limit = "") AND DL_Limit != 0');
	$Data = $Result -> fetch(PDO::FETCH_ASSOC);
	$ValidDataCount = $Data["Count"];
	return $ValidDataCount;
}


#------------------------------------------------------------------------------------------------------------
#	設定情報取得 - getSetting
#	---------------------------------------
#	引　数：なし
#	戻り値：設定情報
#------------------------------------------------------------------------------------------------------------
function getSetting() {
	$Result = Query("SELECT * FROM Setting");
	$Data = $Result -> fetch(PDO::FETCH_ASSOC);
	return $Data;
}

#------------------------------------------------------------------------------------------------------------
#	クエリ送信 - QueryExec
#	---------------------------------------
#	引　数：SQL文
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function QueryExec($SQL) {
	$Han = new PDO("sqlite:./Uploader.db");
	$Result = $Han -> exec($SQL);
}

#------------------------------------------------------------------------------------------------------------
#	クエリ送信 - Query
#	---------------------------------------
#	引　数：SQL文
#	戻り値：結果セット
#------------------------------------------------------------------------------------------------------------
function Query($SQL) {
	$Han = new PDO("sqlite:./Uploader.db");
	$Result = $Han -> query($SQL);
	return $Result;
}

#------------------------------------------------------------------------------------------------------------
#	エスケープ文字削除 - DeleteStripslashes
#	---------------------------------------
#	引　数：変更前配列
#	戻り値：変更済配列
#------------------------------------------------------------------------------------------------------------
function DeleteStripslashes($Post) {
    if(is_array($Post)) {
        $Post = array_map("DeleteStripslashes", $Post);
    } else {
        $Post = preg_replace("/\r/", "", htmlspecialchars((get_magic_quotes_gpc()? stripslashes($Post): $Post), ENT_QUOTES));
    }
    return $Post;
}
?>
