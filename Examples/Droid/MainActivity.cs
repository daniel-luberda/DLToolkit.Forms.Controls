using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Droid;
using FFImageLoading;

namespace Examples.Droid
{
	[Activity(Label = "Examples.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			CachedImageRenderer.Init();
			global::Xamarin.Forms.Forms.Init(this, bundle);
			Renderers.TagEntryRenderer.Init();

			LoadApplication(new App());
		}
	}
}

