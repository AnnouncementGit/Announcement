using Android.OS;
using Android.Views;
using Android.Support.V4.App;

namespace Announcement.Android
{
    public class AddNewModeratorFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.add_new_moderator_layout, null);

            return view;
        }
    }
}


