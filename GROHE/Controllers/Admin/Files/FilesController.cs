using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROHE.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.Data;
using System.Data.Entity;
namespace GROHE.Controllers.Admin.Files
{
    public class FilesController : Controller
    {
        private GROHEContext db = new GROHEContext();
        // GET: Files
        public ActionResult Index(int? page, string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var listFiles = db.tblFiles.Where(p=>p.Cate==0).ToList();

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
            return View(listFiles.ToPagedList(pageNumber, pageSize));
             
        }
        public ActionResult Create()
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var pro = db.tblFiles.OrderByDescending(p => p.Ord).Take(1).ToList();
            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;
            var menuName = db.tblGroupProducts.Where(p => p.Active == true).ToList();
            var GroupsProducts = db.tblGroupProducts.Where(m => m.Active == true).OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(GroupsProducts.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupProducts.Where(m => m.Active == true).OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {
                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.Id.ToString() });

            }
            ViewBag.drMenu = lstMenu;
            return View();
        }
        [HttpPost]
        public ActionResult Create(tblFile tblfile, FormCollection Collection)
        { 
            string nidCate = Collection["drMenu"];
            if (nidCate != "")
            {
                tblfile.idg = int.Parse(nidCate);
                tblfile.DateCreate = DateTime.Now;

                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblfile.idUser = int.Parse(idUser);
                tblfile.Cate = 0;
                tblfile.Tag = StringClass.NameToTag(tblfile.Name);
                db.tblFiles.Add(tblfile);
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Add files", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblFile tblfile = db.tblFiles.Find(id);
            if (tblfile == null)
            {
                return HttpNotFound();
            }
            var Image = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(Image.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {

                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.Id.ToString() });


            }
            int idGroups = int.Parse(tblfile.idg.ToString());
            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", idGroups);
            return View(tblfile);
        }
        [HttpPost]
        public ActionResult Edit(tblFile tblfile, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                if (collection["drMenu"] != "" || collection["drMenu"] != null)
                {
                    int idCate = int.Parse(collection["drMenu"]);
                    tblfile.idg = idCate;
                }
                tblfile.DateCreate = DateTime.Now;
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblfile.idUser = int.Parse(idUser);
                tblfile.Cate = 0;
                tblfile.Tag = StringClass.NameToTag(tblfile.Name);
                db.Entry(tblfile).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tblfile", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                return RedirectToAction("Index");
            }
            return View(tblfile);
        }
        public ActionResult Delete(int id = 0)
        {
            tblFile tblfile = db.tblFiles.Find(id);
            if (tblfile == null)
            {
                return HttpNotFound();
            }
            return View(tblfile);
        }
        public ActionResult DeleteConfirmed(int id)
        {
            tblFile tblfile = db.tblFiles.Find(id);
            db.tblFiles.Remove(tblfile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult FilesEditOrd(int txtSort, string ts)
        {
            var tblfile = db.tblFiles.Find(txtSort);
            var result = string.Empty;
            tblfile.Ord = int.Parse(ts);
            result = "Update tblfile.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord tblfile", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult FilesEditActive(string chk, string nchecked)
        {

            var tblfile = db.tblFiles.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                tblfile.Active = false;
            }
            else
            { tblfile.Active = true; } 
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active tblfile", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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
                            tblFile tblfile = db.tblFiles.Find(id);
                            db.tblFiles.Remove(tblfile);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete tblfile", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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