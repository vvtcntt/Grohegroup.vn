using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROHE.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
namespace GROHE.Controllers.Admin.User
{
    public class UsersController : Controller
    {
        private GROHEContext db = new GROHEContext();

        //
        // GET: /Users/

        public ActionResult Index(int? page, string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var ListUser = db.tblUsers.ToList();
             
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            // Thiết lập phân trang
            var ship = new PagedListRenderOptions
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                DisplayLinkToLastPage = PagedListDisplayMode.Always,
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                DisplayLinkToIndividualPages = true,
                DisplayPageCountAndCurrentLocation = false,
                MaximumPageNumbersToDisplay = 5,
                DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                EllipsesFormat = "&#8230;",
                LinkToFirstPageFormat = "Trang đầu",
                LinkToPreviousPageFormat = "«",
                LinkToIndividualPageFormat = "{0}",
                LinkToNextPageFormat = "»",
                LinkToLastPageFormat = "Trang cuối",
                PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                FunctionToDisplayEachPageNumber = null,
                ClassToApplyToFirstListItemInPager = null,
                ClassToApplyToLastListItemInPager = null,
                ContainerDivClasses = new[] { "pagination-container" },
                UlElementClasses = new[] { "pagination" },
                LiElementClasses = Enumerable.Empty<string>()
            };
            ViewBag.ship = ship;
            return View(ListUser.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Users/Details/5

        

        public ActionResult Create()
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }

            return View();
        }

        //
        // POST: /Users/Create

        [HttpPost] 
        public ActionResult Create(tblUser tbluser)
        {
           
                tbluser.Password = EncryptandDecrypt.Encrypt(tbluser.Password);
                string test = EncryptandDecrypt.Decrypt("Frv8QWyCKRtbUXab4v7IhA==");
                tbluser.DateCreate = DateTime.Now;
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tbluser.idUser = int.Parse(idUser);
                db.tblUsers.Add(tbluser);
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Add User", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
            return Redirect("Index");
            
        }
        public ActionResult Command(FormCollection collection)
        {

            if (collection["btnDeleteAll"] != null)
            {
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.StartsWith("chkitem+"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 8));

                            var Del = (from emp in db.tblUsers where emp.id == id select emp).SingleOrDefault();

                            db.tblUsers.Remove(Del);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete User", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                            return Redirect("Index");
                        }
                    }
                }
            }
            return RedirectToAction("Index");


        }
        //
        // GET: /Users/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblUser tbluser = db.tblUsers.Find(id);
            if (tbluser == null)
            {
                return HttpNotFound();
            }
            return View(tbluser);
        }

        //
        // POST: /Users/Edit/5

        [HttpPost] 
        public ActionResult Edit(tblUser tbluser, int id)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbluser).State = EntityState.Modified;
                var user = db.tblUsers.First(p => p.id == id);
                string pass=tbluser.Password;
                if (pass == user.Password)
                {
                    tbluser.Password = EncryptandDecrypt.Encrypt(tbluser.Password);
                }
                else
                {
                   
                    tbluser.Password = EncryptandDecrypt.Encrypt(tbluser.Password);
                }
                tbluser.UserName = user.UserName;
                tbluser.DateCreate = DateTime.Now;
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tbluser.idUser = int.Parse(idUser);
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit User", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                return RedirectToAction("Index");
            }
            return View(tbluser);
        }

        //
        // GET: /Users/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblUser tbluser = db.tblUsers.Find(id);
            if (tbluser == null)
            {
                return HttpNotFound();
            }
            return View(tbluser);
        }

        //
        // POST: /Users/Delete/5

    
        public ActionResult DeleteConfirmed(int id)
        {
            tblUser tbluser = db.tblUsers.Find(id);
            db.tblUsers.Remove(tbluser);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete User", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
     
 
    }
}