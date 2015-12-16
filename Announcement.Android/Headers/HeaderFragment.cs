using Android.Support.V4.App;
using Android.OS;

namespace Announcement.Android
{		
	public class HeaderFragment : Fragment
	{
		public override global::Android.Views.View OnCreateView (global::Android.Views.LayoutInflater inflater, global::Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.header_view, null);

			return view;
		}
	}
}

