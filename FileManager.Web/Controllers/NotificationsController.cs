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
     public class NotificationsController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               return View(await db.Notifications.Where(e => e.AppUser.UserName.Equals(User.Identity.Name)).OrderByDescending(e => e.Date).ThenBy(e => e.IsRead).ToListAsync());
          }

          public async Task<IActionResult> Details(int tag)
          {
               var note = await db.Notifications.SingleAsync(e => e.Id.Equals(tag));
               UpdateNotify(note.Id);
               return View(note);
          }


          public void UpdateNotify(int tag)
          {
               var note = db.Notifications.Single(e => e.Id.Equals(tag));

               note.Action = "Read";
               note.IsRead = true;

               if (ModelState.IsValid)
               {
                    db.Notifications.Attach(note);
                    db.Entry(note).State = EntityState.Modified;
                    db.SaveChanges();
               }
          }


          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}
