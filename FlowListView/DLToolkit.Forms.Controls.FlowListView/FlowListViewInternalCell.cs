using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections;

namespace DLToolkit.Forms.Controls
{
	internal class FlowListViewInternalCell : ViewCell
	{
		readonly Grid root;

		readonly FlowListView flowListView;

		public FlowListViewInternalCell(FlowListView flowListView)
		{
			this.flowListView = flowListView;

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

			for (int i = 0; i < this.flowListView.FlowColumnsDefinitions.Count; i++)
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

			var container = BindingContext as IList;

			if (container == null)
			{
				return;
			}
				
			List<Type> columnTypes = new List<Type>();

			for (int i = 0; i < container.Count; i++)
			{
				columnTypes.Add(flowListView.FlowColumnsDefinitions[i](container[i]));
			}

			for (int i = 0; i < root.Children.Count; i++)
			{
				if(root.Children[i].GetType() != columnTypes[i])
				{
					root.Children.Clear();
					break;
				}
			}
				
			for (int i = 0; i < this.flowListView.FlowColumnsDefinitions.Count; i++)
			{
				if (i < container.Count)
				{
					if (root.Children.Count <= i || root.Children[i] == null)
					{
						var view = (View)Activator.CreateInstance(columnTypes[i]);
						view.BindingContext = container[i];
						view.GestureRecognizers.Add(new TapGestureRecognizer() {
							Command = new Command((obj) => {
								((IFlowViewCell)view).OnTapped();
								flowListView.FlowPerformTap(view.BindingContext);
							})		
						});

						if (flowListView.FlowColumnExpand != FlowColumnExpand.None && flowListView.FlowColumnsDefinitions.Count > container.Count)
						{
							int diff = flowListView.FlowColumnsDefinitions.Count - container.Count;
							int modifier = i + diff + 1;
							// int propModifier = flowListView.FlowColumnsDefinitions.Count / container.Count;

							if (flowListView.FlowColumnExpand == FlowColumnExpand.First)
							{
								if (i == 0)
								{
									root.Children.Add(view, 0, modifier, 0, 1);
								}
								else
								{
									root.Children.Add(view, modifier - 1, modifier, 0, 1);
								}
							}
							else if (flowListView.FlowColumnExpand == FlowColumnExpand.Last && i == (container.Count-1))
							{
								root.Children.Add(view, i, modifier, 0, 1);
							}
							else
							{
								root.Children.Add(view, i, 0);
							}
						}
						else
						{
							root.Children.Add(view, i, 0);
						}
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

