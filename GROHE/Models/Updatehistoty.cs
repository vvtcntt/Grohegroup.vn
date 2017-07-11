using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GROHE.Models;
namespace GROHE.Models
{
    public class Updatehistoty
    {
        public GROHEContext db = new GROHEContext();
        public static void UpdateHistory(string task,string FullName,string UserID)
        {

            GROHEContext db = new GROHEContext();
            tblHistoryLogin tblhistorylogin = new tblHistoryLogin();
            tblhistorylogin.FullName = FullName;
            tblhistorylogin.Task = task;
            tblhistorylogin.idUser = int.Parse(UserID);
            tblhistorylogin.DateCreate = DateTime.Now;
            tblhistorylogin.Active = true;
            
            db.tblHistoryLogins.Add(tblhistorylogin);
            db.SaveChanges();
           
        }
    }
}