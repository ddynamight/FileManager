using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Model
{
     public class History
     {
          public int Id { get; set; }
          public string Action { get; set; }
          public DateTime Date { get; set; }
          public bool IsApproved { get; set; }
          public int FileId { get; set; }
          public string AppUserId { get; set; }

          //One to Many Histories Relationships
          public virtual File File { get; set; }
          public virtual AppUser AppUser { get; set; }
     }
}
