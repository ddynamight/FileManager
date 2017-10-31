using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Model
{
     public class Notification
     {
          public int Id { get; set; }
          public string Action { get; set; }
          public string AppUserId { get; set; }
          public string Message { get; set; }
          public bool IsRead { get; set; }
          public DateTime Date { get; set; }

          // One to Many Notification Relationships
          public virtual AppUser AppUser { get; set; }
     }
}
