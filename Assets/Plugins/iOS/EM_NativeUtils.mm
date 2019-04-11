#import "EM_NativeUtils.h"

// ----------------------------------------
// EM_ShareController implementation
// ----------------------------------------
@implementation EM_AlertController

static EM_AlertController *_sharedInstance;

static const char *UNITY_ALERT_GAMEOBJECT = "MobileNativeAlert";
static const char *UNITY_ALERT_CALLBACK = "OnNativeAlertCallback";

+ (id)sharedInstance {
    if (_sharedInstance == nil)  {
        _sharedInstance = [[self alloc] init];
    }
    
    return _sharedInstance;
}

+(void) showThreeButtonsAlert: (NSString *) title message: (NSString*) msg button1: (NSString*) b1 button2: (NSString*) b2 button3: (NSString*) b3 {

	if ([UIAlertController class]) 
	{
		UIAlertController* alert = [UIAlertController alertControllerWithTitle:title  
    												message:msg  
    												preferredStyle:UIAlertControllerStyleAlert];
       
	    UIAlertAction* action1 = [UIAlertAction actionWithTitle:b1 
	    										style:UIAlertActionStyleDefault 
	    										handler:^(UIAlertAction *action) {
	        UnitySendMessage(UNITY_ALERT_GAMEOBJECT, UNITY_ALERT_CALLBACK, [@"0" UTF8String]);
	    }];
	        
	    UIAlertAction* action2 = [UIAlertAction actionWithTitle:b2 
	    										style:UIAlertActionStyleDefault 
	    										handler:^(UIAlertAction *action) {
	        UnitySendMessage(UNITY_ALERT_GAMEOBJECT, UNITY_ALERT_CALLBACK, [@"1" UTF8String]);
	    }];
	        
	    UIAlertAction* action3 = [UIAlertAction actionWithTitle:b3 
	    										style:UIAlertActionStyleDefault 
	    										handler:^(UIAlertAction *action) {
	        UnitySendMessage(UNITY_ALERT_GAMEOBJECT, UNITY_ALERT_CALLBACK, [@"2" UTF8String]);
	    }];
	       
	    [alert addAction:action1];
	    [alert addAction:action2];
	    [alert addAction:action3];    
	    
	    UIViewController *vc =  UnityGetGLViewController();
	    [vc presentViewController:alert animated:YES completion:nil];
	} 
	else if ([UIAlertView class])
	{
		UIAlertView *alert = [[UIAlertView alloc] init];
	    
	    [alert setTitle:title];
	    [alert setMessage:msg];
	    [alert setDelegate: [EM_AlertController sharedInstance]];	    
	    [alert addButtonWithTitle:b1];
	    [alert addButtonWithTitle:b2];
	    [alert addButtonWithTitle:b3];
	    
	    [alert show];	    
	}      
}

+ (void) showTwoButtonsAlert: (NSString *) title message: (NSString*) msg button1:(NSString*) b1 button2: (NSString*) b2 {
    
    if ([UIAlertController class])
    {
    	UIAlertController* alert = [UIAlertController alertControllerWithTitle:title  
    												message:msg  
    												preferredStyle:UIAlertControllerStyleAlert];
    
	    UIAlertAction* action1 = [UIAlertAction actionWithTitle:b1 
	    										style:UIAlertActionStyleDefault 
	    										handler:^(UIAlertAction *action) {
	        UnitySendMessage(UNITY_ALERT_GAMEOBJECT, UNITY_ALERT_CALLBACK, [@"0" UTF8String]);
	    }];

	    UIAlertAction* action2 = [UIAlertAction actionWithTitle:b2 
	    										style:UIAlertActionStyleDefault 
	    										handler:^(UIAlertAction *action) {
	        UnitySendMessage(UNITY_ALERT_GAMEOBJECT, UNITY_ALERT_CALLBACK, [@"1" UTF8String]);
	    }];
	    
	    [alert addAction:action1];
	    [alert addAction:action2];   
	    
	    UIViewController *vc =  UnityGetGLViewController();
	    [vc presentViewController:alert animated:YES completion:nil]; 
    }
    else if ([UIAlertView class])
    {
    	UIAlertView *alert = [[UIAlertView alloc] init];
	    
	    [alert setTitle:title];
	    [alert setMessage:msg];
	    [alert setDelegate: [EM_AlertController sharedInstance]];
	    [alert addButtonWithTitle:b1];
	    [alert addButtonWithTitle:b2];
	    
	    [alert show];
    }  
}

