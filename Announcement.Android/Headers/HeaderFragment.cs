using Android.Support.V4.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using Android.Graphics.Drawables;
using System.Collections.Generic;

namespace Announcement.Android
{		
	public class HeaderFragment : Fragment
	{
		private ToggleButton btnMenu;
		private PopupWindow popupWindow;
		private View popupView;

		public override global::Android.Views.View OnCreateView (global::Android.Views.LayoutInflater inflater, global::Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.header_view, null);

			btnMenu = view.FindViewById<ToggleButton> (Resource.Id.btnMenu);
			btnMenu.Click += BtnMenuOnClick;

			popupView = inflater.Inflate (Resource.Layout.popup_menu, null);

			return view;
		}

		void BtnMenuOnClick (object sender, System.EventArgs e)
		{


			popupWindow = new PopupWindow(MainActivityInstance.Current);
			popupWindow.Focusable = true;
			popupWindow.Width = ViewGroup.LayoutParams.WrapContent;
			popupWindow.Height = ViewGroup.LayoutParams.WrapContent;
			popupWindow.SetBackgroundDrawable(new BitmapDrawable());
			popupWindow.ContentView = popupView;
			popupWindow.OutsideTouchable = true;
			
			var list = popupWindow.ContentView.FindViewById<ListView> (Resource.Id.listView);
			List<string> menuItems = new List<string>();
			if(NavigationManager.CurrentFragment == typeof(AdminMainFragment))
				menuItems.Add("Add Moderator");
			menuItems.Add ("Logout");
			list.Adapter = new ArrayAdapter (MainActivityInstance.Current, Resource.Layout.popup_menu_item, menuItems);
			list.ItemClick += PopupMenuItemClick;

				popupWindow.ShowAsDropDown (btnMenu);
			popupWindow.DismissEvent += PopupWindowOnDismissEvent;
		}

		void PopupWindowOnDismissEvent (object sender, System.EventArgs e)
		{
			btnMenu.Checked = false;
		}

		void PopupMenuItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			popupWindow.Dismiss ();
			switch (e.Position) {
			case 0:
				if (((ListView)sender).Count > 1)
					NavigationManager.Forward (typeof(CreateModeratorFragment));
				else 
				{
					NavigationManager.EraseBackStack();
					NavigationManager.Forward (typeof(LoginFragment));
				}
				break;

			case 1:
				NavigationManager.EraseBackStack();
				NavigationManager.Forward (typeof(LoginFragment));
				break;
			}
		}
	}
}

