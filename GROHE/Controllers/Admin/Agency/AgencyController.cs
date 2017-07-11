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
namespace GROHE.Controllers.Admin.Agency
{
    public class AgencyController : Controller
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
         
            var listManufacturers = (from p in db.tblAgencies select p).ToList();
            if (txtSearch != null && txtSearch != "")
            {
                listManufacturers = db.tblAgencies.Where(p => p.Name.Contains(txtSearch)).ToList();
            }
            if (idCate != "" && idCate != null)
            {
                int idcates = int.Parse(idCate);
                listManufacturers = db.tblAgencies.Where(p => p.idMenu == idcates).OrderBy(p => p.Ord).ToList();
                ViewBag.idMenu = idCate;
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
            #region[Load Menu]

            var pro = db.tblAgencies.OrderByDescending(p => p.Ord).Take(1).ToList();
            var Groupmanufacturers = db.tblGroupAgencies.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(Groupmanufacturers.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupAgencies.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {

                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.id.ToString() });


            }
            if (idCate != "" || idCate != null)
            {

                ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", idCate);
            }
            else
                ViewBag.drMenu = lstMenu;
            #endregion

            if (Request.IsAjaxRequest())
            {
                int idCatelogy;
                if (text != null && text != "")
                {
                    var list = db.tblAgencies.Where(p => p.Name.ToUpper().Contains(text.ToUpper())).ToList();
                    return PartialView("AgencyPartial", list);
                }
                if (idCate != null && idCate != "")
                {
                    idCatelogy = int.Parse(idCate);
                    var list = db.tblAgencies.Where(p => p.idMenu == idCatelogy).ToList(); ViewBag.idMenu = idCate;
                    ViewBag.idMenu = idCate;
                    return PartialView("AgencyPartial", list);
                }
                if (text != null && text != "" && idCate != null && idCate != "")
                {
                    idCatelogy = int.Parse(idCate); ViewBag.idMenu = idCate;
                    var list = db.tblAgencies.Where(p => p.Name.ToUpper().Contains(text.ToUpper()) && p.idMenu == (int.Parse(idCate))).ToList();
                    ViewBag.idMenu = idCate;
                    return PartialView("AgencyPartial", list);
                }

                else
                {
                    return PartialView("AgencyPartial", listManufacturers);
                }
            }

            return View(listManufacturers.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Manufacturers/Details/5
        public PartialViewResult AgencyPartial()
        {
            return PartialView();
        }
        public ActionResult Create(string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var pro = db.tblAgencies.OrderByDescending(p => p.Ord).Take(1).ToList();
            var GroupManufacturers = db.tblGroupAgencies.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(GroupManufacturers.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupAgencies.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {

                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.id.ToString() });


            }
            if (id != null && id != "" )
            {

                ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", id);
            }
            else
                ViewBag.drMenu = lstMenu;
            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;
            return View();
        }
        // POST: /Manufacturers/Create

        [HttpPost]
        public ActionResult Create(tblAgency tblagency, FormCollection Collection, int id)
        {
            if (ModelState.IsValid)
            {
                string nidCate = Collection["drMenu"];
                if (nidCate != "")
                {
                    tblagency.idMenu = int.Parse(nidCate);
                    tblagency.DateCreate = DateTime.Now;
                    tblagency.Tag = StringClass.NameToTag(tblagency.Name);
                     string idUser = Request.Cookies["Username"].Values["UserID"];
                     tblagency.idUser = int.Parse(idUser);
                     db.tblAgencies.Add(tblagency);
                    db.SaveChanges();
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Add tblagency", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                    var listagency = db.tblAgencies.OrderByDescending(p => p.id).Take(1).ToList();
                    clsSitemap.CreateSitemap("5/" + StringClass.NameToTag(tblagency.Name), id.ToString(), "Agency");

                }
                return Redirect("Index?idCate=" + id);
            }

            return View(tblagency);
        }

        //
        // GET: /Manufacturers/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblAgency tblagency = db.tblAgencies.Find(id);
            if (tblagency == null)
            {
                return HttpNotFound();
            }
            var GroupAgency = db.tblGroupAgencies.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(GroupAgency.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupAgencies.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {

                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.id.ToString() });


            }



            int idGroups = int.Parse(tblagency.idMenu.ToString());
            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", idGroups);
            return View(tblagency);
        }

        //
        // POST: /Manufacturers/Edit/5

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Edit(tblAgency tblagency, FormCollection collection, int id)
        {
            if (ModelState.IsValid)
            {
                int idCate = int.Parse(collection["drMenu"]);
                tblagency.idMenu = idCate;
                tblagency.DateCreate = DateTime.Now;
                 string idUser = Request.Cookies["Username"].Values["UserID"];
                 tblagency.idUser = int.Parse(idUser);
                 tblagency.Tag = StringClass.NameToTag(tblagency.Name);
                 db.Entry(tblagency).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tblagency", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                clsSitemap.UpdateSitemap("5/"+StringClass.NameToTag(tblagency.Name), id.ToString(),"Agency");
                return Redirect("Index?idCate=" + int.Parse(collection["drMenu"]));
            }
            return View(tblagency);
        }

        //
        // GET: /Manufacturers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblAgency tblagency = db.tblAgencies.Find(id);
            if (tblagency == null)
            {
                return HttpNotFound();
            }
            return View(tblagency);
        }

        //
        // POST: /Manufacturers/Delete/5

          public ActionResult DeleteConfirmed(int id)
        {
            tblAgency tblagency = db.tblAgencies.Find(id);
            db.tblAgencies.Remove(tblagency);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult AgencyEditOrd(int txtSort, string ts)
        {
            var Agenvcy= db.tblAgencies.Find(txtSort);
            var result = string.Empty;
            Agenvcy.Ord = int.Parse(ts);
            //db.Entry(Product).State = System.Data.EntityState.Modified;
            result = "Ord Updated";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord Agenvcy", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult AgencyEditActive(string chk, string nchecked)
        {

            var Agency = db.tblAgencies.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                Agency.Active = false;
            }
            else
            { Agency.Active = true; }

            //db.Entry(Product).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active Agency", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Active Updated.";
            return Json(new { result = result });
        }

        #region[Delete]

        public ActionResult DeleteAgency(int id)
        {

            tblAgency tblagency = db.tblAgencies.Find(id);
            clsSitemap.DeteleSitemap(id.ToString(), "Agency");
            var result = string.Empty;
            db.tblAgencies.Remove(tblagency);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete tblagency", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Bạn đã xóa thành công.";
            return Json(new { result = result });

        }
        [HttpPost]
        public string CheckValue(string text)
        {
            string chuoi = "";
            var ListAgency = db.tblAgencies.Where(p => p.Name == text).ToList();
            if (ListAgency.Count > 0)
            {
                chuoi = "Duplicate Name !";

            }
            Session["Check"] = ListAgency.Count;
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
                            var Del = (from emp in db.tblAgencies where emp.id == id select emp).SingleOrDefault();
                            db.tblAgencies.Remove(Del);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete tblAgencies", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                            clsSitemap.DeteleSitemap(id.ToString(), "Agency");
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