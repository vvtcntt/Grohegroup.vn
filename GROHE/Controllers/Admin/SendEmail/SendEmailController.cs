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
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
namespace GROHE.Controllers.Admin.SendEmail
{
    public class SendEmailController : Controller
    {
        private GROHEContext db = new GROHEContext();

        //
        // GET: /SendEmail/

        public ActionResult Index(int? page)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var ListEmail = db.tblRegisters.ToList();

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
            if (Session["SendEmail"] != "")
            {
                ViewBag.SendEmail = Session["SendEmail"];
                Session["SendEmail"] = "";
            }
            ViewBag.ship = ship;
            return View(ListEmail.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /SendEmail/Details/5
        public ActionResult Send()
        {
            var listEmail = db.tblRegisters.ToList();
            string chuoi = "";
            for (int i = 0; i < listEmail.Count; i++)
            {
                chuoi +=listEmail[i].Email + ";";
            }
            int leght = chuoi.Length;
       
            ViewBag.Chuoi = chuoi.Substring(0, leght - 1);
           return View();
        }
      
        public ActionResult SendEmail(FormCollection collection)
        {
                var Config = db.tblConfigs.OrderBy(p=>p.ID).Take(1).ToList();
                var fromAddress = Config[0].UserEmail;
                string fromPassword = Config[0].PassEmail;
                string subject = collection["txtName"];
                string body = "" + collection["txtContent"] + "";
                var listEmail = db.tblRegisters.ToList();
                for (int i = 0; i < listEmail.Count; i++)
                {
                    var toAddress = listEmail[i].Email;
                    var smtp = new System.Net.Mail.SmtpClient();
                    {
                        smtp.Host = Config[0].Host;
                        smtp.Port = int.Parse(Config[0].Port.ToString()); ;
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                        smtp.Timeout = int.Parse(Config[0].Timeout.ToString());
                    }
                    smtp.Send(fromAddress, toAddress, subject, body);
                    Thread.Sleep(3000);
                }
                Session["SendEmail"] = "<script>$(document).ready(function(){ alert('Bạn đã gửi Mail thành công') });</script>";
                return RedirectToAction("Index");
        }
        public ActionResult Details(int id = 0)
        {
            tblRegister tblregister = db.tblRegisters.Find(id);
            if (tblregister == null)
            {
                return HttpNotFound();
            }
            return View(tblregister);
        }

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SendEmail/Create

        [HttpPost]
 
        public ActionResult Create(tblRegister tblregister)
        {
            if (ModelState.IsValid)
            {
                db.tblRegisters.Add(tblregister);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblregister);
        }

        //
        // GET: /SendEmail/Edit/5

        public ActionResult Edit(int id = 0)
        {
            tblRegister tblregister = db.tblRegisters.Find(id);
            if (tblregister == null)
            {
                return HttpNotFound();
            }
            return View(tblregister);
        }

        //
        // POST: /SendEmail/Edit/5

        [HttpPost]
    
        public ActionResult Edit(tblRegister tblregister)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblregister).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblregister);
        }

        //
        // GET: /SendEmail/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblRegister tblregister = db.tblRegisters.Find(id);
            if (tblregister == null)
            {
                return HttpNotFound();
            }
            return View(tblregister);
        }

        //
        // POST: /SendEmail/Delete/5

        [HttpPost, ActionName("Delete")]
    
        public ActionResult DeleteConfirmed(int id)
        {
            tblRegister tblregister = db.tblRegisters.Find(id);
            db.tblRegisters.Remove(tblregister);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}