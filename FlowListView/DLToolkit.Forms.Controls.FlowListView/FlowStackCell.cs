using System;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
	public class FlowStackCell : StackLayout, IFlowViewCell
	{
		public FlowStackCell()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;
		}

		public virtual void OnTapped()
		{
		}
	}
}

