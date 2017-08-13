using System;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// IFlowViewCell.
	/// </summary>
	[Helpers.FlowListView.Preserve(AllMembers = true)]
    public interface IFlowViewCell
	{
		/// <summary>
		/// Raised when cell is tapped.
		/// </summary>
		void OnTapped();
	}
}

