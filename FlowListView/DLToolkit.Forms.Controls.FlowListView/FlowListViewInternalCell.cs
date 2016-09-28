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
		readonly AbsoluteLayout _rootLayout;

		int _desiredColumnCount;

		bool _flowAutoColumnCount;

		readonly IList<FlowColumnTemplateSelector> _flowColumnsTemplates;

		readonly FlowColumnExpand _flowColumnExpand;

		readonly WeakReference<FlowListView> _flowListViewRef;

		/// <summary>
		/// Initializes a new instance of the <see cref="DLToolkit.Forms.Controls.FlowListViewInternalCell"/> class.
		/// </summary>
		/// <param name="flowListViewRef">Flow list view reference.</param>
		public FlowListViewInternalCell(WeakReference<FlowListView> flowListViewRef)
		{
			_flowListViewRef = flowListViewRef;
			FlowListView flowListView = null;
			flowListViewRef.TryGetTarget(out flowListView);

			_rootLayout = new AbsoluteLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = 0d,
				BackgroundColor = flowListView.FlowRowBackgroundColor,
			};

			View = _rootLayout;

			_flowColumnsTemplates = flowListView.FlowColumnsTemplates;
			_desiredColumnCount = flowListView.DesiredColumnCount;
			_flowAutoColumnCount = flowListView.FlowAutoColumnCount;
			_flowColumnExpand = flowListView.FlowColumnExpand;
		}

		private IList<Type> GetViewTypesFromTemplates(IList container)
		{
			List<Type> columnTypes = new List<Type>();

			if (_flowAutoColumnCount)
			{
				for (int i = 0; i < container.Count; i++)
				{
					var template = _flowColumnsTemplates[0];
					columnTypes.Add(template.GetColumnType(container[i]));
				}
			}
			else
			{
				for (int i = 0; i < container.Count; i++)
				{
					var template = _flowColumnsTemplates[i];
					columnTypes.Add(template.GetColumnType(container[i]));
				}
			}

			return columnTypes;
		}

		private bool RowLayoutChanged(int containerCount, IList<Type> columnTypes)
		{
			bool changed = false;

			// Check if desired number of columns is equal to current number of columns
			if (_rootLayout.Children.Count != containerCount)
			{
				changed = true;
			}
			else
			{
				// Check if desired column view types are equal to current columns view types
				for (int i = 0; i < containerCount; i++)
				{
					if (_rootLayout.Children[i].GetType() != columnTypes[i])
					{
						changed = true;
					}
				}
			}

			if (changed)
			{
				FlowListView flowListView = null;
				_flowListViewRef.TryGetTarget(out flowListView);
				_desiredColumnCount = flowListView.DesiredColumnCount;
				_flowAutoColumnCount = flowListView.FlowAutoColumnCount;
			}

			return changed;
		}

		private void SetBindingContextForView(View view, object bindingContext)
		{
			if (view != null && view.BindingContext != bindingContext)
				view.BindingContext = bindingContext;
		}

		private void AddViewToLayout(View view, int containerCount, int colNumber)
		{
			if (containerCount == 0 || _desiredColumnCount == 0)
				return;
			
			double desiredColumnWidth = 1d / _desiredColumnCount;

            Rectangle bounds = Rectangle.Zero;

			if (_flowColumnExpand != FlowColumnExpand.None && _desiredColumnCount > containerCount)
			{
				int diff = _desiredColumnCount - containerCount;
				bool isLastColumn = colNumber == containerCount - 1;

				switch (_flowColumnExpand)
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
						
						int propFMod = _desiredColumnCount % containerCount;
						double propFSize = desiredColumnWidth * Math.Floor((double)_desiredColumnCount / containerCount);
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

						int propLMod = _desiredColumnCount % containerCount;
						double propLSize = desiredColumnWidth * Math.Floor((double)_desiredColumnCount / containerCount);
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
				if (Math.Abs(1d - desiredColumnWidth) < Epsilon.DoubleValue)
				{
					bounds = new Rectangle(1d, 0d, desiredColumnWidth, 1d);	
				}
				else
				{
					bounds = new Rectangle(desiredColumnWidth * colNumber / (1d - desiredColumnWidth), 0d, desiredColumnWidth, 1d);	
				}
			}

            _rootLayout.Children.Add(view, bounds, AbsoluteLayoutFlags.All);
		}

		/// <summary>
		/// Override this method to execute an action when the BindingContext changes.
		/// </summary>
		/// <remarks></remarks>
		protected override void OnBindingContextChanged()
		{
			_rootLayout.BindingContext = BindingContext;
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
					SetBindingContextForView(_rootLayout.Children[i], container[i]);
				}
			}
			else // RECREATE COLUMNS
			{
				if (_rootLayout.Children.Count > 0)
					_rootLayout.Children.Clear();

				for (int i = 0; i < containerCount; i++)
				{
					var view = (View)Activator.CreateInstance(columnTypes[i]);

					view.GestureRecognizers.Add(new TapGestureRecognizer()
					{
						Command = new Command(async (obj) =>
						{
							var flowCell = view as IFlowViewCell;
							if (flowCell != null)
							{
								flowCell.OnTapped();
							}

							FlowListView flowListView = null;
							_flowListViewRef.TryGetTarget(out flowListView);

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

