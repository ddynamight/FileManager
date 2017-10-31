using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Model
{
    public class UserFile
    {
          public string AppUserId { get; set; }
          public int FileId { get; set; }

          //One to Many UserFiles Relationships
          public virtual AppUser AppUser { get; set; }
          public virtual File File { get; set; }
     }
}
