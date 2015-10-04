using System;
using Foundation;
using UIKit;

namespace IQAudioRecorderController {
    
    /// <summary>
    /// Recording completed delegate.
    /// </summary>
	public delegate void RecordingCompletedDelegate(IQAudioRecorderViewController controller, String fileName);

	/// <summary>
	/// Recording cancelled delegate.
	/// </summary>
	public delegate void RecordingCancelledDelegate(IQAudioRecorderViewController controller);

	/// <summary>
	/// IQAudioRecorderViewController
	/// </summary>
    public class IQAudioRecorderViewController : UINavigationController {
        
        #region Fields

		private IQInternalAudioRecorderController m_internalController;
        #endregion
        
        #region Properties
					
		/// <summary>
		/// Occurs when on recording is cancelled
		/// </summary>
		public event RecordingCancelledDelegate OnCancel  = delegate {};

		/// <summary>
		/// Occurs when recording is complete
		/// </summary>
		public event RecordingCompletedDelegate OnRecordingCompleted = delegate { };

		/// <summary>
		/// Gets/Sets the wave colour when nothing is happenening
		/// </summary>
		/// <value>The color of the normal tint.</value>
		public UIColor NormalTintColor
		{
			get 
			{
				return InternalPlayer.NormalTintColor;
			}
			set
			{
				InternalPlayer.NormalTintColor = value;
			}
		}

		/// <summary>
		/// Gets/Sets the wave colour during recording
		/// </summary>
		/// <value>The color of the recording tint.</value>
		public UIColor RecordingTintColor
		{
			get 
			{
				return InternalPlayer.RecordingTintColor;
			}
			set
			{
				InternalPlayer.RecordingTintColor = value;
			}
		}

		/// <summary>
		/// Gets/Sets the wave colour during recording
		/// </summary>
		/// <value>The color of the recording tint.</value>
		public UIColor PlayingTintColor
		{
			get 
			{
				return InternalPlayer.PlayingTintColor;
			}
			set
			{
				InternalPlayer.PlayingTintColor = value;
			}
		}

		internal IQInternalAudioRecorderController InternalPlayer
		{
			get 
			{
				if (m_internalController == null)
					m_internalController = new IQInternalAudioRecorderController();

				return m_internalController;
					
			}
		}
        #endregion
        
		#region Constructor

		public IQAudioRecorderViewController ()
		{
			
		}

		#endregion

        #region Methods

		/// <summary>
		/// Views the did load.
		/// </summary>
        public override void ViewDidLoad() {

			base.ViewDidLoad ();

            //     
			InternalPlayer.CancelControllerAction = (sv) => 
			{
				this.BeginInvokeOnMainThread (() => 
					{

						OnCancel(sv);

					});
				

			};

			m_internalController.RecordingCompleteAction = (sv, pa) => {

				this.BeginInvokeOnMainThread (() => 
				{
					OnRecordingCompleted(sv,pa);
						
				});
				
			};
				
            //     
			this.ViewControllers = new UIViewController[]{m_internalController};

			 this.NavigationBar.TintColor = UIColor.White;
             this.NavigationBar.Translucent = true;
             this.NavigationBar.BarStyle = UIBarStyle.BlackTranslucent;
             
             this.ToolbarHidden = false;
             this.Toolbar.TintColor = this.NavigationBar.TintColor;
             this.Toolbar.Translucent = this.NavigationBar.Translucent;
             this.Toolbar.BarStyle = this.NavigationBar.BarStyle;

        }
        
        #endregion
    }
}
