using System;
using Android.Util;
using Android.Runtime;
using Android.Content;

namespace Announcement.Android.Controls
{
    public class RadioButton : global::Android.Widget.RadioButton
    {
		public RadioButton(Context context) : base(context)
        {
            Initialize();
        }

		public RadioButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

		public RadioButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();

            ParseAttributes(attrs);
        }

		public RadioButton(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
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
                SetText(LocalizationModule.Translate(attributes.GetString(Resource.Styleable.localization_localizationKey)), BufferType.Normal); 

                attributes.Recycle();
            }
        }

    }
}

