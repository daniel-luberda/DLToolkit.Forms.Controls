using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamvvm;

namespace DLToolkitControlsSamples
{
	public partial class GroupingPage : ContentPage, IBasePage<GroupingPageModel>
	{
		public GroupingPage()
		{
			InitializeComponent();
		}

		public void FlowScrollTo(object item)
		{
			flowListView.FlowScrollTo(item, ScrollToPosition.MakeVisible, true);
		}
	}
}
