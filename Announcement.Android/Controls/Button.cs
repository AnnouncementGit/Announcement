using System;
using Android.Util;
using Android.Runtime;
using Android.Content;

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
            
        protected void ParseAttributes(IAttributeSet attrs)
        {
            using (var attributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.button))
            {
                SetText(LocalizationModule.Translate(attributes.GetString(Resource.Styleable.button_localizationKey)), BufferType.Normal); 

                attributes.Recycle();
            }
        }

    }
}

