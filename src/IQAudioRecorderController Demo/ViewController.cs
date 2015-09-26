using System;
using UIKit;
using Foundation;
using IQAudioRecorderController;

namespace IQAudioRecorderControllerDemo {
    
    
    public class ViewController : UIViewController {
        
        #region Methods
        public override void ViewDidLoad() {
            // 
            // {
            //     [super viewDidLoad];
            //     buttonPlayAudio.enabled = false;
            // }
        }
        
        private void RecordAction(UIButton sender) {
            // 
            // {
            //     IQAudioRecorderController controller = [[IQAudioRecorderController alloc] init];
            //     controller.Delegate = this;
            //     [this presentViewController:controller animated:YES completion:null];
            // }
            throw new System.NotImplementedException();
        }
        
        private void AudioRecorderController(IQAudioRecorderController.IQAudioRecorderController controller, String filePath) {
            // 
            // {
            //     audioFilePath = filePath;
            //     buttonPlayAudio.enabled = true;
            // }
        }
        
		private void AudioRecorderControllerDidCancel(IQAudioRecorderController.IQAudioRecorderController controller) {
            // 
            // {
            //     buttonPlayAudio.enabled = false;
            // }
        }
        
        private void PlayAction(UIButton sender) {
            // 
            // {
            //     MPMoviePlayerViewController controller = [[MPMoviePlayerViewController alloc] initWithContentURL:[NSURL fileURLWithPath:audioFilePath]];
            //     [this presentMoviePlayerViewControllerAnimated:controller];
            // }
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
