using System;
using Xamarin.Forms;
using DLToolkit.PageFactory;

namespace DLToolkitControlsSamples
{
	public class MainNavigationPage : NavigationPage, IBasePage<MainNavigationPageModel>
	{
		public MainNavigationPage()
		{
		}

		public MainNavigationPage(Page root) : base(root)
		{
		}
	}

	public class MainNavigationPageModel : BasePageModel
	{

	}
}
