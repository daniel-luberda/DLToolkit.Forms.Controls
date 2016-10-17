using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamvvm;

namespace DLToolkitControlsSamples
{
	public partial class TemplateSelectorPage : ContentPage, IBasePage<TemplateSelectorPageModel>
	{
		public TemplateSelectorPage()
		{
			InitializeComponent();

			FlowListView.FlowColumnTemplate = new TemplateSelectorPageSelector();
		}
	}
}
