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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace The_Bureau_2.DbClasses
{
    public class ProjectTaskDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "taskName")]
        public string TaskName { get; set; }

        [JsonProperty(PropertyName = "taskDescription")]
        public string TaskDescription { get; set; }

        [JsonProperty(PropertyName = "stageID")]
        public string StageID { get; set; }

        [JsonProperty(PropertyName = "taskPrecedence")]
        public string TaskPrecedence { get; set; }

        [JsonProperty(PropertyName = "projectID")]
        public string ProjectID { get; set; }

        [JsonProperty(PropertyName = "personnellID")]
        public string PersonnellID { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateTime? StartDate { get; set; }

        [JsonProperty(PropertyName = "finishDate")]
        public DateTime? FinishDate { get; set; }

        [JsonProperty(PropertyName = "taskStatus")]
        public string TaskStatus { get; set; }

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