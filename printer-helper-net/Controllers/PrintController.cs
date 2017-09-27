using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Drawing.Printing;
using printer_helper_net.Models;
using System.Drawing;
using printer_helper_net.Helpers;

namespace printer_helper_net.Controllers
{
    public class PrintController : ApiController
    {
        // GET: api/Print
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Print/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Print
        public HttpResponseMessage Post([FromBody]printer_helper_net.Models.Image value)
        {
            HttpResponseMessage response;
            try
            {
                string printerName = System.Configuration.ConfigurationManager.AppSettings["PrinterName"]; 
                string paperName = System.Configuration.ConfigurationManager.AppSettings["PaperName"];
                var bytes = Convert.FromBase64String(value.image64);
                PrintDocument pd = new PrintDocument();
                Stream stream = new MemoryStream(bytes);
                int paperWidth = 324;
                int paperHeight = 157;
                foreach (PaperSize item in pd.PrinterSettings.PaperSizes)
                {
                    if (item.PaperName == paperName)
                    {
                        paperWidth = item.Width;
                        paperHeight = item.Height;
                    }
                }
                pd.PrintPage += (sender, args) =>
                {
                    System.Drawing.Image i = System.Drawing.Image.FromStream(stream);
                    Point p = new Point(100, 100);
                    i = Helpers.Helpers.ResizeImage(i, paperWidth, paperHeight);
                    args.Graphics.DrawImage(i, 0, 0, i.Width, i.Height);

                };
                pd.PrinterSettings.PrinterName = printerName;
                pd.Print();
                response = Request.CreateResponse(HttpStatusCode.Created, "Printed");
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }
        
        // PUT: api/Print/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Print/5
        public void Delete(int id)
        {
        }
    }
}

