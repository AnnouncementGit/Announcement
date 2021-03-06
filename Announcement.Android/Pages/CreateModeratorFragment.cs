﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Announcement.Core;
using Android.Support.V4.App;
using Android.Views.InputMethods;
using Android.App;
using Android.Gms.Analytics;

namespace Announcement.Android
{
    public class CreateModeratorFragment : BaseFragment
	{
        private CreateModeratorViewModel ViewModel 
        { 
            get 
            { 
                return CreateModeratorViewModel.Instance; 
            }
        }
          

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.add_new_moderator_layout, null);

			LoginEditText = view.FindViewById<TextView> (Resource.Id.LoginEditText);

			PasswordEditText = view.FindViewById<TextView> (Resource.Id.PasswordEditText);

			btnAdd = view.FindViewById<Button> (Resource.Id.btnAdd);

			btnAdd.Click += BtnAddOnClick;

            view.SetOnTouchListener(this);

			MainActivity.GATracker.SetScreenName ("Create Moderator Fragment");
			MainActivity.GATracker.Send (new HitBuilders.ScreenViewBuilder ().Build ());

			if(NavigationManager.CurrentHeader != null && NavigationManager.CurrentHeader is HeaderFragment)
				(NavigationManager.CurrentHeader as HeaderFragment).ShowBack (true);
			
			return view;
		}

        protected void BtnAddOnClick (object sender, EventArgs e)
		{
            ViewModel.PushModerator(LoginEditText.Text, PasswordEditText.Text, AddModeratorCallback);
		}

		protected void AddModeratorCallback()
		{
            LoginEditText.Text = string.Empty;

            PasswordEditText.Text = string.Empty;
		}
            

        private TextView LoginEditText;

        private TextView PasswordEditText;

        private Button btnAdd;
	}
}

