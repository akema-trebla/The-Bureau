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
using The_Bureau_2.DbClasses;
using Android.Util;
using Android.Content.Res;
using Microsoft.WindowsAzure.MobileServices;

namespace The_Bureau_2.Fragments
{
    public class Fragment2 : SupportFragment
    {
        AzureService azureService;
        public Fragment2()
        {
            azureService = new AzureService();
        }

        RecyclerView recyclerView2;
        private List<ProjectTaskDto> mProjectTasks = new List<ProjectTaskDto>();
        private RecyclerAdapter2 mAdapter2;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CurrentPlatform.Init();

            // Create your fragment here

            //OnRefreshTasks();
        }

        private async void OnRefreshTasks()
        {
            try
            {
                var projectTasks = await azureService.GetProjectTasks();
                mAdapter2.Clear();


                foreach (ProjectTaskDto current in projectTasks)
                    mAdapter2.Add(current);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("OH NO!" + ex);
                CreateAndShowDialog(ex.Message, "ProjectTaskDto Sync Error");
            }
        }

        private void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        private void CreateAndShowDialog(string message, string title)
        {
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(recyclerView2.Context);

            builder.SetMessage("Unable to sync ProjectTasks, you may be offline" + message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            recyclerView2 = inflater.Inflate(Resource.Layout.Fragment_Task, container, false) as RecyclerView;

            SetUpRecyclerView2(recyclerView2);

            return recyclerView2;
        }

        public void SetUpRecyclerView2(RecyclerView recyclerView2)
        {

            recyclerView2.SetLayoutManager(new LinearLayoutManager(recyclerView2.Context));
            mAdapter2 = new RecyclerAdapter2(recyclerView2.Context, mProjectTasks, Activity.Resources);
            //mProjectTasks.Adapter = mAdapter;
            recyclerView2.SetAdapter(mAdapter2);

            OnRefreshTasks();
        }

    }

    public class RecyclerAdapter2 : RecyclerView.Adapter
    {
        List<ProjectTaskDto> mProjectTasks;
        private readonly TypedValue mTypedValue = new TypedValue();
        private int mBackground;
        Resources mResource;

        public RecyclerAdapter2(Context context, List<ProjectTaskDto> projecttasks, Resources res)
        {
            context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, mTypedValue, true);
            mBackground = mTypedValue.ResourceId;
            mProjectTasks = projecttasks;
            mResource = res;
        }

        public override int ItemCount
        {
            get
            {
                return mProjectTasks.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var simpleHolder = holder as SimpleViewHolder2;
            int indexPosition = (mProjectTasks.Count - 1) - position;

            simpleHolder.mTaskName.Text = mProjectTasks[indexPosition].TaskName;
            simpleHolder.mStartDate.Text = mProjectTasks[indexPosition].StartDate.ToString();
            simpleHolder.mTaskStatus.Text = mProjectTasks[indexPosition].TaskStatus;
            simpleHolder.mTaskPrecedence.Text = mProjectTasks[indexPosition].TaskPrecedence;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.new_row, parent, false);
            view.SetBackgroundResource(mBackground);

            return new SimpleViewHolder2(view);
        }

        public void Add(ProjectTaskDto projectTask)
        {
            mProjectTasks.Add(projectTask);
            NotifyDataSetChanged();
        }

        // Insert a new item to the RecyclerView on a predefined position
        public void Insert(int position, ProjectTaskDto projectTask)
        {
            mProjectTasks.Insert(position, projectTask);
            NotifyItemInserted(0);
        }

        // Remove a RecyclerView item containing a specified Data object
        public void Remove(ProjectTaskDto projectTask)
        {
            int position = mProjectTasks.IndexOf(projectTask);
            mProjectTasks.RemoveAt(mProjectTasks.Count - 1);
            NotifyItemRemoved(0);
        }

        public void Clear()
        {
            mProjectTasks.Clear();
            NotifyDataSetChanged();
        }
    }

    public class SimpleViewHolder2 : RecyclerView.ViewHolder
    {
        public View mMainView2 { get; set; }
        public TextView mTaskName { get; set; }
        public TextView mStartDate { get; set; }
        public TextView mTaskStatus { get; set; }
        public TextView mTaskPrecedence { get; set; }

        public SimpleViewHolder2(View view)
            : base(view)
        {
            mMainView2 = view;
            mTaskName = view.FindViewById<TextView>(Resource.Id.txtRow1);
            mStartDate = view.FindViewById<TextView>(Resource.Id.txtRow2);
            mTaskStatus = view.FindViewById<TextView>(Resource.Id.txtRow3);
            mTaskPrecedence = view.FindViewById<TextView>(Resource.Id.txtRow4);
        }
    }
}