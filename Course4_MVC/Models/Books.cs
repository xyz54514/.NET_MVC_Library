using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course4_MVC.Models
{
    public class Books
    {
        [DisplayName("書本編號")]
        public int bookID { get; set; }

        [DisplayName("書名")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookName { get; set; }

        [DisplayName("作者")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookAuthor { get; set; }

        [DisplayName("出版商")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookPublisher { get; set; }

        [DisplayName("內容簡介")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookIntroduce { get; set; }

        [DisplayName("購書日期")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookBoughtDate { get; set; }

        [DisplayName("圖書類別")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookClass { get; set; }

        [DisplayName("借閱狀態")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookStatus { get; set; }

        [DisplayName("借閱人")]
        public string bookKeeper { get; set; }
    }
}