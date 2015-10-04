using System;

using UIKit;
using IQAudioRecorderController;

namespace IQAudioRecorderControllerSample
{
	public partial class ViewController : UIViewController
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
			var controller = new IQAudioRecorderViewController();

			controller.OnCancel += AudioRecorderControllerDidCancel;
			controller.OnRecordingCompleted += AudioRecorderControllerCompleted;
			this.PresentViewController(controller,true,null);

		}



		#region IIQAudioRecorderControllerDelegate implementation

		public void AudioRecorderControllerCompleted (IQAudioRecorderController.IQAudioRecorderViewController controller, string filePath)
		{
			controller.OnCancel -= AudioRecorderControllerDidCancel;
			controller.OnRecordingCompleted -= AudioRecorderControllerCompleted;

			var alert = UIAlertController.Create ("File recorded", filePath, UIAlertControllerStyle.Alert);

			alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (obj)=>
				{
					alert.DismissViewController(true,null);

				}));
			
			this.PresentViewController (alert, true, null);

		}

		public void AudioRecorderControllerDidCancel (IQAudioRecorderController.IQAudioRecorderViewController controller)
		{			
			controller.OnCancel -= AudioRecorderControllerDidCancel;
			controller.OnRecordingCompleted -= AudioRecorderControllerCompleted;

			var alert = UIAlertController.Create ("Record cancelled", "Recording was canelled", UIAlertControllerStyle.Alert);

			alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (obj)=>
				{
					alert.DismissViewController(true,null);

				}));

			this.PresentViewController (alert, true, null);
		}

		#endregion
	}
}

