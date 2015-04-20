using SAC.Services.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace App.SAC.Controllers.api
{
    public class ImportController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Import(string path)
        {
            if(PdfImporter.ImportPdf(path))
                return new HttpResponseMessage(HttpStatusCode.OK);
            else
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}
