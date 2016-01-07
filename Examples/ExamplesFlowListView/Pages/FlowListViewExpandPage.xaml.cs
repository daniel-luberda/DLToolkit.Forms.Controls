using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Examples.ExamplesFlowListView.ViewModels;
using DLToolkit.PageFactory;
using DLToolkit.Forms.Controls;
using Examples.ExamplesFlowListView.Models;

namespace Examples.ExamplesFlowListView.Pages
{
	public partial class FlowListViewExpandPage : PFContentPage<FlowListViewExpandViewModel>
	{
		public FlowListViewExpandPage()
		{
			InitializeComponent();

			FlowListView.ItemSelected += (sender, e) => { FlowListView.SelectedItem = null; };
			FlowListView.FlowItemTapped += (sender, e) => {
				var item = e.Item as FlowItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("FlowListView tapped: {0}", item.Title);	
			};
		}
	}
}

