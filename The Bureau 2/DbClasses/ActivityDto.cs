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
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace The_Bureau_2.DbClasses
{
    public class ActivityDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "time_Stamp")]
        public DateTime? Time_Stamp { get; set; }

        [JsonProperty(PropertyName = "activity_Date")]
        public DateTime? Activity_Date { get; set; }

        [JsonProperty(PropertyName = "person")]
        public string Person { get; set; }

        [JsonProperty(PropertyName = "project")]
        public string Project { get; set; }

        [JsonProperty(PropertyName = "activity_Type")]
        public string Activity_Type { get; set; }

        [JsonProperty(PropertyName = "customer")]
        public string Customer { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "activity_Cost")]
        public decimal? Activity_Cost { get; set; }

        [JsonProperty(PropertyName = "activity_Revenue")]
        public decimal? Activity_Revenue { get; set; }

        [JsonProperty(PropertyName = "activity_Cash_Received")]
        public decimal? Activity_Cash_Received { get; set; }

        [JsonProperty(PropertyName = "activity_Cash_Paid")]
        public decimal? Activity_Cash_Paid { get; set; }

        [JsonProperty(PropertyName = "txtDate")]
        public string txtDate { get; set; }

        [JsonProperty(PropertyName = "taskID")]
        public string TaskID { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
    }
}