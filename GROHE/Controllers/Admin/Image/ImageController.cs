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
    public class ImageController : Controller
    {
        private GROHEContext db = new GROHEContext();
        public ActionResult Index(int? page, string text, string idCate)
        {
           if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            string txtSearch = "";

            var ListImage = (from p in db.tblImages select p).ToList();
            if (txtSearch != null && txtSearch != "")
            {
                ListImage = db.tblImages.Where(p => p.Name.Contains(txtSearch)).ToList();
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

                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.id.ToString() });


            }
            ViewBag.drMenu = lstMenu;
            #endregion

            if (Request.IsAjaxRequest())
            {
                int idCatelogy;
                if (text != null && text != "")
                {
                    var list = db.tblImages.Where(p => p.Name.ToUpper().Contains(text.ToUpper())).ToList();
                    return PartialView("PartialImageData", list);
                }
                if (idCate != null && idCate != "")
                {
                    idCatelogy = int.Parse(idCate);
                    var list = db.tblImages.Where(p => p.idMenu == idCatelogy).ToList();
                    return PartialView("PartialImageData", list);
                }
                if (text != null && text != "" && idCate != null && idCate != "")
                {
                    idCatelogy = int.Parse(idCate);
                    var list = db.tblImages.Where(p => p.Name.ToUpper().Contains(text.ToUpper()) && p.idMenu == (int.Parse(idCate))).ToList();
                    return PartialView("PartialImageData", list);
                }

                else
                {
                    return PartialView("PartialImageData", ListImage);
                }
            }

            return View(ListImage.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var pro = db.tblImages.OrderByDescending(p => p.Ord).Take(1).ToList();
            var GroupImage = db.tblGroupImages.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(GroupImage.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupImages.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {

                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.id.ToString() });


            }
            ViewBag.drMenu = lstMenu;



            var GroupsProducts = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var listpage1 = new List<SelectListItem>();
            listpage1.Clear();
            listpage1.AddRange(GroupsProducts.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel1 = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var lstMenu1 = new List<SelectListItem>();
            lstMenu1.Clear();
            foreach (var menu in menuModel1)
            {

                lstMenu1.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.Id.ToString() });


            }




            ViewBag.drMenu1 = lstMenu1;

            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;
            return View();
        }
        [HttpPost]
       
        public ActionResult Create(tblImage tblimage ,FormCollection Collection)
        {
            
            string nidCate = Collection["drMenu"];
            string nidMenu = Collection["drMenu1"];

            if (nidCate != "" && nidMenu!="")
                {
                    tblimage.idMenu = int.Parse(nidCate);
                    tblimage.DateCreate = DateTime.Now;
                    tblimage.idCate = int.Parse(nidMenu);
                    tblimage.DateCreate = DateTime.Now;
                    string idUser = Request.Cookies["Username"].Values["UserID"];
                    tblimage.idUser = int.Parse(idUser);
                    db.tblImages.Add(tblimage);
                    db.SaveChanges();
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Create Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                }
                return RedirectToAction("Index");
            
        }

        //
        // GET: /Image/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }

            tblImage tblimage = db.tblImages.Find(id);
            if (tblimage == null)
            {
                return HttpNotFound();
            }
            var Image = db.tblGroupImages.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(Image.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupImages.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {

                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.id.ToString() });


            }

            int idGroups = int.Parse(tblimage.idMenu.ToString());
            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", idGroups);


            var GroupsProducts = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var listpage1 = new List<SelectListItem>();
            listpage1.Clear();
            listpage1.AddRange(GroupsProducts.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel1 = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var lstMenu1 = new List<SelectListItem>();
            lstMenu1.Clear();
            foreach (var menu in menuModel1)
            {

                lstMenu1.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.Id.ToString() });


            }
            int idMenus = int.Parse(tblimage.idCate.ToString());
            ViewBag.drMenu1 = new SelectList(lstMenu1, "Value", "Text", idMenus);
            return View(tblimage);
        }

        //
        // POST: /Image/Edit/5

        [HttpPost]
     
        public ActionResult Edit(tblImage tblimage, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                if (collection["drMenu"] != "" || collection["drMenu"] != null)
                {
                    int idCate = int.Parse(collection["drMenu"]);
                    tblimage.idMenu = idCate;
                    int idMenu = int.Parse(collection["drMenu1"]);
                    tblimage.idCate=idMenu;
                    tblimage.DateCreate = DateTime.Now;
                    string idUser = Request.Cookies["Username"].Values["UserID"];
                    tblimage.idUser = int.Parse(idUser);
                    db.Entry(tblimage).State = EntityState.Modified;
                    db.SaveChanges();
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Sửa Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                }
                return RedirectToAction("Index");
            }
            return View(tblimage);
        }

        //
        // GET: /Image/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblImage tblimage = db.tblImages.Find(id);
            if (tblimage == null)
            {
                return HttpNotFound();
            }
            return View(tblimage);
        }

        //
        // POST: /Image/Delete/5

       
        public ActionResult DeleteConfirmed(int id)
        {
            tblImage tblimage = db.tblImages.Find(id);
            db.tblImages.Remove(tblimage);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Xóa Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public PartialViewResult PartialImageData()
        {
            return PartialView();
        }
        public ActionResult ImageEditOrd(int txtSort, string ts)
        {
            var Image = db.tblImages.Find(txtSort);
            var result = string.Empty;
            Image.Ord = int.Parse(ts);
            //db.Entry(Product).State = System.Data.EntityState.Modified;
            result = "Ord Updated.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Sua Sord Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult ImageEditActive(string chk, string nchecked)
        {

            var Image = db.tblImages.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                Image.Active = false;
            }
            else
            { Image.Active = true; }

            //db.Entry(Product).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Sua Active Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Active Updated.";
            return Json(new { result = result });
        }

        #region[Delete]

        public ActionResult DeleteImage(int id)
        {

            tblImage tblimage = db.tblImages.Find(id);

            var result = string.Empty;
            db.tblImages.Remove(tblimage);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Bạn đã xóa thành công.";
            return Json(new { result = result });

        }
        [HttpPost]
        public string CheckValue(string text)
        {
            string chuoi = "";
            var listProduct = db.tblImages.Where(p => p.Name == text).ToList();
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
                            var Del = (from emp in db.tblImages where emp.id == id select emp).SingleOrDefault();
                            db.tblImages.Remove(Del);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Xoa Image", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                        }
                    }
                }
                return RedirectToAction("Index");
            }
             
                
           
            return View();
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