using System;
using Microsoft.Maui.Controls;

namespace DLToolkit.Maui.Controls.FlowListView.FlowCells
{
	/// <summary>
	/// FlowListView stack cell.
	/// </summary>
	[Helpers.Preserve(AllMembers = true)]
    public class FlowStackCell : StackLayout, IFlowViewCell
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DLToolkit.Maui.Controls.FlowListView.FlowStackCell"/> class.
		/// </summary>
		public FlowStackCell()
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

