using System;
using Foundation;
using AVFoundation;
using CoreAnimation;
using UIKit;

namespace IQAudioRecorderController {
    
    
	internal class IQInternalAudioRecorderController : UIViewController {
        
        #region Fields
        private AVAudioRecorder m_audioRecorder;
        
		private SCSiriWaveformView mMusicFlowView;
        
        private String m_recordingFilePath;
        
        private Boolean m_isRecording;
        
        private CADisplayLink mmeterUpdateDisplayLink;
        
        private AVAudioPlayer m_audioPlayer;
        
        private Boolean m_wasPlaying;
        
        private UIView m_viewPlayerDuration;
        
        private UISlider m_playerSlider;
        
        private UILabel m_labelCurrentTime;
        
        private UILabel m_labelRemainingTime;
        
        private CADisplayLink mplayProgressDisplayLink;
        
        private String m_navigationTitle;
        
        private UIBarButtonItem m_cancelButton;
        
        private UIBarButtonItem m_doneButton;
        
        private UIBarButtonItem m_flexItem1;
        
        private UIBarButtonItem m_flexItem2;
        
        private UIBarButtonItem m_playButton;
        
        private UIBarButtonItem m_pauseButton;
        
        private UIBarButtonItem m_recordButton;
        
        private UIBarButtonItem m_trashButton;
        
        private String m_oldSessionCategory;
        
        private UIColor m_normalTintColor;
        
        private UIColor m_recordingTintColor;
        
        private UIColor m_playingTintColor;
        
        private IIQAudioRecorderControllerDelegate _Delegate;
        
        private Boolean _ShouldShowRemainingTime;
        #endregion
        
        #region Properties
        private IIQAudioRecorderControllerDelegate Delegate {
            get {
                return this._Delegate;
            }
            set {
                this._Delegate = value;
            }
        }
        
        private Boolean ShouldShowRemainingTime {
            get {
                return this._ShouldShowRemainingTime;
            }
            set {
                this._ShouldShowRemainingTime = value;
            }
        }
        #endregion
        
        #region Methods


		/// <summary>
		/// Loads the view.
		/// </summary>
		public override void LoadView() {
            // 
			var view = new UIView(UIScreen.MainScreen.Bounds);
			view.BackgroundColor = UIColor.DarkGray;
			mMusicFlowView = new SCSiriWaveformView(view.Bounds);
			mMusicFlowView.TranslatesAutoresizingMaskIntoConstraints = false;
			view.Add (mMusicFlowView);
	        this.View = view;

			NSLayoutConstraint constraintRatio = NSLayoutConstraint.Create (mMusicFlowView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, mMusicFlowView, NSLayoutAttribute.Height, 1.0f, 0.0f);
			NSLayoutConstraint constraintCenterX = NSLayoutConstraint.Create (mMusicFlowView,NSLayoutAttribute.CenterX ,NSLayoutRelation.Equal,view,NSLayoutAttribute.CenterX,1.0f,0.0f);
			NSLayoutConstraint constraintCenterY = NSLayoutConstraint.Create (mMusicFlowView,NSLayoutAttribute.CenterY,NSLayoutRelation.Equal,view,NSLayoutAttribute.CenterY,1.0f, 0.0f);
			NSLayoutConstraint constraintWidth = NSLayoutConstraint.Create (mMusicFlowView,NSLayoutAttribute.Width,NSLayoutRelation.Equal,view,NSLayoutAttribute.Width,1.0f,0.0f);

			mMusicFlowView.AddConstraint (constraintRatio);
			view.AddConstraints (new NSLayoutConstraint[]{constraintWidth, constraintCenterX, constraintCenterY});

        }
        
		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();

