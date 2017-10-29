using FileManager.Model;
using FileManager.Web.Models.DownloadViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Web.Api
{
     [Route("Api/Downloads/"), Produces("application/json")]
     public class DownloadsController : Controller
     {
          AppDbContext db = new AppDbContext();

          [HttpPost]
          public async void Create(DownloadViewModel model)
          {
               Download download = new Download();
               download.Date = DateTime.Now;
               download.FileId = model.FileId;
               download.AppUserId = model.AppUserId;

               if (ModelState.IsValid)
               {
                    await db.Downloads.AddAsync(download);
                    db.Entry(download).State = EntityState.Added;
                    await db.SaveChangesAsync();
               }
          }
     }
}
