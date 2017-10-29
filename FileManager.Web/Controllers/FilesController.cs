using FileManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Web.Controllers
{
     [Authorize]
     public class FilesController : Controller
     {
          AppDbContext db = new AppDbContext();
          public IHostingEnvironment _environment;

          public FilesController(IHostingEnvironment environment)
          {
               _environment = environment;
          }

          public async Task<IActionResult> Index()
          {
               return View(await db.Files.Where(e => e.AppUser.UserName.Equals(User.Identity.Name)).ToListAsync());
          }

          [HttpGet()]
          public async Task<IActionResult> Create()
          {
               return View();
          }

          [HttpPost]
          public async Task<IActionResult> Create(Model.File modelFile, IFormFile file)
          {

               modelFile.Date = DateTime.Now;
               modelFile.AppUser = await db.AppUsers.SingleAsync(e => e.UserName.Equals(User.Identity.Name));
               //modelFile.IsLocked = true;
               modelFile.Tagname = modelFile.Name.ToLower().Replace(" ", "-").Replace("/", "-") + Guid.NewGuid().ToString().Replace(" ", "-").Substring(0, 10);

               var directory = Path.Combine(_environment.WebRootPath, "UserFiles");

               if (file.Length > 0)
               {
                    if (!Directory.Exists(directory))
                    {
                         Directory.CreateDirectory(directory);
                    }

                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.GetFullPath(directory);

                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create, FileAccess.ReadWrite))
                    {
                         await file.CopyToAsync(fileStream);
                         modelFile.FileUrl = fileName;
                    }
               }

               if (ModelState.IsValid)
               {
                    await db.Files.AddAsync(modelFile);
                    db.Entry(modelFile).State = EntityState.Added;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
               }

               return View(modelFile);
          }

          [HttpGet("Edit/{tag}")]
          public async Task<IActionResult> Edit(string tag)
          {
               return View(await db.Files.SingleAsync(e => e.Tagname.Equals(tag)));
          }

          [HttpPost("Edit/{tag}")]
          public async Task<IActionResult> Edit(string tag, Model.File modelFile, IFormFile file)
          {
               History fileHistory = new History();

               fileHistory.Date = DateTime.Now;
               fileHistory.IsApproved = false;
               fileHistory.Action = "Content Edited";

               modelFile.Histories.Add(fileHistory);

               if (ModelState.IsValid)
               {
                    db.Files.Attach(modelFile);
                    db.Entry(modelFile).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
               }

               return View(await db.Files.SingleAsync(e => e.Tagname.Equals(tag)));
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}
