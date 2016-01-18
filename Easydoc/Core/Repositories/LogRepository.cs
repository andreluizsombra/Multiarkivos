using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.Core.Repositories.Interfaces;
using System.Data.Common;
using System.Data;

namespace MK.Easydoc.Core.Repositories
{
    public class LogRepository
    {
        private decimal idLOG { get; set; }

        public LogRepository(){
            idLOG = 0;
        }
        public void RegistrarLOG(int idCliente, int idServico, int idMovimento=0, int idUsuario=0, int idModulo=0, int idAcao=0,
                                    int idEstacao=0, int idAutorizante=0,string Localizador="")
        {
                try
                {
                    DbCommand _cmd;
                    Database _db = DbConn.CreateDB();
                    _cmd = _db.GetStoredProcCommand("LOG_InserirAuditoria");
                    _db.AddInParameter(_cmd, "@IDCLIENTE", DbType.Int16, idCliente);
                    _db.AddInParameter(_cmd, "@IDSERVICO", DbType.Int16, idServico);
                    _db.AddInParameter(_cmd, "@IDMOVIMENTO", DbType.Int16, idMovimento);
                    _db.AddInParameter(_cmd, "@IDUSUARIO", DbType.Int16, idUsuario);
                    _db.AddInParameter(_cmd, "@IDMODULO", DbType.Int16, idModulo);
                    _db.AddInParameter(_cmd, "@IDACAO", DbType.Int16, idAcao);
                    _db.AddInParameter(_cmd, "@IDESTACAO", DbType.Int16, idEstacao);
                    _db.AddInParameter(_cmd, "@IDAUTORIZANTE", DbType.Int16, idAutorizante);
                    _db.AddInParameter(_cmd, "@LOCALIZADOR", DbType.String, Localizador);

                    //decimal _idLOG = 0;

                    using (IDataReader _dr = _db.ExecuteReader(_cmd))
                    {
                        while (_dr.Read())
                        {
                          idLOG = decimal.Parse(_dr[0].ToString());
                          if(idLOG < 0){
                              throw new Exception("Erro, "+_dr[1].ToString());
                          }
                          //_Ret.Mensagem = _dr[1].ToString();
                        }
                    }
                    //return idLOG;
                }
                catch (Exception ex) { throw ex; }
        }

        public Retorno RegistrarLOGDetalhe(string detalhe, string conteudo)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("LOG_InserirAuditoriaDetalhe");
                _db.AddInParameter(_cmd, "@IdLog", DbType.String, idLOG);
                _db.AddInParameter(_cmd, "@Detalhe", DbType.String, detalhe);
                _db.AddInParameter(_cmd, "@Conteudo", DbType.String, conteudo);

                var _Ret = new Retorno();
                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _Ret.CodigoRetorno = int.Parse(_dr[0].ToString());
                        _Ret.Mensagem = _dr[1].ToString();
                        if (_Ret.CodigoRetorno < 0)
                        {
                            throw new Exception("Erro, " + _Ret.Mensagem);
                        }
                    }
                }
                return _Ret;
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
