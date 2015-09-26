using System;
using Foundation;
using UIKit;

namespace IQAudioRecorderController {
    
    
    public class IQAudioRecorderController : UINavigationController {
        
        #region Fields
        private IIQAudioRecorderControllerDelegate _Delegate;
        #endregion
        
        #region Properties
        public IIQAudioRecorderControllerDelegate Delegate {
            get {
                return this._Delegate;
            }
            set {
                this._Delegate = value;
            }
        }
        #endregion
        
        #region Methods
        public override void ViewDidLoad() {
            // 
            // {
            //     [super viewDidLoad];
            //     
            //     _internalController = [[IQInternalAudioRecorderController alloc] init];
            //     _internalController.Delegate = this.Delegate;
            //     
            //     this.ViewControllers = @[_internalController];
            //     this.navigationBar.tintColor = [UIColor whiteColor];
            //     this.navigationBar.translucent = true;
            //     this.navigationBar.barStyle = UIBarStyleBlackTranslucent;
            //     
            //     this.toolbarHidden = false;
            //     this.toolbar.tintColor = this.navigationBar.tintColor;
            //     this.toolbar.translucent = this.navigationBar.translucent;
            //     this.toolbar.barStyle = this.navigationBar.barStyle;
            // }
        }
        
        private void SetDelegate(IIQAudioRecorderControllerDelegate del) {
            // 
            // {
            //     _delegate = delegate;
            //     _internalController.Delegate = delegate;
            // }
        }
        #endregion
    }
}
