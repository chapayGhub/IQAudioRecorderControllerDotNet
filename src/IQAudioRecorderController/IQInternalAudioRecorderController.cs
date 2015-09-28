using System;
using Foundation;
using AVFoundation;
using CoreAnimation;
using UIKit;
using System.IO;
using CoreAudioKit;
using AudioToolbox;
using CoreGraphics;

namespace IQAudioRecorderController {
    
    
	internal class IQInternalAudioRecorderController : UIViewController,IAVAudioRecorderDelegate, IAVAudioPlayerDelegate {
        
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
        
        private Boolean m_ShouldShowRemainingTime;
        #endregion
        
        #region Properties

		/// <summary>
		/// Gets or sets the delegate.
		/// </summary>
		/// <value>The delegate.</value>
        private IIQAudioRecorderControllerDelegate Delegate {
            get {
                return this._Delegate;
            }
            set {
                this._Delegate = value;
            }
        }
        

		/// <summary>
		/// Gets or sets a value indicating whether this
		/// <see cref="IQAudioRecorderController.IQInternalAudioRecorderController"/> should show remaining time.
		/// </summary>
		/// <value><c>true</c> if should show remaining time; otherwise, <c>false</c>.</value>
        private Boolean ShouldShowRemainingTime {
            get {
                return this.m_ShouldShowRemainingTime;
            }
            set {
                this.m_ShouldShowRemainingTime = value;
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
             m_navigationTitle = @"Audio Recorder";
			m_normalTintColor = UIColor.White;
			m_recordingTintColor = UIColor.FromRGBA(0.0f/255.0f, 128.0f/255.0f,255.0f/255.0f, 1.0f);
			m_playingTintColor = UIColor.FromRGBA(255.0f/255.0f, 64.0f/255.0f,64.0f/255.0f,1.0f);

            //     
            this.View.TintColor = m_normalTintColor;
            mMusicFlowView.BackgroundColor = this.View.BackgroundColor;
			mMusicFlowView.IdleAmplitude = 0;

            //Unique recording URL
			var fileName = NSProcessInfo.ProcessInfo.GloballyUniqueString;

			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			var tmp = Path.Combine (documents, "..", "tmp");

			m_recordingFilePath = Path.Combine(tmp,String.Format("{0}.m4a",fileName));
             {
				
				m_flexItem1 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace,null,null);
				m_flexItem2 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace,null,null);
                
				var img = UIImage.FromBundle("audio_record");

				m_recordButton = new UIBarButtonItem(img,UIBarButtonItemStyle.Plain,RecordingButtonAction);
				m_playButton = new UIBarButtonItem(UIBarButtonSystemItem.Play,PlayAction);
				m_pauseButton = new UIBarButtonItem(UIBarButtonSystemItem.Pause,PauseAction);
				m_trashButton = new UIBarButtonItem(UIBarButtonSystemItem.Trash,DeleteAction);

				this.SetToolbarItems (new UIBarButtonItem[]{ m_playButton, m_flexItem1, m_recordButton, m_flexItem2, m_trashButton}, false);
				         
                 m_playButton.Enabled = false;
                 m_trashButton.Enabled = false;
             }
										
             // Define the recorder setting
             {
				var audioSettings = new AudioSettings () {
					Format = AudioFormatType.MPEG4AAC,
					SampleRate = 44100.0f,
					NumberChannels = 2,
				};

				NSError err = null;

				m_audioRecorder = AVAudioRecorder.Create (NSUrl.FromFilename (m_recordingFilePath), audioSettings,out err);
				                 
				// Initiate and prepare the recorder
				m_audioRecorder.WeakDelegate = this;
				m_audioRecorder.MeteringEnabled = true;

				mMusicFlowView.PrimaryWaveLineWidth = 3.0f;
				mMusicFlowView.SecondaryWaveLineWidth = 1.0f;
             }
 
             //Navigation Bar Settings
             {
                this.NavigationItem.Title = @"Audio Recorder";
				m_cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel,CancelAction);
				this.NavigationItem.LeftBarButtonItem = m_cancelButton;

