using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Examples.ViewModels;
using Examples.Views;
using DLToolkit.PageFactory;
using Examples.Models;

namespace Examples.Pages
{
	public partial class FlowListViewExpandPage : PFContentPage<FlowListViewExpandViewModel>
	{
		public FlowListViewExpandPage()
		{
			InitializeComponent();

			FlowListView.FlowGroupKeySelector = new Func<object, object>((item) => ((FlowItem)item).TitleGroupSelector);
			FlowListView.FlowGroupItemSortingSelector = new Func<object, object>(item => ((FlowItem)item).Title);
			FlowListView.FlowColumnsDefinitions = new List<Func<object, Type>>() {
				new Func<object, Type>((bindingContext) => typeof(FlowListViewExpandCell)),
				new Func<object, Type>((bindingContext) => typeof(FlowListViewExpandCell)),
				new Func<object, Type>((bindingContext) => typeof(FlowListViewExpandCell)),
				new Func<object, Type>((bindingContext) => typeof(FlowListViewExpandCell)),
				// new Func<object, Type>((bindingContext) => typeof(FlowListViewExpandCell)),
			};

			FlowListView.ItemSelected += (sender, e) => { FlowListView.SelectedItem = null; };
			FlowListView.FlowItemTapped += (sender, e) => {
				var item = e.Item as FlowItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("FlowListView tapped: {0}", item.Title);	
			};
		}
	}
}

