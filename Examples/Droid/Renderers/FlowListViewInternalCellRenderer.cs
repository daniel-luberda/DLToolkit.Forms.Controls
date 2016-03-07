using System;
using Xamarin.Forms;
using DLToolkit.Forms.Controls;
using Examples.Droid;
using Xamarin.Forms.Platform.Android;
using ListView = global::Android.Widget.ListView;

[assembly: ExportRenderer(typeof(FlowListViewInternalCell), typeof(FlowListViewInternalCellRenderer))]
namespace Examples.Droid
{
	// DISABLES FLOWLISTVIEW ROW HIGHLIGHT
	public class FlowListViewInternalCellRenderer : ViewCellRenderer
	{
		protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
		{
			var cell = base.GetCellCore(item, convertView, parent, context);

			var listView = parent as ListView;

			if (listView != null)
			{
				listView.SetSelector(Android.Resource.Color.Transparent);
				listView.CacheColorHint = Android.Graphics.Color.Transparent;
			}

			return cell;
		}
	}
}

