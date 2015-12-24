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
    public class UsersAdapter : MonoAdapter<User>
    {
        public UsersAdapter(Context context, List<User> objects) : base(context, objects)
        {
            
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            UsersAdapterItemHolder holder = null;

            var item = GetItem(position);

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.users_list_item_template, null);

                holder = new UsersAdapterItemHolder();

                holder.txtTitle = convertView.FindViewById<TextView>(Resource.Id.Title);

                holder.txtReports = convertView.FindViewById<TextView>(Resource.Id.Reports);

                holder.txtConfirmed = convertView.FindViewById<TextView>(Resource.Id.Confirmed);

                convertView.Tag = holder;
            }
            else
            {
                holder = (UsersAdapterItemHolder)convertView.Tag;
            }
                

            holder.txtTitle.Text =  item.Username;

            holder.txtReports.Text = item.Reports.ToString();

            holder.txtConfirmed.Text = item.ConfirmedReports.ToString();

            holder.position = position;

            return convertView;
        }
    }
            
    public class UsersAdapterItemHolder : Java.Lang.Object
    {
        public TextView txtTitle;

        public TextView txtReports;

        public TextView txtConfirmed;

        public int position;
    }
}

