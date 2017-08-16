using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace bOS.Commons.IO.CSV
{
    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    
}