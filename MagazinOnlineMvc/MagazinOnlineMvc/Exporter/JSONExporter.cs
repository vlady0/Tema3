using MagazinOnlineMvc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MagazinOnlineMvc.Exporter
{
    public class JSONExporter : IExporter
    {
        public JSONExporter()
        {
            exportOperation();
        }


        public void exportOperation()
        {
            List<Product> prodList = new List<Product>();
            using (ProductDBContext dc = new ProductDBContext())
            {
                prodList = dc.Products.ToList();
            }
                        
            string json = JsonConvert.SerializeObject(prodList.ToArray(), Formatting.Indented);

            //write string to file
            //System.IO.File.WriteAllText(@"E:\test.txt", json);

            //download
            HttpContext context = System.Web.HttpContext.Current;
            context.Response.Write(json);
            context.Response.ContentType = "text/json";
            context.Response.AddHeader("Content-Disposition", "attachment; filename = Product.json");
            context.Response.End();
        }
    }
}