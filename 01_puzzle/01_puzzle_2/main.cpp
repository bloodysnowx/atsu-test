#include<iostream>
#include "main.h"

using namespace std;

char ** now_position;

int main()
{
	newPosition();
	initializePosition();
	draw();

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