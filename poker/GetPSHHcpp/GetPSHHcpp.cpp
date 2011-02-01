// GetPSHHcpp.cpp : コンソール アプリケーションのエントリ ポイントを定義します。
//

#include "stdafx.h"
#include <iostream>
#include <iomanip>
#include <string>
#include <Shlwapi.h>
#include <fstream>

int _tmain(int argc, _TCHAR* argv[])
{
	// PokerStarsのwindow handleを取得
	HWND ps_window_handle = FindWindow(_T("#32770"), _T("PokerStars Lobby - Logged in as chiyuki"));
	// PokerStarsのProcess IDを取得
	DWORD ps_handle;
	DWORD thread_id = GetWindowThreadProcessId(ps_window_handle, &ps_handle);
	// 見つかったハンドヒストリの総数
	int hh_found_count_sum = 0;
	// メインループ
	while(true)
	{
		// 今回のループで見つかったハンドヒストリの数
		int hh_found_count_loop = 0;
		// PokerStarsのヒープ一覧を取得
		HANDLE hHeapSnapshot;
		HEAPLIST32 HeapList = {0};
		HEAPENTRY32 HeapEntry = {0};
		hHeapSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPHEAPLIST, ps_handle);
		HeapList.dwSize = sizeof(HEAPLIST32);
		// 最初のヒープに関する情報を取得
		Heap32ListFirst(hHeapSnapshot, &HeapList);
		do
		{
			HeapEntry.dwSize = sizeof(HEAPENTRY32);
			// ヒープの最初のブロックに関する情報を取得
			if (Heap32First(&HeapEntry, HeapList.th32ProcessID, HeapList.th32HeapID) != FALSE)
			{
				do
				{
					// ヒープ領域のメモリを取得する
					char * buff = new char[HeapEntry.dwBlockSize + 1];
					SIZE_T read_len;
					Toolhelp32ReadProcessMemory(HeapEntry.th32ProcessID, (void *)HeapEntry.dwAddress, buff, HeapEntry.dwBlockSize, &read_len);
					buff[HeapEntry.dwBlockSize] = '\0';
					// ヒープ領域のメモリ内容を表示する
#if 0
					for(int i=0;i<HeapEntry.dwBlockSize;++i)
					{
						if(i % 16 == 0)
						{
							std::cout << std::endl;
							std::cout << std::setw(8) << std::hex << std::right << i;
							std::cout << " ";
						}
						unsigned char tmp[4];
						tmp[0] = (buff[i] >> 4) & 0x0f;
						if(tmp[0] < 10) tmp[0] += 0x30;
						else tmp[0] += 0x37;

						tmp[1] = buff[i] & 0x0f;
						if(tmp[1] < 10) tmp[1] += 0x30;
						else tmp[1] += 0x37;

						tmp[2] = ' ';
						tmp[3] = 0;
						std::cout << tmp;
					}
#endif
					std::cout << std::endl << "memory read, length = " << read_len;
					// 文字列を検索する
					char find_str[] = "PokerStars Game ";
					for(int i=0;i<(int)HeapEntry.dwBlockSize - (int)sizeof(find_str);++i)
					{
						if(memcmp(&buff[i], find_str, sizeof(find_str) - 1) == 0)
						{
#if 0
							std::cout << std::endl << "found!";
							std::cout << std::endl << buff + i;
#endif
							std::string str = std::string(buff + i);
							size_t start_index = 16;
							size_t end_index;
							// 自分がプレイしたハンドは除外する
							if(std::string::npos != str.find(": chiyuki ", start_index))
								continue;

							// カレントディレクトリを移動
							BOOL ret = SetCurrentDirectory(_T("E:\\observe_hh"));

							// ハンド番号を取得する
							end_index = str.find(":", start_index);
							std::string hand_number = str.substr(start_index + 1, end_index - start_index - 1);
							// 日付を取得する
							start_index = str.find(" - ", end_index) + 3;
							end_index = str.find(" ", start_index);
							std::string hh_date = str.substr(start_index, end_index - start_index);
							// 日付のフォルダが無ければ作成する
							size_t found_index;
							found_index = hh_date.find("/");
							if(found_index == std::string::npos) continue;
							hh_date.erase(found_index, 1);
							found_index = hh_date.find("/");
							if(found_index == std::string::npos) continue;
							hh_date.erase(found_index, 1);

							if(!PathIsDirectoryA(hh_date.c_str()))
							{
								ret = CreateDirectoryA(hh_date.c_str(), NULL);
							}
							else
							{
								ret = SetCurrentDirectoryA(hh_date.c_str());
							}
							// 時刻を取得する
							start_index = end_index + 1;
							end_index = str.find(" ", start_index);
							std::string hh_time = str.substr(start_index, end_index - start_index);
							found_index = hh_time.find(":");
							if(found_index == std::string::npos) continue;
							hh_time.erase(found_index, 1);
							found_index = hh_time.find(":");
							if(found_index == std::string::npos) continue;
							hh_time.erase(found_index, 1);

							// 卓名を取得する
							start_index = str.find("Table '", end_index) + 7;
							end_index = str.find("'", start_index);
							std::string table_name = str.substr(start_index, end_index - start_index);
							// 卓名のフォルダが無ければ作成する
							if(!PathIsDirectoryA(table_name.c_str()))
							{
								ret = CreateDirectoryA(table_name.c_str(), NULL);
							}
							else
							{
								ret = SetCurrentDirectoryA(table_name.c_str());
							}
							// 時刻+ハンド番号のHHが保存済みで無ければ作成
							std::string hh_file_name = hh_time + "_" + hand_number + ".txt";
							if(!PathFileExistsA(hh_file_name.c_str()))
							{
								std::cout << std::endl << "Found! Table = " << table_name << " , date = " << hh_date << " " << hh_time 
									<< " , number = " << hand_number;
								// ファイル出力
								std::ofstream ofs(hh_file_name);
								ofs << str;
								ofs.close();
								// 見つかった回数をカウントアップ
								++hh_found_count_loop;
							}
						}
					}
					delete [] buff;
					// 次のブロックに関する情報を取得
				} while (Heap32Next(&HeapEntry) != FALSE);
			}
			else break;
			// 次のヒープに関する情報を取得
		} while (Heap32ListNext(hHeapSnapshot, &HeapList) != FALSE);
		// PokerStarsのスナップショットを破棄する
		CloseHandle(hHeapSnapshot);
		// 総数を計算
		hh_found_count_sum += hh_found_count_loop;
		std::cout << std::endl << hh_found_count_loop << " hands is found in this loop.";
		std::cout << std::endl << hh_found_count_sum << " hands is found in this program running.";

		Sleep(60 * 1000);
	}

	return 0;
}

