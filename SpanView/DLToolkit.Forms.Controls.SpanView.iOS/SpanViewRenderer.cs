using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using DLToolkit.Forms.Controls;
using DLToolkit.Forms.Controls.SpanView.iOS;

[assembly: ExportRenderer(typeof(SpanView), typeof(SpanViewRenderer))]
namespace DLToolkit.Forms.Controls.SpanView.iOS
{
    public class SpanViewRenderer : ViewRenderer<SpanView, UILabel>
    {
        public static void Init()
        {
            var dummy = new SpanViewRenderer();
        }

        public SpanViewRenderer()
        {
        }
    }
}

