using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DLToolkit.PageFactory;

namespace DLToolkitControlsSamples
{
	public class MenuPageModel : BasePageModel
	{
		public MenuPageModel()
		{
            //SimpleFlowVIewButtonCommand = new BaseCommand(async _ =>
            //{
            //    var page = PageFactory.Instance.GetPageByModel<SimplePageModel>();
            //    await this.PushPageAsync<SimplePageModel>();
            //});
		}


		public ICommand SimpleFlowVIewButtonCommand
		{
			get { return GetField<ICommand>(); }
			set { SetField(value); }
		}

        public ICommand ImageFlowViewButtonCommand
        {
            get { return GetField<ICommand>(); }
            set { SetField(value); }
        }

	}
}