            // 
            // 
            //     _navigationTitle = @"Audio Recorder";
            //     _normalTintColor = [UIColor whiteColor];
            //     _recordingTintColor = [UIColor colorWithRed:0.0/255.0 green:128.0/255.0 blue:255.0/255.0 alpha:1.0];
            //     _playingTintColor = [UIColor colorWithRed:255.0/255.0 green:64.0/255.0 blue:64.0/255.0 alpha:1.0];
            //     
            //     this.View.tintColor = _normalTintColor;
            //     musicFlowView.BackgroundColor = [this.View backgroundColor];
            // //    musicFlowView.idleAmplitude = 0;
            // 
            //     //Unique recording URL
            //     NSString fileName = [[NSProcessInfo processInfo] globallyUniqueString];
            //     _recordingFilePath = [NSTemporaryDirectory() stringByAppendingPathComponent:[NSString stringWithFormat:@"%@.m4a",fileName]];
            // 
            //     {
            //         _flexItem1 = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFlexibleSpace target:null action:null];
            //         _flexItem2 = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFlexibleSpace target:null action:null];
            //         
            //         _recordButton = [[UIBarButtonItem alloc] initWithImage:[UIImage imageNamed:@"audio_record"] style:UIBarButtonItemStylePlain target:this action:@selector(recordingButtonAction:)];
            //         _playButton = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemPlay target:this action:@selector(playAction:)];
            //         _pauseButton = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemPause target:this action:@selector(pauseAction:)];
            //         _trashButton = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemTrash target:this action:@selector(deleteAction:)];
            //         [this setToolbarItems:@[_playButton,_flexItem1, _recordButton,_flexItem2, _trashButton] animated:NO];
            // 
            //         _playButton.enabled = false;
            //         _trashButton.enabled = false;
            //     }
            //     
            //     // Define the recorder setting
            //     {
            //         NSMutableDictionary recordSetting = [[NSMutableDictionary alloc] init];
            //         
            //         [recordSetting setValue:[NSNumber numberWithInt:kAudioFormatMPEG4AAC] forKey:AVFormatIDKey];
            //         [recordSetting setValue:[NSNumber numberWithFloat:44100.0] forKey:AVSampleRateKey];
            //         [recordSetting setValue:[NSNumber numberWithInt: 2] forKey:AVNumberOfChannelsKey];
            //         
            //         // Initiate and prepare the recorder
            //         _audioRecorder = [[AVAudioRecorder alloc] initWithURL:[NSURL fileURLWithPath:_recordingFilePath] settings:recordSetting error:null];
            //         _audioRecorder.Delegate = this;
            //         _audioRecorder.meteringEnabled = true;
            //         
            //         [musicFlowView setPrimaryWaveLineWidth:3.0f];
            //         [musicFlowView setSecondaryWaveLineWidth:1.0];
            //     }
            // 
            //     //Navigation Bar Settings
            //     {
            //         this.navigationItem.Title = @"Audio Recorder";
            //         _cancelButton = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemCancel target:this action:@selector(cancelAction:)];
            //         this.navigationItem.leftBarButtonItem = _cancelButton;
            //         _doneButton = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemDone target:this action:@selector(doneAction:)];
            //     }
            //     
            //     //Player Duration View
            //     {
            //         _viewPlayerDuration = [[UIView alloc] init];
            //         _viewPlayerDuration.AutoresizingMask = UIViewAutoresizing.FlexibleWidth|UIViewAutoresizing.FlexibleHeight;
            //         _viewPlayerDuration.BackgroundColor = UIColor.Clear;
            // 
            //         _labelCurrentTime = [[UILabel alloc] init];
            //         _labelCurrentTime.Text = [NSString timeStringForTimeInterval:0];
            //         _labelCurrentTime.Font = [UIFont boldSystemFontOfSize:14.0];
            //         _labelCurrentTime.TextColor = _normalTintColor;
            //         _labelCurrentTime.translatesAutoresizingMaskIntoConstraints = false;
            // 
            //         _playerSlider = [[UISlider alloc] initWithFrame:new CGRect(0, 0, this.View.Bounds.Size.Width, 64)];
            //         _playerSlider.minimumTrackTintColor = _playingTintColor;
            //         _playerSlider.value = 0;
            //         [_playerSlider addTarget:this action:@selector(sliderStart:) forControlEvents:UIControlEventTouchDown];
            //         [_playerSlider addTarget:this action:@selector(sliderMoved:) forControlEvents:UIControlEventValueChanged];
            //         [_playerSlider addTarget:this action:@selector(sliderEnd:) forControlEvents:UIControlEventTouchUpInside];
            //         [_playerSlider addTarget:this action:@selector(sliderEnd:) forControlEvents:UIControlEventTouchUpOutside];
            //         _playerSlider.translatesAutoresizingMaskIntoConstraints = false;
            // 
            //         _labelRemainingTime = [[UILabel alloc] init];
            //         _labelCurrentTime.Text = [NSString timeStringForTimeInterval:0];
            //         _labelRemainingTime.userInteractionEnabled = true;
            //         [_labelRemainingTime addGestureRecognizer:[[UITapGestureRecognizer alloc] initWithTarget:this action:@selector(tapRecognizer:)]];
            //         _labelRemainingTime.Font = _labelCurrentTime.Font;
            //         _labelRemainingTime.TextColor = _labelCurrentTime.TextColor;
            //         _labelRemainingTime.translatesAutoresizingMaskIntoConstraints = false;
            //         
            //         [_viewPlayerDuration addSubview:_labelCurrentTime];
            //         [_viewPlayerDuration addSubview:_playerSlider];
            //         [_viewPlayerDuration addSubview:_labelRemainingTime];
            //         
            //         NSLayoutConstraint constraintCurrentTimeLeading = [NSLayoutConstraint constraintWithItem:_labelCurrentTime attribute:NSLayoutAttributeLeading relatedBy:NSLayoutRelationEqual toItem:_viewPlayerDuration attribute:NSLayoutAttributeLeading multiplier:1 constant:10];
            //         NSLayoutConstraint constraintCurrentTimeTrailing = [NSLayoutConstraint constraintWithItem:_playerSlider attribute:NSLayoutAttributeLeading relatedBy:NSLayoutRelationEqual toItem:_labelCurrentTime attribute:NSLayoutAttributeTrailing multiplier:1 constant:10];
            //         NSLayoutConstraint constraintRemainingTimeLeading = [NSLayoutConstraint constraintWithItem:_labelRemainingTime attribute:NSLayoutAttributeLeading relatedBy:NSLayoutRelationEqual toItem:_playerSlider attribute:NSLayoutAttributeTrailing multiplier:1 constant:10];
            //         NSLayoutConstraint constraintRemainingTimeTrailing = [NSLayoutConstraint constraintWithItem:_viewPlayerDuration attribute:NSLayoutAttributeTrailing relatedBy:NSLayoutRelationEqual toItem:_labelRemainingTime attribute:NSLayoutAttributeTrailing multiplier:1 constant:10];
            //         
            //         NSLayoutConstraint constraintCurrentTimeCenter = [NSLayoutConstraint constraintWithItem:_labelCurrentTime attribute:NSLayoutAttributeCenterY relatedBy:NSLayoutRelationEqual toItem:_viewPlayerDuration attribute:NSLayoutAttributeCenterY multiplier:1 constant:0];
            //         NSLayoutConstraint constraintSliderCenter = [NSLayoutConstraint constraintWithItem:_playerSlider attribute:NSLayoutAttributeCenterY relatedBy:NSLayoutRelationEqual toItem:_viewPlayerDuration attribute:NSLayoutAttributeCenterY multiplier:1 constant:0];
            //         NSLayoutConstraint constraintRemainingTimeCenter = [NSLayoutConstraint constraintWithItem:_labelRemainingTime attribute:NSLayoutAttributeCenterY relatedBy:NSLayoutRelationEqual toItem:_viewPlayerDuration attribute:NSLayoutAttributeCenterY multiplier:1 constant:0];
            //         
            //         [_viewPlayerDuration addConstraints:@[constraintCurrentTimeLeading,constraintCurrentTimeTrailing,constraintRemainingTimeLeading,constraintRemainingTimeTrailing,constraintCurrentTimeCenter,constraintSliderCenter,constraintRemainingTimeCenter]];
            //     }
            // }
        }
        
