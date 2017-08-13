[assembly: Xamarin.Forms.Platform.UWP.ExportRenderer(typeof(DLToolkit.Forms.Controls.FlowListView), typeof(CustomListViewRenderer))]

namespace App.Example
{
    using Xamarin.Forms.Platform.UWP;

    public class CustomListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (List != null)
                List.SelectionMode = Windows.UI.Xaml.Controls.ListViewSelectionMode.None;
        }
    }
}