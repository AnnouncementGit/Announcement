using System;
using Android.Util;
using Android.Runtime;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Content.Res;
using Android.Views.InputMethods;
using Android.App;
using Android.Widget;
using Android.Animation;

namespace Announcement.Android.Controls
{
    public class ScaleImageWithProgress : FrameLayout
    {
        public ScaleImageView Image
        {
            get
            {
                return image;
            }
        }

        public bool IsShowProgress
        {
            get
            {
                return progressBar.Visibility == ViewStates.Visible;
            }
            set
            {
                if (value)
                {
                    progressBar.Enabled = true;
                    
                    progressBar.Visibility = ViewStates.Visible;

                    image.Visibility = ViewStates.Gone;
                }
                else
                {
                    progressBar.Visibility = ViewStates.Gone;

                    progressBar.Enabled = false;

                    image.Visibility = ViewStates.Visible;
                }
            }
        }
        
        public ScaleImageWithProgress(Context context) : base(context)
        {
            Initialize();
        }

        public ScaleImageWithProgress(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        public ScaleImageWithProgress(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public ScaleImageWithProgress(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Initialize();
        }

        protected void Initialize()
        {
            var layoutTransition = new LayoutTransition();

            layoutTransition.EnableTransitionType(LayoutTransitionType.Appearing);

            layoutTransition.EnableTransitionType(LayoutTransitionType.Disappearing);

            LayoutTransition = layoutTransition;

            
            image = new ScaleImageView(Context);
           
            image.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

            image.SetScaleType(ImageView.ScaleType.FitCenter);

            image.Visibility = ViewStates.Gone;

            AddView(image);


            progressBar = new ProgressBar(Context);

            progressBar.LayoutParameters = new LayoutParams(ValueConverterManager.DpToPx(50),ValueConverterManager.DpToPx(50));

            ((LayoutParams)progressBar.LayoutParameters).Gravity = GravityFlags.Center;

            progressBar.Visibility = ViewStates.Gone;

            AddView(progressBar);
        }

        private ScaleImageView image;

        private ProgressBar progressBar;
    }
}

