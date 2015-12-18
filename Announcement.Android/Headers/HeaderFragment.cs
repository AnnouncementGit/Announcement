using Android.Support.V4.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using Announcement.Core;
using Android.Graphics.Drawables;
using System.Collections.Generic;

namespace Announcement.Android
{
    public class HeaderFragment : Fragment, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private MenuViewModel ViewModel
        { 
            get
            { 
                return MenuViewModel.Instance; 
            }
        }

        private ToggleButton btnMenu;
        private PopupWindow popupWindow;
        private ListView popupListView;

        public override global::Android.Views.View OnCreateView(global::Android.Views.LayoutInflater inflater, global::Android.Views.ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.header_view, null);

            btnMenu = view.FindViewById<ToggleButton>(Resource.Id.btnMenu);
            btnMenu.Click += BtnMenuOnClick;
            btnMenu.ViewTreeObserver.AddOnGlobalLayoutListener(this);

            var popupView = inflater.Inflate(Resource.Layout.popup_menu, null);
            popupWindow = new PopupWindow(MainActivityInstance.Current);
            popupWindow.Focusable = true;
            popupWindow.Width = ViewGroup.LayoutParams.WrapContent;
            popupWindow.Height = ViewGroup.LayoutParams.WrapContent;
            popupWindow.SetBackgroundDrawable(new BitmapDrawable());
            popupWindow.ContentView = popupView;
			popupWindow.OutsideTouchable = true;
			popupWindow.DismissEvent += PopupWindowOnDismissEvent;

			popupListView = popupWindow.ContentView.FindViewById<ListView>(Resource.Id.listView);
			popupListView.ItemClick += PopupMenuItemClick;

            return view;
        }

        public void OnGlobalLayout()
        {
            if (!popupWindow.IsShowing)
                return;
			
            popupWindow.Dismiss();
            btnMenu.Checked = true;
            ShowPopupMenu();
        }

        void BtnMenuOnClick(object sender, System.EventArgs e)
        {
            ShowPopupMenu();
        }

        void ShowPopupMenu()
        {
            var menuItems = new List<string>();
            if (NavigationManager.CurrentFragment == typeof(AdminMainFragment))
                menuItems.Add(LocalizationModule.Translate("label_add_moderator"));
            menuItems.Add(LocalizationModule.Translate("label_logout"));
            popupListView.Adapter = new ArrayAdapter(MainActivityInstance.Current, Resource.Layout.popup_menu_item, menuItems);

            popupWindow.ShowAsDropDown(btnMenu);
        }

        void PopupWindowOnDismissEvent(object sender, System.EventArgs e)
        {
            btnMenu.Checked = false;
        }

        void PopupMenuItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            popupWindow.Dismiss();
            switch (e.Position)
            {
                case 0:
                    if (((ListView)sender).Count > 1)
                        NavigationManager.Forward(typeof(CreateModeratorFragment));
                    else
                    {
                        ViewModel.Logout(LogoutCallback);
                    }
                    break;

                case 1:
                    ViewModel.Logout(LogoutCallback);
                    break;
            }
        }

        protected void LogoutCallback()
        {
            NavigationManager.EraseBackStack();
            NavigationManager.Forward(typeof(LoginFragment)); 
        }
    }
}

