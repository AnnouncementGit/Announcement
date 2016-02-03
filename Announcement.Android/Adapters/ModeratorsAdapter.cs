using System;
using Announcement.Core;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Announcement.Android.Controls;
using Android.Animation;
using Android.Views.Animations;

namespace Announcement.Android
{
    public class ModeratorsAdapter : MonoAdapter<Moderator>
    {
        public Action<Moderator, View> ItemDeleteClick { get; set; }
        
        public ModeratorsAdapter(Context context, List<Moderator> objects, InterceptedSwipeRefreshLayout swipeRefreshLayout, global::Android.Widget.ListView listView) : base(context, objects)
        {
            this.swipeRefreshLayout = swipeRefreshLayout;

            this.listView = listView;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ModeratorsAdapterItemHolder holder = null;

            var item = GetItem(position);

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.moderator_list_item_template, null);

                holder = new ModeratorsAdapterItemHolder();

                holder.scrlSlider = (MagicHorizontalScrollView)convertView;

                holder.scrlSlider.SlideStateChanged += ItemSlideStateChanged;

                holder.scrlSlider.SwipeRefreshLayout = swipeRefreshLayout;

                holder.txtTitle = convertView.FindViewById<TextView>(Resource.Id.Title);

                holder.imgDelete = convertView.FindViewById<global::Android.Widget.ImageView>(Resource.Id.Delete);

                holder.imgDelete.Click += Holder_imgDelete_Click;

                holder.imgDelete.Tag = convertView;

                convertView.Tag = holder;
            }
            else
            {
                holder = (ModeratorsAdapterItemHolder)convertView.Tag;
            }
                
            holder.txtTitle.Text = item.Username;

            holder.position = position;

            if (slidedItemPostion == position)
            {
                holder.scrlSlider.UpdateState(SlideStates.Left);
            }
            else
            {
                holder.scrlSlider.UpdateState(SlideStates.Right);
            }

            return convertView;
        }

        protected void ItemSlideStateChanged (object sender, EventArgs e)
        {
            var item = sender as MagicHorizontalScrollView;

            if (item.SlideState == SlideStates.Left)
            {
//                if (slidedItemPostion > -1 && slidedItemPostion==((ModeratorsAdapterItemHolder)item.Tag).position)
//                {
//                   ((MagicHorizontalScrollView)GetView(slidedItemPostion)).SlideState = SlideStates.Right;
//                }
//                
                slidedItemPostion = ((ModeratorsAdapterItemHolder)item.Tag).position;
            }
            else
            {
                slidedItemPostion = -1;
            }
        }
            
        protected void Holder_imgDelete_Click (object sender, EventArgs e)
        {
            var view = sender as View;

            if (view != null)
            {
                var row = view.Tag as View;

                if (row != null)
                {
                    var holder = row.Tag as ModeratorsAdapterItemHolder;

                    if (ItemDeleteClick != null)
                    {
                        ItemDeleteClick.Invoke(objects[holder.position], row);
                    }
                }
            }
        }

        public int GetPosition(Moderator item)
        {
            return objects.IndexOf(item);
        }

        public void Remove(Moderator item, View row)
        {
            if (row != null)
            {
                var holder = row.Tag as ModeratorsAdapterItemHolder;
                
                var animatorAlpha = ValueAnimator.OfFloat(new[] { 1.0f, 0.0f });
                
                animatorAlpha.SetDuration(200);
                
                animatorAlpha.Update += (o, animatorUpdateEventArgs) =>
                {
                    row.Alpha = (float)animatorUpdateEventArgs.Animation.AnimatedValue;
                };
                
                animatorAlpha.AnimationEnd += (obj, args) =>
                {
                    var originalHeight = row.Height;
                
                    var animatorHeight = ValueAnimator.OfInt(new[] { originalHeight, 0 });
                
                    animatorHeight.SetDuration(200);
                
                    animatorHeight.Update += (o, animatorUpdateEventArgs) =>
                    {
                        row.LayoutParameters.Height = (int)animatorUpdateEventArgs.Animation.AnimatedValue;
                
                        row.RequestLayout();
                    };
                
                    animatorHeight.AnimationEnd += (o, eventArgs) =>
                    { 
                                Remove(item);
                
                        row.LayoutParameters.Height = originalHeight;
                
                        row.Alpha = 1.0f;   

                                holder.scrlSlider.IsFromFullScroll = true;

                                swipeRefreshLayout.IsIntercept = false;
                    };
                
                    animatorHeight.Start();
                };
                
                animatorAlpha.Start();
            }
        }

        private InterceptedSwipeRefreshLayout swipeRefreshLayout;

        private int slidedItemPostion = -1;

        global::Android.Widget.ListView listView;
    }

    public class ModeratorsAdapterItemHolder : Java.Lang.Object
    {
        public TextView txtTitle;

        public  MagicHorizontalScrollView scrlSlider;

        public global::Android.Widget.ImageView imgDelete;

        public int position;
    }
}

