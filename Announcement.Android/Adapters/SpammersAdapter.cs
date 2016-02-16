using System;
using Announcement.Core;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Announcement.Android.Controls;
using Android.Animation;
using Android.Views.Animations;
using System.Threading;
using Android.Locations;
using System.Linq;

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

				holder.txtCity = convertView.FindViewById<TextView>(Resource.Id.City);

                holder.txtComplaints = convertView.FindViewById<TextView>(Resource.Id.Complaints);

                convertView.Tag = holder;
            }
            else
            {
                holder = (SpammersAdapterItemHolder)convertView.Tag;
            }
                

            holder.txtTitle.Text =  item.PhoneNumber;

			SetCityOfSpammer (holder.txtCity, item.Latitude, item.Longitude);

            holder.txtComplaints.Text = item.Complaints.ToString();

            holder.position = position;

            return convertView;
        }

		void SetCityOfSpammer(TextView cityField, double latitude, double longitude)
		{
			IList<Address> address = new List<Address>();
			new Thread (new ThreadStart (() => {
				try {
					var geo = new Geocoder (NavigationManager.CurrentActivity);
					address = geo.GetFromLocation (latitude, longitude, 1);
				} 
				catch (Exception ex) 
				{
					Console.WriteLine(ex.Message);
				}
				finally
				{
					NavigationManager.CurrentActivity.RunOnUiThread (() => {
						if (address != null && address.Any ()) {
							cityField.Text = address [0].Locality;
						}
						else
							cityField.Text = LocalizationModule.Translate("title_city_unknown");
					});
				}
			})).Start ();		
		}
    }
            
    public class SpammersAdapterItemHolder : Java.Lang.Object
    {
        public TextView txtTitle;

		public TextView txtCity;

        public TextView txtComplaints;

        public int position;
    }
}

