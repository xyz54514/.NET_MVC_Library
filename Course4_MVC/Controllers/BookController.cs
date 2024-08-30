using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course4_MVC.Controllers
{
    public class BookController : Controller
    {
        Models.CodeService codeService = new Models.CodeService();
        // GET: Book

        /// <summary>
        /// 書本資料查詢
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //ViewBag.label = "test";
            ViewBag.BookClassData = this.codeService.GetCodeTable("BookClass");
            ViewBag.BookKeeperData = this.codeService.GetCodeTable("Keeper");
            ViewBag.BookStatusData = this.codeService.GetCodeTable("BookStatus");
            return View();
        }

        /// <summary>
        /// 書本資料查詢(查詢鍵)
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult Index(Models.BookSearchArgs arg)
        {
            Models.BookService bookService = new Models.BookService();
            ViewBag.SearchResult = bookService.GetBooksByCondition(arg);
            ViewBag.BookClassData = this.codeService.GetCodeTable("BookClass");
            ViewBag.BookKeeperData = this.codeService.GetCodeTable("Keeper");
            ViewBag.BookStatusData = this.codeService.GetCodeTable("BookStatus");
            return View("Index");
        }


        public ActionResult BookDetails(string bookID)
        {
            ViewBag.BookClassData = this.codeService.GetCodeTable("BookClass");
            ViewBag.BookKeeperData = this.codeService.GetCodeTable("Keeper");
            ViewBag.BookStatusData = this.codeService.GetCodeTable("BookStatus");
            Models.BookService bookService = new Models.BookService();
            ViewBag.BookDetails = bookService.GetBookDetailsByID(bookID);
            ViewBag.BookClassData = this.codeService.GetDefaultSelected(ViewBag.BookClassData, ViewBag.BookDetails[0].bookClass);
            ViewBag.BookKeeperData = this.codeService.GetDefaultSelected(ViewBag.BookKeeperData, ViewBag.BookDetails[0].bookKeeper);
            ViewBag.BookStatusData = this.codeService.GetDefaultSelected(ViewBag.BookStatusData, ViewBag.BookDetails[0].bookStatus);
            ViewBag.Mode = 'D';
            return View("BookDetails");
        }

        [HttpGet()]
        public ActionResult InsertBook()
        {
            ViewBag.BookClassData = this.codeService.GetCodeTable("BookClass");
            ViewBag.BookStatusData = this.codeService.GetCodeTable("BookStatus");
            ViewBag.BookStatusData = this.codeService.GetDefaultSelected(ViewBag.BookStatusData, "可以借出");
            return View(new Models.Books());
        }

        [HttpPost()]
        public ActionResult InsertBook(Models.Books book)
        {
            ViewBag.insertSuccess = false;
            ViewBag.BookClassData = this.codeService.GetCodeTable("BookClass");
            //ViewBag.BookKeeperData = this.codeService.GetCodeTable("Keeper");
            ViewBag.BookStatusData = this.codeService.GetCodeTable("BookStatus");
            //ViewBag.BookStatusData = this.codeService.GetDefaultSelected(ViewBag.BookStatusData, "可以借出");
            if (ModelState.IsValid)
            {
                Models.BookService bookService = new Models.BookService();
                //ViewBag.BookStatusData = this.codeService.GetDefaultSelected(ViewBag.BookStatusData, "可以借用");
                bookService.InsertBook(book);
                TempData["message"] = "存檔成功";
                ViewBag.insertSuccess = true;
                return RedirectToAction("Index");
            }
            return View(book);
        }

        /// <summary>
        /// 刪除書本
        /// </summary>
        /// <param name="bookID"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteBook(string bookID)
        {
            try
            {
                Models.BookService bookService = new Models.BookService();
                bookService.DeleteBook(bookID);
                return this.Json(true);
            }
            catch (Exception ex)
            {
                return this.Json(false);
            }
        }

        /// <summary>
        /// 修改書本介面
        /// </summary>
        /// <param name="bookID"></param>
        /// <returns></returns>
        public ActionResult UpdateBook(string bookID)
        {
            Models.BookService bookService = new Models.BookService();
            ViewBag.BookDetails = bookService.GetBookDetailsByID(bookID);
            ViewBag.Mode = 'U';
            
            ViewBag.BookClassData = this.codeService.GetCodeTable("BookClass");
            ViewBag.BookKeeperData = this.codeService.GetCodeTable("Keeper");
            ViewBag.BookStatusData = this.codeService.GetCodeTable("BookStatus");
           
            ViewBag.BookClassData = this.codeService.GetDefaultSelected(ViewBag.BookClassData, ViewBag.BookDetails[0].bookClass);
            ViewBag.BookKeeperData = this.codeService.GetDefaultSelected(ViewBag.BookKeeperData, ViewBag.BookDetails[0].bookKeeper);
            ViewBag.BookStatusData = this.codeService.GetDefaultSelected(ViewBag.BookStatusData, ViewBag.BookDetails[0].bookStatus);
            return View("BookDetails");
        }

        /// <summary>
        /// 修改書本
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult BookDetails(Models.Books book)
        {
            ViewBag.updateSuccess = false;
            if (string.IsNullOrEmpty(book.bookKeeper)) book.bookKeeper = "";
            if (ModelState.IsValid)
            {
                Models.BookService bookService = new Models.BookService();
                //bookService.UpdateBook(book);
                bool updateSuccess = bookService.UpdateBook(book);
                if (updateSuccess)
                {
                    TempData["message"] = "存檔成功";
                    ViewBag.updateSuccess = true;
                    return RedirectToAction("Index");
                }
            }
            
            Models.BookService bookService2 = new Models.BookService();
            ViewBag.BookDetails = bookService2.GetBookDetailsByID(book.bookID.ToString());
            ViewBag.Mode = 'U';

            if (string.IsNullOrEmpty(book.bookName)) book.bookName = ""; 
            ViewBag.BookDetails[0].bookName = book.bookName;
            if (string.IsNullOrEmpty(book.bookAuthor)) book.bookAuthor = "";
            ViewBag.BookDetails[0].bookAuthor = book.bookAuthor;
            if (string.IsNullOrEmpty(book.bookPublisher)) book.bookPublisher = "";
            ViewBag.BookDetails[0].bookPublisher = book.bookPublisher;
            if (string.IsNullOrEmpty(book.bookIntroduce)) book.bookIntroduce = "";
            ViewBag.BookDetails[0].bookIntroduce = book.bookIntroduce;
            if (string.IsNullOrEmpty(book.bookBoughtDate)) book.bookBoughtDate = "";
            ViewBag.BookDetails[0].bookBoughtDate = book.bookBoughtDate;
            if (string.IsNullOrEmpty(book.bookClass)) book.bookClass = "";
            ViewBag.BookDetails[0].bookClass = book.bookClass;
            if (string.IsNullOrEmpty(book.bookStatus)) book.bookStatus = "";
            ViewBag.BookDetails[0].bookStatus = book.bookStatus;
            if (string.IsNullOrEmpty(book.bookKeeper)) book.bookKeeper = "";
            ViewBag.BookDetails[0].bookKeeper = book.bookKeeper;

            ViewBag.BookClassData = this.codeService.GetCodeTable("BookClass");
            ViewBag.BookKeeperData = this.codeService.GetCodeTable("Keeper");
            ViewBag.BookStatusData = this.codeService.GetCodeTable("BookStatus");

            ViewBag.BookClassData = this.codeService.GetDefaultSelected(ViewBag.BookClassData, ViewBag.BookDetails[0].bookClass);
            ViewBag.BookKeeperData = this.codeService.GetDefaultSelected(ViewBag.BookKeeperData, ViewBag.BookDetails[0].bookKeeper);
            ViewBag.BookStatusData = this.codeService.GetDefaultSelected(ViewBag.BookStatusData, ViewBag.BookDetails[0].bookStatus);
            return View(book);
        }
    }
}