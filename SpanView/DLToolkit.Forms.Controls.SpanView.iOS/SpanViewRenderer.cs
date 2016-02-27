using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using DLToolkit.Forms.Controls;
using CoreGraphics;
using System.ComponentModel;
using DLToolkit.Forms.Controls.SpanViewRenderer.iOS;

[assembly: ExportRenderer(typeof(SpanView), typeof(SpanViewRenderer))]
namespace DLToolkit.Forms.Controls.SpanViewRenderer.iOS
{
    public class SpanViewRenderer : ViewRenderer<SpanView, UILabel>
    {
        public static void Init()
        {
            var dummy = new SpanViewRenderer();
        }

        public SpanViewRenderer()
        {
            base.AutoPackage = false;
        }
            
        private bool perfectSizeValid;

        private SizeRequest perfectSize;

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (!this.perfectSizeValid)
            {
                this.perfectSize = base.GetDesiredSize(double.PositiveInfinity, double.PositiveInfinity);
                this.perfectSize.Minimum = new Size(Math.Min(10, this.perfectSize.Request.Width), this.perfectSize.Request.Height);
                this.perfectSizeValid = true;
            }
            if (widthConstraint >= this.perfectSize.Request.Width && heightConstraint >= this.perfectSize.Request.Height)
            {
                return this.perfectSize;
            }
            SizeRequest desiredSize = base.GetDesiredSize(widthConstraint, heightConstraint);
            desiredSize.Minimum = new Size(Math.Min(10, desiredSize.Request.Width), desiredSize.Request.Height);
            return desiredSize;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (base.Control == null)
            {
                return;
            }
            switch (base.Element.VerticalTextAlignment)
            {
                case TextAlignment.Start:
                    {
                        CGSize cGSize = base.Control.SizeThatFits(base.Element.Bounds.Size.ToSizeF());
                        nfloat nfloat = (nfloat)Math.Min(this.Bounds.Height, cGSize.Height);
                        base.Control.Frame = new CGRect(0, 0, (nfloat)base.Element.Width, nfloat);
                        return;
                    }
                case TextAlignment.Center:
                    base.Control.Frame = new CGRect(0, 0, (nfloat)base.Element.Width, (nfloat)base.Element.Height);
                    return;
                case TextAlignment.End:
                    {
                        nfloat y = 0;
                        CGSize cGSize = base.Control.SizeThatFits(base.Element.Bounds.Size.ToSizeF());
                        nfloat nfloat = (nfloat)Math.Min(this.Bounds.Height, cGSize.Height);
                        y = (nfloat)(base.Element.Height - nfloat);
                        base.Control.Frame = new CGRect(0, y, (nfloat)base.Element.Width, nfloat);
                        return;
                    }
                default:
                    return;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SpanView> e)
        {
            if (e.NewElement != null)
            {
                if (base.Control == null)
                {
                    base.SetNativeControl(new UILabel(CGRect.Empty) {
                        BackgroundColor = UIColor.Clear
                    });
                }
                this.UpdateText();
                this.UpdateLineBreakMode();
                this.UpdateAlignment();
            }
            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == SpanView.SpansProperty)
            {
                //TODO
                return;
            }
            if (e.PropertyName == SpanView.HorizontalTextAlignmentProperty.PropertyName)
            {
                this.UpdateAlignment();
                return;
            }
            if (e.PropertyName == SpanView.VerticalTextAlignmentProperty.PropertyName)
            {
                this.LayoutSubviews();
                return;
            }
            if (e.PropertyName == SpanView.TextColorProperty.PropertyName)
            {
                this.UpdateText();
                return;
            }
            if (e.PropertyName == SpanView.FontProperty.PropertyName)
            {
                this.UpdateText();
                return;
            }
            if (e.PropertyName == SpanView.LineBreakModeProperty.PropertyName)
            {
                this.UpdateLineBreakMode();
            }
        }

        protected override void SetBackgroundColor(Color color)
        {
            if (color == Color.Default)
            {
                this.BackgroundColor = UIColor.Clear;
                return;
            }
            this.BackgroundColor = color.ToUIColor();
        }

        private void UpdateAlignment()
        {
            base.Control.TextAlignment = ToNativeTextAlignment(Element.HorizontalTextAlignment);
        }

        private static UITextAlignment ToNativeTextAlignment(TextAlignment alignment)
        {
            if (alignment == TextAlignment.Center)
            {
                return UITextAlignment.Center;
            }
            if (alignment != TextAlignment.End)
            {
                return UITextAlignment.Left;
            }
            return UITextAlignment.Right;
        }

        private void UpdateLineBreakMode()
        {
            this.perfectSizeValid = false;
            switch (base.Element.LineBreakMode)
            {
                case LineBreakMode.NoWrap:
                    base.Control.LineBreakMode = UILineBreakMode.Clip;
                    base.Control.Lines = 1;
                    return;
                case LineBreakMode.WordWrap:
                    base.Control.LineBreakMode = UILineBreakMode.WordWrap;
                    base.Control.Lines = 0;
                    return;
                case LineBreakMode.CharacterWrap:
                    base.Control.LineBreakMode = UILineBreakMode.CharacterWrap;
                    base.Control.Lines = 0;
                    return;
                case LineBreakMode.HeadTruncation:
                    base.Control.LineBreakMode = UILineBreakMode.HeadTruncation;
                    base.Control.Lines = 1;
                    return;
                case LineBreakMode.TailTruncation:
                    base.Control.LineBreakMode = UILineBreakMode.TailTruncation;
                    base.Control.Lines = 1;
                    return;
                case LineBreakMode.MiddleTruncation:
                    base.Control.LineBreakMode = UILineBreakMode.MiddleTruncation;
                    base.Control.Lines = 1;
                    return;
                default:
                    return;
            }
        }

        private void UpdateText()
        {
            //TODO
//            this.perfectSizeValid = false;
//            object[] values = base.Element.GetValues(SpanView.FormattedTextProperty, SpanView.TextProperty, SpanView.TextColorProperty);
//            FormattedString formattedString = (FormattedString)values[0];
//            if (formattedString != null)
//            {
//                base.Control.AttributedText = formattedString.ToAttributed(base.Element, (Color)values[2]);
//            }
//            else
//            {
//                base.Control.Text = (string)values[1];
//                base.Control.Font = base.Element.ToUIFont();
//                base.Control.TextColor = ((Color)values[2]).ToUIColor(ColorExtensions.Black);
//            }
            this.LayoutSubviews();
        }
    }
}

