using System;

using Xamarin.Forms;
using DLToolkit.PageFactory;
using Examples.ViewModels;

namespace Examples
{
	public class App : Application
	{
		public App()
		{
			// The root page of your application
			MainPage = new XamarinFormsPageFactory().Init<MenuViewModel, PFNavigationPage>();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

