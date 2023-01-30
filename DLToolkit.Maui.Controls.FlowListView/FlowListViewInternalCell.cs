﻿using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Layouts;
using DLToolkit.Maui.Controls.FlowListView.FlowCells;
using ViewCell = Microsoft.Maui.Controls.ViewCell;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Shapes;

namespace DLToolkit.Maui.Controls.FlowListView
{
    /// <summary>
    /// Flow list view internal cell.
    /// </summary>
    [Helpers.Preserve(AllMembers = true)]
    public class FlowListViewInternalCell : ViewCell
    {
        readonly WeakReference<FlowListView> _flowListViewRef;
        readonly AbsoluteLayout _rootLayout;
        readonly Grid _rootLayoutAuto;
        readonly bool _useGridAsMainRoot;
        int _desiredColumnCount;
        DataTemplate _flowColumnTemplate;
        FlowColumnExpand _flowColumnExpand;
        IList<DataTemplate> _currentColumnTemplates;

        /// <summary>
        /// Initializes a new instance of the <see cref="DLToolkit.Maui.Controls.FlowListView.FlowListViewInternalCell"/> class.
        /// </summary>
        /// <param name="flowListViewRef">Flow list view reference.</param>
        public FlowListViewInternalCell(WeakReference<FlowListView> flowListViewRef)
        {
            _flowListViewRef = flowListViewRef;
            flowListViewRef.TryGetTarget(out FlowListView flowListView);
            _useGridAsMainRoot = !flowListView.FlowUseAbsoluteLayoutInternally;
           
            if (!_useGridAsMainRoot)
            {
               
                _rootLayout = new AbsoluteLayout()
                {
                    Padding = 0d,
                    BackgroundColor = flowListView.FlowRowBackgroundColor,
                    
                };
              
                View = _rootLayout;
                //AbsoluteLayout.SetLayoutBounds(View, new Rect() { X = 0, Y = 0, Width = 1, Height = 1 });
                //AbsoluteLayout.SetLayoutFlags(View, AbsoluteLayoutFlags.All);
            }
            else
            {
                _rootLayoutAuto = new Grid()
                {
                    RowSpacing = 0d,
                    ColumnSpacing = 0d,
                    Padding = 0d,
                    BackgroundColor = flowListView.FlowRowBackgroundColor,
                };
                View = _rootLayoutAuto;
            }

            _flowColumnTemplate = flowListView.FlowColumnTemplate;
            _desiredColumnCount = flowListView.FlowDesiredColumnCount;
            _flowColumnExpand = flowListView.FlowColumnExpand;

            View.GestureRecognizers.Clear();
            View.GestureRecognizers.Add(new TapGestureRecognizer());
        }

        private IList<DataTemplate> GetDataTemplates(IList container)
        {
            List<DataTemplate> templates = new List<DataTemplate>();

            if (_flowColumnTemplate is FlowTemplateSelector flowTemplateSelector)
            {
                _flowListViewRef.TryGetTarget(out FlowListView flowListView);

                for (int i = 0; i < container.Count; i++)
                {
                    var template = flowTemplateSelector.SelectTemplate(container[i], i, flowListView);
                    templates.Add(template);
                }

                return templates;
            }

            if (_flowColumnTemplate is DataTemplateSelector templateSelector)
            {
                _flowListViewRef.TryGetTarget(out FlowListView flowListView);

                for (int i = 0; i < container.Count; i++)
                {
                    var template = templateSelector.SelectTemplate(container[i], flowListView);
                    templates.Add(template);
                }

                return templates;
            }

            for (int i = 0; i < container.Count; i++)
            {
                templates.Add(_flowColumnTemplate);
            }

            return templates;
        }

