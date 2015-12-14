using Android.OS;
using Android.Views;
using Android.Widget;
using Announcement.Core;
using Android.Support.V4.App;

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

			var tabHost = view.FindViewById<TabHost> (Android.Resource.Id.tabHost);
			tabHost.Setup ();

			TabHost.TabSpec tabSpec;
			var tabHostContentFactory = new TabHostContentFactory (inflater);

			tabSpec = tabHost.NewTabSpec ("validationTab");
			var tab1view = inflater.Inflate (Resource.Layout.tab_indicator, null);
			tab1view.FindViewById<TextView> (Resource.Id.indicatorTextView).Text = "Validation";
			tabSpec.SetIndicator (tab1view);
			tabSpec.SetContent (tabHostContentFactory);
			tabHost.AddTab (tabSpec);

			tabSpec = tabHost.NewTabSpec ("ratingTab");
			var tab2view = inflater.Inflate (Resource.Layout.tab_indicator, null);
			tab2view.FindViewById<TextView> (Resource.Id.indicatorTextView).Text = "Rating";
			tabSpec.SetIndicator(tab2view);
			tabSpec.SetContent (tabHostContentFactory);
			tabHost.AddTab(tabSpec);

			tabHost.TabChanged += TabHostOnTabChanged;

            return view;
        }

        void TabHostOnTabChanged (object sender, TabHost.TabChangeEventArgs e)
        {
			
        }
    }

	class TabHostContentFactory : Java.Lang.Object, TabHost.ITabContentFactory
	{
		private LayoutInflater inflater;
		public TabHostContentFactory(LayoutInflater inflater)
		{
			this.inflater = inflater;
		}
		public View CreateTabContent (string tag)
		{
			switch (tag) {
			case "validationTab":
				return inflater.Inflate (Resource.Layout.tab_validation, null);

			case "ratingTab":
				ListView lv = new ListView (MainActivityInstance.Current);
				return lv;

			default:
				return null;
			};
		}
	}
}


