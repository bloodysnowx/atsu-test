//
//  MainViewController.m
//  PhotoComu
//
//  Created by 岩佐 淳史 on 2013/07/04.
//  Copyright (c) 2013年 岩佐 淳史. All rights reserved.
//

#import "MainViewController.h"


@interface MainViewController ()
{
    GKSession* currentSession;
    NSString* currentPeer;
}

@end

@implementation MainViewController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (void)viewWillAppear:(BOOL)animated
{
    [super viewWillAppear:animated];
    GKPeerPickerController* gkPicker = [GKPeerPickerController new];
    gkPicker.connectionTypesMask = GKPeerPickerConnectionTypeNearby;
    gkPicker.delegate = self;
    [gkPicker show];
}

#pragma mark - GKPeerPickerControllerDelegate

- (void)peerPickerController:(GKPeerPickerController *)picker didConnectPeer:(NSString *)peerID toSession:(GKSession *)session
{
    [session setDataReceiveHandler:self withContext:nil];
    currentSession = session;
    currentPeer = peerID;
    picker.delegate = nil;
    [picker dismiss];
}

- (void)peerPickerControllerDidCancel:(GKPeerPickerController *)picker
{
    picker.delegate = nil;
}

- (void)peerPickerController:(GKPeerPickerController *)picker didSelectConnectionType:(GKPeerPickerConnectionType)type
{
    
}

@end
