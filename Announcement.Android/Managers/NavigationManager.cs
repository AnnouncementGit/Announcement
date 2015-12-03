using System;
using Android.Support.V4.App;
using System.Collections.Generic;

namespace Announcement.Android
{
    public static class NavigationManager
    {
        public static global::Android.App.Activity CurrentActivity { get; private set; }
        
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

            transaction.Replace(Resource.Id.FragmentContainer, (Fragment)Activator.CreateInstance(fragmentType));

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

                transaction.Replace(Resource.Id.FragmentContainer, (Fragment)Activator.CreateInstance(fragmentType));

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

