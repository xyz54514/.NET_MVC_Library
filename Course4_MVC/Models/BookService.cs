using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course4_MVC.Models
{
    public class BookService
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        public int InsertBook(Models.Books book)
        {
            //DataTable dt = new DataTable();
            string sql = @"insert into BOOK_DATA 
                           ( 
                               BOOK_NAME, BOOK_AUTHOR, BOOK_PUBLISHER, BOOK_NOTE,
                               BOOK_BOUGHT_DATE, BOOK_CLASS_ID, BOOK_STATUS
                           ) 
                           values 
                           (
                               @bookName, @bookAuthor, @bookPublisher, @bookNote,
                               @bookBoughtDate, @bookClass, @bookStatus
                           )";
            int bookID;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookName", book.bookName));
                cmd.Parameters.Add(new SqlParameter("@bookAuthor", book.bookAuthor));
                cmd.Parameters.Add(new SqlParameter("@bookPublisher", book.bookPublisher));
                cmd.Parameters.Add(new SqlParameter("@bookNote", book.bookIntroduce));
                cmd.Parameters.Add(new SqlParameter("@bookClass", book.bookClass));
                cmd.Parameters.Add(new SqlParameter("@bookBoughtDate", book.bookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@bookStatus", book.bookStatus));
                bookID = Convert.ToInt32(cmd.ExecuteScalar());
                //SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                //sqlAdapter.Fill(dt);
                
                conn.Close();
                return bookID;
                
            }
        }

        public List<Models.Books> GetBooksByCondition(Models.BookSearchArgs arg)
        {
            DataTable dt = new DataTable();
            string sql = @"select bd.BOOK_ID, bc.BOOK_CLASS_NAME, bd.BOOK_NAME, bd.BOOK_BOUGHT_DATE, bco.CODE_NAME,
                           m.USER_CNAME as BOOK_KEEPER, bd.BOOK_AUTHOR, bd.BOOK_PUBLISHER, bd.BOOK_NOTE from BOOK_DATA bd
                           inner join BOOK_CLASS bc on bc.BOOK_CLASS_ID = bd.BOOK_CLASS_ID
                           inner join (select * from BOOK_CODE where CODE_TYPE = 'BOOK_STATUS') bco
                           on bd.BOOK_STATUS = bco.CODE_ID 
                           left join MEMBER_M m on bd.BOOK_KEEPER = m.[USER_ID]
                           where ( UPPER(bd.BOOK_Name) like UPPER(('%' + @bookName + '%')) or @bookName = '') 
                           and (bd.BOOK_CLASS_ID = @bookClass or @bookClass = '')
                           and (BOOK_KEEPER = @bookKeeper or @bookKeeper = '')
                           and (bd.BOOK_STATUS = @bookStatus or @bookStatus = '')

                           order by bc.BOOK_CLASS_NAME, bd.BOOK_BOUGHT_DATE";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookName", arg.BookName == null? string.Empty: arg.BookName));
                cmd.Parameters.Add(new SqlParameter("@bookClass", arg.BookClass == null ? string.Empty : arg.BookClass));
                cmd.Parameters.Add(new SqlParameter("@bookKeeper", arg.BookKeeper == null ? string.Empty : arg.BookKeeper));
                cmd.Parameters.Add(new SqlParameter("@bookStatus", arg.BookStatus == null ? string.Empty : arg.BookStatus));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            return this.MapBookDataToList(dt);
        }

        private List<Models.Books> MapBookDataToList(DataTable bookData)
        {
            List<Models.Books> result = new List<Books>();
            foreach (DataRow row in bookData.Rows)
            {
                result.Add(new Books()
                {
                    bookID = (int)row["BOOK_ID"],
                    bookName = row["BOOK_NAME"].ToString(),
                    bookAuthor = row["BOOK_AUTHOR"].ToString(),
                    bookPublisher = row["BOOK_PUBLISHER"].ToString(),
                    bookBoughtDate = row["BOOK_BOUGHT_DATE"].ToString(),
                    bookIntroduce = row["BOOK_NOTE"].ToString(),
                    bookClass = row["BOOK_CLASS_NAME"].ToString(),
                    bookStatus = row["CODE_NAME"].ToString(),
                    bookKeeper = row["BOOK_KEEPER"].ToString()
                });
            }
            return result;
        }

        public List<Models.Books> GetBookDetailsByID(string bookID)
        {
            DataTable dt = new DataTable();
            string sql = @"select bd.BOOK_ID, bc.BOOK_CLASS_NAME, bd.BOOK_NAME, bd.BOOK_BOUGHT_DATE, bco.CODE_NAME,
                           m.USER_CNAME as BOOK_KEEPER, bd.BOOK_AUTHOR, bd.BOOK_PUBLISHER, bd.BOOK_NOTE from BOOK_DATA bd
                           inner join BOOK_CLASS bc on bc.BOOK_CLASS_ID = bd.BOOK_CLASS_ID
                           inner join (select * from BOOK_CODE where CODE_TYPE = 'BOOK_STATUS') bco
                           on bd.BOOK_STATUS = bco.CODE_ID 
                           left join MEMBER_M m on bd.BOOK_KEEPER = m.[USER_ID]
                           where bd.BOOK_ID = @bookID

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

            return this.MapBookDataToList(dt);
        }

        public void DeleteBook(string bookID)
        {
            try
            {
                string sql = @" Delete from BOOK_DATA
                                 where BOOK_ID = @bookID";

                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@bookID", bookID));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }

        public bool UpdateBook(Models.Books book)
        {
            string sql = @" update BOOK_DATA
                            set BOOK_NAME = @bookName,
                                BOOK_AUTHOR = @bookAuthor,
                                BOOK_PUBLISHER = @bookPublisher,
                                BOOK_NOTE = @bookIntroduce,
                                BOOK_BOUGHT_DATE = @bookBoughtDate,
                                BOOK_CLASS_ID = @bookClass,
                                BOOK_STATUS = @bookStatus,
                                BOOK_KEEPER = @bookKeeper
                             where BOOK_ID = @bookID
                          ";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@bookName", book.bookName));
                    cmd.Parameters.Add(new SqlParameter("@bookAuthor", book.bookAuthor));
                    cmd.Parameters.Add(new SqlParameter("@bookPublisher", book.bookPublisher));
                    cmd.Parameters.Add(new SqlParameter("@bookIntroduce", book.bookIntroduce));
                    cmd.Parameters.Add(new SqlParameter("@bookBoughtDate", book.bookBoughtDate));
                    cmd.Parameters.Add(new SqlParameter("@bookClass", book.bookClass));
                    cmd.Parameters.Add(new SqlParameter("@bookStatus", book.bookStatus));
                    cmd.Parameters.Add(new SqlParameter("@bookKeeper", book.bookKeeper));
                    cmd.Parameters.Add(new SqlParameter("@bookID", book.bookID));
                    int i = cmd.ExecuteNonQuery();
                    //conn.Close();

                    return (i == 0 ? false : true);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}