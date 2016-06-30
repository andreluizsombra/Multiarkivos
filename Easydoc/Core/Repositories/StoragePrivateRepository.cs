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
    public class StoragePrivateRepository : IStoragePrivate
    {
        public int idCliente { get; set; }
        public string Path { get; protected set; }
        public string UserAccount { get; protected set; }
        public string PswAccount { get; protected set; }
        public string DiretorioLocal { get; protected set; }
        public string DiretorioNuvem { get; protected set; }
        public int Nuvem { get; protected set; }
        protected CloudBlockBlob Blob { get; set; }
        //private readonly CloudStorageAccount _storageAccount;
        protected StorageCredentials mycreds { get; set; }
        protected CloudStorageAccount myaccount { get; set; }
        protected CloudBlobClient myclient { get; set; }
        protected CloudBlobContainer mysampleContainer { get; set; }
        public List<ArquivosAzure> ListaArquivos { get; set; }
        public StoragePrivateRepository()
        {
            Path = "";
            DiretorioNuvem = "storageprivate";
            //_storageAccount = CloudStorageAccount.Parse("ixiPNQ0HgCcZn+2JEY51gMKjMOPA7BxEHqMxzm5UOOOcwGb3u2BLL5dpMIo4f2rJavgREGAqMzdsMeI9CUxawA==");
        }
        public StoragePrivateRepository(int idCliente)
        {
            this.DiretorioNuvem = "storageprivate";
            GetPath(idCliente);
            ListaArquivos = new List<ArquivosAzure>();
            mycreds = new StorageCredentials(UserAccount, PswAccount);
            myaccount = new CloudStorageAccount(mycreds, useHttps: true);
            myclient = myaccount.CreateCloudBlobClient();
            mysampleContainer = myclient.GetContainerReference(DiretorioNuvem);
            mysampleContainer.CreateIfNotExists();
        }

        public void UploadAzure(string arquivoLocal, string arquivo)
        {
            try
            {
                //StorageCredentials creds = new StorageCredentials(UserAccount, PswAccount);
                //CloudStorageAccount account = new CloudStorageAccount(mycreds, useHttps: true);
                //CloudBlobClient client = account.CreateCloudBlobClient();
                //CloudBlobContainer sampleContainer = client.GetContainerReference(DiretorioNuvem);
                //mysampleContainer.CreateIfNotExists();

                CloudBlockBlob blob = mysampleContainer.GetBlockBlobReference(arquivo);
                using (Stream file = System.IO.File.OpenRead(arquivoLocal))
                {
                    blob.UploadFromStream(file);
                }

            }
            catch (Exception ex) { throw new Exception("Erro no método CriarDiretorio, " + ex.Message); }
        }
        public void ListarArqsAzure()
        {
            try{
                var bl = mysampleContainer.GetBlobReference("132_201663055288.jpg");
                CloudBlobDirectory docs = mysampleContainer.GetDirectoryReference("storageprivate");

 
                foreach (IListBlobItem item in docs.ListBlobs())
                {
                    ListaArquivos.Add(new ArquivosAzure() { Arquivo = item.Uri.ToString() });
                }

                foreach (IListBlobItem item in mysampleContainer.ListBlobs(null, false))
{
    if (item.GetType() == typeof(CloudBlockBlob))
    {
        CloudBlockBlob blob = (CloudBlockBlob)item;

        Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

    }
    else if (item.GetType() == typeof(CloudPageBlob))
    {
        CloudPageBlob pageBlob = (CloudPageBlob)item;

        Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

    }
    else if (item.GetType() == typeof(CloudBlobDirectory))
    {
        CloudBlobDirectory directory = (CloudBlobDirectory)item;

        Console.WriteLine("Directory: {0}", directory.Uri);
    }
}


                foreach (IListBlobItem item in mysampleContainer.ListBlobs())
                {
                    if (item.GetType() == typeof(CloudBlobDirectory))
                    {
                        CloudBlobDirectory directory = (CloudBlobDirectory)item;
                        string[] uri = directory.Uri.ToString().Split('/');
                       
                    }
                }

                CloudBlobDirectory dir = mysampleContainer.GetDirectoryReference(DiretorioNuvem);
                var files = dir.ListBlobs(true);
                foreach(IListBlobItem item in files)
                {
                    ListaArquivos.Add(new ArquivosAzure() { Arquivo = item.Uri.ToString()});
                }
                foreach (IListBlobItem item in files)
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    blob.FetchAttributes();
                    
                    
                        string FileUrl = item.Uri.ToString();
                        var FileName = item.Uri;
                        var ImageName = blob.Metadata["Name"];
                    
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
                        Nuvem = _dr["Nuvem"] == null ? 0 : int.Parse(_dr["Nuvem"].ToString());
                    }
                }
            }
            catch (Exception ex) { throw new Exception("Erro no método GetPath, " + ex.Message); }
        }
    }
    public class ArquivosAzure
    {
        public ArquivosAzure()
        {
            Arquivo = "";
        }
        public string Arquivo { get; set; }
    }
}
