using myRestful.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using log4net;
using log4net.Config;
using System.IO;
using System.Configuration;

namespace myRestful.RestfulClass
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int Price { get; set; }
        public DateTime ImportDate { get; set; }
        public int SaleNumber { get; set; }
        public int Rank { get; set; }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Book()
        {
        }

        public Book(int bookId, string bookName, int price, DateTime importDate, int saleNumber, int rank)
        {
            BookId = bookId;
            BookName = bookName;
            Price = price;
            ImportDate = importDate;
            SaleNumber = saleNumber;
            Rank = rank;
        }
        public static void GetAllBooks(List<Book> bookList)
        {
            SqlCommand scmd = null;
            string Sqlstr = "select * from Book";   
            
            try
            {
                XmlConfigurator.Configure(new FileInfo(Log4NetUtility.GetLog4netConfigPath()));
                log.Info("star get all books;");

                scmd = SqlUtility.BuildConnectedSqlCommand(); //取得一個開啟transaction的sqlCommand物件
                scmd = new SqlCommand(Sqlstr, scmd.Connection, scmd.Transaction);

                SqlDataReader sReader = scmd.ExecuteReader();
                if (sReader.HasRows)
                {
                    while (sReader.Read())
                    {
                        int bid = sReader.GetInt32(0);
                        string bName = sReader.GetString(1);
                        int bprice = sReader.GetInt32(2);
                        DateTime bImpDate = sReader.GetDateTime(3);
                        int bSaleNum = sReader.GetInt32(4);
                        int bRank = sReader.GetInt32(5);
                        Book b = new Book(bid, bName, bprice, bImpDate, bSaleNum, bRank);

                        bookList.Add(b);
                    }
                }
                sReader.Close();
                scmd.Transaction.Commit();
                log.Info("transation finish");

            }
            catch (SqlException sqlEx)
            {
                log.Debug("transation failed, " + sqlEx.ToString());
                throw sqlEx;
            }
            finally
            {
                scmd.Dispose();
            }
        }
        public static void GetBook(int id, Book bk)
        {

            List<Book> booksobj = new List<Book>();
            SqlCommand scmd = null;
            string Sqlstr = "select * from Book where Id=@Id";

            try
            {
                XmlConfigurator.Configure(new FileInfo(Log4NetUtility.GetLog4netConfigPath()));
                log.Info("star get book, id = " + id);

                scmd = SqlUtility.BuildConnectedSqlCommand(); //取得一個開啟transaction的sqlCommand物件

                //command
                scmd = new SqlCommand(Sqlstr, scmd.Connection, scmd.Transaction);

                //set parameter and value
                List<string> parameter = new List<string>();
                List<Object> value = new List<Object>();
                List<SqlDbType> type = new List<SqlDbType>();

                parameter.Add("@Id");
                value.Add(id.ToString());
                type.Add(SqlDbType.Int);

                SqlUtility.SetParameter(scmd, parameter, value, type);
                SqlDataReader sReader = scmd.ExecuteReader();

                if (sReader.HasRows)
                {
                    while (sReader.Read())
                    {
                        bk.BookId = sReader.GetInt32(0);
                        bk.BookName = sReader.GetString(1);
                        bk.Price = sReader.GetInt32(2);
                        bk.ImportDate = sReader.GetDateTime(3);
                        bk.SaleNumber = sReader.GetInt32(4);
                        bk.Rank = sReader.GetInt32(5);
                    }
                }
                sReader.Close();
                scmd.Transaction.Commit();
                log.Info("finish get book, id = " + id);
            }
            catch (Exception sqlEx)
            {
                scmd.Transaction.Rollback();
                log.Debug("get book failed, id = "+ id +", message:" + sqlEx.ToString());
                throw sqlEx;
            }
            finally
            {
                scmd.Dispose(); //release the memory source;
            }
        }


        public void UpdateBook(Book bk)
        {
            SqlCommand scmd = null;
            try
            {
                XmlConfigurator.Configure(new FileInfo(Log4NetUtility.GetLog4netConfigPath()));
                log.Info("star update book");

                List<string> parameter = new List<string>();
                List<Object> value = new List<Object>();
                List<SqlDbType> type = new List<SqlDbType>();
                Boolean flag = false;

                scmd = SqlUtility.BuildConnectedSqlCommand(); //取得一個開啟transaction的sqlCommand物件

                //first:check repeat
                string Sqlstr = "select * from Book where Id=@Id";
                scmd = new SqlCommand(Sqlstr, scmd.Connection, scmd.Transaction);

                parameter.Add("@Id");
                value.Add(bk.BookId.ToString());
                type.Add(SqlDbType.Int);

                SqlUtility.SetParameter(scmd, parameter, value, type);

                SqlDataReader sReader = scmd.ExecuteReader();
             
                if (sReader.Read())
                {
                    log.Info("book is existed, go to update. id = " + bk.BookId);
                    flag = true;
                }
                else
                {
                    log.Info("book is not existed, to to insert. id = " + bk.BookId);
                    flag = false;
                }
                sReader.Close();
                scmd.Transaction.Commit();

                if (flag)
                {
                    log.Info("book id = " + bk.BookId + " start update");
                    this.Update(bk);
                }
                else
                {
                    log.Info("book id = " + bk.BookId + " start insert");
                    this.Insert(bk);
                }

            }
            catch (SqlException sqlEx)
            {
                scmd.Transaction.Rollback();
                log.Debug("book id = " + bk.BookId +"update or insert failed, because:"+ sqlEx.ToString());
                throw sqlEx;
            }
            finally
            {
                scmd.Dispose();
            }

        }

        public void Update(Book bk)  //todo:check any field is null 
        {
            int id = bk.BookId;
            SqlCommand scmd = null;

            try
            {
                XmlConfigurator.Configure(new FileInfo(Log4NetUtility.GetLog4netConfigPath()));
                log.Info("star update book");

                //command
                string Sqlstr = "Update Book Set Name=@Name, Price=@Price, ImportDate=@ImportDate, SaleNumber=@SaleNumber, Rank=@Rank " +
                                    "where id=@id";

                scmd = SqlUtility.BuildConnectedSqlCommand();
                scmd = new SqlCommand(Sqlstr, scmd.Connection, scmd.Transaction);

                //set parameter and value
                List<string> parameter = new List<string>();
                List<Object> value = new List<Object>();
                List<SqlDbType> type = new List<SqlDbType>();

                parameter.Add("@Name");
                value.Add(bk.BookName);
                type.Add(SqlDbType.NVarChar);
                parameter.Add("@Price");
                value.Add(bk.Price);
                type.Add(SqlDbType.Int);
                parameter.Add("@ImportDate");
                value.Add(bk.ImportDate);
                type.Add(SqlDbType.DateTime);
                parameter.Add("@SaleNumber");
                value.Add(bk.SaleNumber);
                type.Add(SqlDbType.NVarChar);
                parameter.Add("@Rank");
                value.Add(bk.Rank);
                type.Add(SqlDbType.Int);
                parameter.Add("@Id");
                value.Add(bk.BookId);
                type.Add(SqlDbType.Int);

                SqlUtility.SetParameter(scmd, parameter, value, type);

                scmd.ExecuteNonQuery();

                // Attempt to commit the transaction.
                scmd.Transaction.Commit();
                log.Info("book already updated");
            }
            catch (SqlException sqlEx)
            {
                scmd.Transaction.Rollback();
                log.Debug("update book failed, because:"+sqlEx.ToString());
                throw sqlEx;
            }
            finally //無論如何都會做的事情
            {
                scmd.Dispose();
            }
        }

        public void Insert(Book bk)
        {
            SqlCommand scmd = null;

            //for sql parameter
            List<string> parameter = new List<string>();
            List<Object> value = new List<Object>();
            List<SqlDbType> type = new List<SqlDbType>();

            try
            {
                XmlConfigurator.Configure(new FileInfo(Log4NetUtility.GetLog4netConfigPath()));
                log.Info("star insert book");

                String sqlstr = "Insert Into Book (Id, Name, Price, ImportDate, SaleNumber, Rank)" +
                              " values ( @Id, @Name, @Price, @ImportDate, @SaleNumber, @Rank)";

                scmd = SqlUtility.BuildConnectedSqlCommand();
                scmd = new SqlCommand(sqlstr, scmd.Connection, scmd.Transaction);

                //set parameter and value
                parameter.Add("@Id");
                value.Add(bk.BookId);
                type.Add(SqlDbType.Int);
                parameter.Add("@Name");
                value.Add(bk.BookName);
                type.Add(SqlDbType.NVarChar);
                parameter.Add("@Price");
                value.Add(bk.Price);
                type.Add(SqlDbType.Int);
                parameter.Add("@ImportDate");
                value.Add(bk.ImportDate);
                type.Add(SqlDbType.DateTime);
                parameter.Add("@SaleNumber");
                value.Add(bk.SaleNumber);
                type.Add(SqlDbType.NVarChar);
                parameter.Add("@Rank");
                value.Add(bk.Rank);
                type.Add(SqlDbType.Int);

                SqlUtility.SetParameter(scmd, parameter, value, type);

                scmd.ExecuteNonQuery();
                scmd.Transaction.Commit();
                log.Info("book already inserted");
            }
            catch (SqlException sqlEx)
            {
                scmd.Transaction.Rollback();
                log.Debug("insert book failed, because:" + sqlEx.ToString());
                throw sqlEx;
            }
            finally
            {
                scmd.Dispose();
            }
        }

        public void Delete(int id)
        {

            SqlCommand scmd = null;

            //for sql parameter
            List<string> parameter = new List<string>();
            List<Object> value = new List<Object>();
            List<SqlDbType> type = new List<SqlDbType>();

            String sqlstr = "delete from Book where id = @Id";

            try
            {
                XmlConfigurator.Configure(new FileInfo(Log4NetUtility.GetLog4netConfigPath()));
                log.Info("star delete book, id = "+ id);

                scmd = SqlUtility.BuildConnectedSqlCommand();
                scmd = new SqlCommand(sqlstr, scmd.Connection, scmd.Transaction);
                //set parameter and value
                parameter.Add("@Id");
                value.Add(id);
                type.Add(SqlDbType.Int);

                SqlUtility.SetParameter(scmd, parameter, value, type);

                scmd.ExecuteNonQuery();
                scmd.Transaction.Commit();
                log.Info("book has been deleted, id = " + id);
            }
            catch (SqlException sqlEx)
            {
                scmd.Transaction.Rollback();
                log.Debug("deleted book failed, because:" + sqlEx.ToString());
                throw sqlEx;
            }
            finally
            {
                scmd.Dispose();
            }

        }
    }
}