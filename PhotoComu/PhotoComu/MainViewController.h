//
//  MainViewController.h
//  PhotoComu
//
//  Created by 岩佐 淳史 on 2013/07/04.
//  Copyright (c) 2013年 岩佐 淳史. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <GameKit/GameKit.h>

@interface MainViewController : UIViewController<GKPeerPickerControllerDelegate, GKSessionDelegate, UIImagePickerControllerDelegate, UINavigationControllerDelegate>

@property (nonatomic, retain) IBOutlet UIImageView* localImageView;
@property (nonatomic, retain) IBOutlet UIImageView* remoteImageView;
-(IBAction)connectPushed:(id)sender;
-(IBAction)sendPushed:(id)sender;

@end
