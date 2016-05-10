using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using TecFort.Framework.Generico;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.Core.Repositories.Interfaces;
using MK.Easydoc.Core.Repositories;
using MK.Easydoc.Core.Infrastructure.Framework;
using System.Management;
using System.Net.NetworkInformation;
using System.Net;


namespace MK.Easydoc.Core.Repositories
{
    public class LicensaRepository
    {
        public Retorno Incluir(Licensa acs)
        {
            var _Ret = new Retorno();
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("Proc_Manutencao_Licenca");
                _db.AddInParameter(_cmd, "@TipoAcao", DbType.Int16, acs.TipoAcao); //(1)Criar, (2)Alterar, (3)Excluir
                _db.AddInParameter(_cmd, "@idCliente", DbType.Int16, acs.idCliente);
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, acs.idServico);
                _db.AddInParameter(_cmd, "@idUsuario", DbType.Int16, acs.idUsuario);

                _db.AddInParameter(_cmd, "@IPServerHost", DbType.String, acs.IPServerHost);
                _db.AddInParameter(_cmd, "@HostnameServer", DbType.String, acs.HostnameServer);
                _db.AddInParameter(_cmd, "@IPPublicClient", DbType.String, acs.IPPublicClient);
                _db.AddInParameter(_cmd, "@IPPrivateClient", DbType.String, acs.IPPrivateClient);
                _db.AddInParameter(_cmd, "@HostnameClient", DbType.String, acs.HostnameClient);

                

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _Ret.CodigoRetorno = int.Parse(_dr[0].ToString());
                        _Ret.Mensagem = _dr[1].ToString();
                    }
                }
                return _Ret;
            }
            catch (Exception ex)
            {
                _Ret.CodigoRetorno = -2;
                _Ret.Mensagem = ex.Message;
                return _Ret;
                //throw new Exception(ex.Message);
            }
        }

        public Retorno CarregaLicensa(Usuario usu, int tipoAcao=1)
        {
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string ip = context.Request.ServerVariables["REMOTE_ADDR"];
                string HostName = System.Net.Dns.GetHostName();
                string local_ip = context.Request.ServerVariables["LOCAL_ADDR"];

                var lic = new Licensa()
                {
                    TipoAcao = tipoAcao,
                    idUsuario = usu.ID,
                    idServico = usu.ServicoID,
                    idCliente = usu.ClienteID,
                    IPPublicClient = ip,
                    IPServerHost = local_ip,
                    HostnameServer = HostName
                };
                var ret = Incluir(lic);
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro CarregaLicensa, " + ex.Message);
            }
        }
    }
}
