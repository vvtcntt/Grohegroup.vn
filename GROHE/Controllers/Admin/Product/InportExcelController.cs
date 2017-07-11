using System;
using System.Collections.Generic;
using System.Data; 
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.Entity.Validation;
using GROHE.Models;

namespace GROHE.Controllers.Admin.Product
{
    public class InportExcelController : Controller
    {   private GROHEContext db = new GROHEContext();
        //
        // GET: /InportExcel/

        public ActionResult Index()
    {
        if ((Request.Cookies["Username"] == null))
        {
            return RedirectToAction("LoginIndex", "Login");
        }
            return View();
        }
        public PartialViewResult PartialEXCEL()
        {
            var GroupsProducts = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            var menuModel = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {
                lstMenu.Add(new SelectListItem {Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.Id.ToString()});
            }
            ViewBag.drMenu = lstMenu;
            return PartialView();
        }
        #region// Check int, float, datetime
        bool CheckDateTime(string String)
        {
            DateTime Date;
            if (DateTime.TryParse(String, out Date))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool CheckInt(string String)
        {
            int Int;
            if (Int32.TryParse(String, out Int))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool CheckFloat(string String)
        {
            float Float;
            if (float.TryParse(String, out Float))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool CheckActive(string String)
        {
            bool Active;
            if (bool.TryParse(String, out Active) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult OrderMultipleUp(HttpPostedFileBase fileExcel, FormCollection conllection)
        {

            if (fileExcel != null)
            {
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/UploadExcel"), System.IO.Path.GetFileName(fileExcel.FileName));
                fileExcel.SaveAs(path);
                //Declare variables to hold refernces to Excel objects.
                Workbook workBook;
                SharedStringTable sharedStrings;
                IEnumerable<Sheet> workSheets;
                WorksheetPart productSheet;

                //Declare helper variables.
                string keywordID;

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(path, true))
                {
                    //References to the workbook and Shared String Table.
                    workBook = document.WorkbookPart.Workbook;
                    workSheets = workBook.Descendants<Sheet>();
                    sharedStrings = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                    //Reference to Excel Worksheet with Product data.
                    keywordID = workSheets.First().Id;
                    productSheet = (WorksheetPart)document.WorkbookPart.GetPartById(keywordID);
                   
                    string checkstatus = (conllection["checkstatus"]);
                    if(checkstatus=="on")
                    {
                        UPdatePricenoneid(productSheet.Worksheet, sharedStrings);
                    }
                    else
                    {
                        int idMenu = int.Parse(conllection["drMenu"]);
                        UPdatePrice(productSheet.Worksheet, sharedStrings, idMenu);
                    }
                    //Load product data to business object.
                    //SaveKeyword(productSheet.Worksheet, sharedStrings);
                   
                    document.Close();
                    System.IO.File.Delete(path);
                }

            } return Redirect("/Productad/ShowerroInport");
        }
        private void SaveKeyword(Worksheet worksheet, SharedStringTable sharedString)
        {
            try
            {
                //Initialize the product list.
                List<tblProduct> result = new List<tblProduct>();

                //LINQ query to skip first row with column names.
                IEnumerable<Row> dataRows = from row in worksheet.Descendants<Row>()
                                            where row.RowIndex > 1
                                            select row;


                List<tblProduct> keyworddb = db.tblProducts.ToList();

                var Messegebox = "";
                foreach (Row row in dataRows)
                {
                    IEnumerable<String> textValues = from cell in row.Descendants<Cell>()
                                                     where cell.CellValue != null
                                                     select (cell.DataType != null && cell.DataType.HasValue && cell.DataType == CellValues.SharedString ? sharedString.ChildElements[int.Parse(cell.CellValue.InnerText)].InnerText : cell.CellValue.InnerText);


                    //Check to verify the row contained data.

                    if (textValues.Count() > 0)
                    {
                        //Create a product and add it to the list.

                        var textArray = textValues.ToArray();
                        tblProduct sp = new tblProduct();

                        int leng = textArray.Length;

                        string Codes = textArray[0];


                        var kt = db.tblProducts.Where(p => p.Code == Codes).ToList();
                        if (kt.Count() == 0)
                        {
                            int nRow = int.Parse(row.RowIndex.ToString()) - 1;
                            if (textArray[0] != null && textArray[0] != "0")
                            {
                                string Code = textArray[0];
                                sp.Code = Code.Trim(' ');
                            }
                            else
                            {
                                Messegebox = Messegebox + " Hàng " + nRow + " cột 1 Name bị lỗi dữ liệu  ";
                            }
                            string nName = textArray[1];
                            if (nName != null && nName != "0" && nName != " ")
                            {
                                string Name = nName;
                                sp.Name = Name.Trim(' ');

                            }
                            else
                            {
                                Messegebox = Messegebox + "Hàng " + nRow + " cột 2 Price bị lỗi dữ liệu ";
                            }
                            string nPrice = textArray[2];
                            if (nPrice != "1" && nPrice != "0" && nPrice != " ")
                            {
                                string Price = nPrice;
                                sp.Price = int.Parse(Price.Trim(' '));

                            }
                            else
                            {
                                Messegebox = Messegebox + "Hàng " + nRow + " cột 2 Price bị lỗi dữ liệu ";
                            }
                            string nPriceSale = textArray[3];
                            if (nPriceSale != "1" && nPriceSale != "0" && nPriceSale != " ")
                            {
                                string PriceSale = nPriceSale;
                                sp.PriceSale = int.Parse(PriceSale.Trim(' '));

                            }
                            else
                            {
                                Messegebox = Messegebox + "Hàng " + nRow + " cột 3 PriceSale bị lỗi dữ liệu ";
                            }
                            ///

                            sp.idCate = 0;
                            db.tblProducts.Add(sp);
                          db.SaveChanges();

                        }

                        
                    }
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Inport Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                } Session["Thongbao"] = "<script LANGUAGE = \"Javascript\">$(document).ready(function(){ alert('" + Messegebox + "') });</script>";
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }



        }
        private void UPdatePrice(Worksheet worksheet, SharedStringTable sharedString, int idMenu)
        {
            try
            {
                //Initialize the product list.
                List<tblProduct> result = new List<tblProduct>();

                //LINQ query to skip first row with column names.
                IEnumerable<Row> dataRows = from row in worksheet.Descendants<Row>()
                                            where row.RowIndex > 1
                                            select row;


                List<tblProduct> keyworddb = db.tblProducts.ToList();

                var Messegebox = "";
                int count = 0;
                int countnnull = 0;
                string ncodes = "";
                string nid = "";
                foreach (Row row in dataRows)
                {
                    IEnumerable<String> textValues = from cell in row.Descendants<Cell>()
                                                     where cell.CellValue != null
                                                     select (cell.DataType != null && cell.DataType.HasValue && cell.DataType == CellValues.SharedString ? sharedString.ChildElements[int.Parse(cell.CellValue.InnerText)].InnerText : cell.CellValue.InnerText);
                    if (textValues.Count() > 0)
                    {
                        //Create a product and add it to the list.

                        var textArray = textValues.ToArray();
                        tblProduct sp = new tblProduct();

                        int leng = textArray.Length;

                        string Codes = textArray[0];
                        try {  
                        var Product = db.tblProducts.First(p => p.Code == Codes && p.idCate==idMenu);
                        var kt = db.tblProducts.Where(p => p.Code == Codes).ToList();
                        if (Product!=null)
                        {
                            int nRow = int.Parse(row.RowIndex.ToString()) - 1;
                            string nPrice = textArray[1];
                            if (CheckInt(nPrice) == true)
                            {
                                if (nPrice != "1" && nPrice != "0" && nPrice != " ")
                                {
                                    string Price = nPrice;
                                    Product.Price = int.Parse(Price.Trim(' '));

                                }
                                else
                                {
                                    nid += Product.id + "-";
                                    count += 1;
                                    Messegebox = Messegebox + "Hàng " + nRow + " cột 1 Price bị lỗi dữ liệu - ";
                                }
                            }
                            else
                            {
                                nid += Product.id + "-";
                                count += 1;
                                Messegebox = Messegebox + "Hàng " + nRow + " cột 1 Price bị lỗi dữ liệu -";

                            }
                            string nPriceSale = textArray[2];
                            if(CheckInt(nPriceSale)==true)
                            { 
                            if (nPriceSale != "1" && nPriceSale != "0" && nPriceSale != " ")
                            {
                                string PriceSale = nPriceSale;
                                Product.PriceSale = int.Parse(PriceSale.Trim(' '));

                            }
                            else
                            {
                                nid += Product.id + "-";
                                count += 1;
                                Messegebox = Messegebox + "Hàng " + nRow + " cột 2 PriceSale bị lỗi dữ liệu -";
                            }
                            }
                            else
                            {
                                nid += Product.id + "-";
                                count += 1;
                                Messegebox = Messegebox + "Hàng " + nRow + " cột 2 PriceSale bị lỗi dữ liệu -";

                            }
                            sp.idCate = 0;
                            db.SaveChanges();

                        }
                        } catch(Exception ex)
                        {
                              countnnull += 1;
                            ncodes += Codes+" , ";

                        }
                        


                    }
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Inport Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                }
                Session["Null"] = ncodes;
                Session["CountNULL"] = countnnull;
                if(count>0)
                { 
                    Session["Thongbao"] = "<script LANGUAGE = \"Javascript\">$(document).ready(function(){ alert('" + Messegebox + "') });</script>"; 
                    Session["nid"] = nid;
          
                
                }
                else
                {
                    if(countnnull>0)
                    { Session["Thongbao"] = "<script LANGUAGE = \"Javascript\">$(document).ready(function(){ alert('Bạn Inport giá thành công, tuy nhiên còn "+countnnull+" sản phẩm có trong bảng exlcel mà chưa được đăng lên website, những mã mới là : "+ncodes+"') });</script>"; }
                    else
                    {
                        Session["Thongbao"] = "<script LANGUAGE = \"Javascript\">$(document).ready(function(){ alert('Bạn inport giá thành công 100% ! Vui lòng kiểm tra thủ công 1 lần nữa cho chính xác !') });</script>";
                    }
                    
                 
                }
                
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }



        }
        private void UPdatePricenoneid(Worksheet worksheet, SharedStringTable sharedString)
        {
            try
            {
                //Initialize the product list.
                List<tblProduct> result = new List<tblProduct>();

                //LINQ query to skip first row with column names.
                IEnumerable<Row> dataRows = from row in worksheet.Descendants<Row>()
                                            where row.RowIndex > 1
                                            select row;


                List<tblProduct> keyworddb = db.tblProducts.ToList();

                var Messegebox = "";
                int count = 0;
                int countnnull = 0;
                string ncodes = "";
                string nid = "";
                foreach (Row row in dataRows)
                {
                    IEnumerable<String> textValues = from cell in row.Descendants<Cell>()
                                                     where cell.CellValue != null
                                                     select (cell.DataType != null && cell.DataType.HasValue && cell.DataType == CellValues.SharedString ? sharedString.ChildElements[int.Parse(cell.CellValue.InnerText)].InnerText : cell.CellValue.InnerText);
                    if (textValues.Count() > 0)
                    {
                        //Create a product and add it to the list.

                        var textArray = textValues.ToArray();
                        tblProduct sp = new tblProduct();

                        int leng = textArray.Length;

                        string Codes = textArray[0];
                        try
                        {
                            var Product = db.tblProducts.First(p => p.Code == Codes);
                            var kt = db.tblProducts.Where(p => p.Code == Codes).ToList();
                            if (Product != null)
                            {
                                int nRow = int.Parse(row.RowIndex.ToString()) - 1;
                                string nPrice = textArray[1];
                                if (CheckInt(nPrice) == true)
                                {
                                    if (nPrice != "1" && nPrice != "0" && nPrice != " ")
                                    {
                                        string Price = nPrice;
                                        Product.Price = int.Parse(Price.Trim(' '));

                                    }
                                    else
                                    {
                                        nid += Product.id + "-";
                                        count += 1;
                                        Messegebox = Messegebox + "Hàng " + nRow + " cột 1 Price bị lỗi dữ liệu - ";
                                    }
                                }
                                else
                                {
                                    nid += Product.id + "-";
                                    count += 1;
                                    Messegebox = Messegebox + "Hàng " + nRow + " cột 1 Price bị lỗi dữ liệu -";

                                }
                                string nPriceSale = textArray[2];
                                if (CheckInt(nPriceSale) == true)
                                {
                                    if (nPriceSale != "1" && nPriceSale != "0" && nPriceSale != " ")
                                    {
                                        string PriceSale = nPriceSale;
                                        Product.PriceSale = int.Parse(PriceSale.Trim(' '));

                                    }
                                    else
                                    {
                                        nid += Product.id + "-";
                                        count += 1;
                                        Messegebox = Messegebox + "Hàng " + nRow + " cột 2 PriceSale bị lỗi dữ liệu -";
                                    }
                                }
                                else
                                {
                                    nid += Product.id + "-";
                                    count += 1;
                                    Messegebox = Messegebox + "Hàng " + nRow + " cột 2 PriceSale bị lỗi dữ liệu -";

                                }
                                sp.idCate = 0;
                                db.SaveChanges();

                            }
                        }
                        catch (Exception ex)
                        {
                            countnnull += 1;
                            ncodes += Codes + " , ";

                        }



                    }
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Inport Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                }
                Session["Null"] = ncodes;
                Session["CountNULL"] = countnnull;
                if (count > 0)
                {
                    Session["Thongbao"] = "<script LANGUAGE = \"Javascript\">$(document).ready(function(){ alert('" + Messegebox + "') });</script>";
                    Session["nid"] = nid;


                }
                else
                {
                    if (countnnull > 0)
                    { Session["Thongbao"] = "<script LANGUAGE = \"Javascript\">$(document).ready(function(){ alert('Bạn Inport giá thành công, tuy nhiên còn " + countnnull + " sản phẩm có trong bảng exlcel mà chưa được đăng lên website, những mã mới là : " + ncodes + "') });</script>"; }
                    else
                    {
                        Session["Thongbao"] = "<script LANGUAGE = \"Javascript\">$(document).ready(function(){ alert('Bạn inport giá thành công 100% ! Vui lòng kiểm tra thủ công 1 lần nữa cho chính xác !') });</script>";
                    }


                }

            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }



        }
    }
}
