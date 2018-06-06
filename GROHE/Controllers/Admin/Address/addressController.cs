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
namespace GROHE.Controllers.Admin.address
{
    public class UrlController : Controller
    {
        private GROHEContext db = new GROHEContext();

        //
        // GET: /Url/

        public ActionResult Index(int? page, string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var listAddress = db.tblAddresses.ToList();

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
            return View(listAddress.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Create()
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var pro = db.tblAddresses.OrderByDescending(p => p.Ord).Take(1).ToList();
            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;
            return View();
        }

        //
        // POST: /Url/Create

        [HttpPost]
        public ActionResult Create(tblAddress tbladdress)
        {

            db.tblAddresses.Add(tbladdress);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Add tbladdress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return RedirectToAction("Index");
        }

        //
        // GET: /Url/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblAddress tbladdress = db.tblAddresses.Find(id);
            if (tbladdress == null)
            {
                return HttpNotFound();
            }
            return View(tbladdress);
        }

        //
        // POST: /Url/Edit/5

        [HttpPost]
        public ActionResult Edit(tblAddress tbladdress)
        {
            if (ModelState.IsValid)
            {

                db.Entry(tbladdress).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tbladdress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                return RedirectToAction("Index");
            }
            return View(tbladdress);
        }

        //
        // GET: /Url/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblAddress tbladdress = db.tblAddresses.Find(id);
            if (tbladdress == null)
            {
                return HttpNotFound();
            }
            return View(tbladdress);
        }

        //
        // POST: /Url/Delete/5


        public ActionResult DeleteConfirmed(int id)
        {
            tblAddress tbladdress = db.tblAddresses.Find(id);
            db.tblAddresses.Remove(tbladdress);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult AddressEditOrd(int txtSort, string ts)
        {
            var tbladdress = db.tblAddresses.Find(txtSort);
            var result = string.Empty;
            tbladdress.Ord = int.Parse(ts);
            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified;
            result = "Update Ord.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord Url", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult AddressEditActive(string chk, string nchecked)
        {

            var tbladdress = db.tblAddresses.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                tbladdress.Active = false;
            }
            else
            { tbladdress.Active = true; }

            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified; 
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active Url", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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
                            tblAddress tbladdress = db.tblAddresses.Find(id);
                            db.tblAddresses.Remove(tbladdress);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete Url", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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