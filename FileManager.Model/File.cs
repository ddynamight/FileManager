using System;
using System.Collections.Generic;

namespace FileManager.Model
{
     public class File
     {
          public File()
          {
               Downloads = new HashSet<Download>();
               Histories = new HashSet<History>();
          }

          public int Id { get; set; }
          public string Name { get; set; }
          public DateTime Date { get; set; }
          public bool IsLocked { get; set; }
          public string Tagname { get; set; }
          public string FileUrl { get; set; }
          public string AppUserId { get; set; }

          //One to Many Files Relationships
          public virtual AppUser AppUser { get; set; }

          //One File to Many Relationship
          public virtual ICollection<Download> Downloads { get; set; }
          public virtual ICollection<History> Histories { get; set; }
     }
}
