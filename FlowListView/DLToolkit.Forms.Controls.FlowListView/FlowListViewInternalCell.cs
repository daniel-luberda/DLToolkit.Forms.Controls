using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// Flow list view internal cell.
	/// </summary>
	public class FlowListViewInternalCell : ViewCell
	{
		readonly AbsoluteLayout rootLayout;

		readonly int desiredColumnCount;

		readonly bool flowAutoColumnCount;

		readonly IList<FlowColumnTemplateSelector> flowColumnsTemplates;

		readonly FlowColumnExpand flowColumnExpand;

		readonly WeakReference<FlowListView> flowListViewRef;

		/// <summary>
		/// Initializes a new instance of the <see cref="DLToolkit.Forms.Controls.FlowListViewInternalCell"/> class.
		/// </summary>
		/// <param name="flowListViewRef">Flow list view reference.</param>
		public FlowListViewInternalCell(WeakReference<FlowListView> flowListViewRef)
		{
			this.flowListViewRef = flowListViewRef;
			FlowListView flowListView = null;
			flowListViewRef.TryGetTarget(out flowListView);

			rootLayout = new AbsoluteLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = 0d,
				BackgroundColor = flowListView.FlowRowBackgroundColor,
			};

			View = rootLayout;

			flowColumnsTemplates = flowListView.FlowColumnsTemplates;
			desiredColumnCount = flowListView.DesiredColumnCount;
			flowAutoColumnCount = flowListView.FlowAutoColumnCount;
			flowColumnExpand = flowListView.FlowColumnExpand;
		}

		private IList<Type> GetViewTypesFromTemplates(IList container)
		{
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

			return columnTypes;
		}

		private bool RowLayoutChanged(int containerCount, IList<Type> columnTypes)
		{
			// Check if desired number of columns is equal to current number of columns
			if (rootLayout.Children.Count != containerCount)
			{
				return true;
			}

			// Check if desired column view types are equal to current columns view types
			for (int i = 0; i < containerCount; i++)
			{
				if(rootLayout.Children[i].GetType() != columnTypes[i])
				{
					return true;
				}
			}

			return false;
		}

		private void SetBindingContextForView(View view, object bindingContext)
		{
			if (view != null)
				view.BindingContext = bindingContext;
		}

		private void AddViewToLayout(View view, int containerCount, int colNumber)
		{
			if (containerCount == 0 || desiredColumnCount == 0)
				return;
			
			double desiredColumnWidth = 1d / desiredColumnCount;

			Rectangle bounds;

			if (flowColumnExpand != FlowColumnExpand.None && desiredColumnCount > containerCount)
			{
				int diff = desiredColumnCount - containerCount;
				bool isLastColumn = colNumber == containerCount - 1;

				switch (flowColumnExpand)
				{
					case FlowColumnExpand.First:

						if (colNumber == 0)
						{
							bounds = new Rectangle(0d, 0d, desiredColumnWidth + (desiredColumnWidth * diff), 1d);
						}
						else if (isLastColumn)
						{
							bounds = new Rectangle(1d, 0d, desiredColumnWidth, 1d);
						}
						else
						{
							bounds = new Rectangle(desiredColumnWidth * (colNumber + diff) / (1d - desiredColumnWidth), 0d, desiredColumnWidth, 1d);
						}

						break;

					case FlowColumnExpand.Last:

						if (colNumber == 0)
						{
							bounds = new Rectangle(0d, 0d, desiredColumnWidth + (desiredColumnWidth * diff), 1d);
						}
						else if (isLastColumn)
						{
							bounds = new Rectangle(1d, 0d, desiredColumnWidth + (desiredColumnWidth * diff), 1d);
						}
						else
						{
							bounds = new Rectangle(desiredColumnWidth * colNumber / (1d - desiredColumnWidth), 0d, desiredColumnWidth, 1d);
						}

						break;

					case FlowColumnExpand.Proportional:
						
						double propColumnsWidth = 1d / containerCount;
						if (colNumber == 0)
						{
							bounds = new Rectangle(0d, 0d, propColumnsWidth, 1d);	
						}
						else if (isLastColumn)
						{
							bounds = new Rectangle(1d, 0d, propColumnsWidth, 1d);	
						}
						else
						{
							bounds = new Rectangle(propColumnsWidth * colNumber / (1d - propColumnsWidth), 0d, propColumnsWidth, 1d);	
						}

						break;

					case FlowColumnExpand.ProportionalFirst:
						
						int propFMod = desiredColumnCount % containerCount;
						double propFSize = desiredColumnWidth * Math.Floor((double)desiredColumnCount / containerCount);
						double propFSizeFirst = propFSize + desiredColumnWidth * propFMod;

						if (colNumber == 0)
						{
							bounds = new Rectangle(0d, 0d, propFSizeFirst, 1d);
						}
						else if (isLastColumn)
						{
							bounds = new Rectangle(1d, 0d, propFSize, 1d);
						}
						else
						{
							bounds = new Rectangle(((propFSize * colNumber) + (propFSizeFirst - propFSize)) / (1d - propFSize), 0d, propFSize, 1d);	
						}

						break;

					case FlowColumnExpand.ProportionalLast:

						int propLMod = desiredColumnCount % containerCount;
						double propLSize = desiredColumnWidth * Math.Floor((double)desiredColumnCount / containerCount);
						double propLSizeLast = propLSize + desiredColumnWidth * propLMod;

						if (colNumber == 0)
						{
							bounds = new Rectangle(0d, 0d, propLSize, 1d);
						}
						else if (isLastColumn)
						{
							bounds = new Rectangle(1d, 0d, propLSizeLast, 1d);
						}
						else
						{
							bounds = new Rectangle((propLSize * colNumber) / (1d - propLSize), 0d, propLSize, 1d);	
						}

						break;
				}		
			}
			else
			{
				if (Math.Abs(1d - desiredColumnWidth) < double.Epsilon)
				{
					bounds = new Rectangle(1d, 0d, desiredColumnWidth, 1d);	
				}
				else
				{
					bounds = new Rectangle(desiredColumnWidth * colNumber / (1d - desiredColumnWidth), 0d, desiredColumnWidth, 1d);	
				}
			}

			rootLayout.Children.Add(view, bounds, AbsoluteLayoutFlags.All);
		}

		/// <summary>
		/// Override this method to execute an action when the BindingContext changes.
		/// </summary>
		/// <remarks></remarks>
		protected override void OnBindingContextChanged()
		{
			rootLayout.BindingContext = BindingContext;
			base.OnBindingContextChanged();

			var container = BindingContext as IList;

			if (container == null)
				return;
				
			// Getting view types from templates
			var containerCount = container.Count;
			IList<Type> columnTypes = GetViewTypesFromTemplates(container);
			bool layoutChanged = RowLayoutChanged(containerCount, columnTypes);

			if (!layoutChanged) // REUSE VIEWS
			{
				for (int i = 0; i < containerCount; i++)
				{
					SetBindingContextForView(rootLayout.Children[i], container[i]);
				}
			}
			else // RECREATE COLUMNS
			{
				if (rootLayout.Children.Count > 0)
					rootLayout.Children.Clear();

				for (int i = 0; i < containerCount; i++)
				{
					var view = (View)Activator.CreateInstance(columnTypes[i]);
					view.BindingContext = container[i];
					view.GestureRecognizers.Add(new TapGestureRecognizer() {
						Command = new Command(async (obj) => {
							var flowCell = view as IFlowViewCell;
							if (flowCell != null)
							{
								flowCell.OnTapped();
							}

							FlowListView flowListView = null;
							flowListViewRef.TryGetTarget(out flowListView);

							if (flowListView != null)
							{
								int tapBackgroundEffectDelay = flowListView.FlowTappedBackgroundDelay;

								try 
								{
									if (tapBackgroundEffectDelay != 0)
									{
										view.BackgroundColor = flowListView.FlowTappedBackgroundColor;
									}

									flowListView.FlowPerformTap(view.BindingContext);	
								} 
								finally 
								{
									if (tapBackgroundEffectDelay != 0)
									{
										await Task.Delay(tapBackgroundEffectDelay);
										view.BackgroundColor = Color.Transparent;
									}
								}
							}
						})		
					});
						
					SetBindingContextForView(view, container[i]);
					AddViewToLayout(view, containerCount, i);
				}
			}
		}
	}
}

