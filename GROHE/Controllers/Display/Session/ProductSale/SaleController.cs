using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROHE.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.Text;

namespace GROHE.Controllers.Display.Session.ProductSale
{
    public class SaleController : Controller
    {
        //
        // GET: /Sale/
        private GROHEContext db = new GROHEContext();
        public ActionResult Index(string tag)
        {
            var ProductSale = db.tblProductSales.FirstOrDefault(p => p.Tag == tag);
            ViewBag.Title = "<title>" + ProductSale.Name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + ProductSale.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + ProductSale.Name + "\" /> ";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + ProductSale.Name + "\" />";
            ViewBag.dcDescription = "<meta name=\"DC.description\" content=\"" + ProductSale.Description + "\" />";
            //#region[Khuyến mại]
           
            //DateTime thoigian1 = DateTime.Parse(ProductSale.StartSale.ToString());
            //DateTime thoigian2 = DateTime.Parse(ProductSale.StopSale.ToString());
            //int songayketthuc = 0;
            //int thangnay = int.Parse(DateTime.Now.Month.ToString());
            //if (thoigian2.Month > thoigian1.Month)
            //{
            //    int nam = int.Parse(thoigian1.Year.ToString());
            //    int thangbatdau = int.Parse(thoigian1.Month.ToString());
            //    int ngaybatbau = int.Parse(thoigian1.Day.ToString());
            //    bool namnhuan = ((nam % 4 == 0 && nam % 100 != 0) || (nam % 400 == 0 && nam % 100 == 0));
            //    int thang = int.Parse(thoigian1.Month.ToString());
            //    int songay;
            //    if (thang == 4 || thang == 6 || thang == 9 || thang == 11)

            //        songay = 30;

            //    else
            //    {

            //        if (thang == 2)

            //            songay = namnhuan ? 29 : 28;

            //        else

            //            songay = 31;

            //    }
            //    int tongngaykm = (songay - ngaybatbau) + int.Parse(thoigian2.Day.ToString());
            //    int tongngaydenhnay = (songay - ngaybatbau) + int.Parse(DateTime.Now.Day.ToString());

            //    if (thangnay == thangbatdau)
            //    {
            //        songayketthuc = (songay - ngaybatbau) + int.Parse(thoigian2.Day.ToString());
            //    }
            //    else
            //    {
            //        songayketthuc = int.Parse(DateTime.Now.Day.ToString()) - int.Parse(thoigian2.Day.ToString());
            //    }

            //}
            //else
            //{
            //    songayketthuc = int.Parse(thoigian2.Day.ToString()) - int.Parse(DateTime.Now.Day.ToString());
            //}
            //if (songayketthuc > 0)
            //{
            //    ViewBag.ngay = "<span>Chỉ còn <span class=\"blink\">" + songayketthuc.ToString() + "</span> ngày</span>";
            //}
            //else
            //{ ViewBag.ngay = "<span style=\"margin-top:30px;display:block;font-weight:bold; font-size:15px\"> Đã hết khuyến mại</span> "; }

            //ViewBag.Title = "<title>" + ProductSale.Name + "</title>";
            //ViewBag.Description = "<meta name=\"description\" content=\"" + ProductSale.Description + "\"/>";
            //ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + ProductSale.Name + "\" /> ";
            ////Load sản phẩm hot khuyến mại
            //string chuoihot = ProductSale.CodeOne;
            //string hienthihot = "";
            //string[] manghot = chuoihot.Split(',');
            //for (int i = 0; i < manghot.Length; i++)
            //{
            //    string code = manghot[i];
            //    var listProduct = db.tblProducts.Where(p => p.Code == code).Take(1).ToList();
            //    if (listProduct.Count > 0)
            //    {
            //        int idcate = int.Parse(listProduct[0].idCate.ToString());
            //        var listgroup = db.tblGroupProducts.Find(idcate);
            //        string url = listgroup.Tag;
            //        hienthihot += "<div class=\"Tear_top\">";
            //        hienthihot += "<a href=\"/" + url + "" + listProduct[0].Tag + "_" + listProduct[0].id + ".html\" class=\"Name\" title=\"" + listProduct[0].Name + "\">" + listProduct[0].Name + "</a>";
            //        hienthihot += "<span class=\"model\">Model: " + listProduct[0].Code + " | Giá NY : " + string.Format("{0:#,#}", listProduct[0].Price) + " đ</span>";
            //        if (listProduct[0].Note == true)
            //        {
            //            hienthihot += "<div class=\"Soluongcohan\"></div>";
            //        }
            //        hienthihot += "<div class=\"Box_Price\">";
            //        string pricesale = listProduct[0].PriceSale.ToString();
            //        int price = int.Parse(pricesale.Substring(0, pricesale.Length - 3));
            //        hienthihot += "<span>" + string.Format("{0:#,#}", price) + ".</span>";
            //        hienthihot += "</div>";
            //        hienthihot += "<div class=\"Box_Sale\">";
            //        hienthihot += "<div class=\"Left\" style=\"background:url(" + listProduct[0].ImageSale + ") no-repeat left center scroll transparent; background-size:100%\"></div>";
            //        hienthihot += "<div class=\"Right\"><span>" + listProduct[0].Sale + "</span></div>";
            //        hienthihot += "</div>";
            //        hienthihot += "<div class=\"img\">";
            //        hienthihot += "<a href=\"/" + url + "" + listProduct[0].Tag + "_" + listProduct[0].id + ".html\" title=\"" + listProduct[0].Name + "\">";
            //        hienthihot += "<img src=\"" + listProduct[0].ImageLinkThumb + "\" alt=\"" + listProduct[0].Name + "\" />";
            //        hienthihot += "</a>";
            //        hienthihot += "</div>";
            //        hienthihot += "</div>";
            //    }
            //}
            //ViewBag.sanphamhot = hienthihot;
            ////Load sản phẩm khuyến mại bên dưới
            //string chuoimenu = ""; string chuoiProduct = "";
            //string CodeTrue = ProductSale.CodeTrue;
            //string[] chuoicodetrue = CodeTrue.Split(',');
            //List<int> arrayMenu = new List<int>();
            //List<int> arrayMenuParent = new List<int>();
            //List<int> ArrayProduct = new List<int>();
            //for (int i = 0; i < chuoicodetrue.Length; i++)
            //{
            //    string code = chuoicodetrue[i];
            //    var listProduct = db.tblProducts.Where(p => p.Code == code).Take(1).ToList();
            //    if (listProduct.Count > 0)
            //    {
            //        int idMenu = int.Parse(listProduct[0].idCate.ToString());
            //        arrayMenu.Add(idMenu);
            //        ArrayProduct.Add(listProduct[0].id);

            //    }
            //}

            //var listMenuProduct = db.tblGroupProducts.Where(x => arrayMenu.Contains(x.Id) && x.Active == true).ToList();
            //for (int i = 0; i < listMenuProduct.Count; i++)
            //{
            //    string level = listMenuProduct[i].Level;
            //    var listMenuParent = db.tblGroupProducts.First(p => p.Level.Substring(0, 5) == level.Substring(0, 5) && p.Level.Length == 5);

            //    arrayMenuParent.Add(listMenuParent.Id);

            //}
            //var listMenudisplay = db.tblGroupProducts.Where(p => arrayMenuParent.Contains(p.Id) && p.Active == true).OrderBy(p => p.Ord).ToList();
            //for (int i = 0; i < listMenudisplay.Count; i++)
            //{
            //    chuoimenu += "  <li><a href=\"#" + listMenudisplay[i].Id + "\" title=\"" + listMenudisplay[i].Name + "\">" + listMenudisplay[i].Name + "</a></li>";
            //    string level = listMenudisplay[i].Level;
            //    var listmenu1 = db.tblGroupProducts.Where(p => p.Level.Substring(0, 5) == level && arrayMenu.Contains(p.Id)).ToList();
            //    chuoiProduct += "<div class=\"Banner1\">";
            //    chuoiProduct += "<div class=\"Left\" style=\"background:url(" + listMenudisplay[i].Images + ") no-repeat center center scroll #fff\"></div>";
            //    chuoiProduct += "<div class=\"Right\"></div>";
            //    chuoiProduct += "<div class=\"Center\">";
            //    chuoiProduct += "<span>Danh sách " + listMenudisplay[i].Name + " đang được khuyến mại </span>";
            //    chuoiProduct += "</div>";
            //    chuoiProduct += "</div>";
            //    chuoiProduct += "<div class=\"Content_Top2\" id=\"" + listMenudisplay[i].Id + "\">";
            //    chuoiProduct += "<div class=\"Left\">";
            //    int idimage = int.Parse(listMenudisplay[i].Id.ToString());
            //    var listImage = db.tblImages.Where(p => p.idMenu == 3 && p.idCate == idimage && p.Active == true).OrderBy(p => p.Ord).ToList();
            //    for (int m = 0; m < listImage.Count; m++)
            //    {
            //        chuoiProduct += "<a href=\"" + listImage[m].Url + "\" title=\"" + listImage[m].Name + "\"><img src=\"" + listImage[m].Images + "\" alt=\"" + listImage[m].Name + "\" /></a>";
            //    }
            //    chuoiProduct += "</div>";
            //    chuoiProduct += "<div class=\"Right\">";
            //    for (int j = 0; j < listmenu1.Count; j++)
            //    {
            //        string Url = listmenu1[j].Tag;
            //        int idcate = int.Parse(listmenu1[j].Id.ToString());
            //        var listproductdisplay = db.tblProducts.Where(p => ArrayProduct.Contains(p.id) && p.idCate == idcate && p.Active == true).OrderBy(p => p.Ord).ToList();
            //        for (int k = 0; k < listproductdisplay.Count; k++)
            //        {
            //            chuoiProduct += "<div class=\"Tear_pro\">";
            //            chuoiProduct += "<a href=\"/" + Url + "" + listproductdisplay[k].Tag + "_" + listproductdisplay[k].id + ".html\" title=\"" + listproductdisplay[k].Name + "\" class=\"Name\">" + listproductdisplay[k].Name + "</a>";
            //            chuoiProduct += "<span class=\"model\">Model: " + listproductdisplay[k].Code + " | Giá NY : " + string.Format("{0:#,#}", listproductdisplay[k].Price) + " đ</span>";
            //            if (listproductdisplay[k].Note == true)
            //            {
            //                chuoiProduct += "<div class=\"Soluongcohan\"></div>";
            //            }
            //            chuoiProduct += "<div class=\"Box_Price\">";
            //            string pricesale = listproductdisplay[k].PriceSale.ToString();
            //            int price = int.Parse(pricesale.Substring(0, pricesale.Length - 3));
            //            chuoiProduct += "<span>" + string.Format("{0:#,#}", price) + ".</span>";
            //            chuoiProduct += "</div>";
            //            chuoiProduct += "<div class=\"Box_Sale\">";
            //            chuoiProduct += "<div class=\"Left\" style=\"background:url(" + listproductdisplay[k].ImageSale + ") no-repeat left center scroll transparent; background-size:100%\"></div>";
            //            chuoiProduct += "<div class=\"Right\"><span>" + listproductdisplay[k].Sale + "</span></div>";
            //            chuoiProduct += "</div>";
            //            chuoiProduct += "<div class=\"img\">";
            //            chuoiProduct += "<a href=\"/" + Url + "" + listproductdisplay[k].Tag + "_" + listproductdisplay[k].id + ".html\" title=\"" + listproductdisplay[k].Name + "\">";
            //            chuoiProduct += "<img src=\"" + listproductdisplay[k].ImageLinkThumb + "\" alt=\"" + listproductdisplay[k].Name + "\" />";
            //            chuoiProduct += "</a>";
            //            chuoiProduct += "</div>";
            //            chuoiProduct += "</div>";
            //        }

            //    }
            //    chuoiProduct += "</div>";
            //    chuoiProduct += "</div>";
            //    chuoiProduct += "<div class=\"Clear\"></div>";
            //}
            //ViewBag.chuoimenu = chuoimenu;
            //ViewBag.chuoiProduct = chuoiProduct;


            ////danh sách sản phẩm đồng bộ
            //string hienthidongbo = "";
            //string chuoidongbo = ProductSale.CodeSale;
            //if (chuoidongbo != null)
            //{
            //    string[] mangdongbo = chuoidongbo.Split(',');
            //    for (int i = 0; i < mangdongbo.Length; i++)
            //    {
            //        string codedongbo = mangdongbo[i];
            //        var listProductSyn = db.tblProductSyns.Where(p => p.Code == codedongbo && p.Active == true).OrderBy(p => p.Ord).ToList();
            //        if (listProductSyn.Count > 0)
            //        {
            //            hienthidongbo += "<div class=\"spdb\">";
            //            hienthidongbo += "<div class=\"sptb\"></div>";
            //            hienthidongbo += "<div class=\"Box_Price\">";
            //            hienthidongbo += "<span>" + string.Format("{0:#,#}", listProductSyn[0].Price) + " <span>đ</span></span>";
            //            hienthidongbo += "</div>";
            //            hienthidongbo += "<div class=\"img_spdb\">";
            //            hienthidongbo += "<a href=\"/Syn/" + listProductSyn[0].Tag + "\" title=\"" + listProductSyn[0].Name + "\"><img src=\"" + listProductSyn[0].ImageLinkThumb + "\" alt=\"" + listProductSyn[0].Name + "\" /></a>";
            //            hienthidongbo += "</div>";
            //            hienthidongbo += "<a href=\"/Syn/" + listProductSyn[0].Tag + "\" class=\"Name\" title=\"" + listProductSyn[0].Name + "\">" + listProductSyn[0].Name + "</a>";
            //            hienthidongbo += "<span class=\"Note\">" + listProductSyn[0].Description + "</span>";
            //            hienthidongbo += "</div>";
            //        }
            //    }

            //}
            //ViewBag.hienthichuoi = hienthidongbo;
            //#endregion
            return View(ProductSale);
        }
        public ActionResult ListSale(int? page, string id)
        {
            ViewBag.Title = "<title> Cập nhật chương trình khuyến mại Thiết bị vệ sinh Grohe chính hãng</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"Sau đây là những Cập nhật chương trình khuyến mại Thiết bị vệ sinh Grohe chính hãng do trung tâm phân phối sản phẩm Grohe gửi tới quý khách hàng\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"khuyên mại Grohe, thiết bị vệ sinh Grohe khuyến mại\" /> ";
            var listnews = db.tblProductSales.Where(p => p.Active == true).OrderByDescending(p => p.DateCreate).ToList();
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
            return View(listnews.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult detail(string tag)
        {
            var config = db.tblConfigs.FirstOrDefault();

            ViewBag.Title = "<title> " + config.TitleSale + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + config.TitleSale + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + config.TitleSale + "\" /> ";

            var listProductSalePriority = db.tblProducts.Where(p => p.Active == true && p.Priority == true && p.ProductSale == true).OrderBy(p => p.Ord).ToList();
            StringBuilder resultPriority = new StringBuilder();
            foreach (var item in listProductSalePriority)
            {
                resultPriority.Append("<div class=\"item\">");
                resultPriority.Append("<div class=\"contentItem\">");
                resultPriority.Append("<div class=\"img\">");
                resultPriority.Append("<a href=\"/1/" + item.Tag + "\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkThumb + "\" title=\"" + item.Name + "\" /></a>");
                resultPriority.Append("</div>");
                resultPriority.Append("<a href=\"/1/" + item.Tag + "\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
                resultPriority.Append("<div class=\"buy\">");
                resultPriority.Append("<span class=\"note\">Giá chỉ từ</span>");
                resultPriority.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
                resultPriority.Append("<a href=\"/1/" + item.Tag + "\" title=\"" + item.Name + "\">Xem ngay  &raquo;</a>");
                resultPriority.Append("</div>");
                resultPriority.Append("</div>");
                resultPriority.Append("</div>");
            }
            ViewBag.resultPriority = resultPriority.ToString();


            var listProductSyn = db.tblProductSyns.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            if (listProductSyn.Count > 0)
            {
                ViewBag.CheckSyn = "oke";
                StringBuilder resultSyn = new StringBuilder();
                foreach (var item in listProductSyn)
                {
                    resultSyn.Append(" <div class=\"item\">");
                    resultSyn.Append("<div class=\"contentItem\">");
                    resultSyn.Append("<div class=\"img\">");
                    resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkDetail + "\" title=\"" + item.Name + "\" /></a>");
                    resultSyn.Append(" </div>");
                    resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
                    resultSyn.Append("<div class=\"buy\">");
                    resultSyn.Append("<span class=\"note\">Giá chỉ từ</span>");
                    resultSyn.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
                    resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\" >Xem ngay  &raquo;</a>");
                    resultSyn.Append("</div>");
                    resultSyn.Append("</div>");
                    resultSyn.Append("</div>");
                }
                ViewBag.resultSyn = resultSyn.ToString();
            }

            var listProductSale = db.tblProducts.Where(p => p.Active == true && p.ProductSale == true).OrderBy(p => p.idCate).ToList();
            StringBuilder resultSale = new StringBuilder();
            foreach (var item in listProductSale)
            {
                resultSale.Append("<div class=\"item\">");
                resultSale.Append("<div class=\"contentItem\">");
                resultSale.Append("<div class=\"img\">");
                resultSale.Append("<a href=\"/1/" + item.Tag + "\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkThumb + "\" title=\"" + item.Name + "\" /></a>");
                resultSale.Append("</div>");
                resultSale.Append("<a href=\"/1/" + item.Tag + "\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
                resultSale.Append("<div class=\"buy\">");
                resultSale.Append("<span class=\"note\">Giá chỉ từ</span>");
                resultSale.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
                resultSale.Append("<a href=\"/1/" + item.Tag + "\" title=\"" + item.Name + "\">Xem ngay  &raquo;</a>");
                resultSale.Append("</div>");
                resultSale.Append("</div>");

                resultSale.Append("</div>");
            }
            ViewBag.resultSale = resultSale.ToString();
            return View(config);
        }

    }
}