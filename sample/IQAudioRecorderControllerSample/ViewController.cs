using System;

using UIKit;
using IQAudioRecorderController;

namespace IQAudioRecorderControllerSample
{
	public partial class ViewController : UIViewController, IQAudioRecorderControllerDelegate
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
			this.BeginInvokeOnMainThread (() => {
				var alert = UIAlertController.Create ("File recorded", filePath, UIAlertControllerStyle.Alert);

				alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (obj)=>
					{
						alert.DismissViewController(true,null);

					}));
				this.PresentViewController (alert, true, null);
			});

		}

		public void AudioRecorderControllerDidCancel (IQAudioRecorderController.IQAudioRecorderController controller)
		{
			
		}

		#endregion
	}
}

