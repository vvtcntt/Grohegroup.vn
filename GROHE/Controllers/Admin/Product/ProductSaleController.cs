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
namespace GROHE.Controllers.Admin.Product
{
    public class ProductSaleController : Controller
    {
        private GROHEContext db = new GROHEContext();

        //
        // GET: /ProductSale/

        public ActionResult Index(int? page, string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var ListProductSale = db.tblProductSales.ToList();

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
            return View(ListProductSale.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /ProductSale/Details/5

        public ActionResult Details(int id = 0)
        {
            tblProductSale tblproductsale = db.tblProductSales.Find(id);
            if (tblproductsale == null)
            {
                return HttpNotFound();
            }
            return View(tblproductsale);
        }

        //
        // GET: /ProductSale/Create

        public ActionResult Create()
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var pro = db.tblProductSales.OrderByDescending(p => p.Ord).Take(1).ToList();
            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;
            return View();
        }

        //
        // POST: /ProductSale/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblProductSale tblproductsale)
        {
             
                tblproductsale.DateCreate = DateTime.Now;

                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblproductsale.UserID = int.Parse(idUser);
                tblproductsale.Tag = StringClass.NameToTag(tblproductsale.Name);
                db.tblProductSales.Add(tblproductsale);
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Add tblproductsale", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                return RedirectToAction("Index");
             
             
        }

        //
        // GET: /ProductSale/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblProductSale tblproductsale = db.tblProductSales.Find(id);
            if (tblproductsale == null)
            {
                return HttpNotFound();
            }
            return View(tblproductsale);
        }

        //
        // POST: /ProductSale/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tblProductSale tblproductsale)
        {
            if (ModelState.IsValid)
            {
                tblproductsale.DateCreate = DateTime.Now;

                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblproductsale.UserID = int.Parse(idUser);
                tblproductsale.Tag = StringClass.NameToTag(tblproductsale.Name);
                db.Entry(tblproductsale).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tblproductsale", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                return RedirectToAction("Index");
            }
            return View(tblproductsale);
        }

        //
        // GET: /ProductSale/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblProductSale tblproductsale = db.tblProductSales.Find(id);
            if (tblproductsale == null)
            {
                return HttpNotFound();
            }
            return View(tblproductsale);
        }
        public ActionResult DeleteConfirmed(int id)
        {
            tblProductSale tblproductsale = db.tblProductSales.Find(id);
            db.tblProductSales.Remove(tblproductsale);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult ProductSaleEditOrd(int txtSort, string ts)
        {
            var Support = db.tblProductSales.Find(txtSort);
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
        public ActionResult ProductSaleEditActive(string chk, string nchecked)
        {

            var ProductSale = db.tblProductSales.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                ProductSale.Active = false;
            }
            else
            { ProductSale.Active = true; }

            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified; 
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active ProductSale", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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

                            tblProductSale ProductSale = db.tblProductSales.Find(id);
                            db.tblProductSales.Remove(ProductSale);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete ProductSale", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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