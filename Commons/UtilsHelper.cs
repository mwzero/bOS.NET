using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace bOS.Commons
{
    public class UtilsHelper
    {
        public static string GetPartConnectionString(String part, String _connectionString)
        {

            int init;
            String partTemp;
            String partResult = string.Empty;

            init = _connectionString.IndexOf(part) + part.Length + 1;

            for (int contPartConn = init; contPartConn <= _connectionString.Length; contPartConn++)
            {
                partTemp = _connectionString.Substring(contPartConn, 1);

                if (partTemp.Equals(";"))
                {
                    return partResult;
                }
                else
                {
                    partResult += partTemp;
                }
            }
            return partResult;
        }
    }
}