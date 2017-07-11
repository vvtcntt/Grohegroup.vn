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

namespace GROHE.Controllers.Admin.News
{
    public class GroupNewsController : Controller
    {
        private GROHEContext db = new GROHEContext();

        //
        // GET: /GroupNews/

        public ActionResult Index(int? page, string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var ListGroupNews = db.tblGroupNews.Where(p => p.Level.Length == 5).ToList();
            if (id != "" && id != null)
            {
                int idmenu = int.Parse(id);
                var menucha = db.tblGroupNews.Find(idmenu);
                string Lever = menucha.Level;
                int dodai = menucha.Level.Length;
                ListGroupNews = db.tblGroupNews.Where(p => p.Level.Length == (dodai + 5) && p.Level.Substring(0, dodai) == Lever).ToList();
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
            return View(ListGroupNews.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /GroupNews/Details/5

     

        //
        // GET: /GroupNews/Create
        [HttpPost]
        public string CheckValue(string text)
        {
            string chuoi = "";
            var listProduct = db.tblGroupProducts.Where(p => p.Name == text).ToList();
            if (listProduct.Count > 0)
            {
                chuoi = "Duplicate Name !";

            }
            Session["Check"] = listProduct.Count;
            return chuoi;
        }
        public ActionResult Create(int id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
             
            var pro = db.tblGroupNews.OrderByDescending(p => p.Ord).Take(1).ToList();
            var GroupNews = db.tblGroupNews.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(GroupNews.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupNews.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            { 
                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.id.ToString() });
 
            }



            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", id);
            if(pro.Count>0)
            ViewBag.Ord = pro[0].Ord + 1;
            return View();
        }

        //
        // POST: /GroupNews/Create

        [HttpPost] 
        public ActionResult Create(tblGroupNew tblgroupnew, FormCollection collection)
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
                    var dbLeve = db.tblGroupNews.Find(int.Parse(drMenu));
                    nLevel = dbLeve.Level + "00000";
                }
                tblgroupnew.Level = nLevel;
                tblgroupnew.Tag = StringClass.NameToTag(tblgroupnew.Name);
                tblgroupnew.DateCreate = DateTime.Now;
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblgroupnew.idUser = int.Parse(idUser);
                db.tblGroupNews.Add(tblgroupnew);
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Add Group New", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                var listmenu = db.tblGroupNews.OrderByDescending(p => p.id).Take(1).ToList();
                clsSitemap.CreateSitemap("2/"+ StringClass.NameToTag(tblgroupnew.Name), listmenu[0].id.ToString(), "GroupNews");
                return Redirect("Index?id=" + drMenu);
            }

