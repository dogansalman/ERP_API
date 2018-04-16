using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using abkar_api.Contexts;
using abkar_api.Models;
using abkar_api.ModelViews;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/quality")]
    public class QualityFilesController : ApiController
    {
        string root = HttpContext.Current.Server.MapPath("~/Quality/");
        DatabaseContext db = new DatabaseContext();

        
        [HttpPost]
        [Route("files/{id}")]
        public async Task<HttpResponseMessage> PostFormData(int id)
        {
    
           
            object title = HttpContext.Current.Request.Form["title"].ToString();
            object date = HttpContext.Current.Request.Form["date"].ToString();
            if(title == null || date == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError("Eksik parametre"));

            // Validation orders
            if (!db.orders.Any(o => o.id == id)) return Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError("Sipariş kaydı bulunamadı"));           

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string rootFile = root + "/" + id + "/" + date.ToString();
            DirectoryInfo di = Directory.CreateDirectory(rootFile);

           
            CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(rootFile);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                File file = new File();
                file.id = id;

                // Get last filename
                foreach (MultipartFileData files in provider.FileData)
                {
                    FileInfo finfo = new FileInfo(files.LocalFileName);
                    string newFileLocation = rootFile + "/" + title.ToString().Replace(' ', '-') + finfo.Extension;

                    System.IO.File.Move(files.LocalFileName, newFileLocation);

                    file.name = "/Quality/" + date.ToString() + "/" +   title.ToString().Replace(' ', '-') + finfo.Extension;
                }

                return Request.CreateResponse(HttpStatusCode.OK, file);

            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
