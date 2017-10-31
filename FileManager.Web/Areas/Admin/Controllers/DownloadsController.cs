using FileManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Web.Areas.Admin.Controllers
{
     [Authorize(Roles = "Admin"), Area("Admin"), Route("Admin/Downloads/")]
     public class DownloadsController : Controller
    {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               return View(await db.Downloads.Include(e => e.File).ThenInclude(e => e.UserFiles).ToListAsync());
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}
