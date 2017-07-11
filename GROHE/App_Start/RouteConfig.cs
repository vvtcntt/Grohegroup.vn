using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GROHE
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("ListManufacturers", "9/{Tag}/{*catchall}", new { controller = "MenufacturersDisplay", action = "MenufacturerList", tag = UrlParameter.Optional }, new { controller = "^M.*", action = "^MenufacturerList$" });
            routes.MapRoute("DetailManufacturers", "NhaPhanPhoi/{Tag}/{*catchall}", new { controller = "MenufacturersDisplay", action = "MenufacturerDetail", tag = UrlParameter.Optional }, new { controller = "^M.*", action = "^MenufacturerDetail$" });
            routes.MapRoute("ListNews", "2/{tag}/{*catchall}", new { controller = "News", action = "ListNews", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^ListNews$" });
            routes.MapRoute("ChitietNew", "3/{tag}/{*catchall}", new { controller = "News", action = "NewsDetail", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^NewsDetail$" });
            routes.MapRoute("Chi_Tiet-1", "1/{tag}/{*catchall}", new { controller = "Product", action = "ProductDetail", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^ProductDetail$" });
            routes.MapRoute("ProductSyn", "Syn/{Tag}/{*catchall}", new { controller = "ProductSynDisplay", action = "ProductSyn_Detail", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^ProductSyn_Detail$" });
            routes.MapRoute("ProductSearch", "Search/{Tag}/{*catchall}", new { controller = "Product", action = "SearchProduct", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^SearchProduct$" });
            routes.MapRoute("ProductSale", "Sale/{Tag}/{*catchall}", new { controller = "Sale", action = "Index", tag = UrlParameter.Optional }, new { controller = "^S.*", action = "^Index$" });
            routes.MapRoute("Danh_Sach", "0/{Tag}/{*catchall}", new { controller = "Product", action = "ListProduct", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^ListProduct$" });
            routes.MapRoute("ListAgency", "4/{Tag}/{*catchall}", new { controller = "AgencyDisplay", action = "ListAgency", tag = UrlParameter.Optional }, new { controller = "^A.*", action = "^ListAgency$" });
            routes.MapRoute("DetailAgency", "5/{Tag}/{*catchall}", new { controller = "AgencyDisplay", action = "AgencyDetail", tag = UrlParameter.Optional }, new { controller = "^A.*", action = "^AgencyDetail$" });
            routes.MapRoute("Detaicatalogues", "7/{tag}/{*catchall}", new { controller = "Catalogues", action = "CataloguesDetail", tag = UrlParameter.Optional }, new { controller = "^C.*", action = "^CataloguesDetail$" });
            routes.MapRoute("Danh_Sach_manufactures", "6/{Manu}/{*catchall}", new { controller = "Product", action = "ListProductManufactures", manu = UrlParameter.Optional }, new { controller = "^P.*", action = "^ListProductManufactures$" });
            routes.MapRoute(name: "Tin-tuc", url: "Tin-tuc", defaults: new { controller = "NewDisplay", action = "ListNew" });
            routes.MapRoute(name: "Hang-san-xuat", url: "Hang-san-xuat", defaults: new { controller = "ManufacturesDeplay", action = "ListManufactures" });
            routes.MapRoute(name: "He-Thong-phan-phoi", url: "He-Thong-phan-phoi", defaults: new { controller = "Agencys", action = "ListAgency" });
            routes.MapRoute(name: "Contact", url: "Lien-he", defaults: new { controller = "Contact", action = "Index" });
            routes.MapRoute(name: "SearchProduct", url: "SearchProduct", defaults: new { controller = "Products", action = "SearchProduct" });
            routes.MapRoute(name: "Order", url: "Order", defaults: new { controller = "Order", action = "OrderIndex" });
            routes.MapRoute(name: "Khuyenmai", url: "Khuyen-mai-GROHE", defaults: new { controller = "Sale", action = "ListSale" });
            routes.MapRoute(name: "Maps", url: "Ban-do", defaults: new { controller = "MapsDisplay", action = "MapsDetail" });
            routes.MapRoute(name: "Spdongbo", url: "san-pham-GROHE-dong-bo", defaults: new { controller = "ProductSynDisplay", action = "Hienthidongbo" });
            routes.MapRoute(name: "Admin", url: "Admin", defaults: new { controller = "Login", action = "LoginIndex" });
            routes.MapRoute(name: "Catalogues", url: "Catalogues", defaults: new { controller = "Catalogues", action = "ListCatalogues" });
            routes.MapRoute(name: "Default", url: "{controller}/{action}/{id}", defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}