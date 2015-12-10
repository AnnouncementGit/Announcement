using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Announcement.Core;

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

            return view;
        }
    }
}


