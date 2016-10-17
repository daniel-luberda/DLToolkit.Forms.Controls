using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamvvm;

namespace DLToolkitControlsSamples
{
	public partial class SimplePage : ContentPage, IBasePage<SimplePageModel>
	{
		public SimplePage()
		{
			InitializeComponent();
		}
	}
}
