using System;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// FlowListView content view cell.
	/// </summary>
	[Helpers.FlowListView.Preserve(AllMembers = true)]
    public class FlowViewCell : ContentView, IFlowViewCell
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DLToolkit.Forms.Controls.FlowViewCell"/> class.
		/// </summary>
		public FlowViewCell()
		{
		}

		/// <summary>
		/// Raised when cell is tapped.
		/// </summary>
		public virtual void OnTapped()
		{
		}
	}
}

