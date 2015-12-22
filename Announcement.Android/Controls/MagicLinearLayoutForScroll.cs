using System;
using Android.Util;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Graphics;
using Android.Views;

namespace Announcement.Android.Controls
{
	public class MagicLinearLayoutForScroll : LinearLayout, IWidthOnScreen
	{
		public int WidthOnScreen { get; set; }

		public MagicLinearLayoutForScroll(Context context): base(context)
		{
			Initialize();
		}

		public MagicLinearLayoutForScroll(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Initialize();
		}

		public MagicLinearLayoutForScroll(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialize();

			ParseAttributes(attrs);
		}

		public MagicLinearLayoutForScroll(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			Initialize();

			ParseAttributes(attrs);
		}

		protected void Initialize()
		{

		}

		protected void ParseAttributes(IAttributeSet attrs)
		{

		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			if (Parent != null && Parent is IWidthOnScreen)
			{
				WidthOnScreen = ((IWidthOnScreen)Parent).WidthOnScreen;
			}

			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
		}
	}

	public class MagicRelativeLayoutWidthOnScreen : RelativeLayout, IWidthOnScreen
	{
		public int WidthOnScreen { get; set; }

		public MagicRelativeLayoutWidthOnScreen(Context context): base(context)
		{
			Initialize();
		}

		public MagicRelativeLayoutWidthOnScreen(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Initialize();
		}

		public MagicRelativeLayoutWidthOnScreen(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialize();

			ParseAttributes(attrs);
		}

		public MagicRelativeLayoutWidthOnScreen(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			Initialize();

			ParseAttributes(attrs);
		}

		protected void Initialize()
		{

		}

		protected void ParseAttributes(IAttributeSet attrs)
		{

		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			if (Parent != null && Parent is IWidthOnScreen)
			{
				WidthOnScreen = ((IWidthOnScreen)Parent).WidthOnScreen;

				widthMeasureSpec = MeasureSpec.MakeMeasureSpec(WidthOnScreen, MeasureSpecMode.Exactly);
			}

			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
		}
	}
}