            return View(tblgroupnew);
        }
        [HttpPost]
        public ActionResult GroupNewsEditOrd(int txtSort, string ts)
        {
            var GroupNews = db.tblGroupNews.Find(txtSort);
            var result = string.Empty;
            GroupNews.Ord = int.Parse(ts);
            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified;
            result = "Update Ord.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Edit Ord Group New", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult GroupNewsEditActive(string chk, string nchecked)
        {

            var GroupNews = db.tblGroupNews.Find(int.Parse(chk));
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
            Updatehistoty.UpdateHistory("Update Active Group New", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Updated Active.";
            return Json(new { result = result });
        }
       
        //
        // GET: /GroupNews/Edit/5

        public ActionResult Edit(int id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblGroupNew tblgroupnew = db.tblGroupNews.Find(id);
            if (tblgroupnew == null)
            {
                return HttpNotFound();
            }

            var GroupNews = db.tblGroupNews.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(GroupNews.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupNews.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {
              
                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.id.ToString() });

              
            }


             
            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", id);
            return View(tblgroupnew);
        }

        //
        // POST: /GroupNews/Edit/5

        [HttpPost] 
        public ActionResult Edit(tblGroupNew tblgroupnew, FormCollection collection, int id)
        {
            if (ModelState.IsValid)
            {
                string drMenu = collection["drMenu"];
                string nLevel = "";
                clsSitemap.UpdateSitemap("2/"+StringClass.NameToTag(tblgroupnew.Name), id.ToString(),"GroupNews");
                if (drMenu == "")
                {
                    nLevel = "00000";

                    tblgroupnew.Level = nLevel;
                    tblgroupnew.Tag = StringClass.NameToTag(tblgroupnew.Name);
                    tblgroupnew.DateCreate = DateTime.Now;
                    string idUser = Request.Cookies["Username"].Values["UserID"];
                    tblgroupnew.idUser = int.Parse(idUser);
                    db.Entry(tblgroupnew).State = EntityState.Modified;
                    db.SaveChanges();
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Edit Group New", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                    return Redirect("Index?id=" + drMenu);
                }
                else
                {
                    int idMenu = int.Parse(drMenu);
                    var Listda = db.tblGroupNews.First(p => p.id == idMenu);
                    if (drMenu == id.ToString())
                    {
                        nLevel = Listda.Level;
                        tblgroupnew.Tag =StringClass.NameToTag(tblgroupnew.Name);
                        tblgroupnew.DateCreate = DateTime.Now;
                        string idUser = Request.Cookies["Username"].Values["UserID"];
                        tblgroupnew.idUser = int.Parse(idUser);
                        tblgroupnew.Level = nLevel;
                        db.Entry(Listda).CurrentValues.SetValues(tblgroupnew);
                        db.SaveChanges();
                        #region[Updatehistory]
                        Updatehistoty.UpdateHistory("Update Group New", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                        #endregion
                        if (nLevel.Length > 5)
                        {
                            var list = db.tblGroupNews.First(p => p.Level == nLevel.Substring(0, nLevel.Length - 5));
                            return Redirect("Index?id=" + list.id);
                        }
                        else
                            return Redirect("Index");
                    }
                    else
                    {
                        nLevel = Listda.Level + "00000"; tblgroupnew.Level = nLevel;
                        tblgroupnew.Tag = StringClass.NameToTag(tblgroupnew.Name);
                        tblgroupnew.DateCreate = DateTime.Now;
                        string idUser = Request.Cookies["Username"].Values["UserID"];
                        tblgroupnew.idUser = int.Parse(idUser);
                        db.Entry(tblgroupnew).State = EntityState.Modified;
                        db.SaveChanges();
                        #region[Updatehistory]
                        Updatehistoty.UpdateHistory("Update Group New", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                        #endregion
                        return Redirect("Index?id=" + drMenu);
                    }

                }
               
            }
            return View(tblgroupnew);
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
                            var ListLevel = db.tblGroupNews.First(p => p.id == id);
                            string LevelParent = ListLevel.Level;
                            int Length = LevelParent.Length;
                            var ListGroupNews = db.tblGroupNews.Where(p => p.Level.Length > Length && p.Level.Substring(0, Length) == LevelParent).ToList();
                            for (int i = 0; i < ListGroupNews.Count; i++)
                            {
                                var idChild = ListGroupNews[i].id;
                                var ListChild = db.tblGroupNews.First(p => p.id == idChild);
                                var ListNews = db.tblNews.Where(p => p.idCate == ListChild.id).ToList();
                                for (int j = 0; j < ListNews.Count; j++)
                                {

                                    db.tblNews.Remove(ListNews[i]);
                                }
                                db.tblGroupNews.Remove(ListChild);
                                clsSitemap.DeteleSitemap(ListGroupNews[i].id.ToString(), "GroupNews");
                            }
                            db.tblGroupNews.Remove(ListLevel);
                            clsSitemap.DeteleSitemap(id.ToString(), "GroupNews");
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete Group New", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                            return Redirect("Index");
                        }
                    }
                }
            }
            return RedirectToAction("Index");


        }
     
        public ActionResult DeleteConfirmed(int id)
        {
            var ListLevel = db.tblGroupNews.First(p => p.id == id);
            string LevelParent = ListLevel.Level;
            int Length = LevelParent.Length;
            var ListGroupNews = db.tblGroupNews.Where(p => p.Level.Length > Length && p.Level.Substring(0, Length) == LevelParent).ToList();
            for (int i = 0; i < ListGroupNews.Count; i++)
            {
                var idChild = ListGroupNews[i].id;
                var ListChild = db.tblGroupNews.First(p => p.id == idChild);
                var ListNews = db.tblNews.Where(p => p.idCate == ListChild.id).ToList();
                for (int j = 0; j < ListNews.Count; j++)
                {

                    db.tblNews.Remove(ListNews[i]);
                }
                db.tblGroupNews.Remove(ListChild);
                clsSitemap.DeteleSitemap(ListGroupNews[i].id.ToString(), "GroupNews");

            }
            clsSitemap.DeteleSitemap(id.ToString(), "GroupNews");

            db.tblGroupNews.Remove(ListLevel);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete Group New", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Redirect("Index");

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}