using FileManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Web.Areas.Admin.Controllers
{
     [Authorize(Roles = "Admin"), Area("Admin"), Route("Admin/Files/")]
     public class FilesController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               return View(await db.UserFiles.Include(e => e.File).Include(e => e.AppUser).ToListAsync());
          }

          [HttpGet("Create")]
          public async Task<IActionResult> Create()
          {
               return View();
          }

          [HttpPost("Create")]
          public async Task<IActionResult> Create(File modelFile, IFormFile file)
          {
               var user = await db.AppUsers.SingleAsync(e => e.UserName.Equals(User.Identity.Name));

               modelFile.Date = DateTime.Now;
               modelFile.UserFiles = user.UserFiles;

               if (ModelState.IsValid)
               {
                    await db.Files.AddAsync(modelFile);
                    db.Entry(modelFile).State = EntityState.Added;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
               }

               return View(modelFile);
          }

          [HttpGet("Details/{tag}")]
          public async Task<IActionResult> Details(string tag)
          {
               return View(await db.Files.Include(e => e.UserFiles).Include(e => e.Downloads).Include(e => e.Histories).SingleAsync(e => e.Tagname.Equals(tag)));
          }

          [HttpGet("Edit/{tag}")]
          public async Task<IActionResult> Edit(string tag)
          {
               return View(await db.Files.Include(e => e.UserFiles).Include(e => e.Downloads).Include(e => e.Histories).SingleAsync(e => e.Tagname.Equals(tag)));
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}
