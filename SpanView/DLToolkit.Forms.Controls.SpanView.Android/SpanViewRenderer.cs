using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Android.Widget;
using System;
using System.ComponentModel;
using DLToolkit.Forms.Controls.SpanViewRenderer.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using DLToolkit.Forms.Controls;
using Android.Util;

[assembly: ExportRenderer(typeof(SpanView), typeof(SpanViewRenderer))]
namespace DLToolkit.Forms.Controls.SpanViewRenderer.Android
{
    public class SpanViewRenderer : ViewRenderer<SpanView, TextView>
    {
        public static void Init()
        {
            var dummy = new SpanViewRenderer();
        }
            
        private FormsTextView view;

        private Xamarin.Forms.Color lastUpdateColor = Xamarin.Forms.Color.Default;

        private int lastConstraintHeight;

        private int lastConstraintWidth;

        private SizeRequest? lastSizeRequest;

        private ColorStateList labelTextColorDefault;

        private bool wasFormatted;

        private Typeface lastTypeface;

        private float lastTextSize = -1;

        public SpanViewRenderer()
        {
            base.AutoPackage = false;
        }

        static class MeasureSpecFactory
        {
            //
            // Static Methods
            //
            public static int GetSize(int measureSpec)
            {
                return measureSpec & 1073741823;
            }

            public static int MakeMeasureSpec(int size, MeasureSpecMode mode)
            {
                return (int)(size + mode);
            }
        }
            
        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            if (this.lastSizeRequest.HasValue)
            {
                bool flag = widthConstraint == this.lastConstraintWidth && heightConstraint == this.lastConstraintHeight;
                if (!flag)
                {
                    int size = MeasureSpecFactory.GetSize(this.lastConstraintWidth);
                    int size2 = MeasureSpecFactory.GetSize(this.lastConstraintHeight);
                    int size3 = MeasureSpecFactory.GetSize(widthConstraint);
                    int size4 = MeasureSpecFactory.GetSize(heightConstraint);
                    bool arg_E7_0 = this.lastSizeRequest.Value.Request.Width < (double)size && this.lastSizeRequest.Value.Request.Height < (double)size2;
                    bool flag2 = (double)size3 >= this.lastSizeRequest.Value.Request.Width && (double)size4 >= this.lastSizeRequest.Value.Request.Height;
                    flag = (arg_E7_0 & flag2);
                }
                if (flag)
                {
                    return this.lastSizeRequest.Value;
                }
            }
            SizeRequest desiredSize = base.GetDesiredSize(widthConstraint, heightConstraint);
            desiredSize.Minimum = new Xamarin.Forms.Size(Math.Min((double)base.Context.ToPixels(10), desiredSize.Request.Width), desiredSize.Request.Height);
            this.lastConstraintWidth = widthConstraint;
            this.lastConstraintHeight = heightConstraint;
            this.lastSizeRequest = new SizeRequest?(desiredSize);
            return desiredSize;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SpanView> e)
        {
            base.OnElementChanged(e);
            if (this.view == null)
            {
                this.view = new FormsTextView(base.Context);
                this.labelTextColorDefault = this.view.TextColors;
                base.SetNativeControl(this.view);
            }
            if (e.OldElement == null)
            {
                this.UpdateText();
                this.UpdateLineBreakMode();
                this.UpdateGravity();
                return;
            }
            this.view.SkipNextInvalidate();
            this.UpdateText();
            if (e.OldElement.LineBreakMode != e.NewElement.LineBreakMode)
            {
                this.UpdateLineBreakMode();
            }
            if (e.OldElement.HorizontalTextAlignment != e.NewElement.HorizontalTextAlignment || e.OldElement.VerticalTextAlignment != e.NewElement.VerticalTextAlignment)
            {
                this.UpdateGravity();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == SpanView.SpansProperty)
            {
                //TODO
                return;
            }
            if (e.PropertyName == SpanView.HorizontalTextAlignmentProperty.PropertyName || e.PropertyName == SpanView.VerticalTextAlignmentProperty.PropertyName)
            {
                this.UpdateGravity();
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
                return;
            }
        }