		public override void ViewWillAppear(Boolean animated) 
		{
			base.ViewWillAppear (animated);

			StartUpdatingMeter ();
		
        }
        
		public override void ViewWillDisappear(Boolean animated) {
            // 
            // {
            //     [super viewWillDisappear:animated];
            //     
            //     _audioPlayer.Delegate = null;
            //     [_audioPlayer stop];
            //     _audioPlayer = null;
            //     
            //     _audioRecorder.Delegate = null;
            //     [_audioRecorder stop];
            //     _audioRecorder = null;
            //     
            //     [this stopUpdatingMeter];
            // }
        }
        
        private void UpdateMeters() {
            // 
            // {
            //     if (_audioRecorder.isRecording)
            //     {
            //         [_audioRecorder updateMeters];
            //         
            //         CGFloat normalizedValue = pow (10, [_audioRecorder averagePowerForChannel:0] / 20);
            //         
            //         [musicFlowView setWaveColor:_recordingTintColor];
            //         [musicFlowView updateWithLevel:normalizedValue];
            //         
            //         this.navigationItem.Title = [NSString timeStringForTimeInterval:_audioRecorder.currentTime];
            //     }
            //     else if (_audioPlayer.isPlaying)
            //     {
            //         [_audioPlayer updateMeters];
            //         
            //         CGFloat normalizedValue = pow (10, [_audioPlayer averagePowerForChannel:0] / 20);
            //         
            //         [musicFlowView setWaveColor:_playingTintColor];
            //         [musicFlowView updateWithLevel:normalizedValue];
            //     }
            //     else
            //     {
            //         [musicFlowView setWaveColor:_normalTintColor];
            //         [musicFlowView updateWithLevel:0];
            //     }
            // }
        }
        
