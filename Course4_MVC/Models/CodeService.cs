using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course4_MVC.Models
{
    public class CodeService
    {
        /// <summary>
        /// 建立DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        /// <summary>
        /// 取得書本資料
        /// </summary>
        /// <param name="bookID"></param>
        /// <returns></returns>
        public List<SelectListItem> GetBook(string bookID)
        {
            DataTable dt = new DataTable();
            string sql = @"select bc.BOOK_CLASS_NAME as CodeID, bd.BOOK_NAME as CodeName, bd.BOOK_BOUGHT_DATE, bco.CODE_NAME 
                           , m.USER_CNAME from BOOK_DATA bd
                           inner join BOOK_CLASS bc on bc.BOOK_CLASS_ID = bd.BOOK_CLASS_ID
                           inner join (select * from BOOK_CODE where CODE_TYPE = 'BOOK_STATUS') bco
                           on bd.BOOK_STATUS = bco.CODE_ID 
                           left join MEMBER_M m on bd.BOOK_KEEPER = m.[USER_ID]
                           where bd.BOOK_ID != @bookID
                           order by bc.BOOK_CLASS_NAME, bd.BOOK_BOUGHT_DATE";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookID", bookID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }

        /// <summary>
        /// 取得CodeTable的部分資料
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetCodeTable(string type)
        {
            DataTable dt = new DataTable();
            string sql = @"select distinct CodeVal as CodeName, CodeID as CodeID from CodeTable
                           where CodeType = @type";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@type", type));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }

        private List<SelectListItem> MapCodeData(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    //Text = dr["CodeID"].ToString() + dr["CodeName"].ToString(),
                    Text = dr["CodeName"].ToString(),
                    Value = dr["CodeId"].ToString()
                });

            }

            return result;
        }

        public List<SelectListItem> GetDefaultSelected(List<SelectListItem> ddl, string dataText)
        {

            foreach (SelectListItem s in ddl)
            {
                if (s.Text == dataText)
                {
                    s.Selected = true;
                    break;
                }
            }

            return ddl;
        }
    }
}