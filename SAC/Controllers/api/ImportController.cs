using SAC.Models;
using SAC.Services.Import;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace App.SAC.Controllers.api
{
    public class ImportController : ApiController
    {
        private SACServiceContext db = new SACServiceContext();

        [HttpGet]
        public HttpResponseMessage Import(string path, string raceDate)
        {
            DateTime? date = null;
            if (!string.IsNullOrEmpty(raceDate))
            {
                DateTime d;
                if (DateTime.TryParseExact(raceDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                    date = d;
            }
            
            string result = PdfImporter.ImportPdf(path, date, db);
            if(string.IsNullOrEmpty(result))
                return new HttpResponseMessage(HttpStatusCode.OK);
            else
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result);
        }
    }
}
