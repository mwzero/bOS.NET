using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using log4net;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Configuration;
using System.Data;
using bOS.Commons;


namespace bOS.Commons.Database
{
    public enum CrudAuthenticator
    {
        OK = 0,
        KO = 1,
        CHANGE_PASSWORD = 2
    }

    public enum CrudOperation
    {
        INSERT = 0,
        DELETE = 1,
        UPDATE = 2
    }

    public enum CrudResult
    {
        OK = 0,
        KO = 1,
        KO_CRUD_NOTRECOGNIZED = 2
    }

    public class CrudUtils
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(CrudUtils));

        public static bool VerifyConnectivity(String connectionStringName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            EntityConnection conn = new EntityConnection(connectionString);
            try
            {
                // Explicitly open the connection.
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    logger.Info("Testing DB connection: OK");
                    conn.Close();
                }

            }
            catch (EntityException ee)
            {
                logger.Error(ee.GetType());
                logger.Error("Connecting to the database failed. Make sure you connected to the DB Server Work fine. Reason: " + ee.Message);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("Login failed. Make sure that all connection settings are well done. Reason: " + ex.Message);
                return false;
            }
            finally
            {
                // Explicitly dispose of  the connection. 
                //conn.Close();
                conn.Dispose();
            }
            return true;
        }

        public static string getPartConnectionString(String part, String _connectionString)
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
