using System;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// FlowListView column expand mode.
	/// </summary>
	[Helpers.FlowListView.Preserve(AllMembers = true)]
    public enum FlowColumnExpand
	{
		/// <summary>
		/// None (default)
		/// </summary>
		None,

		/// <summary>
		/// Only first column is expanded
		/// </summary>
		First,

		/// <summary>
		/// Only last column is expanded
		/// </summary>
		Last,

		/// <summary>
		/// Columns are expanded proportionally
		/// </summary>
		Proportional,

		/// <summary>
		/// Columns are expanded proportionally
		/// First column expand more to keep columns parallel
		/// </summary>
		ProportionalFirst,

		/// <summary>
		/// Columns are expanded proportionally
		/// Last column expand more to keep columns parallel
		/// </summary>
		ProportionalLast,
	}
}