				m_doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, DoneAction);
             }
				
             //Player Duration View
			{
				m_viewPlayerDuration = new UIView ();
				m_viewPlayerDuration.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
				m_viewPlayerDuration.BackgroundColor = UIColor.Clear;
         
				m_labelCurrentTime = new UILabel ();
				m_labelCurrentTime.Text = NSStringExtensions.TimeStringForTimeInterval (0);
				m_labelCurrentTime.Font =  UIFont.BoldSystemFontOfSize(14.0f);
				m_labelCurrentTime.TextColor = m_normalTintColor;
				m_labelCurrentTime.TranslatesAutoresizingMaskIntoConstraints = false;
         
				m_playerSlider = new UISlider(new CGRect(0, 0, this.View.Bounds.Size.Width, 64));
                 m_playerSlider.MinimumTrackTintColor = m_playingTintColor;
                 m_playerSlider.Value = 0;

				m_playerSlider.TouchDown += SliderStart;
				m_playerSlider.ValueChanged += SliderMoved;
				m_playerSlider.TouchUpInside += SliderEnd;
				m_playerSlider.TouchUpOutside += SliderEnd;
                 m_playerSlider.TranslatesAutoresizingMaskIntoConstraints = false;
        
				m_labelRemainingTime = new UILabel();
				m_labelCurrentTime.Text = NSStringExtensions.TimeStringForTimeInterval (0);
                 m_labelRemainingTime.UserInteractionEnabled = true;
				m_labelRemainingTime.AddGestureRecognizer (new UITapGestureRecognizer(TapRecognizer));
				m_labelRemainingTime.Font = m_labelCurrentTime.Font;
                 m_labelRemainingTime.TextColor = m_labelCurrentTime.TextColor;
                 m_labelRemainingTime.TranslatesAutoresizingMaskIntoConstraints = false;                

				m_viewPlayerDuration.Add (m_labelCurrentTime);
				m_viewPlayerDuration.Add (m_playerSlider);
				m_viewPlayerDuration.Add (m_labelRemainingTime);
				                
				NSLayoutConstraint constraintCurrentTimeLeading = NSLayoutConstraint.Create (m_labelCurrentTime,NSLayoutAttribute.Leading,NSLayoutRelation.Equal,m_viewPlayerDuration,NSLayoutAttribute.Leading,1.0f, 10.0f);
				NSLayoutConstraint constraintCurrentTimeTrailing =  NSLayoutConstraint.Create (m_playerSlider,NSLayoutAttribute.Leading,NSLayoutRelation.Equal,m_labelCurrentTime,NSLayoutAttribute.Trailing,1.0f,10);

				NSLayoutConstraint constraintRemainingTimeLeading =  NSLayoutConstraint.Create (m_labelRemainingTime,NSLayoutAttribute.Leading,NSLayoutRelation.Equal,m_playerSlider,NSLayoutAttribute.Trailing,1.0f, 10.0f);
				NSLayoutConstraint constraintRemainingTimeTrailing =  NSLayoutConstraint.Create (m_viewPlayerDuration,NSLayoutAttribute.Trailing,NSLayoutRelation.Equal,m_labelRemainingTime,NSLayoutAttribute.Trailing,1.0f,10.0f);
                 
				NSLayoutConstraint constraintCurrentTimeCenter = NSLayoutConstraint.Create (m_labelCurrentTime,NSLayoutAttribute.CenterY,NSLayoutRelation.Equal,m_viewPlayerDuration,NSLayoutAttribute.CenterY,1.0f,0.0f);

				NSLayoutConstraint constraintSliderCenter = NSLayoutConstraint.Create (m_playerSlider,NSLayoutAttribute.CenterY,NSLayoutRelation.Equal,m_viewPlayerDuration,NSLayoutAttribute.CenterY,1.0f,0.0f);

				NSLayoutConstraint constraintRemainingTimeCenter = NSLayoutConstraint.Create (m_labelRemainingTime,NSLayoutAttribute.CenterY,NSLayoutRelation.Equal,m_viewPlayerDuration,NSLayoutAttribute.CenterY,1.0f,0.0f);
                 
				m_viewPlayerDuration.AddConstraints(new NSLayoutConstraint[]{constraintCurrentTimeLeading,constraintCurrentTimeTrailing,constraintRemainingTimeLeading,constraintRemainingTimeTrailing,constraintCurrentTimeCenter,constraintSliderCenter,constraintRemainingTimeCenter});
             
			}
        }
        
		/// <summary>
		/// Views the will appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear(Boolean animated) 
		{
			base.ViewWillAppear (animated);

			StartUpdatingMeter ();
		
        }
        
		/// <summary>
		/// Views the will disappear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillDisappear(Boolean animated) 
		{
			base.ViewDidDisappear (animated);

            //     
             m_audioPlayer.Delegate = null;
			m_audioPlayer.Stop ();
             m_audioPlayer = null;
             
             m_audioRecorder.Delegate = null;
			m_audioRecorder.Stop();
             m_audioRecorder = null;
             
			StopUpdatingMeter ();

        }
        
		/// <summary>
		/// Updates the meters.
		/// </summary>
        private void UpdateMeters() {


			if (m_audioRecorder.Recording)
		     {
				m_audioRecorder.UpdateMeters();
		         
				var normalizedValue = Math.Pow (10, m_audioRecorder.AveragePower(0) / 20);
		         
				mMusicFlowView.WaveColor = m_recordingTintColor;
				mMusicFlowView.UpdateWithLevel ((nfloat)normalizedValue);

				this.NavigationItem.Title = NSStringExtensions.TimeStringForTimeInterval (m_audioRecorder.currentTime);

		     }
			else if (m_audioPlayer.Playing)
			{
				m_audioPlayer.UpdateMeters();

				var normalizedValue = Math.Pow (10, m_audioPlayer.AveragePower(0) / 20);

				mMusicFlowView.WaveColor = m_playingTintColor;
				mMusicFlowView.UpdateWithLevel ((nfloat)normalizedValue);

			}
		     else
		     {

				mMusicFlowView.WaveColor = m_normalTintColor;
				mMusicFlowView.UpdateWithLevel (0);
		     }

        }
        
		/// <summary>
		/// Starts the updating meter.
		/// </summary>
        private void StartUpdatingMeter() 
		{
			mmeterUpdateDisplayLink.Invalidate ();
			mmeterUpdateDisplayLink = CADisplayLink.Create (UpdateMeters);
			mmeterUpdateDisplayLink.AddToRunLoop (NSRunLoop.Current, NSRunLoopMode.Common);

        }
        
		/// <summary>
		/// Stops the updating meter.
		/// </summary>
        private void StopUpdatingMeter() 
		{
			mmeterUpdateDisplayLink.Invalidate ();
			mmeterUpdateDisplayLink = null;
        }
        
		/// <summary>
		/// Updates the play progress.
		/// </summary>
        private void UpdatePlayProgress() {
 
			m_labelCurrentTime.Text = NSStringExtensions.TimeStringForTimeInterval(m_audioPlayer.CurrentTime);

			m_labelRemainingTime.Text = NSStringExtensions.TimeStringForTimeInterval((m_ShouldShowRemainingTime) ?  (m_audioPlayer.Duration - m_audioPlayer.CurrentTime) : m_audioPlayer.Duration);

			m_playerSlider.SetValue ((float)m_audioPlayer.CurrentTime, true);

        }
        
		/// <summary>
		/// Sliders the start.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="args">Arguments.</param>
		private void SliderStart(object item, EventArgs args) {
            
		     m_wasPlaying = m_audioPlayer.Playing;
		     
		     if (m_audioPlayer.Playing)
		     {
				m_audioPlayer.Pause ();
		     }
        }
        
		/// <summary>
		/// Sliders the moved.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="args">Arguments.</param>
		private void SliderMoved(object item, EventArgs args) 
		{
         
			if (item is UISlider) 
			{
				m_audioPlayer.CurrentTime = ((UISlider)item).Value;
			}


        }
        
		/// <summary>
		/// Sliders the end.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="args">Arguments.</param>
		private void SliderEnd(object item, EventArgs args) {

		     if (m_wasPlaying)
		     {
				m_audioPlayer.Play();
		     }

        }
        
        private void TapRecognizer(UITapGestureRecognizer gesture) {

			if (gesture.State == UIGestureRecognizerState.Ended) {
				m_ShouldShowRemainingTime = !m_ShouldShowRemainingTime;
			}

		}
        
		private void CancelAction(object item, EventArgs args) 
		{
			if (this.Delegate != null) 
			{
				var controller = (IQAudioRecorderController)this.NavigationController;
				this.Delegate.AudioRecorderControllerDidCancel (controller);
			}

			this.DismissViewController (true, null);
		
        }
        
		private void DoneAction(object item, EventArgs args) {

			if (this.Delegate != null) 
			{
				var controller = (IQAudioRecorderController)this.NavigationController;
				this.Delegate.AudioRecorderController (controller, m_recordingFilePath);
			}

			this.DismissViewController (true, null);
        }
        
        private void RecordingButtonAction(object item, EventArgs args) {
         
             if (m_isRecording == false)
             {
                 m_isRecording = true;
         
                 //UI Update
                 {
					this.ShowNavigationButton(false);

                    m_recordButton.TintColor = m_recordingTintColor;
                    m_playButton.Enabled = false;
                    m_trashButton.Enabled = false;
                 }
                 
				if (File.Exists(m_recordingFilePath))
					File.Delete(m_recordingFilePath);
				
				m_oldSessionCategory = AVAudioSession.SharedInstance().Category;

				AVAudioSession.SharedInstance ().SetCategory (AVAudioSessionCategory.Record);
				m_audioRecorder.PrepareToRecord ();
				m_audioRecorder.Record ();

             }
             else
             {
                 m_isRecording = false;
                 
                 //UI Update
                 {
					this.ShowNavigationButton(true);

                     m_recordButton.TintColor = m_normalTintColor;
                     m_playButton.Enabled = true;
                     m_trashButton.Enabled = true;
                 }
       
				m_audioRecorder.Stop();
				AVAudioSession.SharedInstance ().SetCategory (new NSString(m_oldSessionCategory));
             }
        }
        
		private void PlayAction(object item, EventArgs args) {

			m_oldSessionCategory = AVAudioSession.SharedInstance().Category;
			AVAudioSession.SharedInstance ().SetCategory (AVAudioSessionCategory.Playback);

            //     
			m_audioPlayer = AVAudioPlayer.FromUrl(NSUrl.FromFilename(m_recordingFilePath));
            m_audioPlayer.Delegate = this;
            m_audioPlayer.MeteringEnabled = true;
			m_audioPlayer.PrepareToPlay();
			m_audioPlayer.Play();
			    
	         //UI Update
	         {
				this.SetToolbarItems (new UIBarButtonItem[]{ m_pauseButton,m_flexItem1, m_recordButton,m_flexItem2, m_trashButton}, true);

				this.ShowNavigationButton (false);

	             m_recordButton.Enabled = false;
	             m_trashButton.Enabled = false;
	         }
 
             //Start regular update
             {
				m_playerSlider.Value = (float)m_audioPlayer.CurrentTime;
				m_playerSlider.MaxValue = (float)m_audioPlayer.Duration;
                 m_viewPlayerDuration.Frame = this.NavigationController.NavigationBar.Bounds;
                 
				m_labelCurrentTime.Text = NSStringExtensions.TimeStringForTimeInterval (m_audioPlayer.CurrentTime);
				m_labelRemainingTime.Text = NSStringExtensions.TimeStringForTimeInterval((m_ShouldShowRemainingTime) ? (m_audioPlayer.Duration - m_audioPlayer.CurrentTime): m_audioPlayer.Duration);

				m_viewPlayerDuration.SetNeedsLayout();
				m_viewPlayerDuration.LayoutIfNeeded();

                this.NavigationItem.TitleView = m_viewPlayerDuration;

				mplayProgressDisplayLink.Invalidate ();
				mplayProgressDisplayLink = CADisplayLink.Create (UpdatePlayProgress);
				mplayProgressDisplayLink.AddToRunLoop (NSRunLoop.Current, NSRunLoopMode.Common);

             }
        }
        
		private void PauseAction(object item, EventArgs args) {
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
        
		private void DeleteAction(object item, EventArgs args) {
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

        }
        
        private void AudioRecorderEncodeErrorDidOccur(AVAudioRecorder recorder, NSError error) {

        }
        #endregion
    }
}
