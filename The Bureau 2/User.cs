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
using Newtonsoft.Json;

namespace The_Bureau_2
{
    class User
    {
        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public string User_Name { get; set; }

        [JsonProperty]
        public string Password { get; set; }

        [JsonProperty]
        public int User_LevelID { get; set; }

        [JsonProperty]
        public string User_Description { get; set; }

        [JsonProperty]
        public string Phone1 { get; set; }

        [JsonProperty]
        public string Phone2 { get; set; }

        [JsonProperty]
        public string Email1 { get; set; }

        [JsonProperty]
        public string Email2 { get; set; }
    }
}