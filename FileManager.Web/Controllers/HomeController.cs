using FileManager.Model;
using FileManager.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FileManager.Web.Controllers
{
     [Authorize]
     public class HomeController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {

               ViewData["userCounter"] = await db.AppUsers.CountAsync(e => !e.UserName.Equals("admin@filemanager.com.ng"));
               ViewData["fileCounter"] = await db.Files.CountAsync(e => e.AppUser.UserName.Equals(User.Identity.Name));
               ViewData["downloadCounter"] = await db.Downloads.CountAsync(e => e.AppUser.UserName.Equals(User.Identity.Name));
               ViewData["historyCounter"] = await db.Histories.CountAsync(e => e.AppUser.UserName.Equals(User.Identity.Name));

               return View();
          }

          public IActionResult Error()
          {
               return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}
