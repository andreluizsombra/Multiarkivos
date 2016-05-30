using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using MK.Easydoc.Core.Infrastructure.Framework;
using TecFort.Framework.Generico;
using System.Data.SqlClient;

namespace MK.Easydoc.Core.Infrastructure
{
    public class DbConn
    {
        
        public static DbConnection CreateDBConnection()
        {
            try
            {
                
                Database dataBase = new DatabaseProviderFactory().Create(ConfigurationManager.AppSettings.Get("DefaultConnection"));
                
                DbConnection conn = dataBase.CreateConnection();
                conn.ConnectionString = Easydoc.Core.Infrastructure.Conexao.DescriptografarConexaoString("DefaultConnection");
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
                //Database dataBase = new DatabaseProviderFactory().Create(ConfigurationManager.AppSettings.Get("Banco"));
                string conexaoString = Easydoc.Core.Infrastructure.Conexao.DescriptografarConexaoString("DefaultConnection");
                Database dataBase = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(conexaoString);
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
