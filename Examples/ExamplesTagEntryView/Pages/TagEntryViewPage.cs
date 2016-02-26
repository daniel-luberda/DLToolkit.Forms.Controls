using System;
using Xamarin.Forms;
using DLToolkit.PageFactory;
using DLToolkit.Forms.Controls;
using Examples.ExamplesTagEntryView.PageModels;

namespace Examples.ExamplesTagEntryView.Pages
{
    public class TagEntryViewPage : ContentPage, IBasePage<TagEntryViewPageModel>
	{
        public TagEntryViewPageModel ViewModel
        {
            get { return BindingContext as TagEntryViewPageModel; }
        }

		public TagEntryViewPage()
		{
			Title = "TagEntryView Demo";

			var tagEntryView = new TagEntryView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
                TagValidatorFactory = new Func<string, object>((arg) => ViewModel.ValidateAndReturn(arg)),
				TagViewFactory = new Func<View>(() => new TagItemView())
			};

			tagEntryView.SetBinding<TagEntryViewPageModel>(TagEntryView.TagItemsProperty, v => v.Items);

			tagEntryView.TagTapped += (sender, e) => {
				if (e.Item != null)
					ViewModel.RemoveTag((TagEntryViewPageModel.TagItem)e.Item);
			};

			Content = new ScrollView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = 10,
				Content = tagEntryView
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
				label.SetBinding<TagEntryViewPageModel.TagItem>(Label.TextProperty, v => v.Name);

				Content = label;
			}
		}
	}
}


