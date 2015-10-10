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

		public string TitleGroupSelector
		{
			get 
			{
				if (string.IsNullOrWhiteSpace(Title) || Title.Length == 0)
					return "?";

				return Title[0].ToString().ToUpper();
			}
		}
	}
}

