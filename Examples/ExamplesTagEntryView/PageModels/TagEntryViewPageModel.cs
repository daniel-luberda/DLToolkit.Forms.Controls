using System;
using DLToolkit.PageFactory;
using System.Collections.ObjectModel;
using System.Linq;

namespace Examples.ExamplesTagEntryView.PageModels
{
	public class TagEntryViewPageModel : BasePageModel
	{
		public override void PageFactoryMessageReceived(string message, object sender, object arg)
		{
			if (message == "FillWithData")
			{
				ReloadTags();
			}
		}

		public void ReloadTags()
		{
			var tags = new ObservableCollection<TagItem>(){
				new TagItem() { Name = "#TagExample" },
				new TagItem() { Name = "#Xamarin" },
				new TagItem() { Name = "#DanielLuberda" },
				new TagItem() { Name = "#Test" },
				new TagItem() { Name = "#XamarinForms" },
				new TagItem() { Name = "#TagEntryView" },
				new TagItem() { Name = "#TapMe!" },
				new TagItem() { Name = "#itsworking!" },
			};

			Items = tags;
		}

		public void RemoveTag(TagItem tagItem)
		{
			Items.Remove(tagItem);
		}

		public TagItem ValidateAndReturn(string tag)
		{
			if (string.IsNullOrWhiteSpace(tag))
				return null;

			var tagString = tag.StartsWith("#") ? tag : "#" + tag;

			if (Items.Any(v => v.Name.Equals(tagString, StringComparison.OrdinalIgnoreCase)))
				return null;

			return new TagItem() {
				Name = tagString
			};
		}

		public ObservableCollection<TagItem> Items
		{
			get { return GetField<ObservableCollection<TagItem>>(); }
			set { SetField(value); }
		}

		public class TagItem : BaseModel
		{
			string name;
			public string Name
			{
				get { return name; }
				set { SetField(ref name, value); }
			}
		}
	}
}

