using System;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;

namespace Announcement.Android
{
    public abstract class MonoAdapter<T> : BaseAdapter<T>
    {
        protected LayoutInflater inflater;

        protected Context context;

        protected int resourceId;

        public List<T> objects;

        public MonoAdapter(Context context, List<T> objects)
        {
            this.inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

            this.context = context;

            this.objects = objects;
        }

        public MonoAdapter(Context context, int resourceId, List<T> objects)
        {
            this.inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

            this.context = context;

            this.resourceId = resourceId;

            this.objects = objects;
        }  

        public virtual void Add(T item)
        {
            objects.Add(item);

            NotifyDataSetChanged();
        }

        public virtual void AddAll(List<T> collection)
        {
            objects.AddRange(collection);

            NotifyDataSetChanged();
        }

        public virtual void Insert(int index, T item)
        {
            objects.Insert(index, item);

            NotifyDataSetChanged();
        }

        public virtual void Remove(T item)
        {
            objects.Remove(item);

            NotifyDataSetChanged();
        }

        public virtual void RemoveAt(int index)
        {
            objects.RemoveAt(index);

            NotifyDataSetChanged();
        }

        public virtual void Clear()
        {
            objects.Clear();

            NotifyDataSetChanged();
        }

        public virtual void UpdateAll(List<T> collection)
        {
            objects.Clear();

            objects.AddRange(collection);

            NotifyDataSetChanged();
        }

        public override int Count
        {
            get
            {
                return objects.Count;
            }
        }

        public T GetItem(int position)
        {
            return objects[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override T this[int index]
        {
            get
            {
                return objects[index];
            }
        }
    }
}

