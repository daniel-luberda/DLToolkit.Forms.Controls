using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Examples.ViewModels;
using Examples.Views;
using DLToolkit.PageFactory;
using Examples.Models;
using DLToolkit.Forms.Controls;

namespace Examples.Pages
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

