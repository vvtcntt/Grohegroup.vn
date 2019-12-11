using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GROHE.Models;
namespace GROHE.Controllers.Display.Session.ProductSyn
{
    public class ProductSynDisplayController : Controller
    {
        GROHEContext db = new GROHEContext();
        //
        // GET: /ProductSynDisplay/
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ListProductSyn()
        {
            StringBuilder chuoi = new StringBuilder();
            var listProductSyn = db.tblProductSyns.Where(p => p.Active == true && p.ViewHomes == true).OrderBy(p => p.Ord).ToList();

            chuoi.Append("<div id=\"ProductSYN\">");
            if (listProductSyn.Count > 0)
            {
                chuoi.Append("<div class=\"n_var\">");
                chuoi.Append("<div class=\"Left_n_Var\"></div>");
                chuoi.Append("<div class=\"Center_n_Var\"><h3>Sản phẩm GROHE đồng bộ</h3></div>");
                chuoi.Append("<div class=\"Right_n_Var\"></div>");
                chuoi.Append("</div>");

            }
            chuoi.Append("<div class=\"Adw_Syn\">");
            var listImage = db.tblImages.Where(p => p.Active == true && p.idCate == 2).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listImage.Count; i++)
            {
                chuoi.Append("<a href=\"" + listImage[i].Url + "\" title=\"" + listImage[i].Name + "\"><img src=\"" + listImage[i].Images + "\" alt=\"" + listImage[i].Name + "\"/></a>");

            }
            chuoi.Append(" </div>");
            if (listProductSyn.Count > 0)
            {
                chuoi.Append("<div id=\"Content_ProductSYN\">");

                chuoi.Append("<div class=\"owl-carousel owl-theme\">");
                for (int i = 0; i < listProductSyn.Count; i++)
                {
                    chuoi.Append("<div class=\"item spdb\">");
                    chuoi.Append("<div class=\"sptb\"></div>");
                    chuoi.Append("<div class=\"img_spdb\">");
                    chuoi.Append("<a href=\"/syn/" + listProductSyn[i].Tag + "\" title=\"" + listProductSyn[i].Name + "\"><img src=\"" + listProductSyn[i].ImageLinkThumb + "\" alt=\"" + listProductSyn[i].Name + "\" /></a>");
                    chuoi.Append("</div>");
                    chuoi.Append("<a href=\"/syn/" + listProductSyn[i].Tag + "\" class=\"Name\" title=\"" + listProductSyn[i].Name + "\">" + listProductSyn[i].Name + "</a>");
                    chuoi.Append("<div class=\"Bottom_Tear_Sale\">");
                    chuoi.Append("<div class=\"Price\">");
                    chuoi.Append("<p class=\"PriceSale\">" + string.Format("{0:#,#}", listProductSyn[i].PriceSale) + " <span>đ</span></p>");
                    chuoi.Append(" <p class=\"Price_s\">" + string.Format("{0:#,#}", listProductSyn[i].Price) + "</p>");
                    chuoi.Append("</div>");
                    chuoi.Append("</div>");
                    chuoi.Append("</div>");
                }
                chuoi.Append("</div>");
                chuoi.Append("</div>");
            }
            chuoi.Append("</div>");

            ViewBag.chuoisyc = chuoi;
            return PartialView();
        }

