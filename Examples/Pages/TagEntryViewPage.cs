using System;

using Xamarin.Forms;
using DLToolkit.PageFactory;
using Examples.ViewModels;
using DLToolkit.Forms.Controls;

namespace Examples.Pages
{
	public class TagEntryViewPage : PFContentPage<TagEntryViewViewModel>
	{
		public TagEntryViewPage()
		{
			Title = "TagEntryView Demo";

			var tagEntryView = new TagEntryView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				TagValidatorFactory = new Func<string, object>((tagString) => 
					ViewModel.ValidateAndReturn(tagString)),

				TagViewFactory = new Func<View>(() => new TagItemView())
			};

			tagEntryView.SetBinding<TagEntryViewViewModel>(TagEntryView.TagItemsProperty, v => v.Items);

			tagEntryView.TagTapped += (sender, e) => {
				if (e.Item != null)
					ViewModel.RemoveTag((TagEntryViewViewModel.TagItem)e.Item);
			};

			Content = new ScrollView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new StackLayout() {
					Padding = 10,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Children = {
						tagEntryView
					}
				}
			};
		}

		class TagItemView : Frame
		{
			public TagItemView()
			{
				BackgroundColor = Color.FromHex("#2196F3");
				OutlineColor = Color.Transparent;
				Padding = 10;

				var label = new Label();
				label.SetBinding<TagEntryViewViewModel.TagItem>(Label.TextProperty, v => v.Name);

				Content = label;
			}
		}
	}
}


