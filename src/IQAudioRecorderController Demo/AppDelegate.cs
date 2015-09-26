using System;
using UIKit;
using Foundation;

namespace IQAudioRecorderControllerDemo {
    
    
    public class AppDelegate : UIResponder {
        
        #region Fields
        private UIWindow _Window;
        #endregion
        
        #region Properties
        public UIWindow Window {
            get {
                return this._Window;
            }
            set {
                this._Window = value;
            }
        }
        #endregion
        
        #region Methods
        private Boolean DidFinishLaunchingWithOptions(UIApplication application, NSDictionary launchOptions) {
            // 
            //     // Override point for customization after application launch.
            //     return YES;
            // }
            throw new System.NotImplementedException();
        }
        
        private void ApplicationWillResignActive(UIApplication application) {
            // 
            //     // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
            //     // Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
            // }
        }
        
        private void ApplicationDidEnterBackground(UIApplication application) {
            // 
            //     // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later.
            //     // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
            // }
        }
        
        private void ApplicationWillEnterForeground(UIApplication application) {
            // 
            //     // Called as part of the transition from the background to the inactive state; here you can undo many of the changes made on entering the background.
            // }
        }
        
        private void ApplicationDidBecomeActive(UIApplication application) {
            // 
            //     // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
            // }
        }
        
        private void ApplicationWillTerminate(UIApplication application) {
            // 
            //     // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
            // }
        }
        #endregion
    }
}
