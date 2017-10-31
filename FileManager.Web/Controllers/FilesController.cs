using FileManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
               var user = await db.AppUsers.SingleAsync(e => e.UserName.Equals(User.Identity.Name));

               return View(db.UserFiles.Where(e => e.AppUser.UserName.Equals(User.Identity.Name)).Select(e => e.File));
          }

          [HttpGet()]
          public async Task<IActionResult> Create()
          {
               #region Ddl Stuff
               var users = await db.AppUsers.Where(e => !e.UserName.Equals(User.Identity.Name) && !e.UserName.Equals("admin@filemanager.com.ng")).ToListAsync();

               var userNames = users.Select(e => new
               {
                    e.Id,
                    Name = e.Firstname + " " + e.Lastname
               });

               ViewData["usersId"] = new MultiSelectList(userNames, "Id", "Name");

               #endregion

               return View();
          }

          [HttpPost]
          public async Task<IActionResult> Create(Model.File modelFile, IFormFile file, IFormCollection collection)
          {

               var user = await db.AppUsers.SingleAsync(e => e.UserName.Equals(User.Identity.Name));

               IList<AppUser> users = await db.AppUsers.Where(e => !e.UserName.Equals(User.Identity.Name) || !e.UserName.Equals("admin@filemanager.com.ng")).ToListAsync();

               modelFile.Date = DateTime.Now;
               //modelFile.UserFiles = user.UserFiles;
               //modelFile.IsLocked = true;

               modelFile.Tagname = modelFile.Name.ToLower().Replace(" ", "-").Replace("/", "-") + Guid.NewGuid().ToString().Replace(" ", "-").Substring(0, 10);

               var usersCount = collection["Users"].Count;
               var selectedValues = collection["Users"].ToString().Split(',');
               var userFiles = new UserFile[selectedValues.Count() + 1];
               var userNotifications = new Notification[selectedValues.Count()];


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
                    
                    if(selectedValues.Count() < 1)
                    {
                         for (int i = 0; i < selectedValues.Count(); i++)
                         {
                              var modelUser = new UserFile()
                              {
                                   File = modelFile,
                                   AppUser = users.Single(e => e.Id == selectedValues[i])
                              };
                              userFiles[i] = modelUser;
                              db.Entry(modelUser).State = EntityState.Added;

                              var notify = new Notification()
                              {
                                   Action = "Some Links Here",
                                   AppUser = users.Single(e => e.Id == selectedValues[i]),
                                   IsRead = false,
                                   Date = DateTime.Now,
                                   Message = user.Firstname + " " + user.Lastname + " shared a file with you"
                              };

                              userNotifications[i] = notify;
                              db.Entry(notify).State = EntityState.Added;
                         }

                         var ownerUser = new UserFile()
                         {
                              File = modelFile,
                              AppUser = user
                         };
                         userFiles[selectedValues.Count()] = ownerUser;
                         db.Entry(ownerUser).State = EntityState.Added;

                         if (ModelState.IsValid)
                         {
                              await db.Files.AddAsync(modelFile);
                              await db.Notifications.AddRangeAsync(userNotifications);
                              await db.UserFiles.AddRangeAsync(userFiles);
                              db.Entry(modelFile).State = EntityState.Added;
                              await db.SaveChangesAsync();

                              return RedirectToAction("Index");
                         }
                    }
                    else
                    {
                         var ownerUser = new UserFile()
                         {
                              File = modelFile,
                              AppUser = user
                         };
                         //userFiles[selectedValues.Count()] = ownerUser;
                         db.Entry(ownerUser).State = EntityState.Added;

                         if (ModelState.IsValid)
                         {
                              await db.Files.AddAsync(modelFile);
                              await db.UserFiles.AddAsync(ownerUser);
                              db.Entry(modelFile).State = EntityState.Added;
                              await db.SaveChangesAsync();

                              return RedirectToAction("Index");
                         }
                    }
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
