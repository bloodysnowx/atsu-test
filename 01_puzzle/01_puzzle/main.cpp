#include<iostream>

using namespace std;

char getInput();
void updateGame(char c);
void draw();

/*
 #:•Ç
 .:ƒS[ƒ‹
 o:‰×•¨
 O:‰×•¨+ƒS[ƒ‹
 p:ƒvƒŒƒCƒ„[
 P:ƒvƒŒƒCƒ„[+ƒS[ƒ‹
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
			// •Ç‚É‚Ô‚Â‚©‚Á‚½
		case '#':
			break;
			// ˆÚ“®
		case ' ':
			// ƒS[ƒ‹‚Éd‚È‚Á‚Ä‚¢‚½ê‡
			if(game[present_y][present_x] == 'P')
				game[present_y][present_x] = '.';
			else
				game[present_y][present_x] = ' ';

			present_x -= 1;
			game[present_y][present_x] = 'p';
			break;
			// ƒS[ƒ‹‚ÉˆÚ“®
		case '.':
			if(game[present_y][present_x] == 'P')
				game[present_y][present_x] = '.';
			else
				game[present_y][present_x] = ' ';

			present_x -= 1;
			game[present_y][present_x] = 'P';
			break;
			// ‰×•¨‚ğ‰Ÿ‚·
		case 'o':
			switch (game[present_y][present_x - 2])
			{
				// ‚Ô‚Â‚©‚Á‚½
			case 'o':
			case 'O':
			case '#':
				break;
				// ‰Ÿ‚¹‚½
			case ' ':
				if(game[present_y][present_x] == 'P')
					game[present_y][present_x] = '.';
				else
					game[present_y][present_x] = ' ';

				game[present_y][present_x - 1] = 'p';
				game[present_y][present_x - 2] = 'o';
				
				present_x -= 1;
				break;
				// ƒS[ƒ‹‚É“’…
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
			// ƒS[ƒ‹‚Éd‚È‚Á‚Ä‚¢‚é‰×•¨‚ğ‰Ÿ‚·
		case 'O':
			switch (game[present_y][present_x - 2])
			{
				// ‚Ô‚Â‚©‚Á‚½
			case 'o':
			case 'O':
			case '#':
				break;
				// ‰Ÿ‚¹‚½
			case ' ':
				if(game[present_y][present_x] == 'P')
					game[present_y][present_x] = '.';
				else
					game[present_y][present_x] = ' ';

				game[present_y][present_x - 1] = 'P';
				game[present_y][present_x - 2] = 'o';
				
				present_x -= 1;
				break;
				// ƒS[ƒ‹‚É“’…
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