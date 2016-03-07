using System;
using Xamarin.Forms.Platform.iOS;
using DLToolkit.Forms.Controls;
using Examples.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(FlowListViewInternalCell), typeof(FlowListViewInternalCellRenderer))]
namespace Examples.iOS
{
	// DISABLES FLOWLISTVIEW ROW HIGHLIGHT
	public class FlowListViewInternalCellRenderer : ViewCellRenderer
	{
		public override UIKit.UITableViewCell GetCell(Xamarin.Forms.Cell item, UIKit.UITableViewCell reusableCell, UIKit.UITableView tv)
		{
			var cell = base.GetCell (item, reusableCell, tv);
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;

			return cell;
		}
	}
}

