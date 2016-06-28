using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Repositories.Interfaces;
using MK.Easydoc.Core.Repositories;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.Core.Infrastructure.Framework;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
                                                 
namespace MK.Easydoc.Core.Repositories
{
    public class StoragePrivateRepository:IStoragePrivate
    {
        public int idCliente { get; set; }
        public string Path { get; protected set; }
        public string UserAccount { get; protected set; }
        public string PswAccount { get;  protected set; }
        public string DiretorioLocal { get; protected set; }
        public string DiretorioNuvem { get; protected set; }
        public int Nuvem { get; protected set; }
        protected CloudBlockBlob Blob { get; set; }
        private readonly CloudStorageAccount _storageAccount;
        public StoragePrivateRepository()
        {
            Path = "";
            //_storageAccount = CloudStorageAccount.Parse("ixiPNQ0HgCcZn+2JEY51gMKjMOPA7BxEHqMxzm5UOOOcwGb3u2BLL5dpMIo4f2rJavgREGAqMzdsMeI9CUxawA==");
        }
        public StoragePrivateRepository(int idCliente, string diretorioNuvem="")
        {
            this.DiretorioNuvem = diretorioNuvem;
            GetPath(idCliente);  
        }

        public void CriarDiretorio(string arquivoLocal, string arquivo)
        {
            try
            {
                StorageCredentials creds = new StorageCredentials(UserAccount, PswAccount);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

                CloudBlobClient client = account.CreateCloudBlobClient();

                CloudBlobContainer sampleContainer = client.GetContainerReference(DiretorioNuvem);
                sampleContainer.CreateIfNotExists();

                CloudBlockBlob blob = sampleContainer.GetBlockBlobReference(arquivo);
                using (Stream file = System.IO.File.OpenRead(arquivoLocal))
                {
                    blob.UploadFromStream(file);
                }
          
            }
            catch (Exception ex) { throw new Exception("Erro no método CriarDiretorio, " + ex.Message); }
        }
        public void GetPath(int idCliente)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("Get_ParametroStorage");
                _db.AddInParameter(_cmd, "@idCliente", DbType.Int16, idCliente);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                       idCliente = int.Parse(_dr["IdCliente"].ToString());
                       Path = _dr["Srv_StoragePrivate"].ToString();
                       UserAccount = _dr["user_StoragePrivate"].ToString();
                       PswAccount = _dr["pwd_Storageprivate"].ToString();
                       Nuvem = _dr["Nuvem"] == null?0 : int.Parse(_dr["Nuvem"].ToString());
                    }
                }
            }
            catch (Exception ex) { throw new Exception("Erro no método GetPath, "+ex.Message); }
        }
    }
}
