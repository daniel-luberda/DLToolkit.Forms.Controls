using System;
using System.Collections.Generic;
using Xamarin.Forms;
using DLToolkit.PageFactory;

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
