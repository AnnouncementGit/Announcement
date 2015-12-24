using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Announcement.Core;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using System;

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
            
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.admin_main_layout, null);


			tabHost = view.FindViewById<TabHost> (Android.Resource.Id.tabHost);

			tabHost.Setup ();

			tabHostContentFactory = new TabHostContentFactory (inflater);

			tabHostContentFactory.SetContentCreatedListener(TabHostContentFactoryOnContentCreated);

            tabHost.TabChanged += TabHost_TabChanged;


            AddTab(LocalizationModule.Translate("tab_title_moderators"), MODERATORS_TAB_TAG);

            AddTab(LocalizationModule.Translate("tab_title_validation"), VALIDATION_TAB_TAG);

            AddTab(LocalizationModule.Translate("tab_title_ratings"), RATING_TAB_TAG);


			tabChangeListener = new AnimatedTabHostListener (tabHost);

            return view;
		}
              
        protected void AddTab(string title, string tag)
        {
            var tabSpec = tabHost.NewTabSpec (tag);

            tabSpec.SetContent (tabHostContentFactory);

            var view = Activity.LayoutInflater.Inflate (Resource.Layout.tab_indicator, null);

            view.FindViewById<TextView> (Resource.Id.indicatorTextView).Text = title;

            tabSpec.SetIndicator(view);

            tabHost.AddTab(tabSpec); 
        }

        protected void TabHost_TabChanged (object sender, TabHost.TabChangeEventArgs e)
        {
            var view = tabHostContentFactory.ModeratorsListView;

            if (view != null)
            {
                view.ScrollStateChanged -= View_ScrollStateChanged;

                view.ScrollStateChanged += View_ScrollStateChanged;

                view.VerticalScrollBarEnabled = false;
            }

            view = tabHostContentFactory.ValidationListView;

            if (view != null)
            {
                view.ScrollStateChanged -= View_ScrollStateChanged;

                view.ScrollStateChanged += View_ScrollStateChanged;
                
                view.VerticalScrollBarEnabled = false;
            }

            view = ratingUsersTabList;

            if (view != null)
            {
                view.ScrollStateChanged -= View_ScrollStateChanged;

                view.ScrollStateChanged += View_ScrollStateChanged;
                
                view.VerticalScrollBarEnabled = false;
            }

            view = ratingSpammersTabList;

            if (view != null)
            {
                view.ScrollStateChanged -= View_ScrollStateChanged;

                view.ScrollStateChanged += View_ScrollStateChanged;
                
                view.VerticalScrollBarEnabled = false;
            }
        }

        protected void View_ScrollStateChanged (object sender, AbsListView.ScrollStateChangedEventArgs e)
        {
            var view = sender as View;
            
            switch (e.ScrollState)
            {
                case ScrollState.TouchScroll:
                    
                    if (!view.VerticalScrollBarEnabled)
                    {
                        view.VerticalScrollBarEnabled = true;
                    }
                    break;
            }
        }
                 
        protected void TabHostContentFactoryOnContentCreated (string tag)
        {
            switch (tag)
            {
                case MODERATORS_TAB_TAG:
                    InitializeModerators();
                    break;

                case VALIDATION_TAB_TAG:
                    InitializeValidation();
                    break;

                case RATING_TAB_TAG:
                    ratingTabView = tabHostContentFactory.RatingTabView;

                    InitializeSpamers();
                    InitializeUsers();

                    ratingRadioButtons = ratingTabView.FindViewById<RadioGroup>(Resource.Id.ratingRadioGroup);
                    ratingRadioButtons.CheckedChange += RatingRadioButtonsOnCheckedChange;
                    ratingRadioButtons.Check(Resource.Id.btnItems);
                    break;
            }
        }

        protected void InitializeModerators()
        {
            var moderatorsList = tabHostContentFactory.ModeratorsListView;

            moderatorsList.Adapter = new ModeratorsAdapter(Activity, ViewModel.Moderators);

            moderatorsList.ItemClick += ModeratorsTabListViewOnItemClick;
        }

        protected void InitializeValidation()
		{
            var source = ViewModel.Reports.Select(i => new ListTwoDataHolder { Title = i.PhoneNumber, Description = i.Id }).ToList();

            var validationList = tabHostContentFactory.ValidationListView;

            validationList.Adapter = new ListViewTwoAdapter (Activity.LayoutInflater, source);

            validationList.ItemClick += ValidationListOnItemClick;
		}
            
        protected void InitializeSpamers()
		{
            var	source = ViewModel.RatingTopSpammers.Select(i => new ListTwoDataHolder { Title = i.PhoneNumber, Description = i.Id }).ToList();

            ratingSpammersTabList = ratingTabView.FindViewById<ListView> (Resource.Id.spammersListView);

            ratingSpammersTabList.Adapter = new ListViewTwoAdapter (Activity.LayoutInflater, source);

			ratingSpammersTabList.ItemClick += RatingSpammersTabListViewOnItemClick;
		}

        protected void InitializeUsers()
		{
            var source = ViewModel.RatingTopSpammers.Select(i => new ListOneDataHolder { Title = i.PhoneNumber, Posts = "10234", Confirmed = "8566" }).ToList();

			ratingUsersTabList = ratingTabView.FindViewById<ListView> (Resource.Id.usersListView);

            ratingUsersTabList.Adapter = new ListViewOneAdapter (Activity.LayoutInflater, source);;

			ratingUsersTabList.ItemClick += RatingUsersTabListViewOnItemClick;
		}

        protected void RatingRadioButtonsOnCheckedChange (object sender, RadioGroup.CheckedChangeEventArgs e)
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

        protected void ModeratorsTabListViewOnItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
            
		}

        protected void RatingSpammersTabListViewOnItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{

		}

        protected void RatingUsersTabListViewOnItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{

		}

        protected void ValidationListOnItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
            ViewModel.InitializeReportValidation(ViewModel.Reports[e.Position], () => NavigationManager.Forward(typeof(ReportValidationFragment)));
		}

        private TabHostContentFactory tabHostContentFactory;

        private View ratingTabView;

        private RadioGroup ratingRadioButtons;

        private ListView ratingUsersTabList;

        private ListView ratingSpammersTabList;

        private TabHost tabHost;

        private AnimatedTabHostListener tabChangeListener;

        private const string MODERATORS_TAB_TAG = "moderatorsTab";

        private const string VALIDATION_TAB_TAG = "validationTab";

        private const string RATING_TAB_TAG = "ratingTab";
    }
}


