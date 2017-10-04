using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;

public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
{
    public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

    public override string GetLocalFileName(HttpContentHeaders headers)
    {
        return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
    }
}

public class File
{
    public string name { get; set; }
    public int id { get; set; }
}




namespace abkar_api.Controllers
{

    [RoutePrefix("api/files")]
    public class FilesController : ApiController
    {
        string root = HttpContext.Current.Server.MapPath("~/Files/");
        //Create new file
        [HttpPost]
        [Route("{id}")]
        public async Task<HttpResponseMessage> PostFormData(int id)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            
            string rootFile = root + id;
            DirectoryInfo di = Directory.CreateDirectory(root + id);

            // var provider = new MultipartFormDataStreamProvider(rootFile);
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

                return Request.CreateResponse(HttpStatusCode.OK, file);

            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }


        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult get(int id)
        {
            List<File> files = new List<File>();

            string rootFile = root + id;
            DirectoryInfo di = Directory.CreateDirectory(root + id);
            if (!di.Exists) return NotFound();

            foreach (FileInfo item in di.GetFiles())
            {
                File file = new File();
                file.name = item.Name;
                file.id = id;
                files.Add(file);
            }

            return Ok(files);
          
        }
        [HttpGet]
        [Route("download/{id}/{file}/{ext}")]
        public HttpResponseMessage download(int id, string file, string ext)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            string path = HttpContext.Current.Server.MapPath("~/Files/" + id + "/" + file + "." + ext);
            var stream = new FileStream(path, FileMode.Open);

            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(path);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;

            return result;
 
        }

    }
}