+(void) showOneButtonAlert: (NSString *) title message: (NSString*) msg button:(NSString*) b1 {
    
    if ([UIAlertController class])
    {
    	UIAlertController* alert = [UIAlertController alertControllerWithTitle:title  
    												message:msg  
    												preferredStyle:UIAlertControllerStyleAlert];
    
	    UIAlertAction* action = [UIAlertAction actionWithTitle:b1 
	    										style:UIAlertActionStyleDefault 
	    										handler:^(UIAlertAction *action) {
	        UnitySendMessage(UNITY_ALERT_GAMEOBJECT, UNITY_ALERT_CALLBACK, [@"0" UTF8String]);
	    }];
	    
	    
	    [alert addAction:action];
	    
	    UIViewController *vc =  UnityGetGLViewController();
	    [vc presentViewController:alert animated:YES completion:nil];
    }   
    else if ([UIAlertView class])
    {
    	UIAlertView *alert = [[UIAlertView alloc] init];
	    
	    [alert setTitle:title];
	    [alert setMessage:msg];
	    [alert setDelegate: [EM_AlertController sharedInstance]];
	    [alert addButtonWithTitle:b1];
	    
	    [alert show];
    }  
}

// UIAlertViewDelegate protocol method to handle button click on UIAlertView, which doesn't exist in tvOS SDK
#if !TARGET_OS_TV
- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex {
	NSString *indexStr = [NSString stringWithFormat:@"%ld", (long)buttonIndex];
    UnitySendMessage(UNITY_ALERT_GAMEOBJECT, UNITY_ALERT_CALLBACK,  [indexStr UTF8String]);
}
#endif


#pragma mark - Alert C API
void _ShowThreeButtonsAlert(char* title, char* message, char* button1, char* button2, char* button3) {
    NSString *titleStr = title != NULL ? [NSString stringWithUTF8String: title] : [NSString stringWithUTF8String: ""];
    NSString *messageStr = message != NULL ? [NSString stringWithUTF8String: message] : [NSString stringWithUTF8String: ""];
    NSString *buttonStr1 = button1 != NULL ? [NSString stringWithUTF8String: button1] : [NSString stringWithUTF8String: ""];
    NSString *buttonStr2 = button2 != NULL ? [NSString stringWithUTF8String: button2] : [NSString stringWithUTF8String: ""];
    NSString *buttonStr3 = button3 != NULL ? [NSString stringWithUTF8String: button3] : [NSString stringWithUTF8String: ""];
    
    [EM_AlertController showThreeButtonsAlert:titleStr
                                      message:messageStr
                                      button1:buttonStr1
                                      button2:buttonStr2
                                      button3:buttonStr3];
}

void _ShowTwoButtonsAlert(char* title, char* message, char* button1, char* button2) {
    NSString *titleStr = title != NULL ? [NSString stringWithUTF8String: title] : [NSString stringWithUTF8String: ""];
    NSString *messageStr = message != NULL ? [NSString stringWithUTF8String: message] : [NSString stringWithUTF8String: ""];
    NSString *buttonStr1 = button1 != NULL ? [NSString stringWithUTF8String: button1] : [NSString stringWithUTF8String: ""];
    NSString *buttonStr2 = button2 != NULL ? [NSString stringWithUTF8String: button2] : [NSString stringWithUTF8String: ""];
    
    [EM_AlertController showTwoButtonsAlert:titleStr
                                    message:messageStr
                                    button1:buttonStr1
                                    button2:buttonStr2];
}