        private void UpdateColor()
        {
            Xamarin.Forms.Color textColor = base.Element.TextColor;
            if (textColor == this.lastUpdateColor)
            {
                return;
            }
            this.lastUpdateColor = textColor;
            if (textColor == Color.Mode.Default)
            {
                this.view.SetTextColor(this.labelTextColorDefault);
                return;
            }
            this.view.SetTextColor(textColor.ToAndroid());
        }

        private void UpdateFont()
        {
            Font expr_0B = base.Element.Font;
            Typeface typeface = expr_0B.ToTypeface();
            if (typeface != this.lastTypeface)
            {
                this.view.Typeface = typeface;
                this.lastTypeface = typeface;
            }
            float num = expr_0B.ToScaledPixel();
            if (num != this.lastTextSize)
            {
                this.view.SetTextSize(ComplexUnitType.Sp, num);
                this.lastTextSize = num;
            }
        }

        private void UpdateGravity()
        {
            SpanView element = base.Element;
            this.view.Gravity = (ToHorizontalGravityFlags(element.HorizontalTextAlignment) | ToVerticalGravityFlags(element.VerticalTextAlignment));
            this.lastSizeRequest = null;
        }

        private static GravityFlags ToHorizontalGravityFlags(Xamarin.Forms.TextAlignment alignment)
        {
            if (alignment == Xamarin.Forms.TextAlignment.Center)
            {
                return GravityFlags.AxisSpecified;
            }
            if (alignment != Xamarin.Forms.TextAlignment.End)
            {
                return GravityFlags.Left;
            }
            return GravityFlags.Right;
        }

        private static GravityFlags ToVerticalGravityFlags(Xamarin.Forms.TextAlignment alignment)
        {
            if (alignment == Xamarin.Forms.TextAlignment.Start)
            {
                return GravityFlags.Top;
            }
            if (alignment != Xamarin.Forms.TextAlignment.End)
            {
                return GravityFlags.CenterVertical;
            }
            return GravityFlags.Bottom;
        }

        private void UpdateLineBreakMode()
        {
            switch (base.Element.LineBreakMode)
            {
                case LineBreakMode.NoWrap:
                    this.view.SetSingleLine(true);
                    this.view.Ellipsize = null;
                    break;
                case LineBreakMode.WordWrap:
                    this.view.SetSingleLine(false);
                    this.view.Ellipsize = null;
                    this.view.SetMaxLines(100);
                    break;
                case LineBreakMode.CharacterWrap:
                    this.view.SetSingleLine(false);
                    this.view.Ellipsize = null;
                    this.view.SetMaxLines(100);
                    break;
                case LineBreakMode.HeadTruncation:
                    this.view.SetSingleLine(true);
                    this.view.Ellipsize = TextUtils.TruncateAt.Start;
                    break;
                case LineBreakMode.TailTruncation:
                    this.view.SetSingleLine(true);
                    this.view.Ellipsize = TextUtils.TruncateAt.End;
                    break;
                case LineBreakMode.MiddleTruncation:
                    this.view.SetSingleLine(true);
                    this.view.Ellipsize = TextUtils.TruncateAt.Middle;
                    break;
            }
            this.lastSizeRequest = null;
        }

        private void UpdateText()
        {
            if (base.Element.FormattedText != null)
            {
                FormattedString formattedString = base.Element.FormattedText ?? base.Element.Text;
                this.view.TextFormatted = formattedString.ToAttributed(base.Element.Font, base.Element.TextColor, this.view);
                this.wasFormatted = true;
            }
            else
            {
                if (this.wasFormatted)
                {
                    this.view.SetTextColor(this.labelTextColorDefault);
                    this.lastUpdateColor = Color.Default;
                }
                this.view.Text = base.Element.Text;
                this.UpdateColor();
                this.UpdateFont();
                this.wasFormatted = false;
            }
            this.lastSizeRequest = null;
        }
    }
}