        private void StartUpdatingMeter() {
            // 
            // {
            //     [meterUpdateDisplayLink invalidate];
            //     meterUpdateDisplayLink = [CADisplayLink displayLinkWithTarget:this selector:@selector(updateMeters)];
            //     [meterUpdateDisplayLink addToRunLoop:[NSRunLoop currentRunLoop] forMode:NSRunLoopCommonModes];
            // }
        }
        
        private void StopUpdatingMeter() {
            // 
            // {
            //     [meterUpdateDisplayLink invalidate];
            //     meterUpdateDisplayLink = null;
            // }
        }
        
        private void UpdatePlayProgress() {
            // 
            // {
            //     _labelCurrentTime.Text = [NSString timeStringForTimeInterval:_audioPlayer.currentTime];
            //     _labelRemainingTime.Text = [NSString timeStringForTimeInterval:(_shouldShowRemainingTime)?(_audioPlayer.duration-_audioPlayer.currentTime):_audioPlayer.duration];
            //     [_playerSlider setValue:_audioPlayer.currentTime animated:YES];
            // }
        }
        
        private void SliderStart(UISlider slider) {
            // 
            // {
            //     _wasPlaying = _audioPlayer.isPlaying;
            //     
            //     if (_audioPlayer.isPlaying)
            //     {
            //         [_audioPlayer pause];
            //     }
            // }
        }
        
        private void SliderMoved(UISlider slider) {
            // 
            // {
            //     _audioPlayer.currentTime = slider.value;
            // }
        }
        
        private void SliderEnd(UISlider slider) {
            // 
            // {
            //     if (_wasPlaying)
            //     {
            //         [_audioPlayer play];
            //     }
            // }
        }
        
        private void TapRecognizer(UITapGestureRecognizer gesture) {
            // 
            // {
            //     if (gesture.state == UIGestureRecognizerStateEnded)
            //     {
            //         _shouldShowRemainingTime = !_shouldShowRemainingTime;
            //     }
            // }
        }
        
