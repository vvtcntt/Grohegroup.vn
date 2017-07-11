using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROHE.Models;
 
namespace GROHE.Controllers.Admin.Config
{
    public class ConfigController : Controller
    {
        private GROHEContext db = new GROHEContext();

        //
        // GET: /Config/

        public ActionResult Index( int id=1)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblConfig tblconfig = db.tblConfigs.Find(1);
            
            
            if (tblconfig == null)
            {
                return HttpNotFound();
            }
            if (Session["Config"] != "")
            {
                ViewBag.Config = Session["Config"];
                Session["Config"] = "";
            }
            return View(tblconfig);
        }

        [HttpPost]
        [ValidateInput(false)]
         public ActionResult Index(tblConfig tblconfig, int id=1 )
        {
            if (ModelState.IsValid)
            {
               
                tblconfig.ID = id;
        
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Config Website", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                db.Entry(tblconfig).State = EntityState.Modified;
                db.SaveChanges();
                Session["Config"] = "<script>$(document).ready(function(){ alert('Bạn đã cập nhật thành công') });</script>";
                return RedirectToAction("Index");
            }
            return View(tblconfig);
        }
 
        //
        // GET: /Config/Edit/5

      
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}