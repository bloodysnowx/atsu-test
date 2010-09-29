#include<iostream>
#include "main.h"

using namespace std;

char ** now_position;

int now_x, now_y;
int moving_x, moving_y;
int pushing_x, pushing_y;

int main()
{
	newPosition();
	initializePosition();

	while(true)
	{
		draw();
		getInput();
		updateGame();
	}

	exitPosition();
	return 0;
}

void newPosition()
{
	now_position = new char*[GAME_SIZE_Y];

	for(int i=0; i < GAME_SIZE_Y; ++i)
		now_position[i] = new char[GAME_SIZE_X];
}

void initializePosition()
{
	for(int i=0; i < GAME_SIZE_Y; ++i)
	{
		for(int j=0; j < GAME_SIZE_X; ++j)
			now_position[i][j] = INITIAL_POSITION[i][j];
	}

	now_x = INITIAL_X;
	now_y = INITIAL_Y;
}

void exitPosition()
{
	for(int i=0; i < GAME_SIZE_Y; ++i)
		delete[] now_position[i];

	delete[] now_position;
}

void draw()
{
	for(int i=0; i < GAME_SIZE_Y; ++i)
		cout << now_position[i] << endl;

	cout << "a:left s:right w:up z:down. command? ";
}

void getInput()
{
	char c;
	cin >> c;
	
	switch (c)
	{
	case 'a':
		moving_x = now_x - 1;
		moving_y = now_y;
		pushing_x = now_x - 2;
		pushing_y = now_y;
		break;
	case 's':
		moving_x = now_x + 1;
		moving_y = now_y;
		pushing_x = now_x + 2;
		pushing_y = now_y;
		break;
	case 'w':
		moving_x = now_x;
		moving_y = now_y - 1;
		pushing_x = now_x;
		pushing_y = now_y - 2;
		break;
	case 'z':
		moving_x = now_x;
		moving_y = now_y + 1;
		pushing_x = now_x;
		pushing_y = now_y + 2;
		break;
	default:
		moving_x = now_x;
		moving_y = now_y;
		pushing_x = now_x;
		pushing_y = now_y;
		break;
	}
}

void updateGame()
{
	// �ړ����Ȃ������ꍇ
	if(now_x == moving_x && now_y == moving_y)
		return;

	switch (now_position[moving_y][moving_x])
	{
		// �ǂɂԂ������ꍇ
	case '#':
		break;
		// �X�y�[�X�Ɉړ������ꍇ
	case ' ':
		// �S�[���ɏd�Ȃ��Ă����ꍇ
		if(now_position[now_y][now_x] == 'P')
			now_position[now_y][now_x] = '.';
		else
			now_position[now_y][now_x] = ' ';

		now_x = moving_x;
		now_y = moving_y;

		now_position[now_y][now_x] = 'p';
		break;

		// �S�[���Ɉړ�
	case '.':
		// �S�[���ɏd�Ȃ��Ă����ꍇ
		if(now_position[now_y][now_x] == 'P')
			now_position[now_y][now_x] = '.';
		else
			now_position[now_y][now_x] = ' ';

		now_x = moving_x;
		now_y = moving_y;

		now_position[now_y][now_x] = 'P';
		break;

		// �ו�������
	case 'o':
		switch (now_position[pushing_y][pushing_x])
		{
			// �Ԃ�����
		case 'o':
		case 'O':
		case '#':
			break;
			// ������
		case ' ':
			if(now_position[now_y][now_x] == 'P')
				now_position[now_y][now_x] = '.';
			else
				now_position[now_y][now_x] = ' ';

			now_position[moving_y][moving_x] = 'p';
			now_position[pushing_y][pushing_x] = 'o';
			
			now_x = moving_x;
			now_y = moving_y;
			break;
			// �S�[���ɓ���
		case '.':
			if(now_position[now_y][now_x] == 'P')
				now_position[now_y][now_x] = '.';
			else
				now_position[now_y][now_x] = ' ';

			now_position[moving_y][moving_x] = 'p';
			now_position[pushing_y][pushing_x] = 'O';
			
			now_x = moving_x;
			now_y = moving_y;
			break;
		default:
			break;
		}
		break;

		// �S�[���ɂ����Ȃ����ו�������
	case 'O':
		switch (now_position[pushing_y][pushing_x])
		{
			// �Ԃ�����
		case 'o':
		case 'O':
		case '#':
			break;
			// ������
		case ' ':
			if(now_position[now_y][now_x] == 'P')
				now_position[now_y][now_x] = '.';
			else
				now_position[now_y][now_x] = ' ';

			now_position[moving_y][moving_x] = 'P';
			now_position[pushing_y][pushing_x] = 'o';
			
			now_x = moving_x;
			now_y = moving_y;
			break;
			// �S�[���ɓ���
		case '.':
			if(now_position[now_y][now_x] == 'P')
				now_position[now_y][now_x] = '.';
			else
				now_position[now_y][now_x] = ' ';

			now_position[moving_y][moving_x] = 'P';
			now_position[pushing_y][pushing_x] = 'O';
			
			now_x = moving_x;
			now_y = moving_y;
			break;
		default:
			break;
		}
		break;

	default:
		break;
	}

}