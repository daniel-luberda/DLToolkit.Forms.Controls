using System;
using Microsoft.Maui.Controls;

namespace DLToolkit.Maui.Controls.FlowListView.FlowCells
{
	/// <summary>
	/// FlowListView content view cell.
	/// </summary>
	[Helpers.Preserve(AllMembers = true)]
    public class FlowViewCell : ContentView, IFlowViewCell
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DLToolkit.Maui.Controls.FlowListView.FlowViewCell"/> class.
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

