using Android.OS;
using Android.Views;
using Announcement.Core;
using Android.Support.V4.App;

namespace Announcement.Android
{
    public class ModeratorsFragment : Fragment
    {
        private ModeratorsViewModel ViewModel 
        { 
            get 
            { 
                return ModeratorsViewModel.Instance; 
            }
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.moderators_layout, null);

            return view;
        }
    }
}


