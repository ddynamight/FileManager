using FileManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Web.Controllers
{
     [Authorize]
     public class AllFilesController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               return View(db.UserFiles.Include(e => e.AppUser).Include(e => e.File).Where(e => e.File.IsLocked.Equals(false)));
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}
