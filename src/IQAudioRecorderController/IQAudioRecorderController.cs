using System;
using Foundation;
using UIKit;

namespace IQAudioRecorderController {
    
    
    public class IQAudioRecorderController : UINavigationController {
        
        #region Fields
        private IIQAudioRecorderControllerDelegate _Delegate;

		private IQInternalAudioRecorderController m_internalController;
        #endregion
        
        #region Properties

		/// <summary>
		/// Gets or sets the delegate.
		/// </summary>
		/// <value>The delegate.</value>
        public IIQAudioRecorderControllerDelegate Delegate {
            get {
                return this._Delegate;
            }
            set 
			{
                this._Delegate = value;
				m_internalController.Delegate = value;
            }
        }
        #endregion
        
        #region Methods
        public override void ViewDidLoad() {

			base.ViewDidLoad ();

            //     
			m_internalController = new IQInternalAudioRecorderController();
            m_internalController.Delegate = this.Delegate;

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
