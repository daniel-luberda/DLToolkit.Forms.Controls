using Xamvvm;
using Xamarin.Forms;
using DLToolkit.Forms.Controls;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DLToolkitControlsSamples
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			FlowListView.Init();

			var factory = new XamvvmFormsFactory(this);
			factory.RegisterNavigationPage<MainNavigationPageModel>(() => this.GetPageAsNewInstance<MainPageModel>());
			XamvvmCore.SetCurrentFactory(factory);
			MainPage = this.GetPageFromCache<MainNavigationPageModel>() as Page;
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
