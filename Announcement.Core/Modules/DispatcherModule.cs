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
    }
}

