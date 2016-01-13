using System;
using Android.Util;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Graphics;
using Android.Views;

namespace Announcement.Android.Controls
{
	public class MagicHorizontalScrollView : HorizontalScrollView, IWidthOnScreen
	{
        public bool IsFromFullScroll { get; set; }

        public InterceptedSwipeRefreshLayout SwipeRefreshLayout { get; set; }

		public SlideStates SlideState { get; set; }

		public int WidthOnScreen { get; set; }

		public MagicHorizontalScrollView(Context context): base(context)
		{
			Initialize();
		}

		public MagicHorizontalScrollView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Initialize();
		}

		public MagicHorizontalScrollView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialize();

			ParseAttributes(attrs);
		}

		public MagicHorizontalScrollView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			Initialize();

			ParseAttributes(attrs);
		}

		protected void Initialize()
		{
			SlideState = SlideStates.Right;
		}

		protected void ParseAttributes(IAttributeSet attrs)
		{

		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			WidthOnScreen = MeasureSpec.GetSize(widthMeasureSpec);

			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
            IsFromFullScroll = false;

			switch (e.Action)
			{    
			case MotionEventActions.Down:
				pressedX = e.GetX();
				break;
			case MotionEventActions.Up:

				if (isInSliding)
				{
					isInSliding = false;

					if (SwipeRefreshLayout != null)
					{
						SwipeRefreshLayout.IsIntercept = false;
					}

					if ((e.GetX() - pressedX) > 0)
					{
						if (CanScrollHorizontally(-1))
						{
							FullScroll(FocusSearchDirection.Left);

                                IsFromFullScroll = true;

							return true;
						}
					}
					else
					{
						if (CanScrollHorizontally(1))
						{
							FullScroll(FocusSearchDirection.Right);

                                IsFromFullScroll = true;

							return true;
						}
					}
				}
				else
				{
					isInSliding = false;

					if (SwipeRefreshLayout != null)
					{
						SwipeRefreshLayout.IsIntercept = false;
					}

					var listView = Parent as ListView;

					if (listView == null)
					{
						listView = Parent.Parent as ListView;
					}

					var holder = Tag as ICollectionItemHolder;

					if (listView != null && holder != null)
					{
						listView.PerformItemClick(this, holder.Position, holder.Position);
					}

				}

				break;
			}

			return base.OnTouchEvent(e);
		}
            
		protected override void OnScrollChanged(int l, int t, int oldl, int oldt)
		{
			base.OnScrollChanged(l, t, oldl, oldt);

            if (!IsFromFullScroll)
			{
				isInSliding = true;

				if (SwipeRefreshLayout != null)
				{
					SwipeRefreshLayout.IsIntercept = true;
				}
			}
		}

		private bool isInSliding;

		private float pressedX;
	}

	public interface IWidthOnScreen
	{
		int WidthOnScreen { get; set; }
	}

	public enum SlideStates
	{
		Left,
		Right,
	}

	public interface ICollectionItemHolder
	{
		int Position { get; set; }
	}
}