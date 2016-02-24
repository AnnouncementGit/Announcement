
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Announcement.Core;

namespace Announcement.Android
{
	public class AboutFragment : BaseFragment
	{
		private AboutViewModel ViewModel 
		{ 
			get 
			{ 
				return AboutViewModel.Instance; 
			}
		}

		TextView aboutInfo;
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{			
			var view = inflater.Inflate(Resource.Layout.about_view, null);

			aboutInfo = view.FindViewById<TextView> (Resource.Id.aboutInfo);

			if(NavigationManager.CurrentHeader != null && NavigationManager.CurrentHeader is HeaderFragment)
				(NavigationManager.CurrentHeader as HeaderFragment).ShowBack (true);
			return view;
		}

		public override void OnResume ()
		{
			base.OnResume ();

			if (ViewModel.CurrentPerson != null) {
				string about_text = string.Format (LocalizationModule.Translate ("about_app"), ViewModel.CurrentPerson.City, ViewModel.CurrentPerson.Pib);
				aboutInfo.Text = about_text;
			}
			else
				aboutInfo.Text = string.Empty;
		}
	}
}

