using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using FFImageLoading.Forms.Touch;

namespace DLToolkitControlsSamples.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			CachedImageRenderer.Init();
			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
