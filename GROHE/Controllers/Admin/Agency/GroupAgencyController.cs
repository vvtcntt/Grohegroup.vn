using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROHE.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.Data.Entity;
using System.Data;
namespace GROHE.Controllers.Admin.Agency
{
    public class GroupAgencyController : Controller
    {
        //
        // GET: /GroupAgency/
        private GROHEContext db = new GROHEContext();
        public ActionResult Index(int? page, string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var listManufacturers = db.tblGroupAgencies.Where(p => p.Level.Length == 5).ToList();
            if (id != "" && id != null)
            {
                int idmenu = int.Parse(id);
                var menucha = db.tblGroupAgencies.Find(idmenu);
                string Lever = menucha.Level;
                int dodai = menucha.Level.Length;
                listManufacturers = db.tblGroupAgencies.Where(p => p.Level.Length == (dodai + 5) && p.Level.Substring(0, dodai) == Lever).ToList();
                ViewBag.Idcha = id;
            }
            else
            {
                ViewBag.Idcha = 0;
            }
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
            return View(listManufacturers.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create(int id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }

            var pro = db.tblGroupAgencies.OrderByDescending(p => p.Ord).Take(1).ToList();
            var GroupNews = db.tblGroupAgencies.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(GroupNews.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupAgencies.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {
                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.id.ToString() });

            }
            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", id);
            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;
            return View();
        }

        //
        // POST: /GroupManufacturers/Create

        [HttpPost]
        public ActionResult Create(tblGroupAgency tblgroupagency, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                string drMenu = collection["drMenu"];
                string nLevel;

                if (drMenu == "")
                {
                    nLevel = "00000";
                }
                else
                {
                    var dbLeve = db.tblGroupAgencies.Find(int.Parse(drMenu));
                    nLevel = dbLeve.Level + "00000";
                }
                tblgroupagency.Level = nLevel;
                tblgroupagency.Tag = StringClass.NameToTag(tblgroupagency.Name);
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblgroupagency.idUser = int.Parse(idUser);
                db.tblGroupAgencies.Add(tblgroupagency);
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Add Group tblgroupagency", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                var listagency = db.tblGroupAgencies.OrderByDescending(p => p.id).Take(1).ToList();
                clsSitemap.CreateSitemap("4/" + StringClass.NameToTag(tblgroupagency.Name), listagency[0].id.ToString(),"GroupAgency");
                return Redirect("Index?id=" + drMenu);
            }

            return View(tblgroupagency);
        }
        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }

            tblGroupAgency tblgroupagency = db.tblGroupAgencies.Find(id);
            if (tblgroupagency == null)
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



            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", id);
            return View(tblgroupagency);
        }

        //
        // POST: /GroupManufacturers/Edit/5

        [HttpPost]

        public ActionResult Edit(tblGroupAgency tblgroupagency, FormCollection collection, int id)
        {
            if (ModelState.IsValid)
            {
                clsSitemap.UpdateSitemap("4/" + StringClass.NameToTag(tblgroupagency.Name), id.ToString(),"GroupAgency");
                string drMenu = collection["drMenu"];
                string nLevel = "";

                if (drMenu == "")
                {
                    nLevel = "00000";

                    tblgroupagency.Level = nLevel;
                    tblgroupagency.Tag = StringClass.NameToTag(tblgroupagency.Name);
                    string idUser = Request.Cookies["Username"].Values["UserID"];
                    tblgroupagency.idUser = int.Parse(idUser);
                    db.Entry(tblgroupagency).State = EntityState.Modified;
                    db.SaveChanges();
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Edit tblGroupAgency", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                    return Redirect("Index?id=" + drMenu);
                }
                else
                {
                    int idMenu = int.Parse(drMenu);
                    var Listda = db.tblGroupAgencies.First(p => p.id == idMenu);
                    if (drMenu == id.ToString())
                    {
                        nLevel = Listda.Level;
                        tblgroupagency.Tag = StringClass.NameToTag(tblgroupagency.Name);
                        string idUser = Request.Cookies["Username"].Values["UserID"];
                        tblgroupagency.idUser = int.Parse(idUser);
                        tblgroupagency.Level = nLevel;
                        db.Entry(Listda).CurrentValues.SetValues(tblgroupagency);
                        db.SaveChanges();
                        #region[Updatehistory]
                        Updatehistoty.UpdateHistory("Update tblGroupAgency", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                        #endregion
                        if (nLevel.Length > 5)
                        {
                            var list = db.tblGroupAgencies.First(p => p.Level == nLevel.Substring(0, nLevel.Length - 5));
                            return Redirect("Index?id=" + list.id);
                        }
                        else
                            return Redirect("Index");
                    }
                    else
                    {
                        nLevel = Listda.Level + "00000"; tblgroupagency.Level = nLevel;
                        tblgroupagency.Tag = StringClass.NameToTag(tblgroupagency.Name);
                        string idUser = Request.Cookies["Username"].Values["UserID"];
                        tblgroupagency.idUser = int.Parse(idUser);
                        db.Entry(tblgroupagency).State = EntityState.Modified;
                        db.SaveChanges();
                        #region[Updatehistory]
                        Updatehistoty.UpdateHistory("Update tblGroupAgency", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                        #endregion
                        return Redirect("Index?id=" + drMenu);
                    }

                }

            }

            return View(tblgroupagency);
        }

        //
        // GET: /GroupManufacturers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            clsSitemap.DeteleSitemap(id.ToString(), "GroupAgency");
            tblGroupAgency tblgroupagency = db.tblGroupAgencies.Find(id);
            if (tblgroupagency == null)
            {
                return HttpNotFound();
            }
            return View(tblgroupagency);
        }

        //
        // POST: /GroupManufacturers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            clsSitemap.DeteleSitemap(id.ToString(), "GroupAgency");
            tblGroupAgency tblgroupagency = db.tblGroupAgencies.Find(id);
            db.tblGroupAgencies.Remove(tblgroupagency);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }




        public ActionResult GroupAgencyEditOrd(int txtSort, string ts)
        {
            var GroupNews = db.tblGroupAgencies.Find(txtSort);
            var result = string.Empty;
            GroupNews.Ord = int.Parse(ts);
            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified;
            result = "Update Ord.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Edit Ord GroupAgency", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult GroupAgencyEditActive(string chk, string nchecked)
        {

            var GroupNews = db.tblGroupAgencies.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                GroupNews.Active = false;
            }
            else
            { GroupNews.Active = true; }

            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified; 
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active GroupAgency", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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
                            var ListLevel = db.tblGroupAgencies.First(p => p.id == id);
                            string LevelParent = ListLevel.Level;
                            int Length = LevelParent.Length;
                            var ListGroupNews = db.tblGroupAgencies.Where(p => p.Level.Length > Length && p.Level.Substring(0, Length) == LevelParent).ToList();
                            for (int i = 0; i < ListGroupNews.Count; i++)
                            {
                                var idChild = ListGroupNews[i].id;
                                var ListChild = db.tblGroupAgencies.First(p => p.id == idChild);
                                var ListNews = db.tblAgencies.Where(p => p.idMenu == ListChild.id).ToList();
                                for (int j = 0; j < ListNews.Count; j++)
                                {

                                    db.tblAgencies.Remove(ListNews[i]);
                                }
                                db.tblGroupAgencies.Remove(ListChild);
                                clsSitemap.DeteleSitemap(ListGroupNews[i].id.ToString(), "GroupAgency");
                            }
                            clsSitemap.DeteleSitemap(id.ToString(), "GroupAgency");
                            db.tblGroupAgencies.Remove(ListLevel);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete Group Agency", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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
