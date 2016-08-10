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
    public class AuthenticationToken
    {
        public int UserID { get; set; }
        public string Access_Token { get; set; }
        public string Token_Type { get; set; }

        [JsonProperty(".expires")]
        public DateTime Expires { get; set; }
    }
}