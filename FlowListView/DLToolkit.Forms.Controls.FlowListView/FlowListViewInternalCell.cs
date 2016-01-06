using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections;

namespace DLToolkit.Forms.Controls
{
	internal class FlowListViewInternalCell : ViewCell
	{
		readonly Grid root;

		readonly int desiredColumnCount;

		readonly bool flowAutoColumnCount;

		readonly IList<FlowColumnTemplateSelector> flowColumnsTemplates;

		readonly FlowColumnExpand flowColumnExpand;

		readonly WeakReference<FlowListView> flowListViewRef;

		public FlowListViewInternalCell(WeakReference<FlowListView> flowListViewRef)
		{
			this.flowListViewRef = flowListViewRef;
			FlowListView flowListView = null;
			flowListViewRef.TryGetTarget(out flowListView);

			flowColumnsTemplates = flowListView.FlowColumnsTemplates;
			desiredColumnCount = flowListView.DesiredColumnCount;
			flowAutoColumnCount = flowListView.FlowAutoColumnCount;
			flowColumnExpand = flowListView.FlowColumnExpand;

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
					
			for (int i = 0; i < desiredColumnCount; i++)
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

			if (flowAutoColumnCount)
			{
				for (int i = 0; i < container.Count; i++)
				{
					var template = flowColumnsTemplates[0];
					columnTypes.Add(template.GetColumnType(container[i]));
				}
			}
			else
			{
				for (int i = 0; i < container.Count; i++)
				{
					var template = flowColumnsTemplates[i];
					columnTypes.Add(template.GetColumnType(container[i]));
				}
			}

			for (int i = 0; i < root.Children.Count; i++)
			{
				if(root.Children[i].GetType() != columnTypes[i])
				{
					root.Children.Clear();
					break;
				}
			}

			var columnTemplatesCount = desiredColumnCount;
				
			for (int i = 0; i < columnTemplatesCount; i++)
			{
				if (i < container.Count)
				{
					if (root.Children.Count <= i || root.Children[i] == null)
					{
						var view = (View)Activator.CreateInstance(columnTypes[i]);
						view.BindingContext = container[i];
						view.GestureRecognizers.Add(new TapGestureRecognizer() {
							Command = new Command((obj) => {
								var flowCell = view as IFlowViewCell;
								if (flowCell != null)
								{
									flowCell.OnTapped();
								}
									
								FlowListView flowListView = null;
								flowListViewRef.TryGetTarget(out flowListView);

								if (flowListView != null)
								{
									flowListView.FlowPerformTap(view.BindingContext);
								}
							})		
						});

						// FLOW COLUMN EXPAND ENABLED
						if (flowColumnExpand != FlowColumnExpand.None && columnTemplatesCount > container.Count)
						{
							int diff = columnTemplatesCount - container.Count;
							int modifier = i + diff + 1;

							if (flowColumnExpand == FlowColumnExpand.First)
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
							if (flowColumnExpand == FlowColumnExpand.ProportionalFirst || 
								flowColumnExpand == FlowColumnExpand.ProportionalLast)
							{
								int propSize = columnTemplatesCount / container.Count;
								int propMod = columnTemplatesCount % container.Count;

								if (flowColumnExpand == FlowColumnExpand.ProportionalFirst)
								{
									if (i == 0)
									{
										var firstSize = propSize + propMod;
										root.Children.Add(view, 0, firstSize, 0, 1);
									}
									else
									{
										var pos = (i * propSize) + propMod;
										root.Children.Add(view, pos,  pos + propSize, 0, 1);
									}
								}
								else if (flowColumnExpand == FlowColumnExpand.ProportionalLast)
								{
									if (i == container.Count - 1)
									{
										var pos = i * propSize;
										var lastSize = pos + propSize + propMod;
										root.Children.Add(view, pos, lastSize, 0, 1);
									}
									else
									{
										var pos = i * propSize;
										root.Children.Add(view, pos,  pos + propSize, 0, 1);
									}
								}
							}
							else if (flowColumnExpand == FlowColumnExpand.Last && i == (container.Count-1))
							{
								root.Children.Add(view, i, modifier, 0, 1);
							}
							else
							{
								root.Children.Add(view, i, 0);
							}
						}
						// FLOW COLUMN EXPAND DISABLED
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

