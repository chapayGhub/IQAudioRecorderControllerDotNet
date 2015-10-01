using System;

using UIKit;
using IQAudioRecorderController;

namespace IQAudioRecorderControllerSample
{
	public partial class ViewController : UIViewController, IIQAudioRecorderControllerDelegate
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		partial void OnShowClicked (UIButton sender)
		{
			var controller = new IQAudioRecorderController.IQAudioRecorderController();

			controller.Delegate = this;

			this.PresentViewController(controller,true,null);

		}

		#region IIQAudioRecorderControllerDelegate implementation

		public void AudioRecorderController (IQAudioRecorderController.IQAudioRecorderController controller, string filePath)
		{
			
		}

		public void AudioRecorderControllerDidCancel (IQAudioRecorderController.IQAudioRecorderController controller)
		{
			
		}

		#endregion
	}
}

