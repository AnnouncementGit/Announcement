using System;
using Android.App;
using Android.Views;
using Android.Content;
using Announcement.Android;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Announcement.Android.Controls;
using Android.Graphics;

namespace Announcement.Core
{
    public static class DispatcherModule
    {
        public static void Invoke(Action action)
        {
            Application.SynchronizationContext.Post (ignored => action.Invoke(), null);
        }

        public static void Invoke(Action<bool> action, bool result)
        {
            Application.SynchronizationContext.Post (ignored => action.Invoke(result), null);
        }

        public static void Invoke<T1,T2>(Action<T1, T2> action, T1 t1, T2 t2)
        {
            Application.SynchronizationContext.Post(ignored => action.Invoke(t1, t2), null);
        }
    }
}

