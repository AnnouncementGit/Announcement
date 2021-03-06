﻿using System;
using Android.Support.V4.App;
using System.Collections.Generic;
using Android.Widget;

namespace Announcement.Android
{
    public static class NavigationManager
    {
        public static global::Android.App.Activity CurrentActivity { get; private set; }

		public static Fragment CurrentHeader { get; private set; }

		public static Type CurrentFragment{
			get 
			{
				if(fragmentManager == null || fragmentManager.FindFragmentById (Resource.Id.FragmentContainer) == null)
					return null;

				return fragmentManager.FindFragmentById (Resource.Id.FragmentContainer).GetType();
			}
		}
        
        public static void Initialize(FragmentActivity activity)
        {
            CurrentActivity = activity;
            
            fragmentManager = activity.SupportFragmentManager;

            backStack = new List<Type>();
        }

        public static void Forward(Type fragmentType)
        {
            if (fragmentManager == null)
                return;

            backStack.Add(fragmentType);

            var transaction = fragmentManager.BeginTransaction();

            transaction.SetCustomAnimations(Resource.Animation.slide_forward_in_animation, Resource.Animation.slide_forward_out_animation);

			transaction.Replace(Resource.Id.FragmentContainer, (Fragment)Activator.CreateInstance(fragmentType));

			ShowHeader (true);

            transaction.Commit();
        }

		public static void ShowHeader(bool show)
		{
			var header = CurrentActivity.FindViewById<FrameLayout> (Resource.Id.HeaderContainer);

			if (show)
				header.Visibility = global::Android.Views.ViewStates.Visible;
			else
				header.Visibility = global::Android.Views.ViewStates.Gone;
		}

		public static void AddHeader(Type headerType)
		{
			if (fragmentManager == null)
				return;

			var transaction = fragmentManager.BeginTransaction();

			var headerFragment = (Fragment)Activator.CreateInstance (headerType);
			CurrentHeader = headerFragment;
			transaction.Replace(Resource.Id.HeaderContainer, headerFragment);

			transaction.Commit();
		}

        public static bool Backward()
        {
            if (backStack.Count > 1)
            {
                var index = backStack.Count - 1;
                
                var fragmentType = backStack[index - 1];

                backStack.RemoveAt(index);

                var transaction = fragmentManager.BeginTransaction();

                transaction.SetCustomAnimations(Resource.Animation.slide_backward_in_animation, Resource.Animation.slide_backward_out_animation);

                transaction.Replace(Resource.Id.FragmentContainer, (Fragment)Activator.CreateInstance(fragmentType));

				ShowHeader (true);

                transaction.Commit();

                return true;
            }

            return false;
        }

        public static void EraseBackStack()
        {
            backStack.Clear();
        }

        private static List<Type> backStack;

        private static FragmentManager fragmentManager;
    }
}

