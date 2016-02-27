using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace DLToolkit.Forms.Controls 
{
    public class SpanView : View
    {
        public SpanView()
        {
            new Label();
        }

        public static readonly BindableProperty SpansProperty = BindableProperty.Create("Spans", typeof(IList<ISpan>), typeof(SpanView), null);

        public IList<ISpan> Spans
        {
            get
            {
                return (IList<ISpan>)GetValue(SpanView.SpansProperty);
            }
            set
            {
                SetValue(SpanView.SpansProperty, value);
            }
        }

        public static readonly BindableProperty FontProperty = BindableProperty.Create("Font", typeof(Font), typeof(SpanView), default(Font));

        [Obsolete("Please use the Font attributes which are on the class itself.")]
        public Font Font
        {
            get
            {
                return (Font)GetValue(SpanView.FontProperty);
            }
            set
            {
                base.SetValue(SpanView.FontProperty, value);
            }
        }

        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create("FontAttributes", typeof(FontAttributes), typeof(SpanView), FontAttributes.None);

        public FontAttributes FontAttributes
        {
            get
            {
                return (FontAttributes)GetValue(SpanView.FontAttributesProperty);
            }
            set
            {
                SetValue(SpanView.FontAttributesProperty, value);
            }
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create("FontFamily", typeof(string), typeof(SpanView), null);

        public string FontFamily
        {
            get
            {
                return (string)GetValue(SpanView.FontFamilyProperty);
            }
            set
            {
                SetValue(SpanView.FontFamilyProperty, value);
            }
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(double), typeof(SpanView), -1);

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get
            {
                return (double)GetValue(SpanView.FontSizeProperty);
            }
            set
            {
                SetValue(SpanView.FontSizeProperty, value);
            }
        }

        public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create("HorizontalTextAlignment", typeof(TextAlignment), typeof(SpanView), TextAlignment.Start);

        public TextAlignment HorizontalTextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(SpanView.HorizontalTextAlignmentProperty);
            }
            set
            {
                SetValue(SpanView.HorizontalTextAlignmentProperty, value);
            }
        }

        public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create("LineBreakMode", typeof(LineBreakMode), typeof(SpanView), LineBreakMode.WordWrap);

        public LineBreakMode LineBreakMode
        {
            get
            {
                return (LineBreakMode)GetValue(SpanView.LineBreakModeProperty);
            }
            set
            {
                SetValue(SpanView.LineBreakModeProperty, value);
            }
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(SpanView), Color.Default);

        public Color TextColor
        {
            get
            {
                return (Color)GetValue(SpanView.TextColorProperty);
            }
            set
            {
                SetValue(SpanView.TextColorProperty, value);
            }
        }

        public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create("VerticalTextAlignment", typeof(TextAlignment), typeof(SpanView), TextAlignment.Start);

        public TextAlignment VerticalTextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(SpanView.VerticalTextAlignmentProperty);
            }
            set
            {
                SetValue(SpanView.VerticalTextAlignmentProperty, value);
            }
        }
    }
}

