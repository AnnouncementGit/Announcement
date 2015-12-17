using System;
using Android.Util;
using Android.Runtime;
using Android.Content;
using Android.Graphics;


namespace Announcement.Android.Controls
{
	public class TextView : global::Android.Widget.TextView
	{
		public TextView(Context context) : base(context)
		{
			Initialize();
		}

		public TextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Initialize();
		}

		public TextView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialize();

			ParseAttributes(attrs);
		}

		public TextView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			Initialize();

			ParseAttributes(attrs);
		}

		protected void Initialize()
		{

		}

		protected void ParseAttributes(IAttributeSet attrs)
		{
			using (var attributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.localization))
			{
				Text = LocalizationModule.Translate(attributes.GetString(Resource.Styleable.localization_localizationKey));

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
		}
	}
}

