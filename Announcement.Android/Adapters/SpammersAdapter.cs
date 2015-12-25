﻿using System;
using Announcement.Core;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Announcement.Android.Controls;
using Android.Animation;
using Android.Views.Animations;

namespace Announcement.Android
{
    public class SpammersAdapter : MonoAdapter<Spammer>
    {
        public SpammersAdapter(Context context, List<Spammer> objects) : base(context, objects)
        {
            
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            SpammersAdapterItemHolder holder = null;

            var item = GetItem(position);

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.spammers_list_item_template, null);

                holder = new SpammersAdapterItemHolder();

                holder.txtTitle = convertView.FindViewById<TextView>(Resource.Id.Title);

                holder.txtComplaints = convertView.FindViewById<TextView>(Resource.Id.Complaints);

                convertView.Tag = holder;
            }
            else
            {
                holder = (SpammersAdapterItemHolder)convertView.Tag;
            }
                

            holder.txtTitle.Text =  item.PhoneNumber;

            holder.txtComplaints.Text = item.Complaints.ToString();

            holder.position = position;

            return convertView;
        }
    }
            
    public class SpammersAdapterItemHolder : Java.Lang.Object
    {
        public TextView txtTitle;

        public TextView txtComplaints;

        public int position;
    }
}
