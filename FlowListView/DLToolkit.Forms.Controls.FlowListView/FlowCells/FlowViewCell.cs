using System;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
	public class FlowViewCell : ContentView, IFlowViewCell
	{
		public FlowViewCell()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;
		}

		public virtual void OnTapped()
		{
		}
	}
}

