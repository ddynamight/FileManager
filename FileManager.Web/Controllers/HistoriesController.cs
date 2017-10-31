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
     public class HistoriesController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               return View(await db.Histories.Where(e => e.AppUser.UserName.Equals(User.Identity.Name)).ToListAsync());
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}
