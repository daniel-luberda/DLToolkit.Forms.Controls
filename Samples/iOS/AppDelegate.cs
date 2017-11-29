using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using FFImageLoading.Forms.Touch;
using DLToolkit.Forms.Controls;
using FFImageLoading.Transformations;

namespace DLToolkitControlsSamples.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
            RecyclerViewRenderer.Init();
            CachedImageRenderer.Init();
            var ignore1 = typeof(CircleTransformation);
			global::Xamarin.Forms.Forms.Init();
			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