        private bool RowLayoutChanged(int containerCount, IList<DataTemplate> templates, int columnCount)
        {
            // Check if desired number of columns is equal to current number of columns
            if (_currentColumnTemplates == null || containerCount != _currentColumnTemplates.Count)
            {
                return true;
            }

            // Check if desired column view types are equal to current columns view types
            for (int i = 0; i < containerCount; i++)
            {
                var currentTemplateType = _currentColumnTemplates[i].GetHashCode();
                var templateType = templates[i].GetHashCode();

                if (currentTemplateType != templateType)
                {
                    return true;
                }
            }

            if (_desiredColumnCount != columnCount)
            {
                return true;
            }

            return false;
        }

        private void SetBindingContextForView(View view, object bindingContext)
        {
            if (view != null && view.BindingContext != bindingContext)
                view.BindingContext = bindingContext;
        }

        void AddViewToLayoutAutoHeightDisabled(View view, int containerCount, int colNumber)
        {
            double desiredColumnWidth = 1d / _desiredColumnCount;
            Rect bounds = Rect.Zero;

            if (_flowColumnExpand != FlowColumnExpand.None && _desiredColumnCount > containerCount)
            {
                int diff = _desiredColumnCount - containerCount;
                bool isLastColumn = colNumber == containerCount - 1;

                switch (_flowColumnExpand)
                {
                    case FlowColumnExpand.First:

                        if (colNumber == 0)
                        {
                            bounds = new Rect(0d, 0d, desiredColumnWidth + (desiredColumnWidth * diff), 1d);
                        }
                        else if (isLastColumn)
                        {
                            bounds = new Rect(1d, 0d, desiredColumnWidth, 1d);
                        }
                        else
                        {
                            bounds = new Rect(desiredColumnWidth * (colNumber + diff) / (1d - desiredColumnWidth), 0d, desiredColumnWidth, 1d);
                        }

                        break;

                    case FlowColumnExpand.Last:

                        if (colNumber == 0)
                        {
                            bounds = new Rect(0d, 0d, desiredColumnWidth + (desiredColumnWidth * diff), 1d);
                        }
                        else if (isLastColumn)
                        {
                            bounds = new Rect(1d, 0d, desiredColumnWidth + (desiredColumnWidth * diff), 1d);
                        }
                        else
                        {
                            bounds = new Rect(desiredColumnWidth * colNumber / (1d - desiredColumnWidth), 0d, desiredColumnWidth, 1d);
                        }

                        break;

                    case FlowColumnExpand.Proportional:

                        double propColumnsWidth = 1d / containerCount;
                        if (colNumber == 0)
                        {
                            bounds = new Rect(0d, 0d, propColumnsWidth, 1d);
                        }
                        else if (isLastColumn)
                        {
                            bounds = new Rect(1d, 0d, propColumnsWidth, 1d);
                        }
                        else
                        {
                            bounds = new Rect(propColumnsWidth * colNumber / (1d - propColumnsWidth), 0d, propColumnsWidth, 1d);
                        }

                        break;

                    case FlowColumnExpand.ProportionalFirst:

                        int propFMod = _desiredColumnCount % containerCount;
                        double propFSize = desiredColumnWidth * Math.Floor((double)_desiredColumnCount / containerCount);
                        double propFSizeFirst = propFSize + desiredColumnWidth * propFMod;

                        if (colNumber == 0)
                        {
                            bounds = new Rect(0d, 0d, propFSizeFirst, 1d);
                        }
                        else if (isLastColumn)
                        {
                            bounds = new Rect(1d, 0d, propFSize, 1d);
                        }
                        else
                        {
                            bounds = new Rect(((propFSize * colNumber) + (propFSizeFirst - propFSize)) / (1d - propFSize), 0d, propFSize, 1d);
                        }

                        break;

                    case FlowColumnExpand.ProportionalLast:

                        int propLMod = _desiredColumnCount % containerCount;
                        double propLSize = desiredColumnWidth * Math.Floor((double)_desiredColumnCount / containerCount);
                        double propLSizeLast = propLSize + desiredColumnWidth * propLMod;

                        if (colNumber == 0)
                        {
                            bounds = new Rect(0d, 0d, propLSize, 1d);
                        }
                        else if (isLastColumn)
                        {
                            bounds = new Rect(1d, 0d, propLSizeLast, 1d);
                        }
                        else
                        {
                            bounds = new Rect((propLSize * colNumber) / (1d - propLSize), 0d, propLSize, 1d);
                        }

                        break;
                }
            }
            else
            {
                if (Math.Abs(1d - desiredColumnWidth) < Epsilon.DoubleValue)
                {
                    bounds = new Rect(1d, 0d, desiredColumnWidth, 1d);
                }
                else
                {
                    bounds = new Rect(desiredColumnWidth * colNumber / (1d - desiredColumnWidth), 0d, desiredColumnWidth, 1d);
                }
            }

            _rootLayout.SetLayoutBounds(view, bounds);
            _rootLayout.SetLayoutFlags(view, AbsoluteLayoutFlags.All);
            _rootLayout.Children.Add(view);
         
        }

