#ifndef _MAIN_H_
#define _MAIN_H_

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
const char INITIAL_GAME[GAME_SIZE_Y][GAME_SIZE_X] = 
{
	"########",
	"# .. p #",
	"# oo   #",
	"#      #",
	"########"
};

#endif /* _MAIN_H_ */