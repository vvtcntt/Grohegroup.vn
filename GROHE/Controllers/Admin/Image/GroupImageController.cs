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
namespace GROHE.Controllers.Admin.Image
{
    public class GroupImageController : Controller
    {
        private GROHEContext db = new GROHEContext();

        //
        // GET: /GroupImage/

        public ActionResult Index(int? page, string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var ListGroupImage = db.tblGroupImages.Where(p => p.Level.Length == 5).ToList();
            if (id != "" && id != null)
            {
                int idmenu = int.Parse(id);
                var menucha = db.tblGroupImages.Find(idmenu);
                string Lever = menucha.Level;
                int dodai = menucha.Level.Length;
                ListGroupImage = db.tblGroupImages.Where(p => p.Level.Length == (dodai + 5) && p.Level.Substring(0, dodai) == Lever).ToList();
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
            return View(ListGroupImage.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /GroupImage/Details/5

        

        //
        // GET: /GroupImage/Create

        public ActionResult Create(int id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }

            var pro = db.tblGroupImages.OrderByDescending(p => p.Ord).Take(1).ToList();
            var GroupImage = db.tblGroupImages.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(GroupImage.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupImages.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {
                //if (menu.Level.Length == 5)
                //{
                //    lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.Level.ToString() });
                //}
                //else
                //{
                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.id.ToString() });

                //}
            }



            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", id);
            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;
            return View();
        }

        //
        // POST: /GroupImage/Create

        [HttpPost]
 
        public ActionResult Create(tblGroupImage tblgroupimage, FormCollection collection)
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
                    var dbLeve = db.tblGroupImages.Find(int.Parse(drMenu));
                    nLevel = dbLeve.Level + "00000";
                }
                tblgroupimage.Level = nLevel;
                tblgroupimage.DateCreate = DateTime.Now;
                tblgroupimage.Tag = StringClass.NameToTag(tblgroupimage.Name);
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblgroupimage.idUser = int.Parse(idUser);
                #region[history]
                tblHistoryLogin tblhistorylogin = new tblHistoryLogin();
                tblhistorylogin.FullName = Request.Cookies["Username"].Values["FullName"];
                tblhistorylogin.Task = "Tao moi GroupNew";
                tblhistorylogin.idUser = int.Parse(Request.Cookies["Username"].Values["UserID"]);
                tblhistorylogin.DateCreate = DateTime.Now;
                tblhistorylogin.Active = true;
                db.tblHistoryLogins.Add(tblhistorylogin);
                db.SaveChanges();
                #endregion
                db.tblGroupImages.Add(tblgroupimage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblgroupimage);
        }

        //
        // GET: /GroupImage/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblGroupImage tblgroupimages = db.tblGroupImages.Find(id);
            var tblGroupImage = db.tblGroupImages.OrderBy(m => m.Level).ToList();
            if (tblGroupImage == null)
            {
                return HttpNotFound();
            }
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(tblGroupImage.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupImages.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            { 
                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.id.ToString() }); 
            }

            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", id);
            return View(tblgroupimages);
        }

        //
        // POST: /GroupImage/Edit/5

        [HttpPost] 
        public ActionResult Edit(tblGroupImage tblgroupimage, FormCollection collection, int id)
        {
            if (ModelState.IsValid)
            {
                string drMenu = collection["drMenu"];
                string nLevel = "";
                
                if (drMenu == "")
                {
                    nLevel = "00000";
                    
                    tblgroupimage.Level = nLevel;
                    tblgroupimage.Tag = StringClass.NameToTag(tblgroupimage.Name);
                    tblgroupimage.DateCreate = DateTime.Now;
                    string idUser = Request.Cookies["Username"].Values["UserID"];
                    tblgroupimage.idUser = int.Parse(idUser);
                    db.Entry(tblgroupimage).State = EntityState.Modified;
                    db.SaveChanges(); 
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Update Group Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                    return Redirect("Index?id=" + drMenu);
                }
                else
                {
                    int idMenu = int.Parse(drMenu);
                    var Listda = db.tblGroupImages.First(p => p.id == idMenu);
                    if (drMenu == id.ToString())
                    {
                        nLevel = Listda.Level;
                        tblgroupimage.Tag = StringClass.NameToTag(tblgroupimage.Name);
                        tblgroupimage.DateCreate = DateTime.Now;
                        string idUser = Request.Cookies["Username"].Values["UserID"];
                        tblgroupimage.idUser = int.Parse(idUser);
                        tblgroupimage.Level = nLevel;
                        db.Entry(Listda).CurrentValues.SetValues(tblgroupimage);
                        db.SaveChanges();
                        #region[Updatehistory]
                        Updatehistoty.UpdateHistory("Update Group Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                        #endregion
                        if (nLevel.Length > 5)
                        {
                            var list = db.tblGroupImages.First(p => p.Level == nLevel.Substring(0, nLevel.Length - 5));
                            return Redirect("Index?id=" + list.id);
                        }
                        else
                            return Redirect("Index");
                    }
                    else
                    {
                        nLevel = Listda.Level + "00000"; tblgroupimage.Level = nLevel;
                        tblgroupimage.Tag = StringClass.NameToTag(tblgroupimage.Name);
                        tblgroupimage.DateCreate = DateTime.Now;
                        string idUser = Request.Cookies["Username"].Values["UserID"];
                        tblgroupimage.idUser = int.Parse(idUser);
                        db.Entry(tblgroupimage).State = EntityState.Modified;
                        db.SaveChanges();
                        #region[Updatehistory]
                        Updatehistoty.UpdateHistory("Update Group Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                        #endregion
                        return Redirect("Index?id=" + drMenu);
                    }

                }

            }
            return View(tblgroupimage);
        }

        //
        // GET: /GroupImage/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblGroupImage tblgroupimage = db.tblGroupImages.Find(id);
            if (tblgroupimage == null)
            {
                return HttpNotFound();
            }
            return View(tblgroupimage);
        }

        //
        // POST: /GroupImage/Delete/5

      
        public ActionResult DeleteConfirmed(int id)
        {
            tblGroupImage tblgroupimage = db.tblGroupImages.Find(id);
            db.tblGroupImages.Remove(tblgroupimage);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete Group Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult GroupImageEditOrd(int txtSort, string ts)
        {
            var GroupImage = db.tblGroupImages.Find(txtSort);
            var result = string.Empty;
            GroupImage.Ord = int.Parse(ts);
            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified;
            result = "Update Ord.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord Group Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult GroupImageEditActive(string chk, string nchecked)
        {

            var GroupImage = db.tblGroupImages.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                GroupImage.Active = false;
            }
            else
            { GroupImage.Active = true; }

            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified; 
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active GroupImage", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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
                            var ListLevel = db.tblGroupImages.First(p => p.id == id);
                            string LevelParent = ListLevel.Level;
                            int Length = LevelParent.Length;
                            var ListGroupImage = db.tblGroupImages.Where(p => p.Level.Length > Length && p.Level.Substring(0, Length) == LevelParent).ToList();
                            for (int i = 0; i < ListGroupImage.Count; i++)
                            {
                                var idChild = ListGroupImage[i].id;
                                var ListChild = db.tblGroupImages.First(p => p.id == idChild);
                                var ListImage = db.tblImages.Where(p => p.idMenu == ListChild.id).ToList();
                                for (int j = 0; j < ListImage.Count; j++)
                                {

                                    db.tblImages.Remove(ListImage[i]);
                                }
                                db.tblGroupImages.Remove(ListChild);

                            }
                            db.tblGroupImages.Remove(ListLevel);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete Group Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
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