        void AddViewToLayoutAutoHeightEnabled(View view, int containerCount, int colNumber)
        {
            if (_desiredColumnCount > containerCount)
            {
                int diff = _desiredColumnCount - containerCount;
                bool isLastColumn = colNumber == containerCount - 1;

                switch (_flowColumnExpand)
                {
                    case FlowColumnExpand.None:

                        _rootLayoutAuto.SetColumn(view, colNumber);
                        _rootLayoutAuto.SetRow(view, 0);

                        _rootLayoutAuto.Children.Add(view);
                      
                        break;

                    case FlowColumnExpand.First:

                        if (colNumber == 0)
                        {
                            //https://github.com/dotnet/maui/issues/780
                            //Template from Xamarin.Forms grid.Children.Add(left, right, top, bottom)
                            //SetColumn(view, left);
                            //SetColumnSpan(view, right - left);
                            //SetRow(view, top);
                            //SetRowSpan(view, bottom - top);
                            _rootLayoutAuto.SetColumn(view, colNumber);
                            _rootLayoutAuto.SetColumnSpan(view, (colNumber + diff + 1) - colNumber);
                            _rootLayoutAuto.SetRow(view, 0);
                            _rootLayoutAuto.SetRowSpan(view, 1);
                            _rootLayoutAuto.Children.Add(view/*, colNumber, colNumber + diff + 1, 0, 1*/);
                        }
                        else
                        {
                            _rootLayoutAuto.SetColumn(view, colNumber + diff);
                            _rootLayoutAuto.SetColumnSpan(view, (colNumber + diff + 1) - colNumber + diff);
                            _rootLayoutAuto.SetRow(view, 0);
                            _rootLayoutAuto.SetRowSpan(view, 1);
                            _rootLayoutAuto.Children.Add(view/* colNumber + diff, colNumber + diff + 1, 0, 1*/);
                        }

                        break;

                    case FlowColumnExpand.Last:

                        if (isLastColumn)
                        {
                            _rootLayoutAuto.SetColumn(view, colNumber);
                            _rootLayoutAuto.SetColumnSpan(view,(colNumber + diff + 1) - colNumber);
                            _rootLayoutAuto.SetRow(view, 0);
                            _rootLayoutAuto.SetRowSpan(view, 1);
                           
                            _rootLayoutAuto.Children.Add(view);
                           
                        }
                        else
                        {
                            _rootLayoutAuto.SetColumn(view, colNumber);
                            _rootLayoutAuto.SetRow(view, 0);
                            _rootLayoutAuto.Children.Add(view);
                        }

                        break;

                    case FlowColumnExpand.Proportional:

                        int howManyP = _desiredColumnCount / containerCount - 1;
                        
                        var left1 = colNumber + colNumber * howManyP;
                        _rootLayoutAuto.SetColumn(view, left1);
                        _rootLayoutAuto.SetColumnSpan(view, (colNumber + colNumber * howManyP + howManyP + 1) - left1);
                        _rootLayoutAuto.SetRow(view, 0);
                        _rootLayoutAuto.SetRowSpan(view, 1);
                        _rootLayoutAuto.Children.Add(view/*, colNumber + colNumber * howManyP, colNumber + colNumber * howManyP + howManyP + 1, 0, 1*/);

                        break;

                    case FlowColumnExpand.ProportionalFirst:

                        int firstSizeAdd = (int)((double)_desiredColumnCount) % containerCount; //1
                        int otherSize = (int)Math.Floor((double)_desiredColumnCount / containerCount); //2

                        if (colNumber == 0)
                        {
                            _rootLayoutAuto.SetColumn(view, 0);
                            _rootLayoutAuto.SetColumnSpan(view, otherSize + firstSizeAdd);
                            _rootLayoutAuto.SetRow(view, 0);
                            _rootLayoutAuto.SetRowSpan(view, 1);
                            _rootLayoutAuto.Children.Add(view/*, 0, otherSize + firstSizeAdd, 0, 1*/);
                        }

                        else
                        {
                            var left2 = (colNumber * otherSize) + firstSizeAdd;
                            _rootLayoutAuto.SetColumn(view, left2);
                            _rootLayoutAuto.SetColumnSpan(view, ((colNumber + 1) * otherSize) + firstSizeAdd - left2);
                            _rootLayoutAuto.SetRow(view, 0);
                            _rootLayoutAuto.SetRowSpan(view, 1);
                            _rootLayoutAuto.Children.Add(view/*, (colNumber * otherSize) + firstSizeAdd, ((colNumber + 1) * otherSize) + firstSizeAdd, 0, 1*/);
                        }


                        break;

                    case FlowColumnExpand.ProportionalLast:

                        int lastSizeAdd = (int)((double)_desiredColumnCount) % containerCount; //1
                        int otherSize1 = (int)Math.Floor((double)_desiredColumnCount / containerCount); //2
                        var left3 = colNumber * otherSize1;
                        if (isLastColumn)
                        {
                            _rootLayoutAuto.SetColumn(view, left3);
                            _rootLayoutAuto.SetColumnSpan(view, ((colNumber + 1) * otherSize1) + lastSizeAdd - left3);
                            _rootLayoutAuto.SetRow(view, 0);
                            _rootLayoutAuto.SetRowSpan(view, 1 - 0);
                            _rootLayoutAuto.Children.Add(view/*, (colNumber * otherSize1), ((colNumber + 1) * otherSize1) + lastSizeAdd, 0, 1*/);
                        }
                        else
                        {
                            _rootLayoutAuto.SetColumn(view, (colNumber * otherSize1));
                            _rootLayoutAuto.SetColumnSpan(view, ((colNumber + 1) * otherSize1) - left3);
                            _rootLayoutAuto.SetRow(view, 0);
                            _rootLayoutAuto.SetRowSpan(view, 1 - 0);
                            _rootLayoutAuto.Children.Add(view/*, (colNumber * otherSize1), ((colNumber + 1) * otherSize1), 0, 1*/);
                        }

                        break;
                }
            }
            else
            {
                _rootLayoutAuto.SetColumn(view, colNumber);
                _rootLayoutAuto.SetRow(view, 0);
                _rootLayoutAuto.Children.Add(view/*, colNumber, 0*/);
            }
        }

