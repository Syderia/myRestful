using myRestful.RestfulClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace myRestful.RestfulApiController
{
    public class BooksController : ApiController
    {

        // GET api/<controller>
        public String GetAllBooks()
        {
            List<Book> bookList = new List<Book>();

            Book.GetAllBooks(bookList);

            return JsonConvert.SerializeObject(bookList.ToArray());
        }

        // GET api/<controller>/{id}
        public String GetBook(int id)
        {
            Book bk = new Book();

            Book.GetBook(id, bk);

            return JsonConvert.SerializeObject(bk); 
        }

        // POST api/<controller>
        public String Post(HttpRequestMessage request)
        //public String Post([FromBody]string json)
        {
     
            string json = request.Content.ReadAsStringAsync().Result.ToString();
            Book b = new Book();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                 b = serializer.Deserialize<Book>(json.ToString());
                 b.UpdateBook(b);
                return "update success";
            }
            catch (Exception ex) {
                return ex.ToString();
            }
        }

        // PUT api/<controller>/5
        public String Put(int id, HttpRequestMessage request)
        {
            string json = request.Content.ReadAsStringAsync().Result.ToString();
            Book b = new Book();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                b = serializer.Deserialize<Book>(json.ToString());
                b.UpdateBook(b);
                return "update success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        // DELETE api/<controller>/5
        public String Delete(int id)
        {
            try
            {
                Book b = new Book();
                b.Delete(id);
                return "delete success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        //#region noused
        //public void setParameter() {

        //    List<String> parameter = new List<string>();
        //    List<String> value = new List<string>();



        //}

        //public void GetConnection(SqlConnection sc)
        //{
        //    String connStr = "";
        //    try
        //    {
        //        connStr = ConfigurationManager.ConnectionStrings["RestfulDB"].ConnectionString;
        //        sc = new SqlConnection(connStr);
        //        sc.Open();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public void InterruptConnection(SqlConnection sc)
        //{
        //    try
        //    {
        //        sc.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //#endregion
    }
}