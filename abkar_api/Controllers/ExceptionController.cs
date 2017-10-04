using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace abkar_api.Controllers
{
    public class ExceptionController : ApiController
    {
        public static void Handle(Exception e)
        {
            while (e.InnerException != null) e = e.InnerException;
            if (e.GetType() == typeof(SqlException))
            {
                e = (SqlException)e as SqlException;
                SqlException x = (SqlException)e as SqlException;
                if (x.Number == 2627) throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotImplemented) { Content = new StringContent("{\"Message\":\"Handle unique constraint violation.\"}", System.Text.Encoding.UTF8, "application/json") });
            }

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotImplemented) { Content = new StringContent("{\"Message\":\"Not implemented exception.\"}", System.Text.Encoding.UTF8, "application/json") });
        }
    }
}
