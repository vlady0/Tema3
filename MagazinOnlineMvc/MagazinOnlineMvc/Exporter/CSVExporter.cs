using MagazinOnlineMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MagazinOnlineMvc.Exporter
{
    public class CSVExporter : IExporter
    {
        public CSVExporter()
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

            StringBuilder sb = new StringBuilder();
     
            string header = @"""ID"",""Product Name"",""Descripion"",""Price"",""No. of Products""";

            Product prod = new Product();

            sb.Append(header);

            foreach (var i in prodList)
            {
                sb.AppendLine(string.Join(",",
                    string.Format(@"""{0}""", i.ID),
                    string.Format(@"""{0}""", i.name),                    
                    string.Format(@"""{0}""", i.description),
                    string.Format(@"""{0}""", i.price),
                    string.Format(@"""{0}""", i.numberOfProducts)));
            }
                    

            //download     
            HttpContext context = System.Web.HttpContext.Current;
            context.Response.Write(sb.ToString());
            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("Content-Disposition", "attachment; filename = Product.csv");
            context.Response.End();

            
        }
    }
}