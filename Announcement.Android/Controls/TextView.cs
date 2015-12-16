﻿using System;
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
		}
	}
}

