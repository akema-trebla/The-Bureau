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
using SupportActionBar = Android.Support.V7.App.ActionBar;
using SupportEditText = Android.Support.Design.Widget.TextInputLayout;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace The_Bureau_2
{
    [Activity(Label = "AddNewTask", Theme = "@style/Theme.DesignDemo")]
    public class AddNewTask : MainActivity
    {
        TableLayout mTbLytTask;
        AzureService azureService;
        public AddNewTask()
        {
            azureService = new AzureService();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddNewTask);

            SupportToolbar mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Add New Task!";

            Button mbtnAddTask = FindViewById<Button>(Resource.Id.btnAddTask);
            mbtnAddTask.Click += mbtnAddTask_Click;

            mTbLytTask = FindViewById<TableLayout>(Resource.Id.TbLytTask);
        }

        async void mbtnAddTask_Click(object sender, EventArgs e)
        {
            await azureService.AddProjectTask(mTbLytTask);
        }
    }
}