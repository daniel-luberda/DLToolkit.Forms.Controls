using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamvvm;

namespace DLToolkitControlsSamples
{
	public partial class TagEntryViewExamplePage : ContentPage, IBasePage<TagEntryViewExamplePageModel>
	{
		public TagEntryViewExamplePage()
		{
			Resources = new ResourceDictionary();
			Resources.Add("TagValidatorFactory", new Func<string, object>(
				(arg) => (BindingContext as TagEntryViewExamplePageModel)?.ValidateAndReturn(arg)));

			InitializeComponent();
		}
	}
}
