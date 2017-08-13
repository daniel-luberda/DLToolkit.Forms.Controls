using System;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace DLToolkit.Forms.Controls
{
	// used part of the code of: https://github.com/conceptdev/xamarin-forms-samples/blob/master/Evolve13/Evolve13/Controls/WrapLayout.cs
	[Helpers.TagEntryView.Preserve(AllMembers = true)]
    public class TagEntryView : Layout<View>, IDisposable
	{
		public TagEntryView()
		{
			PropertyChanged += TagEntryViewPropertyChanged;
			PropertyChanging += TagEntryViewPropertyChanging;

			TagEntry = new TagEntry();
			TagEntry.TextChanged += TagEntryTextChanged;
			Children.Add(TagEntry);
		}

		void TagEntryTextChanged (object sender, TextChangedEventArgs e)
		{
			if (TagSeparators.Any(e.NewTextValue.Contains))
			{
				string tag = e.NewTextValue;
				foreach (var item in TagSeparators)
				{
					tag = tag.Replace(item, string.Empty);
				}

				var tagBindingContext = TagValidatorFactory(tag);
				var tagEntry = sender as TagEntry;
	
				if (tagBindingContext != null)
				{
					TagItems.Add(tagBindingContext);
					tagEntry.Text = string.Empty;

				}
				else
				{
					tagEntry.Text = tag;
				}

				tagEntry.Focus();
			}
		}
			
		public event EventHandler<ItemTappedEventArgs> TagTapped;

		public Entry TagEntry { get; private set; }

		internal void PerformTagTap(object item)
		{
			EventHandler<ItemTappedEventArgs> handler = TagTapped;
			if (handler != null) handler(this, new ItemTappedEventArgs(null, item));

			var command = TagTappedCommand;
			if (command != null && command.CanExecute(item)) 
			{
				command.Execute(item);
			}
		}

		public Func<string, object> TagValidatorFactory { get; set; }

		Func<View> _tagViewFactory;
		[Obsolete("Use XAML compatible TagItemTemplate property")]
		public Func<View> TagViewFactory
		{
			get
			{
				return _tagViewFactory;
			}

			set
			{
				TagItemTemplate = new DataTemplate(value);
				_tagViewFactory = value;
			}
		}

		public static readonly BindableProperty TagItemTemplateProperty = BindableProperty.Create(nameof(TagItemTemplate), typeof(DataTemplate), typeof(TagEntryView), default(DataTemplate));

		public DataTemplate TagItemTemplate
		{
			get
			{
				return (DataTemplate)GetValue(TagItemTemplateProperty);
			}
			set
			{
				SetValue(TagItemTemplateProperty, value);
			}
		}

		public static BindableProperty TagTappedCommandProperty = BindableProperty.Create(nameof(TagTappedCommand), typeof(ICommand), typeof(TagEntryView), default(ICommand));

		public ICommand TagTappedCommand
		{
			get { return (ICommand)GetValue(TagTappedCommandProperty); }
			set { SetValue(TagTappedCommandProperty, value); }
		}

		public static readonly BindableProperty TagSeparatorsProperty = BindableProperty.Create(nameof(TagSeparators), typeof(IList<string>), typeof(TagEntryView), new List<string>() { " " });

		public IList<string> TagSeparators
		{
			get { return (IList<string>)GetValue(TagSeparatorsProperty); }
			set { SetValue(TagSeparatorsProperty, value); } 
		}


		public static readonly BindableProperty EntryMinimumWidthProperty = BindableProperty.Create(nameof(EntryMinimumWidth), typeof(double), typeof(TagEntryView), 150d);

		public double EntryMinimumWidth
		{
			get { return (double)GetValue(EntryMinimumWidthProperty); }
			set { SetValue(EntryMinimumWidthProperty, value); } 
		}


		public static readonly BindableProperty TagItemsProperty = BindableProperty.Create(nameof(TagItems), typeof(IList), typeof(TagEntryView), default(IList), BindingMode.TwoWay);

		public IList TagItems
		{
			get { return (IList)GetValue(TagItemsProperty); }
			set { SetValue(TagItemsProperty, value); } 
		}


		public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(TagEntryView), 6d, 
				propertyChanged: (bindable, oldvalue, newvalue) => ((TagEntryView)bindable).OnSizeChanged());

		public double Spacing 
		{
			get { return (double)GetValue(SpacingProperty); }
			set { SetValue(SpacingProperty, value); } 
		}


		private void TagEntryViewPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == TagItemsProperty.PropertyName)
			{
				var tagItems = TagItems as INotifyCollectionChanged;
				if (tagItems != null)
					tagItems.CollectionChanged -= TagItemsCollectionChanged;
			}
		}

		private void TagEntryViewPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == TagItemsProperty.PropertyName)
			{
				var tagItems = TagItems as INotifyCollectionChanged;
				if (tagItems != null)
					tagItems.CollectionChanged += TagItemsCollectionChanged;

				ForceReload();
			}
		}

		private void TagItemsCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			ForceReload();
		}

		public void ForceReload()
		{
			Children.Clear();

			for (int i = 0; i < TagItems.Count; i++)
			{
				View view = null;

				var templateSelector = TagItemTemplate as DataTemplateSelector;
				if (templateSelector != null)
				{
					var template = templateSelector.SelectTemplate(TagItems[i], null);
					view = (View)template.CreateContent();
				}
				else
				{
					view = (View)TagItemTemplate.CreateContent();
				}

				view.BindingContext = TagItems[i];

				view.GestureRecognizers.Add(new TapGestureRecognizer(){
					Command = new Command(() => PerformTagTap(view.BindingContext))
				});

				Children.Add(view);
			}

			if (TagEntry.IsVisible) Children.Add(TagEntry); //via AtlasAF01 (Rob Oller)
		}

		private void OnSizeChanged()
		{
			this.ForceLayout();
		}

		protected override SizeRequest OnSizeRequest (double widthConstraint, double heightConstraint)
		{
			if (WidthRequest > 0)
				widthConstraint = Math.Min (widthConstraint, WidthRequest);
			if (HeightRequest > 0)
				heightConstraint = Math.Min (heightConstraint, HeightRequest);

			double internalWidth = double.IsPositiveInfinity (widthConstraint) ? double.PositiveInfinity : Math.Max (0, widthConstraint);
			double internalHeight = double.IsPositiveInfinity (heightConstraint) ? double.PositiveInfinity : Math.Max (0, heightConstraint);

			return DoHorizontalMeasure(internalWidth, internalHeight);
		}

		private SizeRequest DoHorizontalMeasure(double widthConstraint, double heightConstraint)
		{
			int rowCount = 1;

			double width = 0;
			double height = 0;
			double minWidth = 0;
			double minHeight = 0;
			double widthUsed = 0;

			foreach (var item in Children)    
			{
				var size = item.GetSizeRequest(widthConstraint, heightConstraint);
				height = Math.Max (height, size.Request.Height);

				var newWidth = width + size.Request.Width + Spacing;
				if (newWidth > widthConstraint) {
					rowCount++;
					widthUsed = Math.Max(width, widthUsed);
					width = size.Request.Width;
				} else
					width = newWidth;

				minHeight = Math.Max(minHeight, size.Minimum.Height);
				minWidth = Math.Max (minWidth, size.Minimum.Width);
			}

			if (rowCount > 1) {
				width = Math.Max(width, widthUsed);
				height = (height + Spacing) * rowCount - Spacing; // via MitchMilam 
			}

			return new SizeRequest(new Size(width, height), new Size(minWidth,minHeight));
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			double rowHeight = 0;
			double yPos = y, xPos = x;

			foreach (var child in Children.Where(c => c.IsVisible)) 
			{
				var request = child.GetSizeRequest (width, height);

				double childWidth = request.Request.Width;
				double childHeight = request.Request.Height;


				if (child == TagEntry)
				{
					childWidth = EntryMinimumWidth;

					if (xPos + childWidth < width)
					{
						var orgEntrySize = childWidth;
						childWidth = width - xPos;
						childWidth = Math.Max(childWidth, orgEntrySize);
					}
					else
					{
						childWidth = width;
					}
				}

				rowHeight = Math.Max(rowHeight, childHeight);

				if (xPos + childWidth > width) 
				{
					xPos = x;
					yPos += rowHeight + Spacing;
					rowHeight = 0;
				}

				var region = new Rectangle(xPos, yPos, childWidth, childHeight);
				LayoutChildIntoBoundingRegion(child, region);
				xPos += region.Width + Spacing;
			}
		}

		public void Dispose()
		{
			TagEntry.TextChanged -= TagEntryTextChanged;

			PropertyChanged -= TagEntryViewPropertyChanged;
			PropertyChanging -= TagEntryViewPropertyChanging;

			var tagItems = TagItems as INotifyCollectionChanged;
			if (tagItems != null)
			{
				tagItems.CollectionChanged -= TagItemsCollectionChanged;
			}
		}
	}
}

