using DLToolkit.PageFactory;
using Xamarin.Forms;
using DLToolkit.Forms.Controls;

namespace DLToolkitControlsSamples
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			FlowListView.Init();

			var factory = new XamarinFormsPageFactory();

			factory.Init(this);
			//factory.RegisterView<MainNavigationPageModel, MainNavigationPage>(
			//	createPage: () =>
			//	{
			//		var mainPage = PageFactory.Instance.GetPageFromCache<MainPageModel>();
			//		var navPage = new MainNavigationPage(mainPage as Page);
			//		return navPage;
			//	});

			MainPage = PageFactory.Instance.GetPageFromCache<SimplePageModel>() as Page;
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