        /// <summary>
        /// Override this method to execute an action when the BindingContext changes.
        /// </summary>
        /// <remarks></remarks>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            UpdateData();

            if (BindingContext is INotifyCollectionChanged container)
            {
                container.CollectionChanged -= Container_CollectionChanged;
                container.CollectionChanged += Container_CollectionChanged;
            }
        }

        private void Container_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            if (!(BindingContext is IList container))
                return;

            var newDesiredColumnCount = 0;

            if (_flowListViewRef.TryGetTarget(out FlowListView flowListView) && flowListView != null)
            {
                _flowColumnTemplate = flowListView.FlowColumnTemplate;
                newDesiredColumnCount = flowListView.FlowDesiredColumnCount;
                _flowColumnExpand = flowListView.FlowColumnExpand;
            }

            var flowGroupColumn = BindingContext as FlowGroupColumn;
            if (flowGroupColumn != null)
            {
                newDesiredColumnCount = flowGroupColumn.ColumnCount;
            }

            // Getting view types from templates
            var containerCount = container.Count;
            IList<DataTemplate> templates = GetDataTemplates(container);

            bool layoutChanged = false;
            if (flowGroupColumn != null && flowGroupColumn.ForceInvalidateColumns)
            {
                layoutChanged = true;
                flowGroupColumn.ForceInvalidateColumns = false;
            }
            else
            {
                layoutChanged = RowLayoutChanged(containerCount, templates, newDesiredColumnCount);
            }

