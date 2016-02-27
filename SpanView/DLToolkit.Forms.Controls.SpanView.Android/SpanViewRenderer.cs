using System;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Xamarin.Forms;
using DLToolkit.Forms.Controls;
using DLToolkit.Forms.Controls.SpanView.Android;

[assembly: ExportRenderer(typeof(SpanView), typeof(SpanViewRenderer))]
namespace DLToolkit.Forms.Controls.SpanView.Android
{
    public class SpanViewRenderer : ViewRenderer<SpanView, TextView>
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

