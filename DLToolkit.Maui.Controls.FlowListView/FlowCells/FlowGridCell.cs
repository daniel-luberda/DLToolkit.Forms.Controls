using System;
using Microsoft.Maui.Controls;

namespace DLToolkit.Maui.Controls.FlowListView.FlowCells
{
	/// <summary>
	/// FlowListView grid cell.
	/// </summary>
	[Helpers.Preserve(AllMembers = true)]
    public class FlowGridCell : Grid, IFlowViewCell
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DLToolkit.Maui.Controls.FlowListView.FlowGridCell"/> class.
		/// </summary>
		public FlowGridCell()
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

