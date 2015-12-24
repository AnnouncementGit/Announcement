using System;
using Android.Support.V4.App;
using Announcement.Android.Controls;
using Announcement.Core;
using System.Collections.Generic;
using Android.Widget;
using Android.Views;
using Android.OS;
using Android.Support.V4.View;

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
			if(MainActivityInstance.Current.Resources.DisplayMetrics.WidthPixels < MainActivityInstance.Current.Resources.DisplayMetrics.HeightPixels)
				reportViewPager.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, MainActivityInstance.Current.Resources.DisplayMetrics.WidthPixels);
			else
				reportViewPager.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, MainActivityInstance.Current.Resources.DisplayMetrics.HeightPixels);

	
            reportViewPager.SetOnTouchListener(this);

			reportViewPager.PageScrolled += ReportViewPagerOnPageScrolled;
			reportViewPagerAdapter = new CustomViewPagerAdapter (MainActivityInstance.Current.SupportFragmentManager, viewModel.Photos);
			reportViewPager.Adapter = reportViewPagerAdapter;
			txtPhone = view.FindViewById<Announcement.Android.Controls.EditText> (Resource.Id.reportPhoneNumber);

			view.FindViewById<Announcement.Android.Controls.Button> (Resource.Id.btnConfirm).Click += OnConfirmClick;;
			view.FindViewById<Announcement.Android.Controls.Button> (Resource.Id.btnCancel).Click += OnCancelClick;


			if (!string.IsNullOrWhiteSpace (viewModel.PhoneNumber))
				txtPhone.Text = viewModel.PhoneNumber;

			return view;
		}

		void ReportViewPagerOnPageScrolled (object sender, global::Android.Support.V4.View.ViewPager.PageScrolledEventArgs e)
		{
			
		}

		void OnConfirmClick(object sender, EventArgs e)
		{
			viewModel.ConfirmReport (() => MainActivityInstance.Current.OnBackPressed ());
		}

		void OnCancelClick(object sender, EventArgs e)
		{
			viewModel.RejectReport (() => MainActivityInstance.Current.OnBackPressed ());
		}
	}

	public class CustomViewPagerAdapter : FragmentStatePagerAdapter
	{
		private List<string> list;

		public CustomViewPagerAdapter(FragmentManager fm, List<string> list): base(fm)
		{
			this.list = list;
		}
			
		public override int Count {
			get {
				return list.Count;
			}
		}

		public override Fragment GetItem (int position)
		{
			var item = list [position];
			return new PagerImageView(){imageUrl = item};
		}
	}

	class PagerImageView : Fragment
	{
		private View view;
		public string imageUrl;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstance)
		{
			view = inflater.Inflate (Resource.Layout.validation_images_pager_view, null);

			try {
//				var uri = global::Android.Net.Uri.Parse (imageUrl);
//				view.FindViewById<ScaleImageView> (Resource.Id.pagerImageView).SetImageURI (uri);
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}

			return view;
		}
	}
}

