using System;
using System.Collections.Generic;
using Xamarin.Forms;
using FFImageLoading.Transformations;

namespace Examples.ExamplesFlowListView
{
	public partial class GalleryPageItemView : ContentView
	{
		public GalleryPageItemView()
		{
			InitializeComponent();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var model = BindingContext as GalleryPageModel.ItemModel;
			Image.Source = model?.ImageUrl;
		}
	}
}
