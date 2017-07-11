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
namespace GROHE.Controllers.Admin.FunctionProduct
{
    public class FunctionProductController : Controller
    {
        private GROHEContext db = new GROHEContext();
         public ActionResult Index(int? page, string id)
        {
          if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var listUrl = db.tblFunctionProducts.ToList();

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
            return View(listUrl.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /FunctionProduct/Details/5

         

        //
        // GET: /FunctionProduct/Create

        public ActionResult Create()
         {
             if ((Request.Cookies["Username"] == null))
             {
                 return RedirectToAction("LoginIndex", "Login");
             }
             var pro = db.tblFunctionProducts.OrderByDescending(p => p.Ord).Take(1).ToList();
             if (pro.Count > 0)
                 ViewBag.Ord = pro[0].Ord + 1;
             else
                 ViewBag.Ord = "1";
            return View();
        }

        //
        // POST: /FunctionProduct/Create

        [HttpPost]
         public ActionResult Create(tblFunctionProduct tblfunctionproduct)
         {
            string idUser = Request.Cookies["Username"].Values["UserID"];
            db.tblFunctionProducts.Add(tblfunctionproduct);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Add tblfunctionproduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return RedirectToAction("Index");

         }

        //
        // GET: /FunctionProduct/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblFunctionProduct tblfunctionproduct = db.tblFunctionProducts.Find(id);
            if (tblfunctionproduct == null)
            {
                return HttpNotFound();
            }
            return View(tblfunctionproduct);
        }

        //
        // POST: /FunctionProduct/Edit/5

        [HttpPost]
         public ActionResult Edit(tblFunctionProduct tblfunctionproduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblfunctionproduct).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tblfunctionproduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                return RedirectToAction("Index");
            }
            return View(tblfunctionproduct);
        }

        //
        // GET: /FunctionProduct/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblFunctionProduct tblfunctionproduct = db.tblFunctionProducts.Find(id);
            if (tblfunctionproduct == null)
            {
                return HttpNotFound();
            }
            return View(tblfunctionproduct);
        }

        //
        // POST: /FunctionProduct/Delete/5

          public ActionResult DeleteConfirmed(int id)
        {
            tblFunctionProduct tblfunctionproduct = db.tblFunctionProducts.Find(id);
            db.tblFunctionProducts.Remove(tblfunctionproduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult FunctionProductEditOrd(int txtSort, string ts)
        {
            var Url = db.tblFunctionProducts.Find(txtSort);
            var result = string.Empty;
            Url.Ord = int.Parse(ts);
            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified;
            result = "Update FunctionProduct.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord FunctionProduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult FunctionProductEditActive(string chk, string nchecked)
        {

            var Url = db.tblFunctionProducts.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                Url.Active = false;
            }
            else
            { Url.Active = true; }

            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified; 
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active FunctionProduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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
                            tblFunctionProduct tblurl = db.tblFunctionProducts.Find(id);
                            db.tblFunctionProducts.Remove(tblurl);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete FunctionProduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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