void _ShowOneButtonAlert(char* title, char* message, char* button) {
    NSString *titleStr = title != NULL ? [NSString stringWithUTF8String: title] : [NSString stringWithUTF8String: ""];
    NSString *messageStr = message != NULL ? [NSString stringWithUTF8String: message] : [NSString stringWithUTF8String: ""];
    NSString *buttonStr = button != NULL ? [NSString stringWithUTF8String: button] : [NSString stringWithUTF8String: ""];
    
    [EM_AlertController showOneButtonAlert:titleStr
                                   message:messageStr 
                                    button:buttonStr];
}

@end

// ----------------------------------------
// EM_ShareController implementation
// ----------------------------------------
@implementation EM_ShareController

+(void) withText:(char*)text withURL:(char*)url withImage:(char*)image withSubject:(char*)subject {
    
    NSString *mText = text ? [[NSString alloc] initWithUTF8String:text] : nil;
    NSString *mUrl = url ? [[NSString alloc] initWithUTF8String:url] : nil;
    NSString *mImage = image ? [[NSString alloc] initWithUTF8String:image] : nil;
    NSString *mSubject = subject ? [[NSString alloc] initWithUTF8String:subject] : nil;
    
    NSMutableArray *items = [NSMutableArray new];
    
    if(mText != NULL && mText.length > 0) {
        [items addObject:mText];
    }
    
    if(mUrl != NULL && mUrl.length > 0) {
        NSURL *formattedURL = [NSURL URLWithString:mUrl];
        [items addObject:formattedURL];
    }
    
    if(mImage != NULL && mImage.length > 0) {
        
        if([mImage hasPrefix:@"http"])
        {
            NSURL *urlImage = [NSURL URLWithString:mImage];
            
            NSError *error = nil;
            NSData *dataImage = [NSData dataWithContentsOfURL:urlImage options:0 error:&error];
            
            if (!error) {
                UIImage *imageFromUrl = [UIImage imageWithData:dataImage];
                [items addObject:imageFromUrl];
            } else {
                NSLog(@"Error: Cannot load image to share.");
            }
        }
        else
        {
            NSFileManager *fileMgr = [NSFileManager defaultManager];
            if([fileMgr fileExistsAtPath:mImage]){
                
                NSData *dataImage = [NSData dataWithContentsOfFile:mImage];
                
                UIImage *imageFromUrl = [UIImage imageWithData:dataImage];
                
                [items addObject:imageFromUrl];
            }else{
                NSLog(@"Error: Cannot find share image at the provided path.");
            }
        }
    }
    
    UIViewController *vc = UnityGetGLViewController();
    UIActivityViewController *activity = [[UIActivityViewController alloc] initWithActivityItems:items
                                                                           applicationActivities:nil];
    if(mSubject != NULL) {
        [activity setValue:mSubject forKey:@"subject"];
    } else {
        [activity setValue:@"" forKey:@"subject"];
    }
    
    // Present the controller
    // on iPad, this will be a Popover
    // on iPhone, this will be an action sheet
    NSArray *vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
    if ([[vComp objectAtIndex:0] intValue] >= 8) {
        // iOS 8+ detected        
        vc.modalPresentationStyle = UIModalPresentationPopover;
        // Configure the Popover presentation controller
        UIPopoverPresentationController *popController = [activity popoverPresentationController];
        popController.permittedArrowDirections = UIPopoverArrowDirectionAny;
        
        // Configure the popover position
        popController.sourceView = vc.view;
        popController.sourceRect = CGRectMake(vc.view.frame.size.width/2, vc.view.frame.size.height/4, 0, 0);
    }
    
    [vc presentViewController:activity animated:YES completion:nil];
}

#pragma mark - Sharing C API
void _ShowShareView(struct SharingStruct *confStruct)
{
    [EM_ShareController withText:confStruct->text
                         withURL:confStruct->url
                       withImage:confStruct->image
                     withSubject:confStruct->subject];
}

@end





