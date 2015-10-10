using System;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
	public class FlowViewCell : ContentView
	{
		public FlowViewCell()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;
			Padding = 0;
		}

		public virtual void OnTapped()
		{
		}
	}
}

