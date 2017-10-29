using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Web.Models.DownloadViewModels
{
     public class DownloadViewModel
     {
          [Required]
          [Display(Name = "User ID")]
          public string AppUserId { get; set; }

          [Required]
          [Display(Name = "File ID")]
          public int FileId { get; set; }
     }
}
