using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Announcement.Core;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using System;
using Android.Views.Animations;
using Android.Gms.Analytics;

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

        public static int StartTabIndex { get; set; }
           
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

            tabHost.CurrentTab = StartTabIndex;

			tabChangeListener = new AnimatedTabHostListener (tabHost);

			MainActivity.GATracker.SetScreenName ("Admin Main Fragment");
			MainActivity.GATracker.Send (new HitBuilders.ScreenViewBuilder ().Build ());

			if(NavigationManager.CurrentHeader != null && NavigationManager.CurrentHeader is HeaderFragment)
				(NavigationManager.CurrentHeader as HeaderFragment).ShowBack (false);
			
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

                    ratingsViewSwitcher = ratingTabView.FindViewById<ViewSwitcher>(Resource.Id.RatingsViewSwitcher);

                    ratingsViewSwitcher.SetInAnimation(Activity, Resource.Animation.fade_in_animation);

                    ratingsViewSwitcher.SetOutAnimation(Activity, Resource.Animation.fade_out_animation);

                    InitializeSpamers();
                    InitializeUsers();

                    ratingRadioButtons = ratingTabView.FindViewById<RadioGroup>(Resource.Id.ratingRadioGroup);
                    ratingRadioButtons.CheckedChange += RatingRadioButtonsOnCheckedChange;
                    ratingRadioButtons.Check(Resource.Id.btnSpammers);
                    break;
            }
        }

        protected void InitializeModerators()
        {
            var moderatorsList = tabHostContentFactory.ModeratorsListView;

            var adapter = new ModeratorsAdapter(Activity, ViewModel.Moderators, tabHostContentFactory.moderatorsSwipeRefresh, moderatorsList);

            adapter.ItemDeleteClick = (item, view) =>
            {
                ViewModel.DeleteModerator(item, (moderator, result) =>
                    {
                        if (result)
                        {
                            adapter.Remove(item, view);
                        }
                    });
            };

            moderatorsList.Adapter = adapter;

            tabHostContentFactory.moderatorsSwipeRefresh.Refresh += ModeratorsSwipeRefresh_Refresh;

           // moderatorsList.ItemClick += ModeratorsTabListViewOnItemClick;
        }

        protected void ModeratorsSwipeRefresh_Refresh (object sender, EventArgs e)
        {
            ViewModel.RefreshModerators(RefreshModeratorsCallback);
        }

        protected void RefreshModeratorsCallback ()
        {
            tabHostContentFactory.moderatorsSwipeRefresh.Refreshing = false;
            
            ((ModeratorsAdapter)tabHostContentFactory.ModeratorsListView.Adapter).UpdateAll(ViewModel.Moderators);
        }

        protected void InitializeValidation()
		{
            var validationList = tabHostContentFactory.ValidationListView;

            validationList.Adapter = new ReportsAdapter(Activity, ViewModel.Reports);

            tabHostContentFactory.reportsSwipeRefresh.Refresh += ReportsSwipeRefresh_Refresh;

            validationList.ItemClick += ValidationListOnItemClick;
		}

        protected void ReportsSwipeRefresh_Refresh (object sender, EventArgs e)
        {
            ViewModel.RefreshReports(RefreshReportsCallback);
        }

        protected void RefreshReportsCallback ()
        {
            tabHostContentFactory.reportsSwipeRefresh.Refreshing = false;

            ((ReportsAdapter)tabHostContentFactory.ValidationListView.Adapter).UpdateAll(ViewModel.Reports);
        }
            
        protected void InitializeSpamers()
		{
            ratingSpammersTabList = ratingTabView.FindViewById<ListView> (Resource.Id.spammersListView);

            ratingSpammersTabList.Adapter = new SpammersRatingAdapter (Activity, ViewModel.RatingTopSpammers);

            tabHostContentFactory.spammersSwipeRefresh.Refresh += RatingSwipeRefresh_Refresh;

			//ratingSpammersTabList.ItemClick += RatingSpammersTabListViewOnItemClick;
		}

        protected void InitializeUsers()
		{
			ratingUsersTabList = ratingTabView.FindViewById<ListView> (Resource.Id.usersListView);

            ratingUsersTabList.Adapter = new UsersAdapter (Activity, ViewModel.RatingTopUsers);

            tabHostContentFactory.usersSwipeRefresh.Refresh += RatingSwipeRefresh_Refresh;

			//ratingUsersTabList.ItemClick += RatingUsersTabListViewOnItemClick;
		}

        protected void RatingSwipeRefresh_Refresh (object sender, EventArgs e)
        {
            ViewModel.RefreshRatings(RefreshRatingsCallback);
        }

        protected void RefreshRatingsCallback ()
        {
            tabHostContentFactory.spammersSwipeRefresh.Refreshing = false;

            tabHostContentFactory.usersSwipeRefresh.Refreshing = false;

            ((SpammersRatingAdapter)ratingSpammersTabList.Adapter).UpdateAll(ViewModel.RatingTopSpammers);

            ((UsersAdapter)ratingUsersTabList.Adapter).UpdateAll(ViewModel.RatingTopUsers);
        }

        protected void RatingRadioButtonsOnCheckedChange (object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            var swipeRefreshLayout = ratingsViewSwitcher.CurrentView as global::Android.Support.V4.Widget.SwipeRefreshLayout;

            View currentView = null;

            if (swipeRefreshLayout != null && swipeRefreshLayout.ChildCount > 0)
            {
                currentView = swipeRefreshLayout.GetChildAt(0);
            }

            switch (e.CheckedId)
            {
                case (Resource.Id.btnSpammers):

                    if (currentView != ratingSpammersTabList)
                    {
                        ratingsViewSwitcher.ShowPrevious();
                    }

                    break;

                case (Resource.Id.btnUsers):

                    if (currentView != ratingUsersTabList)
                    {
                        ratingsViewSwitcher.ShowNext();
                    }

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

        private ViewSwitcher ratingsViewSwitcher;

        private const string MODERATORS_TAB_TAG = "moderatorsTab";

        private const string VALIDATION_TAB_TAG = "validationTab";

        private const string RATING_TAB_TAG = "ratingTab";
    }
}


