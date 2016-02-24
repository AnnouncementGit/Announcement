using System;
using Android.Util;
using Android.Runtime;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Content.Res;
using Android.Views.InputMethods;
using Android.App;

namespace Announcement.Android.Controls
{
    public class Button : global::Android.Widget.Button
    {
        public Button(Context context) : base(context)
        {
            Initialize();
        }

        public Button(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        public Button(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();

            ParseAttributes(attrs);
        }

        public Button(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Initialize();

            ParseAttributes(attrs);
        }

        protected void Initialize()
        {
       
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (notIgnoreLandscape)
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec); 
            }
            else
            {
                if (Resources.Configuration.Orientation == Orientation.Landscape)
                {
                    var widthMode = MeasureSpec.GetMode(widthMeasureSpec);

                    var layouts = (ViewGroup.MarginLayoutParams)LayoutParameters;

                    var widthSize = Resources.DisplayMetrics.HeightPixels - layouts.LeftMargin - layouts.RightMargin;

                    base.OnMeasure(MeasureSpec.MakeMeasureSpec(widthSize, widthMode), heightMeasureSpec); 
                }
                else
                {
                    base.OnMeasure(widthMeasureSpec, heightMeasureSpec); 
                }
            }
        }

        public override bool PerformClick()
        {
            var result = base.PerformClick();

            var inputMethodManager = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);

            inputMethodManager.HideSoftInputFromWindow(WindowToken, 0);

            return result;
        }
            
        protected void ParseAttributes(IAttributeSet attrs)
        {
            using (var attributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.localization))
            {
				string text = attributes.GetString (Resource.Styleable.localization_localizationKey);

				if(!string.IsNullOrWhiteSpace(text))
					SetText(LocalizationModule.Translate(text), BufferType.Normal); 

                attributes.Recycle();
            }

            using (var attributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.font))
            {
                var fontName = attributes.GetString(Resource.Styleable.font_fontName);

                if (!string.IsNullOrEmpty(fontName))
                {
                    SetTypeface(FontsManager.LoadByName(fontName), TypefaceStyle.Normal);
                }
                attributes.Recycle();
            }
            using (var attributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.padding_fix))
            {
                var paddingLeft = (int)attributes.GetDimension(Resource.Styleable.padding_fix_android_paddingLeft, 0);

                var paddingTop = (int)attributes.GetDimension(Resource.Styleable.padding_fix_android_paddingTop, 0);

                var paddingRight = (int)attributes.GetDimension(Resource.Styleable.padding_fix_android_paddingRight, 0);

                var paddingBottom = (int)attributes.GetDimension(Resource.Styleable.padding_fix_android_paddingBottom, 0);

                var padding = (int)attributes.GetDimension(Resource.Styleable.padding_fix_android_padding, 0);

                if (padding > 0)
                {
                    SetPadding(padding, padding, padding, padding);
                }
                else
                {
                    SetPadding(paddingLeft, paddingTop, paddingRight, paddingBottom); 
                }

                attributes.Recycle();
            }

            using (var attributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.ignore_landscape))
            {
                notIgnoreLandscape = attributes.GetBoolean(Resource.Styleable.ignore_landscape_notIgnoreLandscape, false);

                attributes.Recycle();
            }
        }

        private bool notIgnoreLandscape;
    }
}

