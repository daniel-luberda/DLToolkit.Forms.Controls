using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DLToolkit.Forms.Controls
{
	internal class FlowListViewInternalCell : ViewCell
	{
		readonly Grid root;

		readonly List<Func<object, Type>> columnDefinitions;

		public FlowListViewInternalCell(List<Func<object, Type>> columnDefinitions)
		{
			this.columnDefinitions = columnDefinitions;

			root = new Grid() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 0f,
				ColumnSpacing = 0f,
				Padding = 0f,
				RowDefinitions = new RowDefinitionCollection() {
					new RowDefinition() { Height = GridLength.Auto }
				},
				ColumnDefinitions = new ColumnDefinitionCollection()
			};

			for (int i = 0; i < columnDefinitions.Count; i++)
			{
				root.ColumnDefinitions.Add(new ColumnDefinition() {
					Width = new GridLength(1, GridUnitType.Star)
				});
			}

			View = root;
		}

		protected override void OnBindingContextChanged()
		{
			root.BindingContext = BindingContext;
			base.OnBindingContextChanged();

			if (BindingContext == null)
			{
				root.Children.Clear();
				return;
			}
				
			var container = (ObservableCollection<object>)BindingContext;

			List<Type> columnTypes = new List<Type>();

			for (int i = 0; i < container.Count; i++)
			{
				columnTypes.Add(columnDefinitions[i](container[i]));
			}

			for (int i = 0; i < root.Children.Count; i++)
			{
				if(root.Children[i].GetType() != columnTypes[i])
				{
					root.Children.Clear();
					break;
				}
			}
				
			for (int i = 0; i < columnDefinitions.Count; i++)
			{
				if (i < container.Count)
				{
					if (root.Children.Count <= i || root.Children[i] == null)
					{
						var view = (FlowViewCell)Activator.CreateInstance(columnTypes[i]);
						view.BindingContext = container[i];
						view.GestureRecognizers.Add(new TapGestureRecognizer() {
							Command = new Command((obj) => {
								view.OnTapped();
							})		
						});



						root.Children.Add(view, i, 0);
					}
					else
					{
						var view = root.Children[i];
						if (view != null)
							view.BindingContext = container[i];
					}
				}
				else
				{
					if (root.Children.Count > i)
					{
						var view = root.Children[i];
						view.BindingContext = null;
						root.Children.Remove(view);	
					}
				}
			}
		}
	}
}

