using Android.OS;
using Android.Views;
using Android.Widget;
using Announcement.Core;
using Android.Support.V4.App;
using Android.Graphics;
using System.Collections.Generic;
using Android.Views.Animations;
using System;
using Android.Graphics.Drawables;
using Android.Animation;

namespace Announcement.Android
{
        public class ModeratorMainFragment : Fragment
        {
            private ModeratorMainViewModel ViewModel 
            { 
                get 
                { 
                    return ModeratorMainViewModel.Instance; 
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
                var view = tabHostContentFactory.ValidationListView;

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

            ratingSpammersTabList.Adapter = new SpammersAdapter (Activity, ViewModel.RatingTopSpammers);

            tabHostContentFactory.spammersSwipeRefresh.Refresh += RatingSwipeRefresh_Refresh;

                //ratingSpammersTabList.ItemClick += RatingSpammersTabListViewOnItemClick;
        }

        protected void InitializeUsers()
        {
            ratingUsersTabList = ratingTabView.FindViewById<ListView> (Resource.Id.usersListView);

            ratingUsersTabList.Adapter = new UsersAdapter (Activity, ViewModel.RatingTopUsers);;

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

            ((SpammersAdapter)ratingSpammersTabList.Adapter).UpdateAll(ViewModel.RatingTopSpammers);

            ((UsersAdapter)ratingUsersTabList.Adapter).UpdateAll(ViewModel.RatingTopUsers);
        }

            protected void RatingRadioButtonsOnCheckedChange (object sender, RadioGroup.CheckedChangeEventArgs e)
            {
                switch (e.CheckedId)
                {
                    case (Resource.Id.btnSpammers):

                        if (ratingsViewSwitcher.CurrentView != ratingSpammersTabList)
                        {
                            ratingsViewSwitcher.ShowPrevious();
                        }

                        break;

                    case (Resource.Id.btnUsers):

                        if (ratingsViewSwitcher.CurrentView != ratingUsersTabList)
                        {
                            ratingsViewSwitcher.ShowNext();
                        }

                        break;
                }
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

            private const string VALIDATION_TAB_TAG = "validationTab";

            private const string RATING_TAB_TAG = "ratingTab";
        }

	public class TabHostContentFactory : Java.Lang.Object, TabHost.ITabContentFactory
	{
		private LayoutInflater inflater;
		public ListView ValidationListView;
		public ListView ModeratorsListView;
        public Announcement.Android.Controls.InterceptedSwipeRefreshLayout moderatorsSwipeRefresh;
        public global::Android.Support.V4.Widget.SwipeRefreshLayout reportsSwipeRefresh;
        public global::Android.Support.V4.Widget.SwipeRefreshLayout spammersSwipeRefresh;
        public global::Android.Support.V4.Widget.SwipeRefreshLayout usersSwipeRefresh;
		public View RatingTabView;
		private Action<string> contentCreatedAction;

		public TabHostContentFactory(LayoutInflater inflater)
		{
			this.inflater = inflater;
		}

		public View CreateTabContent (string tag)
		{
			switch (tag) {
                case "moderatorsTab":

                    moderatorsSwipeRefresh = new Announcement.Android.Controls.InterceptedSwipeRefreshLayout(MainActivityInstance.Current) { LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent) };

                    ModeratorsListView = new ListView(MainActivityInstance.Current){ LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent) };
                    ModeratorsListView.DividerHeight = 0;
                    ModeratorsListView.Divider = null;
                    ModeratorsListView.Selector = new ColorDrawable(Color.Transparent);

                    if (contentCreatedAction != null)
                    {
                        contentCreatedAction.Invoke(tag);
                    }

                    moderatorsSwipeRefresh.AddView(ModeratorsListView);

                    return moderatorsSwipeRefresh;

                case "validationTab":

                    reportsSwipeRefresh = new global::Android.Support.V4.Widget.SwipeRefreshLayout(MainActivityInstance.Current) { LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent) };

                    ValidationListView = new ListView(MainActivityInstance.Current){ LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent) };
                    ValidationListView.DividerHeight = 0;
                    ValidationListView.Divider = null;
                    ValidationListView.SetSelector(Resource.Drawable.list_item_selector);

                    if (contentCreatedAction != null)
                    {
                        contentCreatedAction.Invoke(tag);
                    }

                    reportsSwipeRefresh.AddView(ValidationListView);

                    return reportsSwipeRefresh;

                case "ratingTab":
                    
                    RatingTabView = inflater.Inflate(Resource.Layout.tab_rating, null);

                    spammersSwipeRefresh = RatingTabView.FindViewById<global::Android.Support.V4.Widget.SwipeRefreshLayout>(Resource.Id.SpammersSwipeRefresh);

                    usersSwipeRefresh = RatingTabView.FindViewById<global::Android.Support.V4.Widget.SwipeRefreshLayout>(Resource.Id.UsersSwipeRefresh);

                    if (contentCreatedAction != null)
                    {
                        contentCreatedAction.Invoke(tag);
                    }

				return RatingTabView;

			default:
				return null;
			};
		}

		public void SetContentCreatedListener(Action<string> listener)
		{
			contentCreatedAction = listener;
		}
	}

	public class AnimatedTabHostListener : Java.Lang.Object, global::Android.Widget.TabHost.IOnTabChangeListener
	{
		public static int ANIMATION_TIME = 150;
		private TabHost tabHost;
		private View previousView;
		private View currentView;
		private int currentTab;

		public AnimatedTabHostListener(TabHost tabHost)
		{
			this.tabHost = tabHost;

			this.previousView = tabHost.CurrentView;

            tabHost.TabChanged += (sender, e) => OnTabChanged(e.TabId);
		}

		public void OnTabChanged(string tabId)
		{
			currentView = tabHost.CurrentView;

			if (tabHost.CurrentTab > currentTab)
			{
				previousView.Animation = (outToLeftAnimation());

				currentView.Animation = (inFromRightAnimation());
			}
			else
			{
				previousView.Animation = (outToRightAnimation());

				currentView.Animation = (inFromLeftAnimation());
			}

			previousView = currentView;

			currentTab = tabHost.CurrentTab;
		}

		private Animation inFromRightAnimation()
		{
            var animation = new TranslateAnimation(Dimension.RelativeToParent, 1.0f, Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f);

            return setProperties(animation);
		}

		private Animation outToRightAnimation()
		{
            var animation = new TranslateAnimation(Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 1.0f, Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f);

            return setProperties(animation);
		}

		private Animation inFromLeftAnimation()
		{
			var animation = new TranslateAnimation(Dimension.RelativeToParent, -1.0f, Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f);

            return setProperties(animation);
		}
            
		private Animation outToLeftAnimation()
		{
            var animation = new TranslateAnimation(Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, -1.0f, Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f);

            return setProperties(animation);
		}

		private Animation setProperties(Animation animation)
		{
			animation.Duration = (ANIMATION_TIME);

			animation.Interpolator = (new AccelerateInterpolator());

			return animation;
		}
	}
}