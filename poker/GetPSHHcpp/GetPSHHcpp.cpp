// GetPSHHcpp.cpp : �R���\�[�� �A�v���P�[�V�����̃G���g�� �|�C���g���`���܂��B
//

#include "stdafx.h"
#include <iostream>
#include <iomanip>
#include <string>
#include <Shlwapi.h>
#include <fstream>

int _tmain(int argc, _TCHAR* argv[])
{
	// PokerStars��window handle���擾
	HWND ps_window_handle = FindWindow(_T("#32770"), _T("PokerStars Lobby - Logged in as chiyuki"));
	// PokerStars��Process ID���擾
	DWORD ps_handle;
	DWORD thread_id = GetWindowThreadProcessId(ps_window_handle, &ps_handle);
	// ���������n���h�q�X�g���̑���
	int hh_found_count_sum = 0;
	// ���C�����[�v
	while(true)
	{
		// ����̃��[�v�Ō��������n���h�q�X�g���̐�
		int hh_found_count_loop = 0;
		// PokerStars�̃q�[�v�ꗗ���擾
		HANDLE hHeapSnapshot;
		HEAPLIST32 HeapList = {0};
		HEAPENTRY32 HeapEntry = {0};
		hHeapSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPHEAPLIST, ps_handle);
		HeapList.dwSize = sizeof(HEAPLIST32);
		// �ŏ��̃q�[�v�Ɋւ�������擾
		Heap32ListFirst(hHeapSnapshot, &HeapList);
		do
		{
			HeapEntry.dwSize = sizeof(HEAPENTRY32);
			// �q�[�v�̍ŏ��̃u���b�N�Ɋւ�������擾
			if (Heap32First(&HeapEntry, HeapList.th32ProcessID, HeapList.th32HeapID) != FALSE)
			{
				do
				{
					// �q�[�v�̈�̃��������擾����
					char * buff = new char[HeapEntry.dwBlockSize + 1];
					SIZE_T read_len;
					Toolhelp32ReadProcessMemory(HeapEntry.th32ProcessID, (void *)HeapEntry.dwAddress, buff, HeapEntry.dwBlockSize, &read_len);
					buff[HeapEntry.dwBlockSize] = '\0';
					// �q�[�v�̈�̃��������e��\������
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
					// ���������������
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
							// �������v���C�����n���h�͏��O����
							if(std::string::npos != str.find(": chiyuki ", start_index))
								continue;

							// �J�����g�f�B���N�g�����ړ�
							BOOL ret = SetCurrentDirectory(_T("E:\\observe_hh"));

							// �n���h�ԍ����擾����
							end_index = str.find(":", start_index);
							std::string hand_number = str.substr(start_index + 1, end_index - start_index - 1);
							// ���t���擾����
							start_index = str.find(" - ", end_index) + 3;
							end_index = str.find(" ", start_index);
							std::string hh_date = str.substr(start_index, end_index - start_index);
							// ���t�̃t�H���_��������΍쐬����
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
							// �������擾����
							start_index = end_index + 1;
							end_index = str.find(" ", start_index);
							std::string hh_time = str.substr(start_index, end_index - start_index);
							found_index = hh_time.find(":");
							if(found_index == std::string::npos) continue;
							hh_time.erase(found_index, 1);
							found_index = hh_time.find(":");
							if(found_index == std::string::npos) continue;
							hh_time.erase(found_index, 1);

							// �얼���擾����
							start_index = str.find("Table '", end_index) + 7;
							end_index = str.find("'", start_index);
							std::string table_name = str.substr(start_index, end_index - start_index);
							// �얼�̃t�H���_��������΍쐬����
							if(!PathIsDirectoryA(table_name.c_str()))
							{
								ret = CreateDirectoryA(table_name.c_str(), NULL);
							}
							else
							{
								ret = SetCurrentDirectoryA(table_name.c_str());
							}
							// ����+�n���h�ԍ���HH���ۑ��ς݂Ŗ�����΍쐬
							std::string hh_file_name = hh_time + "_" + hand_number + ".txt";
							if(!PathFileExistsA(hh_file_name.c_str()))
							{
								std::cout << std::endl << "Found! Table = " << table_name << " , date = " << hh_date << " " << hh_time 
									<< " , number = " << hand_number;
								// �t�@�C���o��
								std::ofstream ofs(hh_file_name);
								ofs << str;
								ofs.close();
								// ���������񐔂��J�E���g�A�b�v
								++hh_found_count_loop;
							}
						}
					}
					delete [] buff;
					// ���̃u���b�N�Ɋւ�������擾
				} while (Heap32Next(&HeapEntry) != FALSE);
			}
			else break;
			// ���̃q�[�v�Ɋւ�������擾
		} while (Heap32ListNext(hHeapSnapshot, &HeapList) != FALSE);
		// PokerStars�̃X�i�b�v�V���b�g��j������
		CloseHandle(hHeapSnapshot);
		// �������v�Z
		hh_found_count_sum += hh_found_count_loop;
		std::cout << std::endl << hh_found_count_loop << " hands is found in this loop.";
		std::cout << std::endl << hh_found_count_sum << " hands is found in this program running.";

		Sleep(60 * 1000);
	}

	return 0;
}

