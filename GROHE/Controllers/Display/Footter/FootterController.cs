using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROHE.Models;
namespace GROHE.Controllers.Display.Footter
{
    public class FootterController : Controller
    {
        private GROHEContext db = new GROHEContext();
        // GET: Footter
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ControlFootter()
        {
            tblConfig tblconfig = db.tblConfigs.First();
            var Url = db.tblUrls.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            string chuoi = "";
            for (int i = 0; i < Url.Count;i++ )
            {
                if(Url[i].Rel=="nofollow")
                { chuoi += "<a href=\"" + Url[i].Url + "\" title=\"" + Url[i].Name + "\" rel=\"nofollow\">" + Url[i].Name + "</a>"; }
                else
                chuoi += "<a href=\"" + Url[i].Url + "\" title=\"" + Url[i].Name + "\">" + Url[i].Name + "</a>";
            }
            ViewBag.chuoi = chuoi;
            var maps = db.tblMaps.First();
            ViewBag.maps = maps.Content;
            return PartialView(tblconfig);
        }
        public PartialViewResult callPartial()
        {
            return PartialView(db.tblConfigs.First());
        }
        public ActionResult Command(FormCollection collection, tblRegister registry)
        {
             string Name = collection["txtName"];
                string Hotline = collection["txtHotline"];
                string selectcate = collection["selectcate"];
                registry.Name = Name;
                registry.Mobile = Hotline;
                registry.idCate = int.Parse(selectcate);
                     db.tblRegisters.Add(registry);
                    db.SaveChanges();
                    Session["registry"] = "<script>$(document).ready(function(){ alert('Bạn đã đăng ký thành công') });</script>";
                 
            
            return Redirect("/Default/Index");
        }
        public PartialViewResult MenuMobine()
        {
            var listGroup = db.tblGroupProducts.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).ToList();
            string chuoimenu = "";
            for (int i = 0; i < listGroup.Count; i++)
            {

                string tag = listGroup[i].Tag;

                chuoimenu += "<a href=\"/0/" + tag + "\" title=\"" + listGroup[i].Name + "\">" + listGroup[i].Name + "</a>";

            }
            ViewBag.chuimenu = chuoimenu;
            string chuoi = "";
            var ListMenu = db.tblGroupProducts.Where(p => p.Active == true && p.Level.Length == 5).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                chuoi += "<li><a href=\"/" + ListMenu[i].Tag + "\" title=\"" + ListMenu[i].Name + "\">" + ListMenu[i].Name + "</a>";
                string level = ListMenu[i].Level;
                var listmenuchild = db.tblGroupProducts.Where(p => p.Level.Substring(0, level.Length) == level && p.Level != level & p.Active == true).OrderBy(p => p.Ord).ToList();
                if (listmenuchild.Count > 0)
                {
                    chuoi += "<ul>";
                    for (int j = 0; j < listmenuchild.Count; j++)
                    {
                        chuoi += "<li><a href=\"/0/" + listmenuchild[j].Tag + "\" title=\"" + listmenuchild[j].Name + "\">" + listmenuchild[j].Name + "</a></li>";
                    }
                    chuoi += "</ul>";
                }
                chuoi += "</li>";
            }
            ViewBag.chuoi = chuoi;
            return PartialView();
        }
    }
}