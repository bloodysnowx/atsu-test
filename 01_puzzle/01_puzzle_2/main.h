#ifndef _MAIN_H_
#define _MAIN_H_

void newPosition();
void initializePosition();
void exitPosition();
void draw();
void getInput();
void updateGame();

static const int GAME_SIZE_X = 9;
static const int GAME_SIZE_Y = 5;

/*
 #:壁
 .:ゴール
 o:荷物
 O:荷物+ゴール
 p:プレイヤー
 P:プレイヤー+ゴール
*/
const char INITIAL_POSITION[GAME_SIZE_Y][GAME_SIZE_X] = 
{
	"########",
	"# .. p #",
	"# oo   #",
	"#      #",
	"########"
};

static const int INITIAL_X = 5;
static const int INITIAL_Y = 1;

#endif /* _MAIN_H_ */