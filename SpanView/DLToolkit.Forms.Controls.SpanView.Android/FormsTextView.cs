using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using System;

namespace DLToolkit.Forms.Controls.SpanViewRenderer.Android
{
    public class FormsTextView : TextView
    {
        private bool skip;
       
        protected FormsTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public FormsTextView(Context context) : base(context)
        {
        }

        public FormsTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public FormsTextView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        public override void Invalidate()
        {
            if (!this.skip)
            {
                base.Invalidate();
            }
            this.skip = false;
        }

        public void SkipNextInvalidate()
        {
            this.skip = true;
        }
    }
}

