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

namespace GROHE.Controllers.Admin.Support
{
    public class SupportController : Controller
    {
        private GROHEContext db = new GROHEContext();

        //
        // GET: /Support/

        public ActionResult Index(int? page, string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var ListSupport = db.tblSupports.ToList();

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
            return View(ListSupport.ToPagedList(pageNumber, pageSize));
        }

 

        public ActionResult Create()
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var pro = db.tblSupports.OrderByDescending(p => p.Ord).Take(1).ToList();
            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;
            return View();
        }

        //
        // POST: /Support/Create

        [HttpPost] 
        public ActionResult Create(tblSupport tblsupport)
        {
           
                tblsupport.DateCreate = DateTime.Now;

                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblsupport.idUser = int.Parse(idUser);
                db.tblSupports.Add(tblsupport);
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Add Support", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                return RedirectToAction("Index");
             
        }

        //
        // GET: /Support/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblSupport tblsupport = db.tblSupports.Find(id);
            if (tblsupport == null)
            {
                return HttpNotFound();
            }
            return View(tblsupport);
        }

        //
        // POST: /Support/Edit/5

        [HttpPost] 
        public ActionResult Edit(tblSupport tblsupport)
        {
            if (ModelState.IsValid)
            {
                tblsupport.DateCreate = DateTime.Now;

                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblsupport.idUser = int.Parse(idUser);
                db.Entry(tblsupport).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit Video", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                return RedirectToAction("Index");
            }
            return View(tblsupport);
        }

        //
        // GET: /Support/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSupport tblsupport = db.tblSupports.Find(id);
            if (tblsupport == null)
            {
                return HttpNotFound();
            }
            return View(tblsupport);
        }

        //
        // POST: /Support/Delete/5

    
        public ActionResult DeleteConfirmed(int id)
        {
            tblSupport tblsupport = db.tblSupports.Find(id);
            db.tblSupports.Remove(tblsupport);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult SupportEditOrd(int txtSort, string ts)
        {
            var Support = db.tblSupports.Find(txtSort);
            var result = string.Empty;
            Support.Ord = int.Parse(ts);
            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified;
            result = "Update Ord.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord Support", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult SupportEditActive(string chk, string nchecked)
        {

            var Support = db.tblSupports.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                Support.Active = false;
            }
            else
            { Support.Active = true; }

            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified; 
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active Support", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Updated Active.";
            return Json(new { result = result });
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

                            tblSupport tblsuport = db.tblSupports.Find(id);
                            db.tblSupports.Remove(tblsuport);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete Support", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                            return Redirect("Index");
                        }
                    }
                }
            }
            return RedirectToAction("Index");


        }
    }
}