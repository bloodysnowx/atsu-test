//
//  MainViewController.m
//  PhotoComu
//
//  Created by 岩佐 淳史 on 2013/07/04.
//  Copyright (c) 2013年 岩佐 淳史. All rights reserved.
//

#import "MainViewController.h"
#import <AssetsLibrary/AssetsLibrary.h>

@interface MainViewController ()
{
    GKSession* currentSession;
    NSString* currentPeer;
}

@property (nonatomic, retain) NSDictionary* localGps;
@property (nonatomic, retain) NSDictionary* remoteGps;

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

#pragma mark - DataReceiverHandler

-(void)receiveData:(NSData*)data fromPeer:(NSString*)peer inSession:(GKSession*)session context:(void*)context
{
    NSArray* receivedArray = [NSKeyedUnarchiver unarchiveObjectWithData:data];
    self.remoteGps = receivedArray[0];
    UIImage* remoteImage = [UIImage imageWithData:receivedArray[1]];
    self.remoteImageView.image = remoteImage;
}

#pragma mark - GKSessionDelegate

#pragma mark - UIImagePickerControllerDelegate

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info
{
    UIImage *localImage = [info objectForKey: UIImagePickerControllerOriginalImage];
    self.localImageView.image = localImage;
    [self dismissViewControllerAnimated:YES completion:^() { }];
    
    NSURL *assetURL = [info objectForKey:UIImagePickerControllerReferenceURL];
    ALAssetsLibrary *assetLibrary = [ALAssetsLibrary new];
    __weak MainViewController* __self = self;
    
    [assetLibrary assetForURL:assetURL resultBlock:^(ALAsset *asset) {
        NSDictionary* metaData = [[asset defaultRepresentation] metadata];
        __self.localGps = [metaData objectForKey:@"{GPS}"];
        NSLog(@"%@", __self.localGps);
        if(__self.localGps == nil) __self.localGps = @{@"Latitude":@"0.0", @"LatitudeRef":@"N",
                                             @"Longitude":@"0.0", @"LongitudeRef":@"E" };
        NSData* imageData = UIImagePNGRepresentation(localImage);
        // NSData* gpsData = [NSKeyedArchiver archivedDataWithRootObject:__self.localGps];
        NSArray* sendArray = @[__self.localGps, imageData];
        NSData* sendData = [NSKeyedArchiver archivedDataWithRootObject:sendArray];
        NSError* error = nil;
        [currentSession sendData:sendData toPeers:@[currentPeer] withDataMode:GKSendDataReliable error:&error];
    } failureBlock:^(NSError *error) {
        NSLog(@"ALAssetsLibrary error - %@", error);
        __self.localGps = nil;
    }];
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker
{
    [self dismissViewControllerAnimated:YES completion:^() { }];
}

#pragma mark - UINavigationControllerDelegate

#pragma mark - IBAction

-(IBAction)connectPushed:(id)sender
{
    GKPeerPickerController* gkPicker = [GKPeerPickerController new];
    gkPicker.connectionTypesMask = GKPeerPickerConnectionTypeNearby;
    gkPicker.delegate = self;
    [gkPicker show];
}

-(IBAction)sendPushed:(id)sender
{
    UIImagePickerController *imagePicker = [[UIImagePickerController alloc] init];
    imagePicker.delegate = self;
    imagePicker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    [self presentViewController:imagePicker animated:YES completion:^() {}];
}

@end
