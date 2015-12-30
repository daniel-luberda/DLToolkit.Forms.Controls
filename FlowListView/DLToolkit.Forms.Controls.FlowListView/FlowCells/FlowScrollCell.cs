using System;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
	public class FlowScrollCell : ScrollView, IFlowViewCell
	{
		public FlowScrollCell()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;
		}

		public virtual void OnTapped()
		{
		}
	}
}

