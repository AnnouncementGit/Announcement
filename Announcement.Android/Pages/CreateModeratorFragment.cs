using System;
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

			return view;
		}

        protected void BtnAddOnClick (object sender, EventArgs e)
		{
            if (!string.IsNullOrWhiteSpace(LoginEditText.Text) && !string.IsNullOrWhiteSpace(PasswordEditText.Text))
            {
                ViewModel.PushModerator(LoginEditText.Text, PasswordEditText.Text, AddModeratorCallback);
            }
            else
            {
                AlertModule.ShowInformation("Please fill all data.", null);
            }
		}

		protected void AddModeratorCallback()
		{
			
		}
            

        private TextView LoginEditText;

        private TextView PasswordEditText;

        private Button btnAdd;
	}
}

