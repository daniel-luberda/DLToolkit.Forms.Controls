using System;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
    public class TextSpan : ISpan
    {
        public TextSpan()
        {
            FontFamily = null;
            FontAttributes = FontAttributes.None;
            FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
        }

        public TextSpan(Span span)
        {
            BackgroundColor = span.BackgroundColor;
            FontAttributes = span.FontAttributes;
            FontFamily = span.FontFamily;
            FontSize = span.FontSize;
            ForegroundColor = span.ForegroundColor;
            Text = span.Text;
        }

        public Color BackgroundColor { get; set; }

        public FontAttributes FontAttributes { get; set; }

        public string FontFamily { get; set; }

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize { get; set; }

        public Color ForegroundColor { get; set; }

        public string Text { get; set; }
    }
}

