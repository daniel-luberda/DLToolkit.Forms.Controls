using System;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
	public class FlowGridCell : Grid, IFlowViewCell
	{
		public FlowGridCell()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;
		}

		public virtual void OnTapped()
		{
		}
	}
}