        private void CancelAction(UIBarButtonItem item) {
            // 
            // {
            //     if ([this.Delegate respondsToSelector:@selector(audioRecorderControllerDidCancel:)])
            //     {
            //         IQAudioRecorderController controller = (IQAudioRecorderController)[this navigationController];
            //         [this.Delegate audioRecorderControllerDidCancel:controller];
            //     }
            //     
            //     [this dismissViewControllerAnimated:YES completion:null];
            // }
        }
        
        private void DoneAction(UIBarButtonItem item) {
            // 
            // {
            //     if ([this.Delegate respondsToSelector:@selector(audioRecorderController:didFinishWithAudioAtPath:)])
            //     {
            //         IQAudioRecorderController controller = (IQAudioRecorderController)[this navigationController];
            //         [this.Delegate audioRecorderController:controller didFinishWithAudioAtPath:_recordingFilePath];
            //     }
            //     
            //     [this dismissViewControllerAnimated:YES completion:null];
            // }
        }
        
        private void RecordingButtonAction(UIBarButtonItem item) {
            // 
            // {
            //     if (_isRecording == NO)
            //     {
            //         _isRecording = true;
            // 
            //         //UI Update
            //         {
            //             [this showNavigationButton:NO];
            //             _recordButton.tintColor = _recordingTintColor;
            //             _playButton.enabled = false;
            //             _trashButton.enabled = false;
            //         }
            //         
            //         /
            //          Create the recorder
            //          /
            //         if ([[NSFileManager defaultManager] fileExistsAtPath:_recordingFilePath])
            //         {
            //             [[NSFileManager defaultManager] removeItemAtPath:_recordingFilePath error:null];
            //         }
            //         
            //         _oldSessionCategory = [[AVAudioSession sharedInstance] category];
            //         [[AVAudioSession sharedInstance] setCategory:AVAudioSessionCategoryRecord error:null];
            //         [_audioRecorder prepareToRecord];
            //         [_audioRecorder record];
            //     }
            //     else
            //     {
            //         _isRecording = false;
            //         
            //         //UI Update
            //         {
            //             [this showNavigationButton:YES];
            //             _recordButton.tintColor = _normalTintColor;
            //             _playButton.enabled = true;
            //             _trashButton.enabled = true;
            //         }
            // 
            //         [_audioRecorder stop];
            //         [[AVAudioSession sharedInstance] setCategory:_oldSessionCategory error:null];
            //     }
            // }
        }
        
        private void PlayAction(UIBarButtonItem item) {
            // 
            // {
            //     _oldSessionCategory = [[AVAudioSession sharedInstance] category];
            //     [[AVAudioSession sharedInstance] setCategory:AVAudioSessionCategoryPlayback error:null];
            //     
            //     _audioPlayer = [[AVAudioPlayer alloc] initWithContentsOfURL:[NSURL fileURLWithPath:_recordingFilePath] error:null];
            //     _audioPlayer.Delegate = this;
            //     _audioPlayer.meteringEnabled = true;
            //     [_audioPlayer prepareToPlay];
            //     [_audioPlayer play];
            //     
            //     //UI Update
            //     {
            //         [this setToolbarItems:@[_pauseButton,_flexItem1, _recordButton,_flexItem2, _trashButton] animated:YES];
            //         [this showNavigationButton:NO];
            //         _recordButton.enabled = false;
            //         _trashButton.enabled = false;
            //     }
            //     
            //     //Start regular update
            //     {
            //         _playerSlider.value = _audioPlayer.currentTime;
            //         _playerSlider.maximumValue = _audioPlayer.duration;
            //         _viewPlayerDuration.Frame = this.navigationController.navigationBar.Bounds;
            //         
            //         _labelCurrentTime.Text = [NSString timeStringForTimeInterval:_audioPlayer.currentTime];
            //         _labelRemainingTime.Text = [NSString timeStringForTimeInterval:(_shouldShowRemainingTime)?(_audioPlayer.duration-_audioPlayer.currentTime):_audioPlayer.duration];
            // 
            //         [_viewPlayerDuration setNeedsLayout];
            //         [_viewPlayerDuration layoutIfNeeded];
            //         this.navigationItem.TitleView = _viewPlayerDuration;
            // 
            //         [playProgressDisplayLink invalidate];
            //         playProgressDisplayLink = [CADisplayLink displayLinkWithTarget:this selector:@selector(updatePlayProgress)];
            //         [playProgressDisplayLink addToRunLoop:[NSRunLoop currentRunLoop] forMode:NSRunLoopCommonModes];
            //     }
            // }
        }
        
