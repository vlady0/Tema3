using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MagazinOnlineMvc.Exporter
{
    public class ExportFactory
    {
        internal IExporter getExportType(string type)
        {
            switch (type)
            {
                case "CSV": return new CSVExporter();
                case "JSON": return new JSONExporter();
                default: throw new ArgumentException("Invalid type", "type");
            }
        }
    }
}