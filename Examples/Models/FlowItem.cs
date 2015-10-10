using System;
using DLToolkit.PageFactory;

namespace Examples.Models
{
	public class FlowItem : BaseModel
	{
		string title;
		public string Title
		{
			get { return title; }
			set { SetField(ref title, value); }
		}
	}
}

