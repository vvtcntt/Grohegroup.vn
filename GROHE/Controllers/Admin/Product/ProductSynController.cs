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
using System.Xml;
namespace GROHE.Controllers.Admin.Product
{
    public class ProductSynController : Controller
    {
        private GROHEContext db = new GROHEContext();

        public ActionResult Index(int? page, string text)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            string txtSearch = "";
            var listProduct = (from p in db.tblProductSyns orderby (p.Ord) select p).ToList();

           

            if (txtSearch != null && txtSearch != "")
            {
                listProduct = db.tblProductSyns.Where(p => p.Name.Contains(txtSearch)).ToList();
            }
            Session["txtSearch"] = null;
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
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
                int idCatelogy;
                if (text != null && text != "")
                {
                    var list = db.tblProductSyns.Where(p => p.Name.ToUpper().Contains(text.ToUpper())).ToList();
                    return PartialView("PartialProductData", list);
                }
                

                else
                {
                    return PartialView("PartialProductData", listProduct);
                }
            }

            if (Session["Thongbao"] != null && Session["Thongbao"] != "")
            {

                ViewBag.thongbao = Session["Thongbao"].ToString();
                Session["Thongbao"] = "";
            }
            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }

        public PartialViewResult PartialProductSynData()
        {

            return PartialView();

        }

        //
        // GET: /Product/Create

        public ActionResult Create(string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }          
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblProductSyn tblproductsyn, FormCollection Collection, string id, List<HttpPostedFileBase> uploadFile)
        {

            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
          
                tblproductsyn.DateCreate = DateTime.Now;
                tblproductsyn.Tag = StringClass.NameToTag(tblproductsyn.Name);
                tblproductsyn.DateCreate = DateTime.Now;
                tblproductsyn.Visit = 0;
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblproductsyn.idUser = int.Parse(idUser);
                db.tblProductSyns.Add(tblproductsyn);
                db.SaveChanges();
                var listprro = db.tblProductSyns.OrderByDescending(p => p.id).Take(1).ToList();
                clsSitemap.CreateSitemap("Syn/" + StringClass.NameToTag(tblproductsyn.Name), listprro[0].id.ToString(), "Productsyn");
           
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Add Productsys", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            string Chuoisyn = Collection["CodeSyn"];
            string[] Mang = Chuoisyn.Split(',');
            ProductConnect productconnect = new ProductConnect();
            var listproductSyn = db.tblProductSyns.OrderByDescending(p => p.id).Take(1).ToList();
            int idsyn=int.Parse(listproductSyn[0].id.ToString());
            for (int i = 0; i < Mang.Length;i++ )
            {
                productconnect.idSyn = idsyn;
                string idpd = Mang[i].ToString();
                productconnect.idpd = idpd;
                db.ProductConnects.Add(productconnect);
                db.SaveChanges();

            }
            TempData["Msg"] = "";
            string abc = "";
            string def = "";
            var list = db.tblProductSyns.OrderByDescending(p => p.id).Take(1).ToList();
            int idpro = int.Parse(list[0].id.ToString());
            if (uploadFile != null)
            {
                foreach (var item in uploadFile)
                {
                    if (item != null)
                    {
                        string filename = item.FileName;
                        string path = System.IO.Path.Combine(Server.MapPath("~/Images/ImagesList"), System.IO.Path.GetFileName(item.FileName));
                        item.SaveAs(path);
                        abc = string.Format("Upload {0} file thành công", uploadFile.Count);
                        def += item.FileName + "; ";
                        ImageProduct imgp = new ImageProduct();
                        imgp.idProduct = idpro;
                        imgp.Type = 1;
                        imgp.Images = "/Images/ImagesList/" + System.IO.Path.GetFileName(item.FileName);
                        db.ImageProducts.Add(imgp);
                        db.SaveChanges();
                    }

                }
                TempData["Msg"] = abc + "</br>" + def;
            }
                return Redirect("Index");

        }

        public ActionResult Edit(int? id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            Int32 ids = Int32.Parse(id.ToString());
            tblProductSyn tblproductsyn = db.tblProductSyns.Find(ids);
            if (tblproductsyn == null)
            {
                return HttpNotFound();
            }

            var listImages = db.ImageProducts.Where(p => p.idProduct == ids && p.Type == 1).ToList();
            string chuoi = "";
            for (int i = 0; i < listImages.Count; i++)
            {
                chuoi += " <div class=\"Tear_Images\">";
                chuoi += " <img src=\"" + listImages[i].Images + "\" alt=\"\"/>";
                chuoi += " <input type=\"checkbox\" name=\"chek-" + listImages[i].id + "\" id=\"chek-" + listImages[i].id + "\" /> Xóa";
                chuoi += "</div>";

            }
            ViewBag.chuoi = chuoi;



            return View(tblproductsyn);
        }

        //
        // POST: /Product/Edit/5
     
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tblProductSyn tblproductsyn, FormCollection collection, int? id, List<HttpPostedFileBase> uploadFile)
        {
           
                id = int.Parse(collection["idProduct"]);

                tblproductsyn = db.tblProductSyns.Find(id);
             //sdsd
                tblproductsyn.DateCreate = DateTime.Now;
                string tag = tblproductsyn.Tag;
                string URL = collection["URL"];
                string Name = collection["Name"];
                string Code = collection["Code"];
                string Description = collection["Description"];
                string Content = collection["Content"];
                string Parameter = collection["Parameter"];
                string ImageLinkThumb = collection["ImageLinkThumb"];
                string ImageLinkDetail = collection["ImageLinkDetail"];
                string ImageSale = collection["ImageSale"];
                string chkfile = collection["chkfile"];
                if (collection["Price"] != null)
                {
                    float Price = float.Parse(collection["Price"]);
                    tblproductsyn.Price = Price;

                }
                if (collection["PriceSale"] != null)
                {
                    float PriceSale = float.Parse(collection["PriceSale"]);
                    tblproductsyn.PriceSale = PriceSale;
                }
                if (chkfile == "on")
                {
                    tblFile tblfile = db.tblFiles.Where(p => p.idp == id).First();
                    string fullPath = Request.MapPath("~/" + tblfile.File);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    db.tblFiles.Remove(tblfile);
                    db.SaveChanges();



                }
                bool Vat = (collection["Vat"] == "True") ? true : false;
                bool ProductSale = (collection["ProductSale"] == "True") ? true : false;
                bool Note = (collection["Note"] == "True") ? true : false;
                string Warranty = collection["Warranty"];
                string Address = collection["Address"];
                bool Transport = (collection["Transport"] == "True") ? true : false;
                string Access = collection["Access"];
                string Sale = collection["Sale"];
                if (collection["Ord"] != null)
                {
                    int Ord = int.Parse(collection["Ord"]);
                    tblproductsyn.Ord = Ord;

                }
                if (collection["Status"] != null)
                {
                    bool Status = (collection["Status"] == "True") ? true : false;
                    tblproductsyn.Status = Status;
                }
                bool Active = (collection["Active"] == "True") ? true : false;
                bool New = (collection["New"] == "True") ? true : false;
                bool ViewHomes = (collection["ViewHomes"] == "True") ? true : false;
                string Title = collection["Title"];
                string Keyword = collection["Keyword"];
                string codelist = collection["CodeSyn"];
                tblproductsyn.Visit = tblproductsyn.Visit;
                tblproductsyn.Name = Name;
                tblproductsyn.Code = Code;
 
                tblproductsyn.Description = Description;
 
                tblproductsyn.Content = Content;
                tblproductsyn.CodeSyn = codelist;
                tblproductsyn.Parameter = Parameter;
                tblproductsyn.ImageLinkThumb = ImageLinkThumb;
                tblproductsyn.ImageLinkDetail = ImageLinkDetail;
                tblproductsyn.Vat = Vat;
                tblproductsyn.Warranty = Warranty;
                tblproductsyn.Address = Address;
                tblproductsyn.Transport = Transport;
                tblproductsyn.Access = Access;
                tblproductsyn.Sale = Sale;
                tblproductsyn.Active = Active;
                tblproductsyn.New = New;
               
                tblproductsyn.DateCreate = DateTime.Now;
                tblproductsyn.ViewHomes = ViewHomes;
                tblproductsyn.Title = Title;
                tblproductsyn.Keyword = Keyword;
                 if (URL == "on")
                {
                    tblproductsyn.Tag = StringClass.NameToTag(tblproductsyn.Name);
                    clsSitemap.UpdateSitemap("/Syn/" + StringClass.NameToTag(Name), id.ToString(), "ProductSyn");
                }
                else
                {
                    tblproductsyn.Tag = collection["NameURL"];
                }

            //sdsd

                 string idUser = Request.Cookies["Username"].Values["UserID"];
                 tblproductsyn.idUser = int.Parse(idUser);
                 db.SaveChanges();

              
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit Productsyn", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion


                var listsyn = db.ProductConnects.Where(p => p.idSyn == id).ToList();
                if(listsyn.Count>0)
                { 
                for(int i=0;i<listsyn.Count;i++)
                {
                    Int32 idpp = listsyn[i].id;
                    var listchld = db.ProductConnects.First(p => p.id == idpp);
                    db.ProductConnects.Remove(listchld);
                    db.SaveChanges();
                }
                }
                string Chuoisyn = collection["CodeSyn"];
                string[] Mang = Chuoisyn.Split(',');
                ProductConnect productconnect = new ProductConnect();
                var listproductSyn = db.tblProductSyns.OrderByDescending(p => p.id).Take(1).ToList();
                int idsyn = int.Parse(listproductSyn[0].id.ToString());
                for (int i = 0; i < Mang.Length; i++)
                {
                    productconnect.idSyn = id;
                    string idpd = Mang[i].ToString();
                    productconnect.idpd = idpd;
                    db.ProductConnects.Add(productconnect);
                    db.SaveChanges();

                }
             foreach (string key in Request.Form.Cast<string>().Where(key => key.StartsWith("chek-")))
                {
                    var checkbox = "";
                    checkbox = Request.Form["" + key];
                    if (checkbox != "false")
                    {
                        Int32 idchk = Convert.ToInt32(key.Remove(0, 5));
                        var image = db.ImageProducts.First(p => p.id == idchk && p.Type == 1);
                          db.ImageProducts.Remove(image);
                        db.SaveChanges();
                    }
                }

             TempData["Msg"] = "";
             string abc = "";
             string def = "";

             if (uploadFile != null)
             {
                 foreach (var item in uploadFile)
                 {
                     if (item != null)
                     {
                         string filename = item.FileName;
                         string path = System.IO.Path.Combine(Server.MapPath("~/Images/ImagesList"), System.IO.Path.GetFileName(item.FileName));
                         item.SaveAs(path);
                         abc = string.Format("Upload {0} file thành công", uploadFile.Count);
                         def += item.FileName + "; ";
                         ImageProduct imgp = new ImageProduct();
                         imgp.idProduct = id;
                         imgp.Type = 1;
                         imgp.Images = "/Images/ImagesList/" + System.IO.Path.GetFileName(item.FileName);
                         db.ImageProducts.Add(imgp);
                         db.SaveChanges();
                     }

                 }
                 TempData["Msg"] = abc + "</br>" + def;
             }

             return RedirectToAction("Index");
        }



        public ActionResult DeleteConfirmed(int id)
        {
            tblProductSyn tblproductsyn = db.tblProductSyns.Find(id);
            clsSitemap.DeteleSitemap(id.ToString(), "Productsyn");
            db.tblProductSyns.Remove(tblproductsyn);
            db.SaveChanges();
            var listsyn = db.ProductConnects.Where(p => p.idSyn == id).ToList();
            for (int i = 0; i < listsyn.Count; i++)
            {
                Int32 idpp = listsyn[i].id;
                var listchld = db.ProductConnects.First(p => p.id == idpp);
                db.ProductConnects.Remove(listchld);
                db.SaveChanges();
            }
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete Productsyn", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult ProductEditOrd(int txtSort, string ts)
        {
            var Product = db.tblProductSyns.Find(txtSort);
            var result = string.Empty;
            Product.Ord = int.Parse(ts);
            //db.Entry(Product).State = System.Data.EntityState.Modified;
            result = "Ord Updated.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Edit Ord Productsyn", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        public PartialViewResult ListImages(int? id)
        {
           
            return PartialView();

        }
        [HttpPost]
        public ActionResult ProductEditActive(string chk, string nchecked)
        {

            var Product = db.tblProductSyns.Find(int.Parse(chk));
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
            Updatehistoty.UpdateHistory("Edit  Active Productsyn", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Active Updated.";
            return Json(new { result = result });
        }

        #region[Delete]

        public ActionResult DeleteProduct(int id)
        {
            tblProductSyn tblproductsyn = db.tblProductSyns.Find(id);
            clsSitemap.DeteleSitemap(id.ToString(), "Productsyn");

            var result = string.Empty;
            db.tblProductSyns.Remove(tblproductsyn);
            db.SaveChanges();
            var listsyn = db.ProductConnects.Where(p => p.idSyn == id).ToList();
            for (int i = 0; i < listsyn.Count; i++)
            {
                Int32 idpp = listsyn[i].id;
                var listchld = db.ProductConnects.First(p => p.id == idpp);
                db.ProductConnects.Remove(listchld);
                db.SaveChanges();
            }
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete Productsyn", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Bạn đã xóa thành công.";
            return Json(new { result = result });

        }
        [HttpPost]
   
        public string CheckValue(string text)
        {
            string chuoi = "";
            var listProduct = db.tblProductSyns.Where(p => p.Name == text).ToList();
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
                            clsSitemap.DeteleSitemap(id.ToString(), "Productsyn");

                            var Del = (from emp in db.tblProductSyns where emp.id == id select emp).SingleOrDefault();
                            db.tblProductSyns.Remove(Del);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete Productsyn", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                            var listsyn = db.ProductConnects.Where(p => p.idSyn == id).ToList();
                            for (int i = 0; i < listsyn.Count; i++)
                            {
                                Int32 idpp = listsyn[i].id;
                                var listchld = db.ProductConnects.First(p => p.id == idpp);
                                db.ProductConnects.Remove(listchld);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            if (collection["btnExport"] != null)
            {
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Export  Productsyn", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                GridView gv = new GridView();

                var listid = 0;
                List<int> exceptionList = new List<int>();
                foreach (string key in Request.Form.Keys)
                {
                    var checkbox = "";
                    if (key.StartsWith("chkitem+"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            int id = Convert.ToInt32(key.Remove(0, 8));
                            exceptionList.Add(id);
                        }
                    }
                }


                gv.DataSource = db.tblProductSyns.Where(x => exceptionList.Contains(x.id)).ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Marklist.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("Index");
            }
            if (collection["btlPrinter"] != null)
            {


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
                            //  var sp = db.tblProductSyns.Where(m => m.id == iddd).First();
                            //  chuoi+= "<tr><td>Tên sản phẩm</td><td>"+sp.Name+"</td></tr>";
                            //chuoi+="<tr> <td></td><td></td> </tr>";
                            // chuoi+=" <tr> <td>Mã sản phẩm</td><td>"+2017+"</td></tr>";
                            //chuoi = chuoi + "," + key.Remove(0, 8);
                            exceptionList.Add(idp);
                            //dem = dem + 1;

                        }
                    }
                }
                var list = db.tblProductSyns.Where(x => exceptionList.Contains(x.id)).ToList();
                return View("Printer", list);
            }
            return View();
        }

        [HttpPost]
        public ActionResult print(FormCollection a)
        {
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Printer Productsyn", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
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
        #region[Export]
        [HttpPost]
        public ActionResult ExportData(FormCollection collection)
        {
            GridView gv = new GridView();

            var listid = 0;
            List<int> exceptionList = new List<int>();
            foreach (string key in Request.Form.Keys)
            {
                var checkbox = "";
                if (key.StartsWith("chkitem+"))
                {
                    checkbox = Request.Form["" + key];
                    if (checkbox != "false")
                    {
                        int id = Convert.ToInt32(key.Remove(0, 8));
                        exceptionList.Add(id);
                    }
                }
            }


            gv.DataSource = db.tblProductSyns.Where(x => exceptionList.Contains(x.id)).ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Marklist.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Export Excel Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return View();
        }
        #endregion
        public ActionResult ShowerroInport()
        {
            string chuoi = "";
            //string kiemtra = Session["nid"].ToString();
            if ((Session["nid"] != null) && (Session["nid"] != ""))
            {
                string mang = Session["nid"].ToString();
                string[] Mang = mang.Split('-');
                int leght = Mang.Length - 1;
                ViewBag.leght = "Đang có " + leght + " Sảm sản phẩm bị lỗi ở dưới đây, bạn nhập giá thủ công cho từng sản phẩm hoặc sửa lại file excel";
                for (int i = 0; i < leght; i++)
                {
                    int id = int.Parse(Mang[i].ToString());
                    var Product = db.tblProductSyns.Find(id);
                    chuoi += "<div class=\"clo_1\"><span style=\"text-align:center; width:10%\">" + (i + 1) + "</span></div>";
                    chuoi += "<div class=\"clo_2\" style=\"width: 80%\"><a href=\"/Productad/Edit?id=" + Product.id + "\" target=\"_blank\" title=\"@item.Name\">" + Product.Name + "</a> </div>";
                    chuoi += "<br/>";
                }

                ViewBag.chuoi = chuoi;
                int countnull = int.Parse(Session["CountNULL"].ToString());
                string ncode = Session["Null"].ToString();
                if (countnull > 0)
                {
                    ViewBag.Erro = "Hiện tại có " + countnull + " mã sản phẩm có trong bảng exlcel mà không có trên web, các mã đó cụ thể là: " + ncode + ", bạn vui lòng nhập mã còn lại lên website hoặc kiểm tra lại mã trong bảng excel xem đúng không !";

                }
                Session["CountNULL"] = "";
                Session["Null"] = null;
                Session["nid"] = null;
            }
            else
            {
                return RedirectToAction("Index");

            }
            return View();
        }
    }
}
