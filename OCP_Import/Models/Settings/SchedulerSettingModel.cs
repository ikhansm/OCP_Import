using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OCP_Import.Models.Settings
{
    public class SchedulerSettingModel
    {
        [Required]
        public int sellerId { get; set; }
        [Required]
        public string ftpHost { get; set; }
        [Required]
        public string ftpUserName{ get; set; }
        [Required]
        public string ftpPassword { get; set; }
        [Required] 
        public string ftpPort { get; set; }
        [Required] 
        public string ftpFilePath { get; set; }
        [Required] 
        public string syncTime { get; set; }
        [Required]
        public string brand { get; set; }

    }
}