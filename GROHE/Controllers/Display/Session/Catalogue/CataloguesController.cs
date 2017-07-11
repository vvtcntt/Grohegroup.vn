using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using GROHE.Models;
namespace GROHE.Controllers.Display.Session.Catalogue
{
    public class CataloguesController : Controller
    {
        private GROHEContext db = new GROHEContext();
        //
        // GET: /Catalogues/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListCatalogues(int? page)
        {
            var ListCatalogues = db.tblFiles.Where(p => p.Cate==0&& p.Active == true).OrderByDescending(p => p.Ord).ToList();
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

            
          
            ViewBag.Title = "<title>Tổng hợp catalogues sản phẩm Thiết bị vệ sinh GROHE</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"Bộ danh sách những catalogues sản phẩm Thiết bị vệ sinh GROHE chính hãng đầy đủ dành cho khách hàng\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"Catalogues GROHE, Báo giá catalogues\" /> ";
           
            return View(ListCatalogues.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult CataloguesDetail(string tag)
        {
            var tblfile = db.tblFiles.First(p => p.Tag == tag);
            ViewBag.Title = "<title>" + tblfile.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblfile.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblfile.Name + "\" /> ";
            return View(tblfile);
        }
	}
}