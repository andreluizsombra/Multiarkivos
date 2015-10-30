using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace MK.Easydoc.Core.Infrastructure
{
    public class DbConn
    {
        public static DbConnection CreateDBConnection()
        {
            try
            {
                //DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
                Database dataBase = new DatabaseProviderFactory().Create(ConfigurationManager.AppSettings.Get("DefaultConnection"));
                DbConnection conn = dataBase.CreateConnection();
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
                
            }
        }
        public static Database CreateDB()
        {
            try
            {
                //DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());

                //Database dataBase = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings.Get("Banco"));
                Database dataBase = new DatabaseProviderFactory().Create(ConfigurationManager.AppSettings.Get("Banco"));
                
                return dataBase;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);

            }
        }

        public DataTable RetornaDados(string _sql){
            try{
                var ds = new DataSet();
                var _db = DbConn.CreateDB();
                ds = _db.ExecuteDataSet(CommandType.Text, _sql);
                return ds.Tables[0];
            }
            catch{
                throw;
            }
        }

    }
}
