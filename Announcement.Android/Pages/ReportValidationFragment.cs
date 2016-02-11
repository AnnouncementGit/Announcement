using System;
using Android.Support.V4.App;
using Announcement.Android.Controls;
using Announcement.Core;
using System.Collections.Generic;
using Android.Widget;
using Android.Views;
using Android.OS;
using Android.Support.V4.View;
using Square.Picasso;
using Android.App;
using Android.Content;
using Javax.Net.Ssl;
using Square.OkHttp;
using Android.Runtime;
using Android.Gms.Analytics;

namespace Announcement.Android
{
	public class ReportValidationFragment : BaseFragment
	{
		private ReportValidationViewModel viewModel { get { return ReportValidationViewModel.Instance; } }

		private ViewPager reportViewPager;
		private CustomViewPagerAdapter reportViewPagerAdapter;
		private Announcement.Android.Controls.EditText txtPhone;

		public override global::Android.Views.View OnCreateView (global::Android.Views.LayoutInflater inflater, global::Android.Views.ViewGroup container, global::Android.OS.Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.report_validation_view, null);

			reportViewPager = view.FindViewById<ViewPager> (Resource.Id.reportViewPager);

            if (MainActivityInstance.Current.Resources.DisplayMetrics.WidthPixels < MainActivityInstance.Current.Resources.DisplayMetrics.HeightPixels)
            {
                reportViewPager.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, MainActivityInstance.Current.Resources.DisplayMetrics.WidthPixels);
            }
            else
            {
                reportViewPager.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, MainActivityInstance.Current.Resources.DisplayMetrics.HeightPixels);
            }

	
            reportViewPager.SetOnTouchListener(this);

			reportViewPager.PageScrolled += ReportViewPagerOnPageScrolled;

			reportViewPagerAdapter = new CustomViewPagerAdapter (MainActivityInstance.Current.SupportFragmentManager, viewModel.Photos);

			reportViewPager.Adapter = reportViewPagerAdapter;

			txtPhone = view.FindViewById<Announcement.Android.Controls.EditText> (Resource.Id.reportPhoneNumber);

			view.FindViewById<Announcement.Android.Controls.Button> (Resource.Id.btnConfirm).Click += OnConfirmClick;

            view.FindViewById<Announcement.Android.Controls.Button> (Resource.Id.btnReject).Click += OnCancelClick;


            if (!string.IsNullOrWhiteSpace(viewModel.PhoneNumber))
            {
                txtPhone.Text = viewModel.PhoneNumber;
            }

			MainActivity.GATracker.SetScreenName ("Report Validation Fragment");
			MainActivity.GATracker.Send (new HitBuilders.ScreenViewBuilder ().Build ());

			return view;
		}

		protected void ReportViewPagerOnPageScrolled (object sender, global::Android.Support.V4.View.ViewPager.PageScrolledEventArgs e)
		{
			
		}

        protected void OnConfirmClick(object sender, EventArgs e)
		{
            viewModel.ConfirmReport (txtPhone.Text, ConfirmReportCallback);
		}

        protected void OnCancelClick(object sender, EventArgs e)
		{
            viewModel.RejectReport (RejectReportCallback);
		}

        protected void ConfirmReportCallback()
        {
            AdminMainFragment.StartTabIndex = 1;
            
            NavigationManager.Backward();
        }

        protected void RejectReportCallback()
        {
            AdminMainFragment.StartTabIndex = 1;

            NavigationManager.Backward();
        }
	}

	public class CustomViewPagerAdapter : FragmentStatePagerAdapter
	{
		private List<string> list;

		public CustomViewPagerAdapter(global::Android.Support.V4.App.FragmentManager fm, List<string> list): base(fm)
		{
			this.list = list;
		}
			
		public override int Count {
			get {
				return list.Count;
			}
		}

        public override global::Android.Support.V4.App.Fragment GetItem (int position)
		{
			var item = list [position];

            var url = "https://s3-eu-west-1.amazonaws.com/stop-spam/reports/" + item;

            return new PagerImageView() { imageUrl = url };
		}
	}

    public class PagerImageView : global::Android.Support.V4.App.Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstance)
		{
			view = inflater.Inflate (Resource.Layout.validation_images_pager_view, null);

			try 
            {
                var viewImageWithProgress = view.FindViewById<ScaleImageWithProgress> (Resource.Id.pagerImageView);

                Picasso.With(Application.Context).Load(imageUrl).Into(new Target(viewImageWithProgress));
			} 
            catch(Exception ex)
            {
				Console.WriteLine (ex.Message);
			}

			return view;
		}

        private View view;

        public string imageUrl;
	}
        
    public class Target : Java.Lang.Object, ITarget
    {
        public Target(ScaleImageWithProgress view)
        {
            this.view = view;
        }
        
        public void OnBitmapFailed(global::Android.Graphics.Drawables.Drawable drawable)
        {
            view.Image.SetImageDrawable(null);

            view.IsShowProgress = false;
        }

        public void OnBitmapLoaded(global::Android.Graphics.Bitmap bitmap, Picasso.LoadedFrom loadFrom)
        {
            view.IsShowProgress = false;

            view.Image.SetImageBitmap(bitmap);
        }

        public void OnPrepareLoad(global::Android.Graphics.Drawables.Drawable drawable)
        {
            view.Image.SetImageDrawable(null);
            
            view.IsShowProgress = true;
        }

        private ScaleImageWithProgress view;
    }
}