        private void PauseAction(UIBarButtonItem item) {
            // 
            // {
            //     //UI Update
            //     {
            //         [this setToolbarItems:@[_playButton,_flexItem1, _recordButton,_flexItem2, _trashButton] animated:YES];
            //         [this showNavigationButton:YES];
            //         _recordButton.enabled = true;
            //         _trashButton.enabled = true;
            //     }
            //     
            //     {
            //         [playProgressDisplayLink invalidate];
            //         playProgressDisplayLink = null;
            //         this.navigationItem.TitleView = null;
            //     }
            // 
            //     _audioPlayer.Delegate = null;
            //     [_audioPlayer stop];
            //     _audioPlayer = null;
            //     
            //     [[AVAudioSession sharedInstance] setCategory:_oldSessionCategory error:null];
            // }
        }
        
        private void DeleteAction(UIBarButtonItem item) {
            // 
            // {
            //     UIActionSheet actionSheet = [[UIActionSheet alloc] initWithTitle:null delegate:this cancelButtonTitle:@"Cancel" destructiveButtonTitle:@"Delete Recording" otherButtonTitles:null, null];
            //     actionSheet.tag = 1;
            //     [actionSheet showInView:this.View];
            // }
        }
        
        private void ClickedButtonAtIndex(UIActionSheet actionSheet, nint buttonIndex) {
            // 
            // {
            //     if (actionSheet.tag == 1)
            //     {
            //         if (buttonIndex == actionSheet.destructiveButtonIndex)
            //         {
            //             [[NSFileManager defaultManager] removeItemAtPath:_recordingFilePath error:null];
            //             
            //             _playButton.enabled = false;
            //             _trashButton.enabled = false;
            //             [this.navigationItem setRightBarButtonItem:null animated:YES];
            //             this.navigationItem.Title = _navigationTitle;
            //         }
            //     }
            // }
        }
        
        private void ShowNavigationButton(Boolean show) {
            // 
            // {
            //     if (show)
            //     {
            //         [this.navigationItem setLeftBarButtonItem:_cancelButton animated:YES];
            //         [this.navigationItem setRightBarButtonItem:_doneButton animated:YES];
            //     }
            //     else
            //     {
            //         [this.navigationItem setLeftBarButtonItem:null animated:YES];
            //         [this.navigationItem setRightBarButtonItem:null animated:YES];
            //     }
            // }
        }
        
        private void AudioPlayerDidFinishPlaying(AVAudioPlayer player, Boolean flag) {
            // 
            // {
            //     //To update UI on stop playing
            //     NSInvocation invocation = [NSInvocation invocationWithMethodSignature:[_pauseButton.target methodSignatureForSelector:_pauseButton.action]];
            //     invocation.target = _pauseButton.target;
            //     invocation.selector = _pauseButton.action;
            //     [invocation invoke];
            // }
        }
        
        private void AudioRecorderDidFinishRecording(AVAudioRecorder recorder, Boolean flag) {
            // 
            // {
            //     
            // }
        }
        
        private void AudioRecorderEncodeErrorDidOccur(AVAudioRecorder recorder, NSError error) {
            // 
            // {
            // //    NSLog(@"%@: %@",NSStringFromSelector(_cmd),error);
            // }
        }
        #endregion
    }
}
