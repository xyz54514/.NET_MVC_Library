using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course4_MVC.Models
{
    public class BookSearchArgs
    {
        [DisplayName("圖書類別")]
        public string BookClass { get; set; }

        [DisplayName("書名")]
        public string BookName { get; set; }

        [DisplayName("購書日期")]
        public string BookBoughtDate { get; set; }

        [DisplayName("借閱狀態")]
        public string BookStatus { get; set; }

        [DisplayName("借閱人")]
        public string BookKeeper { get; set; }
    }
}