        public ActionResult ProductSyn_Detail(string tag)
        {
            var tblproductSyn = db.tblProductSyns.First(p => p.Tag == tag);
            int id = int.Parse(tblproductSyn.id.ToString());
            string chuoi = "Khách hàng vui lòng kích vào chi tiết từng sản phẩm ở trên để xem thông thông số kỹ thuật !";
            ViewBag.Title = "<title>" + tblproductSyn.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblproductSyn.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblproductSyn.Keyword + "\" /> ";
            //Load Images Liên Quan
            var listImage = db.ImageProducts.Where(p => p.idProduct == id && p.Type==1).ToList();
            string chuoiimages = "";
            for (int i = 0; i < listImage.Count; i++)
            {
                chuoiimages += " <li class=\"Tear_pl\"><a href=\"" + listImage[i].Images + "\" rel=\"prettyPhoto[gallery1]\" title=\"" + listImage[i].Name + "\"><img src=\"" + listImage[i].Images + "\"   alt=\"" + listImage[i].Name + "\" /></a></li>";
            }
            ViewBag.chuoiimage = chuoiimages;
            int idsyn = int.Parse(tblproductSyn.id.ToString());
             
            var Product = db.ProductConnects.Where(p => p.idSyn == idsyn).ToList();
            string chuoipr = "";
            string chuoisosanh = "";
            float tonggia = 0;
            if (Product.Count > 0)
            {
                chuoipr += "<div id=\"Content_spdb\">";
                chuoipr += "<span class=\"tinhnang\">&diams; Danh sách sản phẩm có trong " + tblproductSyn.Name + "</span>";
                chuoisosanh += "<div id=\"equa\">";
                chuoisosanh += "<div class=\"nVar_Equa\"><span>Bảng so sánh giá mua lẻ và mua theo bộ</span></div>";
                chuoisosanh += "<div class=\"Clear\"></div>";
                chuoisosanh += "<table width=\"200\" border=\"1\">";
                chuoisosanh += "<tr style=\"color:#333; text-transform:uppercase; line-height:25px; text-align:center\">";
                chuoisosanh += "<td style=\"width:5%;text-align:center\">STT</td>";
                chuoisosanh += "<td style=\"width:40%\">Tên Sản phẩm</td>";
                chuoisosanh += "<td style=\"width:10%;text-align:center\">Số lượng</td>";
                chuoisosanh += "<td style=\"width:20%;text-align:center\">Đơn Giá</td>";
                chuoisosanh += "<td style=\"text-align:center; width:20%\">Thành Tiền</td>";
                chuoisosanh += "</tr>";
                chuoisosanh += "</div>";
                for (int i = 0; i < Product.Count; i++)
                {
                    string codepd = Product[i].idpd;

                    var Productdetail = db.tblProducts.Where(p => p.Code == codepd && p.Active == true).Take(1).ToList();
                    if (Productdetail.Count > 0)
                    {
                        int idCate = int.Parse(Productdetail[0].idCate.ToString());
                        var ListGroup = db.tblGroupProducts.Find(idCate);
                        chuoipr += "<div class=\"Tear_syn\">";
                        chuoipr += "<div class=\"img_syn\">";
                        chuoipr += "<div class=\"nvar_Syn\">";
                        chuoipr += "<h2><a href=\"/" + ListGroup.Tag + "" + Productdetail[0].Tag + "_" + Productdetail[0].id + ".html\" title=\"" + Productdetail[0].Name + "\">" + Productdetail[0].Name + "</a></h2>";
                        chuoipr += "</div>";
                        chuoipr += "<div class=\"img_syn\">";
                        chuoipr += "<a href=\"/" + ListGroup.Tag + "" + Productdetail[0].Tag + "_" + Productdetail[0].id + ".html\" title=\"" + Productdetail[0].Name + "\"><img src=\"" + Productdetail[0].ImageLinkThumb + "\" alt=\"" + Productdetail[0].Name + "\" /></a>";
                        chuoipr += "</div>";
                        chuoipr += "</div>";
                        chuoipr += "</div>";
                        chuoisosanh += "<tr style=\"line-height:20px\">";
                        chuoisosanh += "<td style=\"width:5%;text-align:center\">" + (i + 1) + "</td>";
                        chuoisosanh += "<td style=\"width:40%; text-indent:7px\">" + Productdetail[0].Name + "</td>";
                        chuoisosanh += "<td style=\"width:10%;text-align:center\"> 1 </td>";
                        chuoisosanh += "<td style=\"width:20%;text-align:center\">" + string.Format("{0:#,#}", Productdetail[0].PriceSale) + "</td>";
                        chuoisosanh += "<td style=\"text-align:center; width:20%\">" + string.Format("{0:#,#}", Productdetail[0].PriceSale) + "</td>";
                        chuoisosanh += " </tr>";
                        tonggia = tonggia + float.Parse(Productdetail[0].PriceSale.ToString());
                    }

                }
                chuoipr += "</div>";
                chuoisosanh += "<tr style=\"line-height:25px \">";
                chuoisosanh += "<td colspan=\"4\"><span style=\"text-align:center; margin-right:5px; font-weight:bold; display:block\">TỔNG GIÁ MUA LẺ</span></td>";
                chuoisosanh += "<td style=\"font-weight:bold; font-size:16px; text-align:center\">" + string.Format("{0:#,#}", Convert.ToInt32(tonggia)) + " đ</td>";
                chuoisosanh += "</tr>";
                chuoisosanh += "<tr>";
                int sodu = Convert.ToInt32(tonggia) - int.Parse(tblproductSyn.PriceSale.ToString());

                chuoisosanh += "<td colspan=\"4\"><span style=\"text-align:center; margin-right:5px; font-weight:bold; display:block; color:#ff5400\">GIÁ MUA THEO BỘ</span></td>";
                chuoisosanh += "<td style=\"font-weight:bold; color:#ff5400; font-size:18px; font-family:UTMSwiss; text-align:center\">" + string.Format("{0:#,#}", tblproductSyn.PriceSale) + "đ<span style=\"font-style:italic; color:#333; font-size:12px; font-family:Arial, Helvetica, sans-serif; margin:5px; display:block; font-weight:normal\">Bạn đã tiết kiệm : " + string.Format("{0:#,#}", sodu) + "đ khi mua theo bộ</span></td>";
                chuoisosanh += "</tr>";
                chuoisosanh += "</table>";
            }

            ViewBag.chuoi = chuoi;
            ViewBag.chuoisosanh = chuoisosanh;
            ViewBag.chuoipr = chuoipr;
            return View(tblproductSyn);
        }
        public PartialViewResult RightProductSyn(string tag)
        {
            var tblproductSyn = db.tblProductSyns.First(p => p.Tag == tag);
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


            //Load sản phẩm liên quan
            string chuoiproduct = "";
            var listProductSyn = db.tblProductSyns.Where(p => p.Active == true).OrderBy(p => p.Ord).Take(7).ToList();
            for (int i = 0; i < listProductSyn.Count; i++)
            {

                

             
                chuoiproduct += " <div class=\"spdb\">";
                chuoiproduct += " <div class=\"sptb\"></div>";

                chuoiproduct += " <div class=\"img_spdb\">";
                chuoiproduct += " <a href=\"/Syn/" + listProductSyn[i].Tag + "\" title=\"" + listProductSyn[i].Name + "\"><img src=\"" + listProductSyn[i].ImageLinkThumb + "\" alt=\"" + listProductSyn[i].Name + "\" /></a>";
                chuoiproduct += " </div>";
                chuoiproduct += " <a href=\"/Syn/" + listProductSyn[i].Tag + "\" class=\"Name\" title=\"" + listProductSyn[i].Name + "\">" + listProductSyn[i].Name + "</a>";
                chuoiproduct += "<div class=\"Bottom_Tear_Sale\">";
                chuoiproduct += "<div class=\"Price\">";
                chuoiproduct += "<p class=\"PriceSale\">" + string.Format("{0:#,#}", listProductSyn[i].PriceSale) + " <span>đ</span></p>";
                chuoiproduct += "<p class=\"Price_s\">" + string.Format("{0:#,#}", listProductSyn[i].Price) + "</p>";
                chuoiproduct += "</div>";
                chuoiproduct += "<div class=\"sevices\">";
                if (listProductSyn[i].Status == true)
                {
                    chuoiproduct += "<span class=\"Status\"></span>";
                }
                else
                {
                    chuoiproduct += "<span class=\"StatusNo\"></span>";
                }

                chuoiproduct += "<span class=\"Transport\"><span class=\"icon\">";
                if (listProductSyn[i].Transport == true)
                {
                    chuoiproduct += "</span> Toàn quốc</span>";
                }
                else
                {
                    chuoiproduct += "</span> Liên hệ</span>";
                }
                chuoiproduct += "<span class=\"View\"></span>";
                chuoiproduct += "</div>";
                chuoiproduct += "</div>";

                chuoiproduct += "  </div>";
            }
            ViewBag.chuoiproduct = chuoiproduct;
            tblConfig tblconfig = db.tblConfigs.First();
            return PartialView(tblconfig);

        }
        public ActionResult Hienthidongbo()
        {
            ViewBag.Title = "<title> Danh sách sản phẩm GROHE đồng bộ - Khuyến mại lớn !</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"Danh sách những sản phẩm GROHE đồng bộ áp dụng khuyến mại dành cho khách hàng khi mua thiết bị vệ sinh GROHE\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"Sản phẩm GROHE đồng bộ, thiết bị vệ sinh GROHE đồng bộ\" /> ";
            var listsanphamdongbo = db.tblProductSyns.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            var listImage = db.tblImages.Where(p => p.Active == true && p.idMenu == 2).ToList();
            string chuoi = "";
            for (int i = 0; i < listImage.Count; i++)
            {
                chuoi += "<a href=\"" + listImage[i].Url + "\" title=\"" + listImage[i].Name + "\" rel=\"" + listImage[i].Link + "\"><img src=\"" + listImage[i].Images + "\" alt=\"" + listImage[i].Name + "\" /></a>";
            }
            ViewBag.hienthianh = chuoi;
            return View(listsanphamdongbo);
        }
        public ActionResult list()
        {
            ViewBag.Title = "<title>Sản phẩm grohe theo bộ</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"Sản phẩm grohe theo bộ, sản phẩm grohe combo theo bộ giá rẻ nhất Việt Nam\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"Sản phẩm grohe theo bộ\" /> ";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"Sản phẩm grohe theo bộ\" />";
            ViewBag.dcDescription = "<meta name=\"DC.description\" content=\"Sản phẩm grohe theo bộ, sản phẩm grohe combo theo bộ giá rẻ nhất Việt Nam\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"\" />";
            meta += "<meta itemprop=\"url\" content=\"\" />";
            meta += "<meta itemprop=\"description\" content=\"Sản phẩm grohe theo bộ, sản phẩm grohe combo theo bộ giá rẻ nhất Việt Nam\" />";
            meta += "<meta itemprop=\"image\" content=\"\" />";
            meta += "<meta property=\"og:title\" content=\"Sản phẩm grohe theo bộ\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://grohegroup.vn\" />";
            meta += "<meta property=\"og:description\" content=\"Sản phẩm grohe theo bộ, sản phẩm grohe combo theo bộ giá rẻ nhất Việt Nam\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chủ\" rel=\"nofollow\"><span class=\"iCon\"></span> Trang chủ</a> / <h1>Sản phẩm grohe theo bộ</h1>";
            StringBuilder chuoi = new StringBuilder();
            chuoi.Append(" <div class=\"List_product\">");
            chuoi.Append(" <div id=\"Box_Name\">");
            chuoi.Append("<div id=\"Leff_BoxName\">   <h2><a href=\"/san-pham-grohe-dong-bo\" title=\"Sản phẩm grohe theo bộ\">Sản phẩm grohe theo bộ</a></h2></div>");
            chuoi.Append("<div id=\"Rigt_Box_Name\">");
            chuoi.Append("<select>");
            chuoi.Append("<option value=\"0\"> - Sắp xếp -</option>");
            chuoi.Append("<option value=\"1\"> - Giá tăng dần -</option>");
            chuoi.Append("<option value=\"1\"> - GIá giảm giần -</option>");
            chuoi.Append(" </select>");
            chuoi.Append("</div>");
            chuoi.Append("</div>");
            chuoi.Append("<div class=\"Clear\"></div>");

            chuoi.Append("<div class=\"ContentProduct ContentProductSyn\">");
            var listProduct = db.tblProductSyns.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int j = 0; j < listProduct.Count; j++)
            {
                chuoi.Append("<div class=\"Tear_1\">");
                if (listProduct[j].New == true)
                {
                    chuoi.Append(" <div class=\"Box_ProductNews\"></div>");
                }
   
                chuoi.Append("<div class=\"img\">");
                chuoi.Append("<a href=\"/syn/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\">");
                chuoi.Append("<img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" title=\"" + listProduct[j].Name + "\" />");
                chuoi.Append("</a>");
                chuoi.Append("</div>");
                chuoi.Append("<h3><a href=\"/syn/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\" class=\"Name\">" + listProduct[j].Name + "</a></h3>");
                chuoi.Append("<div class=\"Info\">");
                chuoi.Append("<div class=\"LeftInfo\">");
                string note = "";
                if (listProduct[j].PriceSale > 10)
                {
                    chuoi.Append("<span class=\"PriceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>");

                    
                }
                else
                    chuoi.Append("<span class=\"PriceSale\">Liên hệ</span>");
                if (listProduct[j].Price < 10)
                    chuoi.Append("<span class=\"Price\">Liên hệ</span>");
                else
                    chuoi.Append("<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>");
                chuoi.Append("</div>");
                chuoi.Append("<div class=\"RightInfo\">");
                chuoi.Append("<div class=\"Top_RightInfo\">");
                chuoi.Append("<a href=\"\" title=\"\"><span></span></a>");
                chuoi.Append("</div>");
                chuoi.Append(" <div class=\"Bottom_RightInfo\">");
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
                            chuoi.Append("<a href=\"" + listfuc[z].Url + "\" rel=\"nofollow\" title=\"" + listfuc[z].Name + "\"><img src=\"" + listfuc[z].Images + "\" alt=\"" + listfuc[z].Name + "\" /></a>");
                        }
                    }

                }
                chuoi.Append("</div>");
                chuoi.Append("</div>");
                chuoi.Append("</div>");
               
                chuoi.Append("</div>");
            }
            chuoi.Append("<div class=\"Clear\"></div>");
            chuoi.Append("</div>");
            chuoi.Append("</div>");
            ViewBag.chuoi = chuoi.ToString();
            return View();
        }

    }
}