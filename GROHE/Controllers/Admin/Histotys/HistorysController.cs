using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROHE.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
namespace GROHE.Controllers.Admin.Histotys
{
    public class HistorysController : Controller
    {
        //
        // GET: /Historys/
        GROHEContext db = new GROHEContext();

        //public ActionResult Index(int? page)
        //{
        //    if ((Request.Cookies["Username"] == null))
        //    {
        //        return RedirectToAction("LoginIndex", "Login");
        //    }
        //    var ListHistorys = db.HistoryViews.OrderByDescending(p=>p.DateCreate).ToList();

        //    const int pageSize = 50;
        //    var pageNumber = (page ?? 1);
        //    // Thiết lập phân trang
        //    var ship = new PagedListRenderOptions
        //    {
        //        DisplayLinkToFirstPage = PagedListDisplayMode.Always,
        //        DisplayLinkToLastPage = PagedListDisplayMode.Always,
        //        DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
        //        DisplayLinkToNextPage = PagedListDisplayMode.Always,
        //        DisplayLinkToIndividualPages = true,
        //        DisplayPageCountAndCurrentLocation = false,
        //        MaximumPageNumbersToDisplay = 5,
        //        DisplayEllipsesWhenNotShowingAllPageNumbers = true,
        //        EllipsesFormat = "&#8230;",
        //        LinkToFirstPageFormat = "Trang đầu",
        //        LinkToPreviousPageFormat = "«",
        //        LinkToIndividualPageFormat = "{0}",
        //        LinkToNextPageFormat = "»",
        //        LinkToLastPageFormat = "Trang cuối",
        //        PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
        //        ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
        //        FunctionToDisplayEachPageNumber = null,
        //        ClassToApplyToFirstListItemInPager = null,
        //        ClassToApplyToLastListItemInPager = null,
        //        ContainerDivClasses = new[] { "pagination-container" },
        //        UlElementClasses = new[] { "pagination" },
        //        LiElementClasses = Enumerable.Empty<string>()
        //    };
        //    ViewBag.ship = ship;
        //    return View(ListHistorys.ToPagedList(pageNumber, pageSize));
        
        // }

    }
}