            _desiredColumnCount = newDesiredColumnCount;

            if (!layoutChanged) // REUSE VIEWS
            {
                if (_useGridAsMainRoot)
                {
                    for (int i = 0; i < containerCount; i++)
                    {
                        SetBindingContextForView((View)_rootLayoutAuto.Children[i], container[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < containerCount; i++)
                    {
                        SetBindingContextForView((View)_rootLayout.Children[i], container[i]);
                    }
                }
            }
            else // RECREATE COLUMNS
            {
                _currentColumnTemplates = new List<DataTemplate>(templates);

                if (_useGridAsMainRoot)
                {
                    if (_rootLayoutAuto.Children.Count > 0)
                        _rootLayoutAuto.Children.Clear();

                    var colDefs = new ColumnDefinitionCollection();
                    for (int i = 0; i < _desiredColumnCount; i++)
                    {
                        colDefs.Add(new ColumnDefinition() { Width = new GridLength(1d, GridUnitType.Star) });
                    }
                    _rootLayoutAuto.ColumnDefinitions = colDefs;

                    for (int i = 0; i < containerCount; i++)
                    {
                        if (!(templates[i].CreateContent() is View view))
                            throw new InvalidCastException("DataTemplate must return a View");

                        AddTapGestureToView(view);

                        SetBindingContextForView(view, container[i]);
                        if (containerCount == 0 || _desiredColumnCount == 0)
                            return;

                        AddViewToLayoutAutoHeightEnabled(view, containerCount, i);
                    }
                }
                else
                {
                    if (_rootLayout.Children.Count > 0)
                        _rootLayout.Children.Clear();

                    for (int i = 0; i < containerCount; i++)
                    {
                        if (!(templates[i].CreateContent() is View view))
                            throw new InvalidCastException("DataTemplate must return a View");

                        AddTapGestureToView(view);

                        SetBindingContextForView(view, container[i]);
                        if (containerCount == 0 || _desiredColumnCount == 0)
                            return;

                        AddViewToLayoutAutoHeightDisabled(view, containerCount, i);
                    }
                }
            }
        }

        void AddTapGestureToView(View view)
        {
            var command = new Command(async (obj) =>
            {
                await ExecuteTapGestureRecognizer(view);
            });

            view.GestureRecognizers.Add(new TapGestureRecognizer() { Command = command });
            view.GestureRecognizers.Add(new ClickGestureRecognizer() { Command = command, Buttons = ButtonsMask.Primary, NumberOfClicksRequired = 1 });
        }

        async Task ExecuteTapGestureRecognizer(View view)
        {
            if (view is IFlowViewCell flowCell)
            {
                flowCell.OnTapped();
            }

            _flowListViewRef.TryGetTarget(out FlowListView flowListView);

            if (flowListView != null)
            {
                int tapBackgroundEffectDelay = flowListView.FlowTappedBackgroundDelay;

                try
                {
                    if (tapBackgroundEffectDelay != 0)
                    {
                        view.BackgroundColor = flowListView.FlowTappedBackgroundColor;
                    }

                    flowListView.FlowPerformTap(view, view.BindingContext);
                }
                finally
                {
                    if (tapBackgroundEffectDelay != 0)
                    {
                        await Task.Delay(tapBackgroundEffectDelay);
                        view.BackgroundColor = flowListView.FlowRowBackgroundColor;
                    }
                }
            }
        }
    }
}

