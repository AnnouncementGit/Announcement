﻿using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Announcement.Core;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;

namespace Announcement.Android
{
    public class AdminMainFragment : Fragment
    {
        private AdminMainViewModel ViewModel 
        { 
            get 
            { 
				return AdminMainViewModel.Instance; 
            }
		}

		TabHostContentFactory tabHostContentFactory;
		View ratingTabView;
		LayoutInflater Inflater;
		RadioGroup ratingRadioButtons;
		ListView ratingUsersTabList;
		ListView ratingSpammersTabList;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			//ViewModel.Initialize ();
			Inflater = inflater;
            var view = inflater.Inflate(Resource.Layout.admin_main_layout, null);

			var tabHost = view.FindViewById<TabHost> (Android.Resource.Id.tabHost);
			tabHost.Setup ();

			TabHost.TabSpec tabSpec;
			tabHostContentFactory = new TabHostContentFactory (inflater);
			tabHostContentFactory.SetContentCreatedListener(TabHostContentFactoryOnContentCreated);

			tabSpec = tabHost.NewTabSpec ("moderatorsTab");
			tabSpec.SetContent (tabHostContentFactory);
			var tab0view = inflater.Inflate (Resource.Layout.tab_indicator, null);
			tab0view.FindViewById<TextView> (Resource.Id.indicatorTextView).Text = "Moderators";
			tabSpec.SetIndicator (tab0view);
			tabHost.AddTab (tabSpec);

			tabSpec = tabHost.NewTabSpec ("validationTab");
			tabSpec.SetContent (tabHostContentFactory);
			var tab1view = inflater.Inflate (Resource.Layout.tab_indicator, null);
			tab1view.FindViewById<TextView> (Resource.Id.indicatorTextView).Text = "Validation";
			tabSpec.SetIndicator (tab1view);
			tabHost.AddTab (tabSpec);

			tabSpec = tabHost.NewTabSpec ("ratingTab");
			tabSpec.SetContent (tabHostContentFactory);
			var tab2view = inflater.Inflate (Resource.Layout.tab_indicator, null);
			tab2view.FindViewById<TextView> (Resource.Id.indicatorTextView).Text = "Rating";
			tabSpec.SetIndicator(tab2view);
			tabHost.AddTab(tabSpec);

			var tabChangeListener = new AnimatedTabHostListener (tabHost);

            return view;
		}

		void OnMenuItemClick (object sender, PopupMenu.MenuItemClickEventArgs e)
		{
			switch (e.Item.ItemId) {

			case 0:
				NavigationManager.Forward(typeof(CreateModeratorFragment));
				break;
			case 1:
				NavigationManager.Forward (typeof(LoginFragment));
				break;
			}
		}

		void TabHostContentFactoryOnContentCreated (string tag)
		{
    		switch (tag) {
			case "moderatorsTab":
				CreateModeratorsList ();
				break;

			case "ratingTab":
				ratingTabView = tabHostContentFactory.RatingTabView;

				CreateSpamersList ();
				CreateUsersList ();

				ratingRadioButtons = ratingTabView.FindViewById<RadioGroup> (Resource.Id.ratingRadioGroup);
				ratingRadioButtons.CheckedChange += RatingRadioButtonsOnCheckedChange;
				ratingRadioButtons.Check (Resource.Id.btnItems);

				break;

			case "validationTab":
				CreateValidationList();
				break;
			}
		}

		void CreateValidationList()
		{
			var validationList = tabHostContentFactory.ValidationListView;

			List<ListTwoDataHolder> valList;
			if (ViewModel.Moderators != null)
				valList = ViewModel.Reports.Select (i => new ListTwoDataHolder () {
					Title = i.PhoneNumber,
					Description = i.Id
				}).ToList ();
			else {
				valList = new List<ListTwoDataHolder> ();
			}

			var validationListAdapter = new ListViewTwoAdapter (Inflater, valList);
			validationList.Adapter = validationListAdapter;
			validationList.ItemClick += ValidationListOnItemClick;
		}

		void CreateModeratorsList()
		{
			var moderatorsList = tabHostContentFactory.ModeratorsListView;
			List<ListTwoDataHolder> modList;
			if (ViewModel.Moderators != null)
				modList = ViewModel.Moderators.Select (i => new ListTwoDataHolder () {
					Title = i.Username,
					Description = i.Id
				}).ToList ();
			else
				modList = new List<ListTwoDataHolder> ();
			
			var moderatorsTabListAdapter = new ListViewTwoAdapter (Inflater, modList);
			moderatorsList.Adapter = moderatorsTabListAdapter;
			moderatorsList.ItemClick += ModeratorsTabListViewOnItemClick;
		}

		void CreateSpamersList()
		{
			ratingSpammersTabList = ratingTabView.FindViewById<ListView> (Resource.Id.spammersListView);
			List<ListTwoDataHolder> spammersList;
			if (ViewModel.RatingTopSpammers != null)
				spammersList = ViewModel.RatingTopSpammers.Select (i => new ListTwoDataHolder () {
					Title = i.PhoneNumber,
					Description = i.Id
				}).ToList ();
			else 
				spammersList = new List<ListTwoDataHolder> ();

			var ratingSpammersTabListAdapter = new ListViewTwoAdapter (Inflater, spammersList);
			ratingSpammersTabList.Adapter = ratingSpammersTabListAdapter;
			ratingSpammersTabList.ItemClick += RatingSpammersTabListViewOnItemClick;
		}

		void CreateUsersList()
		{
			ratingUsersTabList = ratingTabView.FindViewById<ListView> (Resource.Id.usersListView);
			List<ListOneDataHolder> usersList;
			if (ViewModel.RatingTopUsers != null)
				usersList = ViewModel.RatingTopSpammers.Select (i => new ListOneDataHolder () {
					Title = i.PhoneNumber,
					Posts = "10234",
					Confirmed = "8566"
				}).ToList ();
			else 
				usersList = new List<ListOneDataHolder> ();
			
			var ratingUsersTabListAdapter = new ListViewOneAdapter (Inflater, usersList);
			ratingUsersTabList.Adapter = ratingUsersTabListAdapter;
			ratingUsersTabList.ItemClick += RatingUsersTabListViewOnItemClick;
		}

		void RatingRadioButtonsOnCheckedChange (object sender, RadioGroup.CheckedChangeEventArgs e)
		{
			switch (e.CheckedId) 
			{
			case (Resource.Id.btnItems):
				ratingUsersTabList.Visibility = ViewStates.Gone;
				ratingSpammersTabList.Visibility = ViewStates.Visible;
				break;

			case (Resource.Id.btnUsers):
				ratingSpammersTabList.Visibility = ViewStates.Gone;
				ratingUsersTabList.Visibility = ViewStates.Visible;
				break;
			}
		}

		void ModeratorsTabListViewOnItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
		}

		void RatingSpammersTabListViewOnItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{

		}

		void RatingUsersTabListViewOnItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{

		}

		void ValidationListOnItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			ViewModel.InitializeReportValidation (ViewModel.Reports [e.Position], () => {
				NavigationManager.Forward (typeof(ReportValidationFragment));
			});
		}
    }
}


