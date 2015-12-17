using System;
using Android.Util;
using Android.Runtime;
using Android.Content;
using Android.Graphics;
using Android.Animation;
using Android.Views.Animations;
using Android.App;

namespace Announcement.Android.Controls
{
    public class ProgressIndicator : global::Android.Views.View
    {
        public ProgressIndicator(Context context) : base(context)
        {
            Initialize();
        }

        public ProgressIndicator(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        public ProgressIndicator(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();

            ParseAttributes(attrs);
        }

        public ProgressIndicator(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Initialize();

            ParseAttributes(attrs);
        }

        protected void Initialize()
        {
            linePaint = new Paint();

            linePaint.SetStyle(Paint.Style.Fill);

            linePaint.AntiAlias = true;


            borderPaint = new Paint();

            borderPaint.SetStyle(Paint.Style.Stroke);

            borderRect = new Rect();


            linePath = new Path();

            lineRect = new RectF();

            valueAnimator = new ValueAnimator();

            valueAnimator.RepeatMode = ValueAnimatorRepeatMode.Restart;

            valueAnimator.RepeatCount = ValueAnimator.Infinite;

            valueAnimator.SetInterpolator(new AccelerateDecelerateInterpolator());

            valueAnimator.SetDuration(1000);

            valueAnimator.Update += ValueAnimator_Update;


            backgroundPaint = new Paint();
                
            backgroundPaint.SetStyle(Paint.Style.Fill);

            backgroundRect = new Rect();
        }
 
        protected void ParseAttributes(IAttributeSet attrs)
        {
            using (var attributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.progress_indicator))
            {
                backgroundPaint.Color = attributes.GetColor(Resource.Styleable.progress_indicator_backgroundColor, 0);

                borderPaint.Color = attributes.GetColor(Resource.Styleable.progress_indicator_borderColor, -1);

                linePaint.Color = attributes.GetColor(Resource.Styleable.progress_indicator_lineColor, -1);

                borderPaint.StrokeWidth = attributes.GetDimension(Resource.Styleable.progress_indicator_borderWidth, ValueConverterManager.DpToPx(BORDER_WIDTH_IN_DP));

                borderOffset = (int)(borderPaint.StrokeWidth / 2);

                lineWidthPercent = attributes.GetInt(Resource.Styleable.progress_indicator_lineWidthPercent, LINE_WIDTH_IN_PERCENT);

                attributes.Recycle();
            }
        }

        protected void ValueAnimator_Update (object sender, ValueAnimator.AnimatorUpdateEventArgs e)
        {
            var offset = (int)valueAnimator.AnimatedValue;

            linePath.Rewind();

            linePath.MoveTo(-lineWidth + offset, MeasuredHeight);

            linePath.LineTo(-lineWidth + miterWidth + offset, 0);

            linePath.LineTo(0 + offset, 0);

            linePath.LineTo(0 - miterWidth + offset, MeasuredHeight);

            linePath.LineTo(-lineWidth + offset, MeasuredHeight);

            linePath.Close();

            PostInvalidate(backgroundRect.Left, backgroundRect.Top, backgroundRect.Right, backgroundRect.Bottom);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            lineWidth = (int)((MeasuredWidth / 100.0f) * lineWidthPercent);

            miterWidth = (int)((lineWidth / 100.0f) * LINE_MITER_WIDTH_IN_PERCENT);

            borderRect.Set(0 + borderOffset + 1, 0 + borderOffset + 1, MeasuredWidth - borderOffset, MeasuredHeight - borderOffset);

            linePath.Rewind();

            linePath.MoveTo(-lineWidth, MeasuredHeight);

            linePath.LineTo(-lineWidth + miterWidth, 0);

            linePath.LineTo(0, 0);

            linePath.LineTo(0 - miterWidth, MeasuredHeight);

            linePath.LineTo(-lineWidth, MeasuredHeight);

            linePath.Close();

            valueAnimator.SetIntValues(0, MeasuredWidth + lineWidth);

            backgroundRect.Set(borderRect);
        }

        public override void Draw(Canvas canvas)
        {
            canvas.DrawRect(backgroundRect, backgroundPaint);
            
            canvas.DrawPath(linePath, linePaint);

            canvas.DrawRect(borderRect, borderPaint);

            if (!valueAnimator.IsStarted)
            {
                valueAnimator.Start();
            }
        }

        private Paint backgroundPaint;

        private Rect backgroundRect;

        private Paint borderPaint;

        private int borderOffset;

        private Rect borderRect;

        private Paint linePaint;

        private Path linePath;

        private RectF lineRect;

        private ValueAnimator valueAnimator;

        private int lineWidth;

        private int lineWidthPercent;

        private int miterWidth;

        private const int LINE_WIDTH_IN_PERCENT = 56;

        private const int LINE_MITER_WIDTH_IN_PERCENT = 8;

        private const int BORDER_WIDTH_IN_DP = 1;
    }
}

