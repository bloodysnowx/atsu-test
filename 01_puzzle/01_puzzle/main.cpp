#include<iostream>

using namespace std;

char getInput();
void updateGame(char c);
void draw();

/*
 #:壁
 .:ゴール
 o:荷物
 O:荷物+ゴール
 p:プレイヤー
 P:プレイヤー+ゴール
*/
char game[5][9] = 
{
	"########",
	"# .. p #",
	"# oo   #",
	"#      #",
	"########"
};

int present_x = 5;
int present_y = 1;

int main()
{
	char c;
	draw();

	while(true)
	{
		c = getInput();
		updateGame(c);
		draw();
	}

	return 0;
}

char getInput()
{
	char c;
	cin >> c;
	
	return c;
}

void updateGame(char c)
{
	switch (c)
	{
		// left
	case 'a':
		switch (game[present_y][present_x - 1])
		{
			// 壁にぶつかった
		case '#':
			break;
			// 移動
		case ' ':
			// ゴールに重なっていた場合
			if(game[present_y][present_x] == 'P')
				game[present_y][present_x] = '.';
			else
				game[present_y][present_x] = ' ';

			present_x -= 1;
			game[present_y][present_x] = 'p';
			break;
			// ゴールに移動
		case '.':
			if(game[present_y][present_x] == 'P')
				game[present_y][present_x] = '.';
			else
				game[present_y][present_x] = ' ';

			present_x -= 1;
			game[present_y][present_x] = 'P';
			break;
			// 荷物を押す
		case 'o':
			switch (game[present_y][present_x - 2])
			{
				// ぶつかった
			case 'o':
			case 'O':
			case '#':
				break;
				// 押せた
			case ' ':
				if(game[present_y][present_x] == 'P')
					game[present_y][present_x] = '.';
				else
					game[present_y][present_x] = ' ';

				game[present_y][present_x - 1] = 'p';
				game[present_y][present_x - 2] = 'o';
				
				present_x -= 1;
				break;
				// ゴールに到着
			case '.':
				if(game[present_y][present_x] == 'P')
					game[present_y][present_x] = '.';
				else
					game[present_y][present_x] = ' ';

				game[present_y][present_x - 1] = 'p';
				game[present_y][present_x - 2] = 'O';
				
				present_x -= 1;
				break;
			default:
				break;
			}
			break;
			// ゴールに重なっている荷物を押す
		case 'O':
			switch (game[present_y][present_x - 2])
			{
				// ぶつかった
			case 'o':
			case 'O':
			case '#':
				break;
				// 押せた
			case ' ':
				if(game[present_y][present_x] == 'P')
					game[present_y][present_x] = '.';
				else
					game[present_y][present_x] = ' ';

				game[present_y][present_x - 1] = 'P';
				game[present_y][present_x - 2] = 'o';
				
				present_x -= 1;
				break;
				// ゴールに到着
			case '.':
				if(game[present_y][present_x] == 'P')
					game[present_y][present_x] = '.';
				else
					game[present_y][present_x] = ' ';

				game[present_y][present_x - 1] = 'P';
				game[present_y][present_x - 2] = 'O';
				
				present_x -= 1;
				break;
			default:
				break;
			}
			break;
			
			break;
		default:
			break;
		}
		break;
		// right
	case 's':
		break;
		// up
	case 'w':
		break;
		// down
	case 'z':
		break;
	default:
		break;
	}
}

void draw()
{
	for(int i=0; i < 5; ++i)
		cout << game[i] << endl;

	cout << "a:left s:right w:up z:down. command? ";
}