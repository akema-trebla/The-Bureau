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
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using The_Bureau_2.DbClasses;
using System.Threading.Tasks;
using System.IO;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using ModernHttpClient;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using The_Bureau_2.Fragments;

namespace The_Bureau_2
{
    public class AzureService
    {
        public MobileServiceClient client { get; set; }
        //IMobileServiceSyncTable<User> BureauUser;
        IMobileServiceSyncTable<ProjectTaskDto> BureauTask;
        IMobileServiceSyncTable<DbClasses.ActivityDto> BureauActivity;

         bool isInitialized;
        public async Task Initialize()
        {
            if (isInitialized)
                return;

            
            //Create our client
            client = new MobileServiceClient("https://bureauappservice.azurewebsites.net"
                , new NativeMessageHandler()
                );
            
            var token = new AuthenticationToken();
            if (!string.IsNullOrWhiteSpace(token.Access_Token) && !string.IsNullOrWhiteSpace(token.UserID.ToString()))
            {
                client.CurrentUser = new MobileServiceUser(token.UserID.ToString());
                client.CurrentUser.MobileServiceAuthenticationToken = token.Access_Token;
            }

            const string localDbFilename = "localstore.db";
    
            // new code to initialize the SQLite store
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), localDbFilename);

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            var store = new MobileServiceSQLiteStore(path);
            //store.DefineTable<User>();
            store.DefineTable<The_Bureau_2.DbClasses.ActivityDto>();
            store.DefineTable<ProjectTaskDto>();

            // Uses the default conflict handler, which fails on conflict
            // To use a different conflict handler, pass a parameter to InitializeAsync. For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
            await client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            // Get the Mobile Service sync table instance to use
            //BureauUser = client.GetSyncTable<User>();
            BureauActivity = client.GetSyncTable<DbClasses.ActivityDto>();
            BureauTask = client.GetSyncTable<ProjectTaskDto>();

            isInitialized = true;
        }

        public async Task<AuthenticationToken> GetAuthenticationToken(string email, string password)
        {
            await Initialize();
  
            // define request content
            HttpContent content = new StringContent(
            string.Format("username={0}&password={1}&grant_type=password",
                          email.ToLower(),
                          password));

            // set header
            content.Headers.ContentType
            = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            // invoke Api
            HttpResponseMessage response
                = await client.InvokeApiAsync("/oauth/token",
                                               content,
                                                HttpMethod.Post,
                                               null,
                                               null);

            // read and parse token
            string flatToken = await response.Content.ReadAsStringAsync();
            return JsonConvert
                    .DeserializeObject<AuthenticationToken>(flatToken);
        }

        public async Task Authenticate(string email, string password)
        {
            if (isInitialized)
                return;

            // get the token
            var token = await GetAuthenticationToken(email, password);

            // authenticate: create and use a mobile service user
            MobileServiceUser user = new MobileServiceUser(token.UserID.ToString());
            user.MobileServiceAuthenticationToken = token.Access_Token;
            client.CurrentUser = user;

            isInitialized = true;
        }

        public async Task<List<DbClasses.ActivityDto>> GetActivities()
        {
            await Initialize();
            await SyncActivities(pullData: true);
            return await BureauActivity.OrderByDescending(c => c.Id).ToListAsync();
        }

        public async Task SyncActivities(bool pullData = false)
        {
            try
            {
                await client.SyncContext.PushAsync();

                if (pullData)
                {
                    await BureauActivity.PullAsync("allActivities", BureauActivity.CreateQuery()); // query ID is used for incremental sync
                }
            }
            catch (Java.Net.MalformedURLException)
            {
                System.Diagnostics.Debug.WriteLine(new System.Exception("There was an error creating the Mobile Service. Verify the URL"), "URL Error");
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to sync Activities, that is alright as we have offline capabilities: " + e);
            }

        }

        [Java.Interop.Export()]
        public async Task<DbClasses.ActivityDto> AddActivity(View view)
        {
            await Initialize();

            var activity = new DbClasses.ActivityDto()
            {
                Activity_Date = DateTime.Parse(view.FindViewById<EditText>(Resource.Id.txtActivityDate).Text),
                Time_Stamp = DateTime.UtcNow,
                Person = view.FindViewById<EditText>(Resource.Id.txtPersonName).Text,
                Project = view.FindViewById<EditText>(Resource.Id.txtProject).Text,
                Activity_Type = view.FindViewById<EditText>(Resource.Id.txtActivityType).Text,
                Customer = view.FindViewById<EditText>(Resource.Id.txtCustomer).Text,
                Description = view.FindViewById<EditText>(Resource.Id.txtADescription).Text,
                Activity_Cash_Paid =  decimal.Parse(view.FindViewById<EditText>(Resource.Id.txtActivityCashPaid).Text),
                Activity_Cash_Received = decimal.Parse(view.FindViewById<EditText>(Resource.Id.txtActivityCashReceived).Text),
                Activity_Cost = decimal.Parse(view.FindViewById<EditText>(Resource.Id.txtActivityCost).Text),
                Activity_Revenue = decimal.Parse(view.FindViewById<EditText>(Resource.Id.txtActivityRevenue).Text),
                txtDate = view.FindViewById<EditText>(Resource.Id.txtTextDate).Text
            };

            await BureauActivity.InsertAsync(activity); // insert the new item into the local database

            await SyncActivities(); // send activities to the mobile service

            return activity;
        }

        public async Task<IEnumerable<ProjectTaskDto>> GetProjectTasks()
        {
            await Initialize();
            await SyncProjectTasks(pullData: true);
            return await BureauTask.OrderByDescending(c => c.Id).ToEnumerableAsync();
        }

        public async Task SyncProjectTasks(bool pullData = false)
        {
            try
            {
                await client.SyncContext.PushAsync();

                if (pullData)
                {
                    await BureauTask.PullAsync("allTasks", BureauTask.CreateQuery()); // query ID is used for incremental sync
                }
            }
            catch (Java.Net.MalformedURLException)
            {
                System.Diagnostics.Debug.WriteLine(new System.Exception("There was an error creating the Mobile Service. Verify the URL"), "URL Error");
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to sync ProjectTasks, that is alright as we have offline capabilities: " + e);
            }
        }

         [Java.Interop.Export()]
        public async Task<ProjectTaskDto> AddProjectTask(View view)
        {
            await Initialize();

            var projectTask = new ProjectTaskDto()
            {
                TaskName = view.FindViewById<EditText>(Resource.Id.txtTaskName).Text,
                TaskDescription = view.FindViewById<EditText>(Resource.Id.txtTDescription).Text,
                TaskPrecedence = view.FindViewById<EditText>(Resource.Id.txtTaskPrecedence).Text,
                StageID = view.FindViewById<EditText>(Resource.Id.txtStageID).Text,
                ProjectID = view.FindViewById<EditText>(Resource.Id.txtProjectID).Text,
                PersonnellID = view.FindViewById<EditText>(Resource.Id.txtPersonnelID).Text,
                StartDate = DateTime.Parse(view.FindViewById<EditText>(Resource.Id.txtStartDate).Text),
                FinishDate = DateTime.Parse(view.FindViewById<EditText>(Resource.Id.txtFinishDate).Text)
            };

            await BureauTask.InsertAsync(projectTask);

            await SyncProjectTasks();

            return projectTask;
         }
    }
}