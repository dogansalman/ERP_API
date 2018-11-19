using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;
using abkar_api.Libary.Identity;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace abkar_api.Controllers.SupplierController
{
    [RoutePrefix("api/supplier/requisitions")]
    public class RequisitionsController : ApiController
    {
        string root = HttpContext.Current.Server.MapPath("~/Documents/");
        List<string> extention = new List<string> { ".pdf", ".doc", ".docx", ".gif", ".jpg", ".jpeg", ".rft", ".rar", ".zip", ".png" };
        private bool validateExt(string fileExtentions)
        {
            foreach (var ext in extention)
            {
                if (ext == fileExtentions) return true;
            }
            return false;
        }
        public static object getFolders(string targetDirectory)
        {
            // Process the list of files found in the directory.
            List<object> files = new List<object>();
            foreach (string s in Directory.GetDirectories(targetDirectory))
            {
                DirectoryInfo info = new DirectoryInfo(s);
                files.Add(new {
                    folder = info.Name,
                    files = filesCount(s)
                });
            }
            return files;
        }
        public static int filesCount(string folderPath)
        {
            string[] fileEntries = Directory.GetFiles(folderPath);
            return fileEntries.Length;
        }

        public static  object Files(string folderPath)
        {
            DirectoryInfo info = new DirectoryInfo(folderPath);
            List<object> files = new List<object>();
            string[] fileEntries = Directory.GetFiles(folderPath);
            foreach (var file in fileEntries)
            {
                files.Add(new
                {
                    mainFolder = info.Name,
                    filename = new FileInfo(file).Name
                });
            }
            return files;
            
        }
        DatabaseContext db = new DatabaseContext();

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "supplier")]
        public object Get()
        {
            int supplier_id = Identity.get(User);
            return (
                from s in db.supplyrequisitions
                join sc in db.stockcards on s.stockcard_id equals sc.id
                where s.supplier_id == supplier_id
                select new
                {
                    id = s.id,
                    stockcard_id = s.stockcard_id,
                    supplier = s.supplier,
                    message = s.message,
                    unit = s.unit,
                    real_unit = s.real_unit,
                    waybill = s.waybill,
                    notify = s.notify,
                    delivery_date = s.delivery_date,
                    created_date = s.created_date,
                    updated_date = s.updated_date,
                    supplier_id = s.supplier_id,
                    stockcard = sc.name,
                    state = s.state,
                    stockcard_code = sc.code

                }
                ).OrderByDescending(sr => sr.created_date).ToList();
        }

        [HttpGet]
        [Route("stats")]
        [Authorize(Roles = "supplier")]
        public object stats()
        {
            int supplier_id = Identity.get(User);
            int waiting =  db.supplyrequisitions.Where(sr => sr.supplier_id == supplier_id && sr.state == 0).Count();
            int delivered = db.supplyrequisitions.Where(sr => sr.supplier_id == supplier_id && sr.state == 1).Count();
            int cancelled = db.supplyrequisitions.Where(sr => sr.supplier_id == supplier_id && sr.state == 2).Count();
            return ( new {  waiting = waiting, delivered = delivered, cancelled = cancelled } );
        }

        [HttpPost]
        [Route("docs/{id}")]
        //[Authorize(Roles = "supplier")]
        public async Task<HttpResponseMessage> upload(int id)
        {
            
            string date = HttpContext.Current.Request.Form["date"].ToString().Replace('.','-');
            string filaname = HttpContext.Current.Request.Form["filename"].ToString().Trim();
            string note = HttpContext.Current.Request.Form["note"].ToString();

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string rootFile = root + id + @"\" + date;
            DirectoryInfo di = Directory.CreateDirectory(rootFile);
            DirectoryInfo di_profile = Directory.CreateDirectory(rootFile);

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
                    file.name = files.Headers.ContentDisposition.FileName.Replace('"', ' ').Trim();
                }
            
                //change filename
                FileInfo photoFile = new FileInfo(rootFile + "/" + file.name);

                // Validate extentions
                if(!validateExt(photoFile.Extension)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Dosya formatı hatalı");


                if (photoFile.Exists)
                {
                    string vExtension = photoFile.Extension;
                    string vNewFileName = rootFile + vExtension;
                    string vNewFullName = photoFile.DirectoryName + "/" + filaname + vExtension;
                    photoFile.MoveTo(vNewFullName);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new {name = file.name });

            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        
        [HttpGet]
        //[Authorize(Roles = "supplier")]
        [Route("docs/{id}")]
        public object getDocs(int id)
        {
            if (Directory.Exists(root + @"\" + id))
            {
                return getFolders(root + @"\" + id);
            }
            return Ok();
        }
        [HttpGet]
        //[Authorize(Roles = "supplier")]
        [Route("files/{id}/{folder}")]
        public object getFiles(int id, string folder) 
        {
            if (Directory.Exists(root + @"\" + id + @"\" + folder))
            {
                return Files(root + @"\" + id + @"\" + folder);
            }
            return Ok();
        }
    }
}
