using System;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
    public class RecyclerView : View
    {
        public RecyclerView()
        {
        }


        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(RecyclerViewOrientation), typeof(RecyclerView), default(RecyclerViewOrientation));

        public RecyclerViewOrientation Orientation
        {
            get { return (RecyclerViewOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }


        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ICollection), typeof(RecyclerView), default(ICollection));

        public ICollection ItemsSource
        {
            get { return (ICollection)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }


        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RecyclerView), default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }


        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.Create(nameof(ItemTappedCommand), typeof(ICommand), typeof(RecyclerView), default(ICommand));

        public ICommand ItemTappedCommand
        {
            get { return (ICommand)GetValue(ItemTappedCommandProperty); }
            set { SetValue(ItemTappedCommandProperty, value); }
        }

        public event EventHandler<ItemTappedEventArgs> OnItemTapped; 

        internal void InvokeItemTappedEvent(object sender, object item)
        {
            var args = new ItemTappedEventArgs(item);
            OnItemTapped?.Invoke(this, args);
            ItemTappedCommand?.Execute(item);
        }

        public event EventHandler<ScrolledEventArgs> OnScrolled;

        internal void InvokeScrolledEvent(double currentX, double currentY)
        {
            var args = new ScrolledEventArgs(currentX, currentY);
            OnScrolled?.Invoke(this, args);
        }

        public class ScrolledEventArgs : Xamarin.Forms.ScrolledEventArgs
        {
            public ScrolledEventArgs(double scrollX, double scrollY) : base(scrollX, scrollY)
            {
            }
        }

        public class ItemTappedEventArgs : SelectedItemChangedEventArgs
        {
            public ItemTappedEventArgs(object item) : base(item)
            {
            }
        }
    }
}
