using System;
using Android.Util;
using Android.Runtime;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Content.Res;
using Android.Views.InputMethods;
using Android.App;
using Android.Widget;
using Announcement.Core;
using Android.OS;
using Android.Gms.Analytics;

namespace Announcement.Android
{
    public class SpammersFragment : BaseFragment
    {
        private SpammersMainViewModel ViewModel 
        { 
            get 
            { 
                return SpammersMainViewModel.Instance; 
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.spammers_layout, null);

            spammersListView = view.FindViewById<ListView> (Resource.Id.SpammersListView);

            spammersListView.Adapter = new SpammersAdapter(Activity, ViewModel.Spammers);

            spammersListView.ItemClick += SpammersListView_ItemClick;

            spammersSwipeRefresh = view.FindViewById<global::Android.Support.V4.Widget.SwipeRefreshLayout> (Resource.Id.SpammersSwipeRefresh);

            spammersSwipeRefresh.Refresh += SpammersListViewRefreshLayout_Refresh;

			MainActivity.GATracker.SetScreenName ("Spammers Fragment");
			MainActivity.GATracker.Send (new HitBuilders.ScreenViewBuilder ().Build ());

			if(NavigationManager.CurrentHeader != null && NavigationManager.CurrentHeader is HeaderFragment)
				(NavigationManager.CurrentHeader as HeaderFragment).ShowBack (true);
			
            return view;
        }

        protected void SpammersListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
            ViewModel.InitializeAudioRecordForSpammer(((SpammersAdapter)spammersListView.Adapter).GetItem(e.Position), InitializeAudioRecordForSpammerCallback);
        }

        protected void InitializeAudioRecordForSpammerCallback ()
        {
            NavigationManager.Forward(typeof(AddAudioFileFragment));
        }

        protected void SpammersListViewRefreshLayout_Refresh (object sender, EventArgs e)
        {
            ViewModel.RefreshSpammers(RefreshSpammersCallback);
        }

        protected void RefreshSpammersCallback()
        {
            spammersSwipeRefresh.Refreshing = false;

            ((SpammersAdapter)spammersListView.Adapter).UpdateAll(ViewModel.Spammers);
        }

        private ListView spammersListView;

        private global::Android.Support.V4.Widget.SwipeRefreshLayout spammersSwipeRefresh;
    }
}


