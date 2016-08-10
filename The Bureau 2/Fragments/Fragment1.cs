using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Content.Res;
using Microsoft.WindowsAzure.MobileServices;

namespace The_Bureau_2.Fragments
{
    public class Fragment1 : SupportFragment
    {
        AzureService azureService;
        public Fragment1()
        {
            azureService = new AzureService();
        }

        RecyclerView recyclerView;
        private List<DbClasses.ActivityDto> mActivities = new List<DbClasses.ActivityDto>();
        private RecyclerAdapter mAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
             
            CurrentPlatform.Init();
            
            // Create your fragment here

            //OnRefreshActivities();
        }

        private async void OnRefreshActivities()
        {
            try
            {
                var activities = await azureService.GetActivities();
                mAdapter.Clear();


                foreach (DbClasses.ActivityDto current in activities)
                    mAdapter.Add(current);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("OH NO!" + ex);
                CreateAndShowDialog(ex.Message, "Activity Sync Error");
            } 
        }

        private void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        private void CreateAndShowDialog(string message, string title)
        {
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(recyclerView.Context);

            builder.SetMessage("Unable to sync Activities, you may be offline" + message);
            builder.SetTitle(title);
            builder.Create().Show();
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            recyclerView = inflater.Inflate(Resource.Layout.Fragment_Activity, container, false) as RecyclerView;

            //recyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerview);

            SetUpRecyclerView(recyclerView);

            return recyclerView;

        }

        public void SetUpRecyclerView(RecyclerView recyclerView)
        {

            recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
            mAdapter = new RecyclerAdapter(recyclerView.Context, mActivities, Activity.Resources);
            //mActivities.Adapter = mAdapter;
            recyclerView.SetAdapter(mAdapter);

            OnRefreshActivities();
        }
        
    }

    //public class MyList<T>
    //{
    //    private List<T> mItems;
    //    private RecyclerView.Adapter mAdapter;

    //    public MyList()
    //    {
    //        mItems = new List<T>();
    //    }

    //    public RecyclerView.Adapter Adapter
    //    {
    //        get { return mAdapter; }
    //        set { mAdapter = value; }
    //    }

    //    public void Add(T item)
    //    {
    //        mItems.Add(item);

    //        if (Adapter != null)
    //        {
    //            Adapter.NotifyItemInserted(0);
    //        }

    //    }

    //    public void Remove(int position)
    //    {
    //        mItems.RemoveAt(position);

    //        if (Adapter != null)
    //        {
    //            Adapter.NotifyItemRemoved(0);
    //        }
    //    }

    //    public void Clear()
    //    {
    //        mItems.Clear();

    //        if (Adapter != null)
    //        {
    //            Adapter.NotifyDataSetChanged();
    //        }
    //    }

    //    public T this[int index]
    //    {
    //        get { return mItems[index]; }
    //        set { mItems[index] = value; }
    //    }

    //    public int Count
    //    {
    //        get { return mItems.Count; }
    //    }

    //}

    public class RecyclerAdapter : RecyclerView.Adapter
    {
        List<DbClasses.ActivityDto> mActivities;
        private readonly TypedValue mTypedValue = new TypedValue();
        private int mBackground;
        Resources mResource;

        public RecyclerAdapter(Context context, List<DbClasses.ActivityDto> activities, Resources res)
		{
            context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, mTypedValue, true);
            mBackground = mTypedValue.ResourceId;
            mActivities = activities;
            mResource = res;
		}

        public override int ItemCount
        {
            get
            {
                return mActivities.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var simpleHolder = holder as SimpleViewHolder;
            int indexPosition = (mActivities.Count - 1) - position;

            simpleHolder.mProject.Text = mActivities[indexPosition].Project;
            simpleHolder.mPerson.Text = mActivities[indexPosition].Person;
            simpleHolder.mActivity_Date.Text = mActivities[indexPosition].Activity_Date.ToString();
            simpleHolder.mTime_Stamp.Text = mActivities[indexPosition].Time_Stamp.ToString();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.new_row, parent, false);
            view.SetBackgroundResource(mBackground);

            return new SimpleViewHolder(view);
        }

        public void Add(DbClasses.ActivityDto activity)
        {
            mActivities.Add(activity);
            NotifyDataSetChanged();
        }

        // Insert a new item to the RecyclerView on a predefined position
        public void Insert(int position, DbClasses.ActivityDto activity)
        {
            mActivities.Insert(position, activity);
            NotifyItemInserted(0);
        }

        // Remove a RecyclerView item containing a specified Data object
        public void Remove(DbClasses.ActivityDto activity)
        {
            int position = mActivities.IndexOf(activity);
            mActivities.RemoveAt(mActivities.Count - 1);
            NotifyItemRemoved(0);
        }

        public void Clear()
        {
            mActivities.Clear();
            NotifyDataSetChanged();
        }
    }

    public class SimpleViewHolder : RecyclerView.ViewHolder
    {
        public View mMainView { get; set; }
        public TextView mProject { get; set; }
        public TextView mPerson { get; set; }
        public TextView mActivity_Date { get; set; }
        public TextView mTime_Stamp { get; set; }

          public SimpleViewHolder (View view) : base(view)
            {
                mMainView = view;
                mProject = view.FindViewById<TextView>(Resource.Id.txtRow1);
                mPerson = view.FindViewById<TextView>(Resource.Id.txtRow2);
                mActivity_Date = view.FindViewById<TextView>(Resource.Id.txtRow3);
                mTime_Stamp = view.FindViewById<TextView>(Resource.Id.txtRow4);
            }
    }
}