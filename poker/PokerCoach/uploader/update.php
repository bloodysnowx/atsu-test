<?
#============================================================================================================
#	PHPファイルアップロードスクリプト　Sch Uploader
#	Copyright 2010 sitm. All Rights Reserved.
#	URL       : http://www.sanadake.info/sch/
#	Support   : http://www.sanadake.info/bbs/read.php/support/
#	-----------------------------------------------
#	データ追加、削除
#	update.php
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

### 処理選択
$Action = $_POST["Action"];
# 新規アップロード
if($Action == "New") {
	NewFile();
# ファイル削除
} elseif($Action == "Delete") {
	Delete();
}

#------------------------------------------------------------------------------------------------------------
#	新規アップロード - NewFile
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function NewFile() {
	### 設定情報取得
	$Set = getSetting();

	### リファラチェック
	if($Set["RefererCheck"] == "on" && !preg_match("(".$_SERVER["HTTP_HOST"].")", $_SERVER["HTTP_REFERER"])) {
		PrintHTML("Error!", "リファラ情報が変な気がする。");
	}

	### 連投規制が有効の場合
	if($Set["Interval"] > 0) {
		# ホスト
		$Host = gethostbyaddr($_SERVER["REMOTE_ADDR"]);
		# 現在時刻/秒
		$TimeStamp = time();
		# 規制中か確認
		$Result = Query('SELECT Time FROM Temp WHERE Host = "'.$Host.'"');
		$Data = $Result -> fetch(PDO::FETCH_ASSOC);
		unset($Result);
		# 規制中の場合
		if(($Data["Time"] + $Set["Interval"]) > $TimeStamp) {
			PrintHTML("連投規制中", "前回のアップロードから <B>".$Set["Interval"]."</B> 秒経たないと実行できません。（あと ".(($Data["Time"] + $Set["Interval"]) - $TimeStamp)." 秒）");
		}
	}

	### アップロード可否チェック
	# 古いデータの自動削除フラグ
	$AutoDelete = "off";

	# 最大ファイル保持数が1以上
	if($Set["MaxKeepFile"] > 0) {
		# 有効データ数取得
		$ValidDataCount = getValidDataCount();

		# 有効データ数が最大ファイル保持数に達している場合
		if($ValidDataCount >= $Set["MaxKeepFile"]) {
			# 古いデータの自動削除が無効の場合（アップロード不可）
			if($Set["AutoDelete"] == "off") {
				PrintHTML("もうだめ", "アップロードされているファイルが <B>上限数：".$Set["MaxKeepFile"]."件</B> に達したためアップロードができません。削除されるか、無効になるまでお待ち下さい。");
			# 古いデータの自動削除が有効の場合（アップロード可）
			} elseif($Set["AutoDelete"] == "on") {
				# アップロード時に最も古いデータを削除する
				$AutoDelete = "on";
			}
		}
	}

	### 情報取得
	$File =$_FILES["File"];					# ファイル情報
	$Comment = $_POST["Comment"];			# コメント
	$DLKey = $_POST["DLKey"];				# DLキー
	$DelKey = $_POST["DelKey"];			# 削除キー
	$DL_Limit = $_POST["DL_Limit"];		# DL可能回数
	$Time_Limit = $_POST["Time_Limit"];	# DL可能時間

	### ファイルサイズチェック
	if($File["size"] > $Set["MaxSize"]) {
		PrintHTML("アップロード失敗", "ファイルサイズが大きいよ。");
	}

	### ファイル未選択 or 削除キーが入力されていない場合終了
	if($File["tmp_name"] == "" || $DelKey == "") {
		PrintHTML("アップロード失敗", "ファイルを選択して削除キーを入力して下さい。");
	}

	### 拡張子が存在しない場合終了
	$Ext = strtolower(pathinfo($File["name"], PATHINFO_EXTENSION));
	if($Ext == "") {
		PrintHTML("Error!", "変なファイルの予感。");
	}

	### アップロードチェック
	# 成功/失敗フラグ
	$Flag = false;

	### ファイルが存在するか
	if(is_uploaded_file($File["tmp_name"])) {
		### 一時ファイル名取得
		$Random = mt_rand(1000000000, 2147483647);

		### アップロードが正常終了した場合
		if(move_uploaded_file($File["tmp_name"], "./src/".$Random)) {
			##### DB登録処理
			### DL可能回数
			# 数字以外 or 0 は空白
			if(1 > $DL_Limit) {
				$DL_Limit = "";
			}

			### DL期限
			# 無制限を許可 かつ 空白
			if($Set["NoLimit"] == "on" && $Time_Limit == "") {
				$Time_Limit = "";
			} else {
				# 最大1440分まで
				if($Time_Limit > 0) {
					$Time = ($Time_Limit > 1440? 1440: $Time_Limit);
				# 数字以外 or 0 はデフォルト値
				} else {
					$Time = $Set["Default_TimeLimit"];
				}
				# DL期限計算
				$Time_Limit = date("Y/m/d H:i:s",strtotime($Time." minute"));
			}

			### ファイル名
			# 現在の最大No + 1
			$Result = Query("SELECT MAX(No) AS No FROM FileList");
			$Data = $Result -> fetch(PDO::FETCH_ASSOC);
			unset($Result);
			## 新ファイル名
			# リネーム機能が有効の場合
			if($Set["ReName"] == "on") {
				$FileName = ($Data["No"] + 1).".".$Ext;
			# 無効の場合
			} else {
				$FileName = $File["name"];
			}

			# DLキー or DL回数制限 or DL期限有り
			if($DLKey != "" || $DL_Limit != "" || $Time_Limit != "") {
				# 新ファイル名をハッシュ関数で生成/20桁
				$Hash = substr(hash("md5", $Random.$File["name"]), 0, 20);
				# リネーム機能が有効の場合
				if($Set["ReName"] == "on") {
					$FileName = $Hash."_".$FileName;
				# 無効の場合
				} else {
					$FileName = $Hash."_".$File["name"];
				}
			}

			### ファイルを圧縮する場合
			if($Set["FileArchive"] == "on") {
				# 新ファイル名変更
				$FileName .= ".gz";
				# アップロードファイル読み込み
				$FP = fopen("./src/".$Random, "r");
				$FileData = fread($FP, filesize("./src/".$Random));
				fclose($FP);
				# 圧縮ファイル作成
				$FP = gzopen("./src/".$FileName, "w9");
				gzwrite($FP, $FileData);
				gzclose($FP);
				# 元のファイルを削除
				unlink("./src/".$Random);
				# ファイルサイズ更新
				$File["size"] = filesize("./src/".$FileName);
			### 圧縮しない場合
			} else {
				# 名前変更
				rename("./src/".$Random, "./src/".$FileName);
				# 権限変更
				chmod("./src/".$FileName, 0606);
			}

			### ファイルサイズ
			# 1MB以上
			if($File["size"] >= 1048576) {
				$Size = floor($File["size"] / 1024 / 1024)."MB";
			# 1KB以上
			} elseif($File["size"] >= 1024) {
				$Size = floor($File["size"] / 1024)."KB";
			# 1KB未満
			} else {
				$Size = $File["size"]."バイト";
			}

			### DBへ登録
			$SQL = 'INSERT INTO FileList VALUES(null, "'.$DLKey.'", "'.$DelKey.'", "'.$Comment.'", "'.$Size.'", "'.DATE.'", "'.$FileName.'", "'.$File["name"].'", "'.$DL_Limit.'", "'.$Time_Limit.'", "'.gethostbyaddr($_SERVER["REMOTE_ADDR"]).'")';
			QueryExec($SQL);

			# フラグ更新
			$Flag = true;

			### 無効化したデータ・古いデータの削除
			DeleteFile($Set["LimitCheck"], $AutoDelete);

			### 連投規制が有効の場合
			if($Set["Interval"] > 0) {
				# 一時テーブルに追加
				$SQL = 'DELETE FROM Temp WHERE Host = "'.$Host.'";'
						."INSERT INTO Temp VALUES(".$TimeStamp.', "'.$Host.'")';
				QueryExec($SQL);
			}
		}
	}

	### 後処理
	# 成功
	if($Flag) {
		header("location: ./uploader.php");
	# 失敗
	} else {
		PrintHTML("アップロード失敗", "ファイルをアップロードできませんでした。");
	}
}

