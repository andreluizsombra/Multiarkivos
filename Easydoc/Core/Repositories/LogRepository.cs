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
        protected decimal idLOG { get; set; }
        protected decimal idServico { get; set; }

        public LogRepository(){
            idLOG = 0;
            idServico = 0;
        }


        public void RegistrarLOG(int idCliente, int _idServico, int idMovimento=0, int idUsuario=0, int idModulo=0, int idAcao=0,
                                    int idEstacao=0, int idAutorizante=0,string Localizador="")
        {
                try
                {
                    
                    this.idServico = _idServico;

                    DbCommand _cmd;
                    Database _db = DbConn.CreateDB();
                    _cmd = _db.GetStoredProcCommand("LOG_InserirAuditoria");
                    _db.AddInParameter(_cmd, "@IDCLIENTE", DbType.Int16, idCliente);
                    _db.AddInParameter(_cmd, "@IDSERVICO", DbType.Int16, this.idServico);
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

        public Retorno RegistrarLOGDetalhe(int idAcao, string conteudo)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("LOG_InserirAuditoriaDetalhe");
                _db.AddInParameter(_cmd, "@IdLog", DbType.Int32, this.idLOG);
                _db.AddInParameter(_cmd, "@IdAcao", DbType.Int32, idAcao);
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, this.idServico);
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

        public List<Log> ConsultaLOG(int idServico, int idAcao = 0, string Localizador = "")
        {
            var lstLOG = new List<Log>();
            var _Ret = new Retorno();
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("GET_LocalizarAuditoria");
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, idServico);
                _db.AddInParameter(_cmd, "@idAcao", DbType.Int16, idAcao);
                _db.AddInParameter(_cmd, "@Localizador", DbType.String, Localizador);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {

                        if (_dr.FieldCount==2 && int.Parse(_dr[0].ToString())<0)
                        {
                            throw new Exception("Erro, " + _dr[1].ToString());
                        }
                        else
                        {
                            lstLOG.Add(new Log()
                            {
                                IDLOG = decimal.Parse(_dr["IDLOG"].ToString()),
                                Cliente = _dr["Cliente"].ToString(),
                                Servico = _dr["Serviço"].ToString(),
                                Acao = _dr["Ação"].ToString(),
                                Localizador = _dr["Localizador"].ToString(),
                                DataHora = DateTime.Parse(_dr["Data"].ToString())
                            });
                        }
                        
                    }
                }
                
                return lstLOG;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<LogDetalhe> ConsultaLOG_Detalhe(int idLOG)
        {
            var lstLOG_Detalhe = new List<LogDetalhe>();
            var _Ret = new Retorno();
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("GET_LocalizarAuditoriaDetalhe");
                _db.AddInParameter(_cmd, "@idLog", DbType.Int16, idLOG);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {

                        if (_dr.FieldCount == 2 && int.Parse(_dr[0].ToString()) < 0)
                        {
                            throw new Exception("Erro, " + _dr[1].ToString());
                        }
                        else
                        {
                            lstLOG_Detalhe.Add(new LogDetalhe()
                            {
                                Data = DateTime.Parse(_dr["Data do evento"].ToString()).ToString("dd/MM/yyyy hh:mm:ss"),
                                Detalhe = _dr["Descrição"].ToString(),
                                Conteudo = _dr["Localizador"].ToString(),
                            });
                        }

                    }
                }

                return lstLOG_Detalhe;
            }
            catch (Exception ex) { throw ex; }
        }



    }
}
