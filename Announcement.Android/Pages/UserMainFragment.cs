using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Announcement.Core;

namespace Announcement.Android
{
    public class UserMainFragment : Fragment
    {
        private UserMainViewModel ViewModel 
        { 
            get 
            { 
                return UserMainViewModel.Instance; 
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.user_main_layout, null);

            return view;
        }
    }
}


