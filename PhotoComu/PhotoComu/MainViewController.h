//
//  MainViewController.h
//  PhotoComu
//
//  Created by 岩佐 淳史 on 2013/07/04.
//  Copyright (c) 2013年 岩佐 淳史. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <GameKit/GameKit.h>

@interface MainViewController : UIViewController<GKPeerPickerControllerDelegate, GKSessionDelegate>

-(IBAction)connectPushed:(id)sender;
-(IBAction)sendPushed:(id)sender;

@end
