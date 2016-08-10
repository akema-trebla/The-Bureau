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


namespace The_Bureau_2
{
    [Activity(Label = "AddNewActivity", Theme = "@style/Theme.DesignDemo")]
    public class AddNewActivity : MainActivity
    {
        TableLayout mTbLytActivity;
        AzureService azureService;
        public AddNewActivity()
        {
            azureService = new AzureService();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddNewActivity);

            SupportToolbar mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Add New Activity!";

            Button mbtnAddActivity = FindViewById<Button>(Resource.Id.btnAddActivity);
            mbtnAddActivity.Click += mbtnAddActivity_Click;

            mTbLytActivity = FindViewById<TableLayout>(Resource.Id.TbLytActivity);
        }

        private async void mbtnAddActivity_Click(object sender, EventArgs e)
        {
            await azureService.AddActivity(mTbLytActivity);
        }
    }
}