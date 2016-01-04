﻿using Android.Views;
using Announcement.Core;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.App;
using Android.OS;
using System.Collections.Generic;
using Android.Views.InputMethods;
using Android.Content.Res;

namespace Announcement.Android
{
    public class HeaderFragment : BaseFragment, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private MenuViewModel ViewModel
        { 
            get
            { 
                return MenuViewModel.Instance; 
            }
        }

        public override View OnCreateView(global::Android.Views.LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
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
            {
                isOrientationChanged = false;

                return;
            }

            if (isOrientationChanged)
            {
                isOrientationChanged = false;
                
                popupWindow.Dismiss();

                btnMenu.Checked = true;

                ShowPopupMenu();
            }
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            isOrientationChanged = previousOrientation != newConfig.Orientation;

            base.OnConfigurationChanged(newConfig);

            previousOrientation = newConfig.Orientation;
        }

        protected void BtnMenuOnClick(object sender, System.EventArgs e)
        {
            var view = Activity.Window.CurrentFocus;

            if (view != null)
            {
                var manager = (InputMethodManager)Application.Context.GetSystemService(global::Android.Content.Context.InputMethodService);

                if (manager.IsActive)
                {
                    manager.HideSoftInputFromWindow(view.WindowToken, 0);
                }

                var editText = view as EditText;

                if (editText != null)
                {
                    editText.ClearFocus();
                }
            }

            ShowPopupMenu();
        }

        protected void ShowPopupMenu()
        {
            var menuItems = new List<string>();

            if (NavigationManager.CurrentFragment == typeof(AdminMainFragment))
            {
                menuItems.Add(LocalizationModule.Translate("label_add_moderator"));
            }

            menuItems.Add(LocalizationModule.Translate("label_logout"));

            popupListView.Adapter = new ArrayAdapter(MainActivityInstance.Current, Resource.Layout.popup_menu_item, menuItems);

            popupWindow.ShowAsDropDown(btnMenu);
        }

        protected void PopupWindowOnDismissEvent(object sender, System.EventArgs e)
        {
            btnMenu.Checked = false;
        }

        protected void PopupMenuItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            popupWindow.Dismiss();

            switch (e.Position)
            {
                case 0:
                    if (((ListView)sender).Count > 1)
                    {
                        AdminMainViewModel.Instance.InitializeCreateModerator(() =>
                            {
                                NavigationManager.Forward(typeof(CreateModeratorFragment));
                            });
                    }
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
			SocialServices.Instance.LogOut ();

            NavigationManager.EraseBackStack();

            NavigationManager.Forward(typeof(LoginFragment)); 
        }

        private ToggleButton btnMenu;

        private PopupWindow popupWindow;

        private ListView popupListView;

        private bool isOrientationChanged;

        private global::Android.Content.Res.Orientation previousOrientation;
    }
}

