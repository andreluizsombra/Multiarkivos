using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.Core.Entities;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace MK.Easydoc.Core.Repositories
{
    public class DocumentoModeloRepository
    {
        public DocumentoModeloRepository()
        {
            
        }
        public Retorno Incluir(DocumentoModelo p)
        {

            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("Proc_Manutencao_DocumentoModelo");
                _db.AddInParameter(_cmd, "@TipoAcao", DbType.Int16, p.TipoAcao);
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, p.idServico);
                _db.AddInParameter(_cmd, "@Descricao", DbType.String, p.Descricao);
                _db.AddInParameter(_cmd, "@Rotulo", DbType.String, p.Rotulo);
                _db.AddInParameter(_cmd, "@TipificaLote", DbType.Int16, p.Tipificalote);
                _db.AddInParameter(_cmd, "@Multipagina", DbType.Int16, p.Multi_Pagina);
                _db.AddInParameter(_cmd, "@ScriptSQLTipificar", DbType.String, p.ScriptSQLTipificar);
                _db.AddInParameter(_cmd, "@ScriptSQLValidar", DbType.String, p.ScriptSQLValidar);
                _db.AddInParameter(_cmd, "@ScriptSQLConsulta", DbType.String, p.ScriptSQLConsulta);
                _db.AddInParameter(_cmd, "@ScriptSQLModulo", DbType.String, p.ScriptSQLModulo);
                _db.AddInParameter(_cmd, "@DocumentoModeloPai", DbType.String, p.DocumentoModeloPai);
                _db.AddInParameter(_cmd, "@ArquivoDados", DbType.Int16, p.ArquivoDados);
                _db.AddInParameter(_cmd, "@idCampoModelo", DbType.String, p.idCampoModelo);

                var _Ret = new Retorno();

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _Ret.CodigoRetorno = int.Parse(_dr[0].ToString());
                        _Ret.Mensagem = _dr[1].ToString();
                        _Ret.idDocumentoModelo = int.Parse(_dr["idDocumentoModelo"].ToString());
                    }
                }
                return _Ret;
            }
            catch (Exception ex) 
            { 
                throw new Exception("Erro, "+ex.Message); 
            }
        }


        public Retorno IncluirCampos(DocumentoCampoModelo p)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("Proc_Insert_DocumentoCampoModelo");
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, p.idServico);
                _db.AddInParameter(_cmd, "@idDocumentoModelo", DbType.Int16, p.idDocumentoModelo);
                _db.AddInParameter(_cmd, "@idCampoModelo", DbType.Int16, p.idCampoModelo);
                _db.AddInParameter(_cmd, "@Requerido", DbType.Int16, p.Requerido);
                _db.AddInParameter(_cmd, "@ProcSqlValidacao", DbType.String, p.ProcSqlValidacao);
                _db.AddInParameter(_cmd, "@Tabulacao", DbType.Int16, p.Tabulacao);
                _db.AddInParameter(_cmd, "@ClasseCSS", DbType.String, p.ClasseCSS);
                _db.AddInParameter(_cmd, "@Digita", DbType.Int16, p.Digita);
                _db.AddInParameter(_cmd, "@Reconhece", DbType.Int16, p.Reconhece);
                _db.AddInParameter(_cmd, "@FiltroConsulta", DbType.Int16, p.FiltroConsulta);
                _db.AddInParameter(_cmd, "@IdDocumentoModeloPai", DbType.Int16, p.IdDocumentoModeloPai);

                var _Ret = new Retorno();

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _Ret.CodigoRetorno = int.Parse(_dr[0].ToString());
                        _Ret.Mensagem = "Mensagem não implementada na Proc."; //_dr[1].ToString();
                    }
                }
                return _Ret;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro, " + ex.Message);
            }
        }
        public List<DocumentoModelo> Listar(int idServico)
        {
            try
            {
                List<DocumentoModelo> _itens = new List<DocumentoModelo>();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Proc_Get_DocumentoModelo"));

                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, idServico);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _itens.Add(new DocumentoModelo() { ID = int.Parse(_dr["idDocumentoModelo"].ToString()), Descricao = _dr["Nome"].ToString() });
                    }
                }

                return _itens;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar o script Proc_Get_DocumentoModelo, detalhes: " + ex.Message);
            }
        }

        public List<DocumentoCampoModelo> ListarDocumentoCampoModelo(int idServico, int idDocumentoModelo)
        {
            try
            {
                List<DocumentoCampoModelo> _itens = new List<DocumentoCampoModelo>();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Get_DocumentoCampoModelo"));

                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, idServico);
                _db.AddInParameter(_cmd, "@idDocumentoModelo", DbType.Int32, idDocumentoModelo);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        var d = new DocumentoCampoModelo();
                        d.idDocumentoModelo = idDocumentoModelo;
                        d.Descricao = _dr["Descricao"].ToString();
                        d.Descricao_Campo = _dr["Descricao_Campo"].ToString();
                        d.Rotulo = _dr["Rotulo"].ToString();
                        d.RotuloAbreviado = _dr["RotuloAbreviado"].ToString();
                        d.ScriptSQLConsulta = _dr["ScriptSQLConsulta"].ToString();
                        d.ScriptSQLModulo = _dr["ScriptSQLModulo"].ToString();
                        d.ScriptSQLTipificar = _dr["ScriptSQLTipificar"].ToString();
                        d.ScriptSQLValidar = _dr["ScriptSQLValidar"].ToString();
                        d.IdDocumentoModeloPai = int.Parse(_dr["DocumentoModeloPai"].ToString());
                        d.Tipificalote = (bool)_dr["Tipificalote"]==false?0:1;
                        d.Multi_Pagina = (bool)_dr["Multipagina"]==false?0:1;
                        d.ArquivoDados = int.Parse(_dr["ArquivoDados"].ToString());
                        d.Requerido = (bool)_dr["Requerido"]==false?0:1;
                        d.Digita = (bool)_dr["Digita"]==false?0:1;
                        d.Reconhece = (bool)_dr["Reconhece"]==false?0:1;
                        d.FiltroConsulta = (bool)_dr["FiltroConsulta"] == false ? 0 : 1;
                        d.ProcSqlValidacao = _dr["ProcSqlValidacao"].ToString();
                        
                        _itens.Add(d);
                    }
                }

                return _itens;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar o script Get_DocumentoCampoModelo, detalhes: " + ex.Message);
            }
        }

        public List<CamposNaoSelecionados> ListarCamposNaoSelecionados(int idServico)
        {
            try
            {
                List<CamposNaoSelecionados> _itens = new List<CamposNaoSelecionados>();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Lista_Campos_Nao_Selecionado"));

                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, idServico);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        var d = new CamposNaoSelecionados();
                        d.idCampoModelo = int.Parse(_dr["idCampoModelo"].ToString());
                        d.RotuloAbreviado = _dr["RotuloAbreviado"].ToString();
                        d.Rotulo = _dr["Rotulo"].ToString();
                        d.Descricao_Campo = _dr["Rotulo"].ToString();
                        _itens.Add(d);
                    }
                }

                return _itens;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar o script Lista_Campos_Nao_Selecionado, detalhes: " + ex.Message);
            }
        }
    }
}
