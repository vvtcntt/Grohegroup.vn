using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROHE.Models;
namespace GROHE.Controllers.Display.Session.Product
{
    public class ProductController : Controller
    {
        // GET: Product
        private GROHEContext db = new GROHEContext();
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult PartialProductSaleHomes()
        {
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.ProductSale == true).OrderBy(p => p.Ord).ToList();
            string chuoi = "";
            if(listProduct.Count>0)
            { 
            for (int i = 0; i < listProduct.Count; i++)
            {
                int idcates = int.Parse(listProduct[i].idCate.ToString());

                string Url = db.tblGroupProducts.First(p => p.Id == idcates).Tag;
                chuoi += "<div class=\"Tear_Sale\">";
                 chuoi += " <div class=\"Box_Sale\" style=\"background:url(" + listProduct[i].ImageSale + ") no-repeat\"></div>";
                chuoi += "<div class=\"img\">";
                chuoi += " <a href=\"/1/" + listProduct[i].Tag + "\" title=\"" + listProduct[i].Name + "\"><img src=\"" + listProduct[i].ImageLinkThumb + "\" alt=\"" + listProduct[i].Name + "\" title=\"" + listProduct[i].Name + "\" /></a>";
                chuoi += "</div>";
                chuoi += "<h2><a href=\"/1/" + listProduct[i].Tag + "\" title=\"" + listProduct[i].Name + "\" class=\"Name\">" + listProduct[i].Name + "</a></h2>";
                chuoi += "<div class=\"Info\">";
                chuoi += "<div class=\"Left_Info\">";
                chuoi += "<div class=\"Top_Left_Info\">";
                if (listProduct[i].Status == true)
                { chuoi += "<span class=\"Status\"></span>"; }
                else
                { chuoi += "<span class=\"Status1\"></span>"; }
                 chuoi += "</div>";
                chuoi += "<div class=\"Buttom_Left_Info\">";
                //load tính năng
            
                int id = int.Parse(listProduct[i].id.ToString());
                var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == id).ToList();
                if (checkfun.Count > 0)
                {
                    for (int j = 0; j < listfuc.Count; j++)
                    {
                        int idfun = int.Parse(listfuc[j].id.ToString());
                        var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == id).ToList();
                        if (connectfun.Count > 0)
                        {
                            chuoi += "<a href=\"" + listfuc[j].Url + "\" rel=\"nofollow\" title=\"" + listfuc[j].Name + "\"><img src=\"" + listfuc[j].Images + "\" alt=\"" + listfuc[j].Name + "\" /></a>";
                        }
                    }

                }
                chuoi += "</div>";
                chuoi += "</div>";
                chuoi += "<div class=\"Right_Info\">";
                chuoi += "<span class=\"Pricesale\">" + string.Format("{0:#,#}", listProduct[i].PriceSale) + "<span>đ</span></span>";
                chuoi += "<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[i].Price) + "đ</span>";
                chuoi += "</div>";
                chuoi += "</div>";
                chuoi += "</div>";

            }
            }
            ViewBag.chuoi = chuoi;
            var listImgesadw = db.tblImages.Where(p => p.Active == true && p.idMenu == 9).OrderByDescending(p=>p.Ord).Take(1).ToList();
            string chuoispkm="";
            if (listImgesadw.Count > 0)
            {
                chuoispkm += "<div id=\"Tear_SalePriority\">";
                chuoispkm += "<a href=\"" + listImgesadw[0].Url + "\" title=\"" + listImgesadw[0].Name + "\"><img src=\"" + listImgesadw[0].Images + "\" alt=\"" + listImgesadw[0].Name + "\" title=\"" + listImgesadw[0].Name + "\" /></a>";
                chuoispkm += "</div>";
            }
         ViewBag.chuoikm = chuoispkm;
            return PartialView();
        }
        public PartialViewResult PartialProductHomes()
        {
            var chuoi = "";
            var MenuParent = db.tblGroupProducts.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < MenuParent.Count; i++)
            {
                chuoi += "<div class=\"ProductHomes\">";
                chuoi += "<div class=\"nVar\">";
                chuoi += "<div class=\"Left_nVar\">";
                chuoi += "<div class=\"Name\">";
                chuoi += "<span class=\"iCon\" style=\"background:url(" + MenuParent[i].iCon + ") no-repeat\"></span>";
                chuoi += "<h2>" + MenuParent[i].Name + "</h2>";
                chuoi += "</div>";
                chuoi += "<div class=\"iCon_nVar\">";
                chuoi += "<span>T" + (i + 1) + "</span>";
                chuoi += "</div>";
                chuoi += "</div>";
                chuoi += "<div class=\"Right_nVar\">";
                string level = MenuParent[i].Level;
                var Menuchild = db.tblGroupProducts.Where(p => p.Level.Substring(0, level.Length) == level && p.Level.Length > level.Length && p.Active == true).OrderBy(p => p.Ord).Take(4).ToList();
                if (Menuchild.Count > 0)
                {
                    chuoi += "<ul class=\"ul_1\">";
                    for (int j = 0; j < Menuchild.Count; j++)
                    {
                        string ntag = Menuchild[j].Tag;
                       
                        chuoi += "<li class=\"li_1\">";

                        chuoi += " <a href=\"/0/" + ntag + "\" title=\"" + Menuchild[j].Name + "\"> " + Menuchild[j].Name + "<span></span></a>";
                        string level1 = Menuchild[j].Level;
                        var Menuchild1 = db.tblGroupProducts.Where(p => p.Active == true && p.Level.Substring(0, level1.Length) == level1 && p.Level.Length > level1.Length).OrderBy(p => p.Ord).ToList();
                        if (Menuchild1.Count > 0)
                        {
                            chuoi += "<ul class=\"ul_2\">";
                            for (int k = 0; k < Menuchild1.Count; k++)
                            {
                                chuoi += "<li class=\"li_2\">";
                                chuoi += "<a href=\"/0/" + Menuchild1[k].Tag + "\" title=\"" + Menuchild1[k].Name + "\">› " + Menuchild1[k].Name + "</a>";
                                chuoi += "</li>";
                            }
                            chuoi += "</ul>";
                        }

                        chuoi += " </li>";
                    }
                    chuoi += "  </ul>";
                }
                string tag1 = MenuParent[i].Tag;
                
                chuoi += "<a href=\"/0/" + tag1 + "\" title=\"Xem chi tiết\" class=\"Xemchitiet\">Xem thêm &raquo;</a>";
                chuoi += "</div>";
                chuoi += "</div>";

                chuoi += " <div class=\"bg_ProductHomes\">";
                chuoi += "<div class=\"Content_ProductHomes\">";
                chuoi += "<div class=\"Left_Content_ProductHomes\">";
                int idmenu = int.Parse(MenuParent[i].Id.ToString());
                var listImages = db.tblImages.Where(p => p.idMenu == 3 && p.Active == true && p.idCate == idmenu).OrderBy(p => p.Ord).ToList();
                for (int x = 0; x < listImages.Count; x++)
                {
                    chuoi += " <a href=\"" + listImages[x].Url + "\" title=\"" + listImages[x].Name + "\" rel=\"nofollow\">";
                    chuoi += "<img src=\"" + listImages[x].Images + "\" alt=\"" + listImages[x].Name + "\" />";
                    chuoi += "</a>";
                }
                chuoi += "</div>";
                chuoi += "<div class=\"Right_Content_ProductHomes\">";
                var listGroup = db.tblGroupProducts.Where(p => p.Active == true && p.Level.Substring(0, level.Length) == level).OrderBy(p => p.Ord).ToList();
                for (int z = 0; z < listGroup.Count; z++)
                {
                    string Url = listGroup[z].Tag;
                    int idm = int.Parse(listGroup[z].Id.ToString());
                    var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idm && p.ViewHomes == true).OrderBy(p => p.Ord).ToList();
                    for (int y = 0; y < listProduct.Count; y++)
                    {
                        chuoi += "<div class=\"Tear_1\">";
                        chuoi += "<div class=\"img\">";
                        chuoi += "<a href=\"/1/" + listProduct[y].Tag + "\" title=\"" + listProduct[y].Name + "\">";
                        chuoi += "<img src=\"" + listProduct[y].ImageLinkThumb + "\" alt=\"" + listProduct[y].Name + "\" title=\"" + listProduct[y].Name + "\"  />";
                        chuoi += "</a>";
                        chuoi += "</div>";
                        chuoi += "<h3><a href=\"/1/" + listProduct[y].Tag + "\" title=\"" + listProduct[y].Name + "\" class=\"Name\">" + listProduct[y].Name + "</a></h3>";
                        chuoi += "<div class=\"Info\">";

                        chuoi += " <div class=\"LeftInfo\">";
                        if (listProduct[y].PriceSale<10)
                        { chuoi += "<span class=\"PriceSale\">Liên hệ</span>"; }
                        else
                        chuoi += "<span class=\"PriceSale\">" + string.Format("{0:#,#}", listProduct[y].PriceSale) + "đ</span>";
                        if (listProduct[y].Price<10)
                        { chuoi += "<span class=\"Price\">Liên hệ</span>"; }
                        else
                        chuoi += "<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[y].Price) + "đ</span>";
                        chuoi += "</div>";
                        chuoi += "<div class=\"RightInfo\">";
                        chuoi += "<div class=\"Top_RightInfo\">";
                        chuoi += "<a href=\"/Order/OrderIndex?idp=" + listProduct[y].id + "\" title=\"Đặt hàng\" rel=\"nofollow\"><span></span></a>";
                        chuoi += "</div>";
                        chuoi += "<div class=\"Bottom_RightInfo\">";
                        int id = int.Parse(listProduct[y].id.ToString());
                        var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                        var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == id).ToList();
                        if (checkfun.Count > 0)
                        {
                            for (int j = 0; j < listfuc.Count; j++)
                            {
                                int idfun = int.Parse(listfuc[j].id.ToString());
                                var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == id).ToList();
                                if (connectfun.Count > 0)
                                {
                                    chuoi += "<a href=\"" + listfuc[j].Url + "\" rel=\"nofollow\" title=\"" + listfuc[j].Name + "\"><img src=\"" + listfuc[j].Images + "\" alt=\"" + listfuc[j].Name + "\" /></a>";
                                }
                            }

                        }

                        chuoi += "</div>";
                        chuoi += "</div>";
                        chuoi += "</div>";
                        chuoi += "</div>";
                    }
                }
                    chuoi += "</div>";
                    chuoi += "</div>";
                    chuoi += "</div>";
                    chuoi += "</div>";
                }
            
            ViewBag.chuoi = chuoi;
            return PartialView();
        }
        public ActionResult ProductDetail(string tag)
        {

            tblProduct Product = db.tblProducts.First(p => p.Tag == tag);
            int id = int.Parse(Product.id.ToString());
            int visit = int.Parse(Product.Visit.ToString());
            if (visit > 0)
            {
                Product.Visit = Product.Visit + 1;
                db.SaveChanges();
            }
            else
            {
                Product.Visit = Product.Visit + 1;
                db.SaveChanges();
            }
            ViewBag.Title = "<title>" + Product.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + Product.Title + "\" />";
            ViewBag.dcDescription = "<meta name=\"DC.description\" content=\"" + Product.Description + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + Product.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + Product.Keyword + "\" /> ";
            //Load Images Liên Quan
            var listImage = db.ImageProducts.Where(p => p.idProduct == id).ToList();
            string chuoiimages = "";
            for (int i = 0; i < listImage.Count; i++)
            {
                chuoiimages += " <li class=\"Tear_pl\"><a href=\"" + listImage[i].Images + "\" rel=\"prettyPhoto[gallery1]\" title=\"" + listImage[i].Name + "\"><img src=\"" + listImage[i].Images + "\"   alt=\"" + listImage[i].Name + "\" title=\"" + listImage[i].Name + "\" /></a></li>";
            }
            ViewBag.chuoiimage = chuoiimages;

            int idMenu = int.Parse(Product.idCate.ToString());
            ViewBag.Nhomsp = db.tblGroupProducts.First(p => p.Id == idMenu).Name;
            string code = Product.Code;
            //Load sản phẩm đổng bộ
            var ProductSyn = db.ProductConnects.Where(p => p.idpd == code).ToList();
            List<int> exceptionList = new List<int>();
            for (int i = 0; i < ProductSyn.Count; i++)
            {
                exceptionList.Add(int.Parse(ProductSyn[i].idSyn.ToString()));
            }
            string chuoisym = "";
            var listSyn = db.tblProductSyns.Where(x => exceptionList.Contains(x.id)).ToList();
            if (listSyn.Count > 0)
            {
                chuoisym += "<div id=\"Top7\">";
                chuoisym += "<div id=\"iCon\"></div>";
                chuoisym += "<div id=\"Content_Top7\"><p>Hiện tại sản phẩm <span>" + Product.Name + "</span> có giá rẻ hơn khi mua sản phẩm đồng bộ, bạn có thể xem sản phẩm đồng bộ này</p>";
                chuoisym += "<ul>";
                for (int i = 0; i < listSyn.Count; i++)
                {
                    chuoisym += "<li><a href=\"/Syn/" + listSyn[i].Tag + "\" title=\"" + listSyn[i].Name + "\" class=\"Syn\" rel=\"nofollow\"><span></span> " + listSyn[i].Name + "</a></li>";
                }
                chuoisym += "</ul>";
                chuoisym += " </div>";
                chuoisym += "</div>";
            }
            ViewBag.chuoisym = chuoisym;
            string nUrl = "";
            int idCate = int.Parse(Product.idCate.ToString());
            tblGroupProduct grouproduct = db.tblGroupProducts.Find(idCate);
            int dodai = grouproduct.Level.Length / 5;
            for (int i = 0; i < dodai; i++)
            {
                var NameGroups = db.tblGroupProducts.First(p => p.Level.Substring(0, (i + 1) * 5) == grouproduct.Level.Substring(0, (i + 1) * 5) && p.Level.Length == (i + 1) * 5);
                string ids = NameGroups.Id.ToString();
                string levals = grouproduct.Level.Substring(0, (i + 1) * 5);

 
                nUrl = nUrl + " <a href=\"/0/" + NameGroups.Tag + "\" title=\"" + NameGroups.Name + "\"> " + " " + NameGroups.Name + "</a> /";
            }
            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chủ\" rel=\"nofollow\"><span class=\"iCon\"></span> Trang chủ</a> /" + nUrl;
            // Load màu sản phẩm
            string chuoicolor = "";

            var listcolor = db.tblColorProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            var kiemtracolor = db.tblConnectColorProducts.Where(p => p.idPro == id).ToList();
            if (kiemtracolor.Count > 0)
            {

                chuoicolor += "<div id=\"Top4\">";
                chuoicolor += "<span> Màu sản phẩm : </span>";
                chuoicolor += " <div id=\"Left_Top4\">";
                for (int i = 0; i < listcolor.Count; i++)
                {
                    int idcolor = int.Parse(listcolor[i].id.ToString());
                    var listconnectcolor = db.tblConnectColorProducts.Where(p => p.idPro == id && p.idColor == idcolor).ToList();
                    if (listconnectcolor.Count > 0)
                    {

                        chuoicolor += "<span class=\"Mau\" style=\"background:url(" + listcolor[i].Images + ") no-repeat; background-size:100%\"></span> ";
                    }

                }
                chuoicolor += "</div>";
                chuoicolor += "</div>";
            }

            ViewBag.color = chuoicolor;
            //load tính năng
            string chuoifun = "";
            var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == id).ToList();
            if (checkfun.Count > 0)
            {

                chuoifun += " <div id=\"Tech\">";
                chuoifun += "<span class=\"tinhnang\">Những tính năng nổi bật của " + Product.Name + "</span>";
                for (int i = 0; i < listfuc.Count; i++)
                {
                    int idfun = int.Parse(listfuc[i].id.ToString());
                    var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == id).ToList();
                    if (connectfun.Count > 0)
                    {
                        chuoifun += "<div class=\"Tear_tech\">";
                        chuoifun += "<span class=\"imagetech\" style=\"background:url(" + listfuc[i].Images + ") no-repeat center center scroll transparent;\"></span>";
                        chuoifun += "<span class=\"Destech\">" + listfuc[i].Name + "</span>";
                        chuoifun += "<p>Xem chi tiết về " + listfuc[i].Name + " <a href=\"" + listfuc[i].Url + "\" title=\"" + listfuc[i].Name + "\">Tại đây &raquo;</a></p>";
                        chuoifun += "</div>";
                    }
                }
                chuoifun += "</div>";

            }
            ViewBag.chuoifun = chuoifun;
            //Load file kỹ thuật

            var filesbaogia = db.tblFiles.Where(p => p.idp == id & p.Cate == 1).Take(1).ToList();
            string files="";
            if(filesbaogia.Count>0)
            {
                files += "<object src=\"" + filesbaogia[0].File + "\"><embed src=\"" + filesbaogia[0].File + "\"></embed></object>";
                ViewBag.thongso = files;
            }
            return View(Product);
        }
        public PartialViewResult PartialRightProductDetail(string tag)
        {

            tblProduct Product = db.tblProducts.First(p => p.Tag == tag);
            int id = int.Parse(Product.id.ToString());
            tblConfig tblconfig = db.tblConfigs.First();
            string chuoisupport = "";
            var listSupport = db.tblSupports.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listSupport.Count; i++)
            {
                chuoisupport += "<div class=\"Line_Buttom\"></div>";
                chuoisupport += "<div class=\"Tear_Supports\">";
                chuoisupport += "<div class=\"Left_Tear_Support\">";
                chuoisupport += "<span class=\"htv1\">" + listSupport[i].Mission + ":</span>";
                chuoisupport += "<span class=\"htv2\">" + listSupport[i].Name + " :</span>";
                chuoisupport += "</div>";
                chuoisupport += "<div class=\"Right_Tear_Support\">";
                chuoisupport += "<a href=\"ymsgr:sendim?" + listSupport[i].Yahoo + "\">";
                chuoisupport += "<img src=\"http://opi.yahoo.com/online?u=" + listSupport[i].Yahoo + "&m=g&t=1\" alt=\"Yahoo\" class=\"imgYahoo\" />";
                chuoisupport += " </a>";
                chuoisupport += "<a href=\"Skype:" + listSupport[i].Skyper + "?chat\">";
                chuoisupport += "<img class=\"imgSkype\" src=\"/Content/Display/iCon/skype-icon.png\" title=\"Kangaroo\" alt=\"" + listSupport[i].Name + "\">";
                chuoisupport += "</a>";
                chuoisupport += "</div>";
                chuoisupport += "</div>";
            }
            ViewBag.chuoisupport = chuoisupport;

            //lIST Menu
            int idCate = int.Parse(Product.idCate.ToString());
            tblGroupProduct grouproduct = db.tblGroupProducts.Find(idCate);
            string level = grouproduct.Level.ToString();
            int leght = level.Length;
            string chuoimenu = "";
            var listGroupProduct = db.tblGroupProducts.Where(p => p.Level.Substring(0, leght - 5) == level.Substring(0, leght - 5) && p.Active == true && p.Level.Length == level.Length).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listGroupProduct.Count; i++)
            {
                string ntag = listGroupProduct[i].Tag;

                chuoimenu += "<h2><a href=\"/0/" + ntag + "\" title=\"" + listGroupProduct[i].Name + "\">› " + listGroupProduct[i].Name + "</a></h2>";

            }
            ViewBag.chuoimenu = chuoimenu;
            //Load sản phẩm liên quan
            string Url = grouproduct.Tag;
            string chuoiproduct = "";
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idCate).OrderByDescending(p => p.Visit).OrderBy(p => p.Ord).Take(5).ToList();
            for (int i = 0; i < listProduct.Count; i++)
            {

                chuoiproduct += " <div class=\"Tear_1\">";
                chuoiproduct += "<div class=\"img\">";
                chuoiproduct += "<a href=\"/1/" + listProduct[i].Tag + "\" title=\"" + listProduct[i].Name + "\">";
                chuoiproduct += "<img src=\"" + listProduct[i].ImageLinkThumb + "\" alt=\"" + listProduct[i].Name + "\" />";
                chuoiproduct += "</a>";
                chuoiproduct += "</div>";
                chuoiproduct += "<h3><a href=\"/1/" + listProduct[i].Tag + "\" title=\"" + listProduct[i].Name + "\" class=\"Name\">" + listProduct[i].Name + "</a></h3>";
                chuoiproduct += "<div class=\"Info\">";
                chuoiproduct += "<div class=\"LeftInfo\">";
                if (listProduct[i].PriceSale<10)
                chuoiproduct += "<span class=\"PriceSale\">Liên hệ</span>";
                else
                    chuoiproduct += "<span class=\"PriceSale\">" + string.Format("{0:#,#}", listProduct[i].PriceSale) + "đ</span>";
                if (listProduct[i].Price < 10)
                chuoiproduct += "<span class=\"Price\">Liên hệ</span>";
                else
                    chuoiproduct += "<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[i].Price) + "đ</span>";
                chuoiproduct += " </div>";
                chuoiproduct += "<div class=\"RightInfo\">";
                chuoiproduct += "<div class=\"Top_RightInfo\">";
                chuoiproduct += "<a href=\"/Order/OrderIndex?idp=" + listProduct[i].id + "\" title=\"" + listProduct[i].Name + "\" rel=\"nofollow\"><span></span></a>";
                chuoiproduct += "</div>";
                chuoiproduct += "<div class=\"Bottom_RightInfo\">";
                int ids = int.Parse(listProduct[i].id.ToString());
                var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == ids).ToList();
                if (checkfun.Count > 0)
                {
                    for (int j = 0; j < listfuc.Count; j++)
                    {
                        int idfun = int.Parse(listfuc[j].id.ToString());
                        var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == ids).ToList();
                        if (connectfun.Count > 0)
                        {
                            chuoiproduct += "<a href=\"" + listfuc[j].Url + "\" rel=\"nofollow\" title=\"" + listfuc[j].Name + "\"><img src=\"" + listfuc[j].Images + "\" alt=\"" + listfuc[j].Name + "\" /></a>";
                        }
                    }

                }
                chuoiproduct += "</div>";
                chuoiproduct += "</div>";
                chuoiproduct += "</div>";
                chuoiproduct += "</div>";
            }
            ViewBag.chuoiproduct = chuoiproduct;
            return PartialView(tblconfig);
         }
        public ActionResult ListProduct(string tag)
        {

            var GroupProduct = db.tblGroupProducts.First(p => p.Tag == tag);
            ViewBag.Title = "<title>" + GroupProduct.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + GroupProduct.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + GroupProduct.Keyword + "\" /> ";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + GroupProduct.Title + "\" />";
            ViewBag.dcDescription = "<meta name=\"DC.description\" content=\"" + GroupProduct.Description + "\" />";
            string nUrl = "";
            int dodai = GroupProduct.Level.Length / 5;
            for (int i = 0; i < dodai; i++)
            {
                var NameGroups = db.tblGroupProducts.First(p => p.Level.Substring(0, (i + 1) * 5) == GroupProduct.Level.Substring(0, (i + 1) * 5) && p.Level.Length == (i + 1) * 5);
                string id = NameGroups.Id.ToString();
                string ntaga = NameGroups.Tag;
                string levals = GroupProduct.Level.Substring(0, (i + 1) * 5);
                nUrl = nUrl + " <a href=\"/0/" + ntaga + "\" title=\"" + NameGroups.Name + "\"> " + " " + NameGroups.Name + "</a> /";
            }
            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chủ\" rel=\"nofollow\"><span class=\"iCon\"></span> Trang chủ</a> /" + nUrl+"/<h1>"+GroupProduct.Title+"</h1>";

             string level = GroupProduct.Level;

             string chuoi = "";
            var listGroupProduct = db.tblGroupProducts.Where(p => p.Active == true && p.Level.Substring(0, level.Length) == level && p.Level.Length>level.Length).OrderBy(p => p.Ord).ToList();
            if (listGroupProduct.Count > 0)
            {
                for (int i = 0; i < listGroupProduct.Count; i++)
                {
                    chuoi += " <div class=\"List_product\">";
                    chuoi += " <div id=\"Box_Name\">";
                    chuoi += "<div id=\"Leff_BoxName\">   <h2>" + listGroupProduct[i].Name + "</h2></div>";
                    chuoi += "</div>";
                    chuoi += "<div class=\"Clear\"></div>";
                    chuoi += "<div class=\"ContentProduct\">";
                    int idcate = int.Parse(listGroupProduct[i].Id.ToString());
                    var listProduct = db.tblProducts.Where(p => p.idCate == idcate && p.Active == true).OrderBy(p => p.Ord).ToList();
                    for (int j = 0; j < listProduct.Count; j++)
                    {
                        chuoi += "<div class=\"Tear_1\">"; 
                        chuoi += "<div class=\"img\">";
                        chuoi += "<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\">";
                        chuoi += "<img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" />";
                        chuoi += "</a>";
                        chuoi += "</div>";
                        chuoi += "<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\" class=\"Name\">" + listProduct[j].Name + "</a>";
                        chuoi += "<div class=\"Info\">";
                        chuoi += "<div class=\"LeftInfo\">";
                        if (listProduct[j].PriceSale<10)
                            chuoi += "<span class=\"PriceSale\">Liên hệ</span>";
                        else
                        chuoi += "<span class=\"PriceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>";
                        if (listProduct[j].Price < 10)
                        { chuoi += "<span class=\"Price\">Liên hệ</span>"; }
                        else
                        chuoi += "<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>";
                        chuoi += "</div>";
                        chuoi += "<div class=\"RightInfo\">";
                        chuoi += "<div class=\"Top_RightInfo\">";
                        chuoi += "<a href=\"\" title=\"\"><span></span></a>";
                        chuoi += "</div>";
                        chuoi += " <div class=\"Bottom_RightInfo\">";
                        int ids = int.Parse(listProduct[j].id.ToString());
                        var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                        var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == ids).ToList();
                        if (checkfun.Count > 0)
                        {
                            for (int z = 0; z < listfuc.Count; z++)
                            {
                                int idfun = int.Parse(listfuc[z].id.ToString());
                                var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == ids).ToList();
                                if (connectfun.Count > 0)
                                {
                                    chuoi += "<a href=\"" + listfuc[z].Url + "\" rel=\"nofollow\" title=\"" + listfuc[z].Name + "\"><img src=\"" + listfuc[z].Images + "\" alt=\"" + listfuc[z].Name + "\" /></a>";
                                }
                            }

                        }
                        chuoi += "</div>";
                        chuoi += "</div>";
                        chuoi += "</div>";
                        chuoi += "</div>";
                    }
                    chuoi += "<div class=\"Clear\"></div>";
                    chuoi += "</div>";
                    chuoi += "</div>";
                }
            }
            else
            {
                chuoi += " <div class=\"List_product\">";
                chuoi += " <div id=\"Box_Name\">";
                chuoi += "<div id=\"Leff_BoxName\">   <h2>" + GroupProduct.Name + "</h2></div>";
                 chuoi += "<div id=\"Rigt_Box_Name\">";
                         chuoi += "<select>";
                             chuoi += "<option value=\"0\"> - Sắp xếp -</option>";
                             chuoi += "<option value=\"1\"> - Giá tăng dần -</option>";
                             chuoi += "<option value=\"1\"> - GIá giảm giần -</option>";
                        chuoi += " </select>";
                        chuoi += "</div>";
                chuoi += "</div>";
                chuoi += "<div class=\"Clear\"></div>";
                chuoi += "<div class=\"ContentProduct\">";
                int idcate = int.Parse(GroupProduct.Id.ToString());
                var listProduct = db.tblProducts.Where(p => p.idCate == idcate && p.Active == true).OrderBy(p => p.Ord).ToList();
                for (int j = 0; j < listProduct.Count; j++)
                {
                    chuoi += "<div class=\"Tear_1\">";
                    chuoi += "<div class=\"img\">";
                    chuoi += "<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\">";
                    chuoi += "<img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" />";
                    chuoi += "</a>";
                    chuoi += "</div>";
                    chuoi += "<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\" class=\"Name\">" + listProduct[j].Name + "</a>";
                    chuoi += "<div class=\"Info\">";
                    chuoi += "<div class=\"LeftInfo\">";
                    if (listProduct[j].PriceSale<10)
                    chuoi += "<span class=\"PriceSale\">Liên hệ</span>";
                    else
                    chuoi += "<span class=\"PriceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>";
                    if (listProduct[j].Price<10)
                    chuoi += "<span class=\"Price\">Liên hệ</span>";
                    else
                     chuoi += "<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>";   
                    chuoi += "</div>";
                    chuoi += "<div class=\"RightInfo\">";
                    chuoi += "<div class=\"Top_RightInfo\">";
                    chuoi += "<a href=\"\" title=\"\"><span></span></a>";
                    chuoi += "</div>";
                    chuoi += " <div class=\"Bottom_RightInfo\">";
                    int ids = int.Parse(listProduct[j].id.ToString());
                    var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                    var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == ids).ToList();
                    if (checkfun.Count > 0)
                    {
                        for (int z = 0; z < listfuc.Count; z++)
                        {
                            int idfun = int.Parse(listfuc[z].id.ToString());
                            var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == ids).ToList();
                            if (connectfun.Count > 0)
                            {
                                chuoi += "<a href=\"" + listfuc[z].Url + "\" rel=\"nofollow\" title=\"" + listfuc[z].Name + "\"><img src=\"" + listfuc[z].Images + "\" alt=\"" + listfuc[z].Name + "\" /></a>";
                            }
                        }

                    }
                    chuoi += "</div>";
                    chuoi += "</div>";
                    chuoi += "</div>";
                    chuoi += "</div>";
                }
                chuoi += "<div class=\"Clear\"></div>";
                chuoi += "</div>";
                chuoi += "</div>";

            }


            ViewBag.chuoi = chuoi;

            string catalogis = "";
            int idg=int.Parse(GroupProduct.Id.ToString());
            var tblcatalogis = db.tblFiles.Where(p => p.idg == idg).ToList();
            if(tblcatalogis.Count>0)
            { 
            catalogis += "<div id=\"Download\">";
            catalogis+=" <a href=\""+tblcatalogis[0].File+"\" title=\""+tblcatalogis[0].Name+"\"><span></span></a>";
            catalogis += "</div>";
            }
            ViewBag.catalogis = catalogis;
            return View(GroupProduct);
        }
        public ActionResult Command(FormCollection collection, string tag)
        {
            if (collection["btnOrder"] != null)
            {

                Session["idProduct"] = collection["idPro"];
                Session["idMenu"] = collection["idCate"];
                Session["OrdProduct"] = collection["txtOrd"];
                Session["Url"] = Request.Url.ToString();
                return RedirectToAction("OrderIndex", "Order");
            }
            return View();
        }
        public ActionResult SearchProduct(string tag)
        {
            string chuoi = "";
            ViewBag.Title = "<title> Tìm kiếm : " + tag + "</title>";
            ViewBag.name = tag;
            ViewBag.Description = "<meta name=\"description\" content=\"" + tag + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tag + "\" /> ";
            chuoi += "   <div class=\"Name_Cate\">";

            string chuoiproduct = "";
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.Name.Contains(tag)).OrderBy(p => p.Ord).ToList();
            for (int j = 0; j < listProduct.Count; j++)
            {
                int idcate = int.Parse(listProduct[j].idCate.ToString());
                string GroupProduct = db.tblGroupProducts.First(p => p.Id == idcate).Tag;
                string Url = GroupProduct;
                chuoiproduct += " <div class=\"Tear_1\">";
                chuoiproduct += "<div class=\"img\">";
                chuoiproduct += "<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\">";
                chuoiproduct += "<img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" />";
                chuoiproduct += "</a>";
                chuoiproduct += "</div>";
                chuoiproduct += "<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\" class=\"Name\">" + listProduct[j].Name + "</a>";
                chuoiproduct += "<div class=\"Info\">";
                chuoiproduct += "<div class=\"LeftInfo\">";
                if(listProduct[j].PriceSale<10)
                    chuoiproduct += "<span class=\"PriceSale\">Liên hệ</span>";
                else
                chuoiproduct += "<span class=\"PriceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>";
                if (listProduct[j].Price<10)
                { 
                chuoiproduct += "<span class=\"Price\">Liên hệ</span>";
                } 
                else
                    chuoiproduct += "<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>";
                chuoiproduct += " </div>";
                chuoiproduct += "<div class=\"RightInfo\">";
                chuoiproduct += "<div class=\"Top_RightInfo\">";
                chuoiproduct += "<a href=\"/Order/OrderIndex?idp=" + listProduct[j].id + "\" title=\"" + listProduct[j].Name + "\" rel=\"nofollow\"><span></span></a>";
                chuoiproduct += "</div>";
                chuoiproduct += "<div class=\"Bottom_RightInfo\">";
                int ids = int.Parse(listProduct[j].id.ToString());
                var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == ids).ToList();
                if (checkfun.Count > 0)
                {
                    for (int z = 0; z < listfuc.Count; z++)
                    {
                        int idfun = int.Parse(listfuc[z].id.ToString());
                        var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == ids).ToList();
                        if (connectfun.Count > 0)
                        {
                            chuoiproduct += "<a href=\"" + listfuc[z].Url + "\" rel=\"nofollow\" title=\"" + listfuc[z].Name + "\"><img src=\"" + listfuc[z].Images + "\" alt=\"" + listfuc[z].Name + "\" /></a>";
                        }
                    }

                }
                chuoiproduct += "</div>";
                chuoiproduct += "</div>";
                chuoiproduct += "</div>";
                chuoiproduct += "</div>";
            }

            ViewBag.chuoisanpham = chuoiproduct;
            return View();
        }
        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            string tag = collection["txtSearch"];
            return Redirect("/Search/" + tag + "");
        }
    }
}