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
namespace GROHE.Controllers.Admin.Manufacturers
{
    public class ManufacturesController : Controller
    {
        private GROHEContext db = new GROHEContext();

        //
        // GET: /Manufacturers/

        public ActionResult Index(int? page, string text, string idCate)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            string txtSearch = "";
            if (idCate != "" && idCate != null)
            { 
                ViewBag.idMenu = idCate;
            }
            var listManufacturers = (from p in db.tblManufactures select p).ToList();
            if (txtSearch != null && txtSearch != "")
            {
                listManufacturers = db.tblManufactures.Where(p => p.Name.Contains(txtSearch)).ToList();
            }
            Session["txtSearch"] = null;
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
            if (Request.IsAjaxRequest())
            {
             
                if (text != null && text != "")
                {
                    var list = db.tblManufactures.Where(p => p.Name.ToUpper().Contains(text.ToUpper())).ToList();
                    return PartialView("ManufacturesPartial", list);
                }
                 

                else
                {
                    return PartialView("ManufacturesPartial", listManufacturers);
                }
            }
             
            return View(listManufacturers.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Manufacturers/Details/5
        public PartialViewResult ManufacturesPartial()
        {
            return PartialView();
        }
        public ActionResult Create(string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var pro = db.tblManufactures.OrderByDescending(p => p.Ord).Take(1).ToList();
            
            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;
            return View();
        }
        // POST: /Manufacturers/Create

        [HttpPost]
         public ActionResult Create(tblManufacture tblManufacture , FormCollection Collection )
        {
            if (ModelState.IsValid)
            {
                string nidCate = Collection["drMenu"];
                if (nidCate != "")
                {
                     tblManufacture.DateCreate = DateTime.Now;
                     tblManufacture.Tag = StringClass.NameToTag(tblManufacture.Name);
                     string idUser = Request.Cookies["Username"].Values["UserID"];
                     tblManufacture.idUser = int.Parse(idUser);
                     db.tblManufactures.Add(tblManufacture);
                    db.SaveChanges();
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Add tblManufacture", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion

                }
                var listmanufactures = db.tblManufactures.OrderByDescending(p => p.id).Take(1).ToList();
                clsSitemap.CreateSitemap("6/" + StringClass.NameToTag(tblManufacture.Name), listmanufactures[0].id.ToString(), "Manufactures");
                return RedirectToAction("Index");
            }

            return View(tblManufacture);
        }

        //
        // GET: /Manufacturers/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblManufacture tblManufacture = db.tblManufactures.Find(id);
            if (tblManufacture == null)
            {
                return HttpNotFound();
            }
            


            
            return View(tblManufacture);
        }

        //
        // POST: /Manufacturers/Edit/5

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Edit(tblManufacture tblManufacture, FormCollection collection,int id)
        {
            if (ModelState.IsValid)
            {
                
                 tblManufacture.DateCreate = DateTime.Now;
                 string idUser = Request.Cookies["Username"].Values["UserID"];
                 tblManufacture.idUser = int.Parse(idUser);
                 tblManufacture.Tag = StringClass.NameToTag(tblManufacture.Name);
                 db.Entry(tblManufacture).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tblManufacture", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                clsSitemap.UpdateSitemap("6/"+StringClass.NameToTag(tblManufacture.Name),id.ToString(), "Manufactures");
                return RedirectToAction("Index");
            }
            return View(tblManufacture);
        }

        //
        // GET: /Manufacturers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblManufacture tblManufacture = db.tblManufactures.Find(id);
            if (tblManufacture == null)
            {
                return HttpNotFound();
            }
            return View(tblManufacture);
        }

        //
        // POST: /Manufacturers/Delete/5

        [HttpPost]
         public ActionResult DeleteConfirmed(int id)
        {
            tblManufacture tblManufacture = db.tblManufactures.Find(id);
            db.tblManufactures.Remove(tblManufacture);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult ManufacturesEditOrd(int txtSort, string ts)
        {
            var Product = db.tblManufactures.Find(txtSort);
            var result = string.Empty;
            Product.Ord = int.Parse(ts);
            //db.Entry(Product).State = System.Data.EntityState.Modified;
            result = "Ord Updated";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord Manufacturers", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult ManufacturesEditActive(string chk, string nchecked)
        {
            var Product = db.tblManufactures.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                Product.Active = false;
            }
            else
            { Product.Active = true; }
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active Manufacturers", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Active Updated.";
            return Json(new { result = result });
        }

        #region[Delete]

        public ActionResult DeleteManufacturers(int id)
        {
            clsSitemap.DeteleSitemap(id.ToString(), "Manufactures");
            tblManufacture tblManufactures = db.tblManufactures.Find(id);

            var result = string.Empty;
            db.tblManufactures.Remove(tblManufactures);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete tblManufactures", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Bạn đã xóa thành công.";
            return Json(new { result = result });

        }
        [HttpPost]
        public string CheckValue(string text)
        {
            string chuoi = "";
            var listProduct = db.tblManufactures.Where(p => p.Name == text).ToList();
            if (listProduct.Count > 0)
            {
                chuoi = "Duplicate Name !";

            }
            Session["Check"] = listProduct.Count;
            return chuoi;
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
                            var Del = (from emp in db.tblManufactures where emp.id == id select emp).SingleOrDefault();
                            db.tblManufactures.Remove(Del);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete tblManufactures", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                            clsSitemap.DeteleSitemap(id.ToString(), "Manufactures");
                        }
                    }
                }
                return RedirectToAction("Index");
            }

         
            return View();
        }

        [HttpPost]
        public ActionResult print(FormCollection a)
        {
            string chuoi = "";
            chuoi = "<script type=\"text/javascript\">$(document).ready(function() {window.print();});</script>";
            ViewBag.Print = chuoi;
            return View("Printer");
        }

        #endregion
        #region[Search]
        public ActionResult Search(string Name, string idCate)
        {
            if (Name != null || idCate != null)
            {
                Session["txtSearch"] = Name;
                Session["idCate"] = idCate;

            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}