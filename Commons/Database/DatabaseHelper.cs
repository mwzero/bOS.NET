using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace bOS.Commons.Database
{
    public class DatabaseHelper
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(DatabaseHelper));

        public static DataTable GetSchema(DbContext ctx, String tablename)
        {
            DbCommand cmd = null;
            DataTable schema = null;

            try
            {
                cmd = ctx.Database.Connection.CreateCommand();
                cmd.Connection.Open();
                string[] restrictions = new string[4] { null, cmd.Connection.Database, tablename, null };
                schema = cmd.Connection.GetSchema("Columns", restrictions);

                /*
                // Display the contents of the table.
                foreach (System.Data.DataRow row in schema.Rows)
                {
                    foreach (System.Data.DataColumn col in schema.Columns)
                    {
                        Console.WriteLine("{0} = {1}",
                          col.ColumnName, row[col]);
                    }
                }
                */
                return schema;


            }
            catch (Exception err)
            {
                logger.Error("Error executing query: " + cmd.CommandText, err);
                return null;
            }
            finally
            {
                if ((cmd.Connection != null) && (cmd.Connection.State == ConnectionState.Open)) cmd.Connection.Close();
                if (cmd != null) cmd.Dispose();


            }

        }

        public static DataTable GetSchema4SqlServer(DbContext ctx, String tablename)
        {
            DbCommand cmd = null;
            DataTable schema = null;

            try
            {
                cmd = ctx.Database.Connection.CreateCommand();
                cmd.Connection.Open();
                string[] restrictions = new string[4] { null, null, tablename, null };
                schema = cmd.Connection.GetSchema("Columns", restrictions);

                /*
                // Display the contents of the table.
                foreach (System.Data.DataRow row in schema.Rows)
                {
                    foreach (System.Data.DataColumn col in schema.Columns)
                    {
                        Console.WriteLine("{0} = {1}",
                          col.ColumnName, row[col]);
                    }
                }
                */
                return schema;


            }
            catch (Exception err)
            {
                logger.Error("Error executing query: " + cmd.CommandText, err);
                return null;
            }
            finally
            {
                if ((cmd.Connection != null) && (cmd.Connection.State == ConnectionState.Open)) cmd.Connection.Close();
                if (cmd != null) cmd.Dispose();


            }

        }

        

        public static void UpdateVisita(DbContext ctx, String[] sqls)
        {
            DbCommand cmd = null;
            try
            {
                cmd = ctx.Database.Connection.CreateCommand();
                cmd.Connection.Open();
                foreach (String command in sqls)
                {
                    cmd.CommandText += command;
                }
                cmd.CommandType = CommandType.Text;

                int rowsaffected = cmd.ExecuteNonQuery();
                

            }
            catch (Exception err)
            {
                logger.Error("Error executing query: " + cmd.CommandText, err);
            }
            finally
            {
                if ((cmd.Connection != null) && (cmd.Connection.State == ConnectionState.Open)) cmd.Connection.Close();
                if (cmd != null) cmd.Dispose();


            }

            
        }
    
    }
}