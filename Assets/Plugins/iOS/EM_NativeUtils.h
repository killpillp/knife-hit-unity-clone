#import <Foundation/Foundation.h>
#import "UnityAppController.h"

#define SYSTEM_VERSION_EQUAL_TO(v)                  ([[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] == NSOrderedSame)
#define SYSTEM_VERSION_GREATER_THAN(v)              ([[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] == NSOrderedDescending)
#define SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(v)  ([[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] != NSOrderedAscending)
#define SYSTEM_VERSION_LESS_THAN(v)                 ([[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] == NSOrderedAscending)
#define SYSTEM_VERSION_LESS_THAN_OR_EQUAL_TO(v)     ([[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] != NSOrderedDescending)

@interface EM_AlertController : NSObject
+ (EM_AlertController *) sharedInstance;

#ifdef __cplusplus
extern "C" {
#endif
    
    void _ShowThreeButtonsAlert(char* title, char* message, char* button1, char* button2, char* button3);
    void _ShowTwoButtonsAlert(char* title, char* message, char* button1, char* button2);
    void _ShowOneButtonAlert(char* title, char* message, char* button);
    
#ifdef __cplusplus
}
#endif

@end

@interface EM_ShareController : UIViewController

struct SharingStruct {
    char* text;
    char* url;
    char* image;
    char* subject;
};

#ifdef __cplusplus
extern "C" {
#endif
    
    void _ShowShareView(struct SharingStruct *confStruct);
    
#ifdef __cplusplus
}
#endif

@end