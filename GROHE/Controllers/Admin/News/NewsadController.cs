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
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
namespace GROHE.Controllers.Admin.Newsad
{
    public class NewsadController : Controller
    {
        private GROHEContext db = new GROHEContext();

        //
        // GET: /News/

        public ActionResult Index(int? page, string text, string idCate)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            string txtSearch = "";

            var listNews = (from p in db.tblNews select p).OrderByDescending(p=>p.Ord).ToList();
            if (txtSearch != null && txtSearch != "")
            {
                listNews = db.tblNews.Where(p => p.Name.Contains(txtSearch)).OrderByDescending(p => p.Ord).ToList();
            }
            Session["txtSearch"] = null;
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            // Thiết lập phân trang
            if (idCate != "" && idCate != null)
            {
                int idcates = int.Parse(idCate);
                listNews = db.tblNews.Where(p => p.idCate == idcates).OrderByDescending(p => p.Ord).ToList();
                ViewBag.idMenu = idCate;
            }
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
                    var list = db.tblNews.Where(p => p.Name.ToUpper().Contains(text.ToUpper())).OrderByDescending(p => p.Ord).ToList();
                    return PartialView("PartialNewstData", list);
                }
                if (idCate != null && idCate != "")
                {
                    idCatelogy = int.Parse(idCate);
                    var list = db.tblNews.Where(p => p.idCate == idCatelogy).OrderByDescending(p => p.Ord).ToList(); ViewBag.idMenu = idCate;
                    return PartialView("PartialNewstData", list);
                }
                if (text != null && text != "" && idCate != null && idCate != "")
                {
                    idCatelogy = int.Parse(idCate);ViewBag.idMenu = idCate;
                    var list = db.tblNews.Where(p => p.Name.ToUpper().Contains(text.ToUpper()) && p.idCate == (int.Parse(idCate))).OrderByDescending(p => p.Ord).ToList();
                    return PartialView("PartialNewstData", list);
                }

                else
                {
                    return PartialView("PartialNewstData", listNews);
                }
            }

            return View(listNews.ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult PartialNewstData()
        {
            return PartialView();
        }
        public ActionResult Create(string idCate)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var pro = db.tblNews.OrderByDescending(p => p.Ord).Take(1).ToList();
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


            } if (idCate != "" || idCate != null)
            {

                ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", idCate);
            }
            else
                ViewBag.drMenu = lstMenu;
             if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;

           
            return View();
        }

        //
        // POST: /News/Create

        [HttpPost] 
        [ValidateInput(false)]
        public ActionResult Create(tblNew tblnew, FormCollection Collection,string idCate)
        {
            if (ModelState.IsValid)
            {
                string nidCate = Collection["drMenu"];
                if (nidCate != "")
                {
                    tblnew.idCate = int.Parse(nidCate);
                    tblnew.DateCreate = DateTime.Now;
                    tblnew.Tag = StringClass.NameToTag(tblnew.Name);
                    tblnew.DateCreate = DateTime.Now;
                    string idUser = Request.Cookies["Username"].Values["UserID"];
                    tblnew.idUser = int.Parse(idUser);
                    db.tblNews.Add(tblnew);
                    db.SaveChanges();
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Add new", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                    var listnew = db.tblNews.OrderByDescending(p => p.id).Take(1).ToList();

                    clsSitemap.CreateSitemap("/3/" + StringClass.NameToTag(tblnew.Name), listnew[0].id.ToString(), "News");

                }
                return Redirect("Index?idCate=" + nidCate);
            }

            return View(tblnew);
        }

        //
        // GET: /News/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblNew tblnew = db.tblNews.Find(id);
            if (tblnew == null)
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



            int idGroups = int.Parse(tblnew.idCate.ToString());
            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", idGroups);
            return View(tblnew);
        }

        //
        // POST: /News/Edit/5

        [HttpPost]
    
        [ValidateInput(false)]
        public ActionResult Edit(tblNew tblnew, FormCollection collection,int id)
        {
            if (ModelState.IsValid)
            {
                int idCate = int.Parse(collection["drMenu"]);
                tblnew.idCate = idCate;
                tblnew.DateCreate = DateTime.Now;
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblnew.idUser = int.Parse(idUser);
                tblnew.Tag = StringClass.NameToTag(tblnew.Name);
                db.Entry(tblnew).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit new", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                clsSitemap.UpdateSitemap("/3/" + StringClass.NameToTag(tblnew.Name), id.ToString(), "News");
             
                return Redirect("Index?idCate=" + idCate);
            }
            return View(tblnew);
        }

        //
        // GET: /News/Delete/5

       
        public ActionResult DeleteConfirmed(int id)
        {
            tblNew tblnew = db.tblNews.Find(id);
            clsSitemap.DeteleSitemap(id.ToString(), "News");
            db.tblNews.Remove(tblnew);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete new", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult NewsEditOrd(int txtSort, string ts)
        {
            var Product = db.tblNews.Find(txtSort);
            var result = string.Empty;
            Product.Ord = int.Parse(ts);
            //db.Entry(Product).State = System.Data.EntityState.Modified;
            result = "Ord Updated";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord new", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult NewsEditActive(string chk, string nchecked)
        {

            var Product = db.tblNews.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                Product.Active = false;
            }
            else
            { Product.Active = true; }

            //db.Entry(Product).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active new", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Active Updated.";
            return Json(new { result = result });
        }

        #region[Delete]

        public ActionResult DeleteNews(int id)
        {

            tblNew tblnew = db.tblNews.Find(id);

            var result = string.Empty;
            db.tblNews.Remove(tblnew);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete new", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            clsSitemap.DeteleSitemap(id.ToString(), "News");
            result = "Bạn đã xóa thành công.";
            return Json(new { result = result });

        }
        [HttpPost]
        public string CheckValue(string text)
        {
            string chuoi = "";
            var listProduct = db.tblProducts.Where(p => p.Name == text).ToList();
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
                            var Del = (from emp in db.tblNews where emp.id == id select emp).SingleOrDefault();
                            db.tblNews.Remove(Del);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete new", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                            clsSitemap.DeteleSitemap(id.ToString(), "News");
                        }
                    }
                }
                return RedirectToAction("Index");
            }
             
            if (collection["btlPrinter"] != null)
            {

                string chuoi = "";
                int dem = 0;
                int[] exceptionLista;
                List<int> exceptionList = new List<int>();
                foreach (string key in Request.Form)
                {
                    var checkbox = "";

                    if (key.StartsWith("chkitem+"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            int idp = int.Parse(key.Remove(0, 8));
                            //  int iddd = int.Parse(idp);
                            //  var sp = db.tblProducts.Where(m => m.id == iddd).First();
                            //  chuoi+= "<tr><td>Tên sản phẩm</td><td>"+sp.Name+"</td></tr>";
                            //chuoi+="<tr> <td></td><td></td> </tr>";
                            // chuoi+=" <tr> <td>Mã sản phẩm</td><td>"+2017+"</td></tr>";
                            //chuoi = chuoi + "," + key.Remove(0, 8);
                            exceptionList.Add(idp);
                            //dem = dem + 1;

                        }
                    }
                }
                var list = db.tblNews.Where(x => exceptionList.Contains(x.id)).ToList();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Printer new", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                return View("Printer", list);
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