#------------------------------------------------------------------------------------------------------------
#	ファイル削除 - Delete
#	---------------------------------------
#	引　数：なし
#	戻り値：なし
#------------------------------------------------------------------------------------------------------------
function Delete() {
	### 情報取得
	# 削除対象No
	$DelNo = $_POST["DelNo"];
	# 削除キー
	$DelKey = $_POST["DelKey"];

	### 削除対象No、削除キーが入力された場合のみ
	if($DelNo != "" && $DelKey != "") {
		# 削除キー確認
		$SQL = "SELECT FileName FROM FileList WHERE No = ".$DelNo.' AND DelKey = "'.$DelKey.'"';
		$Result = Query($SQL);
		$Data = $Result -> fetch(PDO::FETCH_ASSOC);
		unset($Result);

		### 削除キーが正しい
		if($Data != false) {
			# DBから削除
			$SQL = "DELETE FROM FileList WHERE No = ".$DelNo;
			QueryExec($SQL);
			# ファイル削除
			@unlink("./src/".$Data["FileName"]);
			header("location: ./uploader.php");
		### 正しくない
		} else {
			PrintHTML("Error!", "削除キーが違います。");
		}
	### 未入力がある場合
	} else {
		PrintHTML("Error!", "削除対象Noが未選択　または　削除キーが入力されていません。");
	}
}
?>
