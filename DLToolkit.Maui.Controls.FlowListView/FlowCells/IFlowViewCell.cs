using System;
using Microsoft.Maui.Controls;

namespace DLToolkit.Maui.Controls.FlowListView.FlowCells
{
	/// <summary>
	/// IFlowViewCell.
	/// </summary>
	[Helpers.Preserve(AllMembers = true)]
    public interface IFlowViewCell
	{
		/// <summary>
		/// Raised when cell is tapped.
		/// </summary>
		void OnTapped();
	}
}

