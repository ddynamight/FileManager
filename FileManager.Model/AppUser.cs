using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FileManager.Model
{
     public class AppUser : IdentityUser
     {
          public AppUser()
          {
               Downloads = new HashSet<Download>();
               Files = new HashSet<File>();
               Histories = new HashSet<History>();
               Notifications = new HashSet<Notification>();
          }

          public string Firstname { get; set; }
          public string Lastname { get; set; }
          public DateTime DateReg { get; set; }

          //One AppUser to Many Relationship
          public virtual ICollection<Download> Downloads { get; set; }
          public virtual ICollection<File> Files { get; set; }
          public virtual ICollection<History> Histories { get; set; }
          public virtual ICollection<Notification> Notifications { get; set; }
     }
}
