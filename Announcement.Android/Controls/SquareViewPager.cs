using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Orientation = Android.Content.Res.Orientation;
using Android.Content.Res;
using Android.Widget;
using Android.Support.V4.View;

namespace Announcement.Android.Controls
{
	class SquareViewPager : ViewPager
	{
		protected SquareViewPager(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public SquareViewPager(Context context) : base(context)
		{
		}

		public SquareViewPager(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

			//if (Resources.Configuration.Orientation == Orientation.Portrait)
				SetMeasuredDimension(MeasuredWidth, MeasuredWidth);
			//else
			//	SetMeasuredDimension(MeasuredHeight, MeasuredHeight);
		}
	}
}