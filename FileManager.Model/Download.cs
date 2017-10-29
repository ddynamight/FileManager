using System;

namespace FileManager.Model
{
     public class Download
     {
          public int Id { get; set; }
          public DateTime Date { get; set; }
          public string AppUserId { get; set; }
          public int FileId { get; set; }

          //One to Many Download Relationships
          public virtual AppUser AppUser { get; set; }
          public virtual File File { get; set; }
     }
}
