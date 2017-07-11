using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROHE.Models;

namespace GROHE.Controllers.Display.Header
{
    public class HeaderController : Controller
    {
        private GROHEContext db = new GROHEContext();
        // GET: Header
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ControlHeader()
        {
            var tblconfig = db.tblConfigs.First();
            return PartialView(tblconfig);
        }
        public PartialViewResult PartialSearch()
        {
            return PartialView();
        }
        public PartialViewResult PartialnVar()
        {
            return PartialView();
        }
      public PartialViewResult PartialMenu()
        {
            var listParent = db.tblGroupProducts.Where(p => p.Active == true && p.Level.Length == 5).OrderBy(p => p.Ord).ToList();
            string chuoi = "";
            chuoi += "<div class=\"Menu\">";
            chuoi += "<ul class=\"ul1\">";
            for (int i = 0; i < listParent.Count;i++ )
            {
             
                chuoi += "<li class=\"li1\">";
                chuoi += "<a href=\"/0/" + listParent[i].Tag + "\" title=\"" + listParent[i].Name + "\"><span style=\"background:url(" + listParent[i].iCon + ") no-repeat\"></span>" + listParent[i].Name + "</a>";
                string level = listParent[i].Level.ToString();
                var listchild = db.tblGroupProducts.Where(p => p.Active == true && p.Level.Substring(0, level.Length) == level && p.Level != level).OrderBy(p => p.Ord).ToList();
                if (listchild.Count > 0)
                {
                    chuoi += "<ul class=\"ul2\">";
                    for (int j = 0; j < listchild.Count; j++)
                    {
                        chuoi += "<li class=\"li2\">";
                        chuoi += "<div class=\"tear_Menu\">";
                        chuoi += "<a href=\"/0/" + listchild[j].Tag + "\" title=\"" + listchild[j].Name + "\"><img src=\"" + listchild[j].Images + "\" alt=\"" + listchild[j].Name + "\" title=\"" + listchild[j].Name + "\" /></a>";
                        chuoi += "</div>";
                        chuoi += "<div class=\"Line\"></div>";
                        chuoi += "<a href=\"/0/" + listchild[j].Tag + "\" class=\"Name\" title=\"" + listchild[j].Name + "\">" + listchild[j].Name + "</a>";
                        chuoi += "</li>";
                    }
                    chuoi += " </ul>";
                }
                chuoi += "</li>";
              

            }
            chuoi += "</ul>";
            chuoi += "</div>";
            ViewBag.chuoi = chuoi;
                return PartialView();
        }
        public PartialViewResult PatialHeader()
      {
          var listimageslide = db.tblImages.Where(p => p.Active == true && p.idMenu == 1).OrderByDescending(p => p.Ord).Take(4).ToList();
          string chuoislide = "";
          for (int i = 0; i < listimageslide.Count; i++)
          {
              if (i == 0)
              {
                  chuoislide += "url(" + listimageslide[i].Images + ") " + (845 * i) + "px 0 no-repeat";
              }
              else
              {

                  chuoislide += ", url(" + listimageslide[i].Images + ") " + (845 * i) + "px 0 no-repeat";
              }
          }
          ViewBag.chuoislide = chuoislide;
          var listImage = db.tblImages.Where(p => p.Active == true && p.idMenu == 6).OrderBy(p=>p.Ord).ToList();
            string chuoi="";
          for (int i = 0; i < listImage.Count;i++ )
          {
              chuoi+= " <a href=\"" + listImage[i].Url + "\" title=\"" + listImage[i].Name + "\"><img src=\"" + listImage[i].Images + "\" alt=\"" + listImage[i].Name + "\" title=\"" + listImage[i].Name + "\" /></a>";
          }
          ViewBag.chuoi = chuoi;
          return PartialView(listimageslide);
      }
    }
}