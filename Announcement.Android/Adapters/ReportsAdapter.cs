using System;
using Announcement.Core;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Announcement.Android.Controls;
using Android.Animation;
using Android.Views.Animations;

namespace Announcement.Android
{
    public class ReportsAdapter : MonoAdapter<Report>
    {
        public ReportsAdapter(Context context, List<Report> objects) : base(context, objects)
        {
          
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ReportsAdapterItemHolder holder = null;

            var item = GetItem(position);

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.validation_list_item_template, null);

                holder = new ReportsAdapterItemHolder();

                holder.txtTitle = convertView.FindViewById<TextView>(Resource.Id.Title);

                convertView.Tag = holder;
            }
            else
            {
                holder = (ReportsAdapterItemHolder)convertView.Tag;
            }

            var phoneNumber = item.PhoneNumber;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                phoneNumber = LocalizationModule.Translate("title_phone_number_not_recognized");
            }

            holder.txtTitle.Text =  phoneNumber;

            holder.position = position;

            return convertView;
        }
    }
            
    public class ReportsAdapterItemHolder : Java.Lang.Object
    {
        public TextView txtTitle;

        public int position;
    }
}

