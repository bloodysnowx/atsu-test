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
						std::cout << std::endl << "found!";
						std::cout << std::endl << buff + i;
						std::string str = std::string(buff + i);
						size_t start_index = 16;
						size_t end_index;
						// �������v���C�����n���h�͏��O����
						if(std::string::npos != str.find(": chiyuki ", start_index))
							continue;

						// �J�����g�f�B���N�g�����ړ�
						bool ret = SetCurrentDirectory(_T("E:\\observe_hh"));

						// �n���h�ԍ����擾����
						end_index = str.find(":", start_index);
						std::string hand_number = str.substr(start_index + 1, end_index - start_index);
						// ���t���擾����
						start_index = str.find(" - ", end_index) + 3;
						end_index = str.find(" ", start_index);
						std::string hh_date = str.substr(start_index, end_index - start_index);
						// ���t�̃t�H���_��������΍쐬����
						hh_date.erase(hh_date.find("/"), 1);
						hh_date.erase(hh_date.find("/"), 1);
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
						hh_time.erase(hh_time.find(":"), 1);
						hh_time.erase(hh_time.find(":"), 1);

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
						// ������HH���ۑ��ς݂Ŗ�����΍쐬
						if(!PathFileExistsA(hh_time.c_str()))
						{
							std::ofstream ofs(hh_time + ".txt");
							ofs << str;
							ofs.close();
						}
					}
				}
#if 0
				for(int i=0;i<HeapEntry.dwBlockSize - sizeof(find_str);++i)
				{
					// size_t buff_len = strlen(&buff[i]);
					
					// if(buff_len > sizeof(find_str))
					{
						// std::cout << std::endl << "find start, length = " << buff_len;
						char * finded_str = strstr(&buff[i], find_str);
						if(finded_str != NULL)
							std::cout << std::endl << finded_str;
						// i += buff_len;
					}
				}
#endif
				delete [] buff;
				// ���̃u���b�N�Ɋւ�������擾
            } while (Heap32Next(&HeapEntry) != FALSE);
        }
        else break;
		// ���̃q�[�v�Ɋւ�������擾
    } while (Heap32ListNext(hHeapSnapshot, &HeapList) != FALSE);
	// PokerStars�̃X�i�b�v�V���b�g��j������
    CloseHandle(hHeapSnapshot);

#if 0
	// �v���Z�X�ɃA�N�Z�X
	HANDLE opened_handle = OpenProcess(PROCESS_QUERY_INFORMATION, false, ps_handle);
	// �������g�p�ʂ��擾
	PROCESS_MEMORY_COUNTERS ps_memory_info;
	GetProcessMemoryInfo(opened_handle, &ps_memory_info, sizeof(ps_memory_info));
	unsigned int memory_size = ps_memory_info.WorkingSetSize;
	CloseHandle(opened_handle);
	// ���������e���擾
	opened_handle = OpenProcess(PROCESS_VM_READ, false, ps_handle);
	char * memory = new char[100000000];
	ReadProcessMemory(opened_handle, 10000, 
	free(memory);
#endif
	return 0;
}

