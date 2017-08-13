using Xamarin.Forms;
using Xamvvm;

namespace DLToolkitControlsSamples.SamplesFlowListView
{
    public partial class GroupingAdvancedPage : ContentPage, IBasePage<GroupingAdvancedPageModel>
    {
        public GroupingAdvancedPage()
        {
            InitializeComponent();
        }

		public void FlowScrollTo(object item)
		{
			flowListView.FlowScrollTo(item, ScrollToPosition.MakeVisible, true);
		}
    }
}
