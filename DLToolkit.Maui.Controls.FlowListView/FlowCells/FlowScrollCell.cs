using System;
using Microsoft.Maui.Controls;

namespace DLToolkit.Maui.Controls.FlowListView.FlowCells
{
	/// <summary>
	/// FlowListView scroll cell.
	/// </summary>
	[Helpers.Preserve(AllMembers = true)]
    public class FlowScrollCell : ScrollView, IFlowViewCell
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DLToolkit.Maui.Controls.FlowListView.FlowScrollCell"/> class.
		/// </summary>
		public FlowScrollCell()
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

