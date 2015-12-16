using Android.OS;
using Android.Views;
using Android.Widget;
using Announcement.Core;
using Android.Support.V4.App;
using Android.Graphics;
using System.Collections.Generic;
using Android.Views.Animations;
using System;

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

		TabHostContentFactory tabHostContentFactory;
		View ratingSpammersTabView;
		LayoutInflater Inflater;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			Inflater = inflater;
            var view = inflater.Inflate(Resource.Layout.admin_main_layout, null);
			var tabHost = view.FindViewById<TabHost> (Android.Resource.Id.tabHost);
			tabHost.Setup ();

			TabHost.TabSpec tabSpec;
			tabHostContentFactory = new TabHostContentFactory (inflater);
			tabHostContentFactory.SetContentCreatedListener(TabHostContentFactoryOnContentCreated);

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

			tabHost.TabChanged += TabHostOnTabChanged;
			var tabChangeListener = new AnimatedTabHostListener (tabHost);

            return view;
        }

        void TabHostContentFactoryOnContentCreated (string tag)
        {
//			switch (tag) {
//			case "ratingTab":
//				ratingSpammersTabView = tabHostContentFactory.RatingTabView;
//				var ratingSpammersTabList = ratingSpammersTabView.FindViewById<ListView> (Resource.Id.spammersListView);
//				var ratingSpammersTabListAdapter = new ListViewTwoAdapter (Inflater, ViewModel.RatingTopSpammers);
//				ratingSpammersTabList.Adapter = ratingSpammersTabListAdapter;
//				ratingSpammersTabList.ItemClick += RatingTabListViewOnItemClick;
//				break;
//
//			case "validationTab":
//				var validationList = tabHostContentFactory.ValidationListView;
//				var validationListAdapter = new ListViewTwoAdapter (Inflater, ViewModel.Reports);
//				validationList.Adapter = validationListAdapter;
//				validationList.ItemClick += ValidationListOnItemClick;
//				break;
//			}
        }

        void RatingTabListViewOnItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
			
        }

        void ValidationListOnItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
			
        }

        void TabHostOnTabChanged (object sender, TabHost.TabChangeEventArgs e)
        {
			
        }
    }

	class TabHostContentFactory : Java.Lang.Object, TabHost.ITabContentFactory
	{
		private LayoutInflater inflater;
		public ListView ValidationListView;
		public ListView ModeratorsListView;
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
				ModeratorsListView = new ListView (MainActivityInstance.Current){ LayoutParameters = new ViewGroup.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent) };
				ModeratorsListView.DividerHeight = 0;
				if (contentCreatedAction != null)
					contentCreatedAction.Invoke (tag);
				return ModeratorsListView;

			case "validationTab":
				ValidationListView = new ListView (MainActivityInstance.Current){ LayoutParameters = new ViewGroup.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent) };
				ValidationListView.DividerHeight = 0;
				if (contentCreatedAction != null)
					contentCreatedAction.Invoke (tag);
				return ValidationListView;

			case "ratingTab":
				RatingTabView =  inflater.Inflate (Resource.Layout.tab_rating, null);
				if (contentCreatedAction != null)
					contentCreatedAction.Invoke (tag);
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

	class ListViewOneAdapter : BaseAdapter
	{
		LayoutInflater inflater;
		List<Moderator> list;

		public ListViewOneAdapter(LayoutInflater inflater, List<Moderator> list)
		{
			this.inflater = inflater;
			this.list = list;
		}

		#region implemented abstract members of BaseAdapter
		public override Java.Lang.Object GetItem (int position)
		{
			throw new System.NotImplementedException ();
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count {
			get {
				return list.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view;
			var data = list [position];
			view = convertView;
			ListViewOneItemHolder holder;
			if (view == null) {
				view = inflater.Inflate (Resource.Layout.list_one_item_view, null);
				holder = new ListViewOneItemHolder ()
				{
					ItemTitle = view.FindViewById<TextView> (Resource.Id.itemTitle),
					ItemPosts = view.FindViewById<TextView> (Resource.Id.itemPosts),
					ItemConfirmed = view.FindViewById<TextView> (Resource.Id.itemConfirmed)
				};
				view.Tag = holder;
			} 
			else 
				holder = (ListViewOneItemHolder)view.Tag;

			holder.ItemTitle.Text = data.Username;
			holder.ItemPosts.Text = "10234";
			holder.ItemConfirmed.Text = "8566";

			return view;
		}
		#endregion

		public void UpdateList(List<Moderator> newList)
		{
			if (newList == null)
				return;

			list = newList;
			NotifyDataSetChanged ();
		}

		class ListViewOneItemHolder : Java.Lang.Object
		{
			public TextView ItemTitle { get; set; }
			public TextView ItemPosts { get; set; }
			public TextView ItemConfirmed { get; set; }
		}
	}

	class ListTwoDataHolder
	{
		public string Title { get; set; }
		public string Description { get; set; }
	}

	class ListViewTwoAdapter : BaseAdapter
	{
		LayoutInflater inflater;
		List<ListTwoDataHolder> list;

		public ListViewTwoAdapter(LayoutInflater inflater, List<ListTwoDataHolder> list)
		{
			this.inflater = inflater;
			this.list = list;
		}

		#region implemented abstract members of BaseAdapter
		public override Java.Lang.Object GetItem (int position)
		{
			throw new System.NotImplementedException ();
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count {
			get {
				return list.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view;
			var data = list [position];
			view = convertView;
			ListViewTwoItemHolder holder;
			if (view == null) {
				view = inflater.Inflate (Resource.Layout.list_two_item_view, null);
				holder = new ListViewTwoItemHolder ()
				{
					ItemTitle = view.FindViewById<TextView> (Resource.Id.itemTitle),
					ItemDescription = view.FindViewById<TextView> (Resource.Id.itemDescription)
				};
				view.Tag = holder;
			} 
			else 
				holder = (ListViewTwoItemHolder)view.Tag;

			holder.ItemTitle.Text = data.Title;
			holder.ItemDescription.Text = data.Description;

			return view;
		}
		#endregion

		public void UpdateList(List<ListTwoDataHolder> newList)
		{
			if (newList == null)
				return;
			
			list = newList;
			NotifyDataSetChanged ();
		}

		class ListViewTwoItemHolder : Java.Lang.Object
		{
			public TextView ItemTitle { get; set; }
			public TextView ItemDescription { get; set; }
		}
	}

	/*
	 * Times new Roman
	 */

	public class AnimatedTabHostListener : Java.Lang.Object, global::Android.Widget.TabHost.IOnTabChangeListener
	{
		private static int ANIMATION_TIME = 150;
		private TabHost tabHost;
		private View previousView;
		private View currentView;
		private int currentTab;

		public AnimatedTabHostListener(TabHost tabHost)
		{
			this.tabHost = tabHost;
			this.previousView = tabHost.CurrentView;

			tabHost.TabChanged += (sender, e) => {OnTabChanged(e.TabId);};
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
			Animation inFromRight = new TranslateAnimation(Dimension.RelativeToParent, 1.0f, Dimension.RelativeToParent, 0.0f,
				Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f);
			return setProperties(inFromRight);
		}

		private Animation outToRightAnimation()
		{
			Animation outToRight = new TranslateAnimation(Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 1.0f,
				Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f);
			return setProperties(outToRight);
		}

		private Animation inFromLeftAnimation()
		{
			Animation inFromLeft = new TranslateAnimation(Dimension.RelativeToParent, -1.0f, Dimension.RelativeToParent, 0.0f,
				Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f);
			return setProperties(inFromLeft);
		}
			
		private Animation outToLeftAnimation()
		{
			Animation outtoLeft = new TranslateAnimation(Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, -1.0f,
				Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f);
			return setProperties(outtoLeft);
		}

		private Animation setProperties(Animation animation)
		{
			animation.Duration = (ANIMATION_TIME);
			animation.Interpolator = (new AccelerateInterpolator());
			return animation;
		}
	}
}