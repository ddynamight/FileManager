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
     [Authorize(Roles = "Admin"), Area("Admin"), Route("Admin/")]
     public class HomeController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               ViewData["userCounter"] = await db.AppUsers.CountAsync(e => !e.UserName.Equals("admin@filemanager.com.ng"));
               ViewData["fileCounter"] = await db.Files.CountAsync();
               ViewData["downloadCounter"] = await db.Downloads.CountAsync();
               ViewData["historyCounter"] = await db.Histories.CountAsync();

               return View();
          }
     }
}
