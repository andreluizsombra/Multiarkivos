using Microsoft.Practices.EnterpriseLibrary.Data;
using Newtonsoft.Json;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;




using TecFort.Framework.Generico;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.Core.Repositories.Interfaces;

namespace MK.Easydoc.Core.Repositories
{
    public class DocumentoRepository : IDocumentoRepository 
    {
        #region Constructors
        public DocumentoRepository() { }
        #endregion Constructor

        #region IServicoRepository Members
        public DocumentoModelo CriarDocumento(IDictionary<string, object> _queryParams)
        { 
            DocumentoModelo _Documento = new DocumentoModelo();
            _Documento = (new DocumentoModelo { ID = 1 });
            return _Documento;
        }

        //public DocumentoModelo AtualizarDocumento(IDictionary<string, object> _queryParams)
        //{
        //    DocumentoModelo _Documento = new DocumentoModelo();
        //    return _Documento;
        //}
        public string ValidarCamposDocumento(IDictionary<string, object> _queryParams) {
            string _mensagem = string.Empty;
            try
            {
                //Lote _lote = new Lote();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                

                CampoModelo _campo = (CampoModelo)_queryParams["DocumentoCampo"];
                //_cmd = _db.GetStoredProcCommand(String.Format("proc_valida_doc01_serv01"));
                _cmd = _db.GetStoredProcCommand(_campo.ProcSqlValidacao);

                _db.AddInParameter(_cmd, "@IdDocumentoCampo", DbType.Int32, _campo.IndexUI);

                if (!string.IsNullOrEmpty(_campo.Valor))
                    _db.AddInParameter(_cmd, "@Valor", DbType.String, _campo.Valor);

                _mensagem = _db.ExecuteScalar(_cmd).ToString();

            }
            catch (Exception ex)
            {
                _mensagem = ex.Message;
            }
            return _mensagem;
        }

        public string ValidarDocumento(IDictionary<string, object> _queryParams)
        {
            string _mensagem = string.Empty;
            try
            {
                //Lote _lote = new Lote();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();

                string ProcedureSQL = _queryParams["ProcSqlValidacao"].ToString();
                int Documento_ID = int.Parse(_queryParams["Documento_ID"].ToString());
                
                
                //int DocumentoModelo_ID = int.Parse(_queryParams["DocumentoModelo_ID"].ToString());

                _cmd = _db.GetStoredProcCommand(ProcedureSQL);
                _db.AddInParameter(_cmd, "@idDocumento", DbType.Int32, Documento_ID);
                //_db.AddInParameter(_cmd, "@idDocumentoModelo", DbType.Int32, DocumentoModelo_ID);
                //_db.AddInParameter(_cmd, "@idDocumento", DbType.Int32, Documento_ID);
                _mensagem = _db.ExecuteScalar(_cmd).ToString();


            }
            catch (Exception ex)
            {
                _mensagem = ex.Message;
            }
            return _mensagem;
        }

        
        
        public void AtualizarDocumento(IDictionary<string, object> _queryParams)
        {
            try
            {
                //Lote _lote = new Lote();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();


                Documento _documento = (Documento)_queryParams["Documento"];
                
                _cmd = _db.GetStoredProcCommand(String.Format("proc_documento_alt"));

                _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, _documento.ID);

                if (_documento.StatusDocumento != 0)
                    _db.AddInParameter(_cmd, "@StatusDocumento", DbType.Int32, _documento.StatusDocumento);

                int iret = _db.ExecuteNonQuery(_cmd);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //walmir
        //IncluirMotivo(int IdDocumento,int Atalho,int UserID)
        public void IncluirMotivo(IDictionary<string, object> _queryParams)
        {
            //try
            //{
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                
                _cmd = _db.GetStoredProcCommand(String.Format("proc_DocumentoMotivo_get"));
                _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["IdDocumento"].ToString()));
                _db.AddInParameter(_cmd, "@Atalho", DbType.Int32, int.Parse(_queryParams["Atalho"].ToString()));
                _db.AddInParameter(_cmd, "@UserID", DbType.Int32, int.Parse(_queryParams["UserID"].ToString()));
                _db.AddInParameter(_cmd, "@tipo", DbType.Int32, int.Parse(_queryParams["tipo"].ToString()));    

                int iret = _db.ExecuteNonQuery(_cmd);

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
        public void AlteraDuplicidade(IDictionary<string, object> _queryParams)
        {
            //try
            //{
            DbCommand _cmd;
            Database _db = DbConn.CreateDB();

            _cmd = _db.GetStoredProcCommand(String.Format("proc_AlteraDuplicidade"));
            _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["IdDocumento"].ToString()));
            _db.AddInParameter(_cmd, "@ID", DbType.Int32, int.Parse(_queryParams["Atalho"].ToString()));
            
            int iret = _db.ExecuteNonQuery(_cmd);

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }



        public void ExcluirDocumento(IDictionary<string, object> _queryParams)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();

                _cmd = _db.GetStoredProcCommand(String.Format("proc_documento_del"));
                _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["Documento_ID"].ToString()));
                int iret = _db.ExecuteNonQuery(_cmd);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void MudaStatusDocumento(IDictionary<string, object> _queryParams)
        {
            try
            {              
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_MudaStatusDocumento"));
                _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["Documento_ID"].ToString()));
                _db.AddInParameter(_cmd, "@Idusuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@Idstatus", DbType.Int32, int.Parse(_queryParams["Status_ID"].ToString()));
                int iret = _db.ExecuteNonQuery(_cmd);
            }
            catch (Exception ex)
            {
                throw;
            }
        }







        public void AtualizarDocumentoCampo(IDictionary<string, object> _queryParams)
        {
            try
            {
                //Lote _lote = new Lote();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();

                CampoModelo _campo = (CampoModelo)_queryParams["DocumentoCampo"];
                
                
                _cmd = _db.GetStoredProcCommand(String.Format("proc_documento_campo_alt"));

                _db.AddInParameter(_cmd, "@IdDocumentoCampo", DbType.Int32, _campo.IndexUI);

                if (!string.IsNullOrEmpty(_campo.Valor))
                    _db.AddInParameter(_cmd, "@Valor", DbType.String, _campo.Valor);

                //if (int.Parse(_queryParams["NumPagina"].ToString()) != 0)
                //    _db.AddInParameter(_cmd, "@numPagina", DbType.Int32, _campo.);

                int iret = _db.ExecuteNonQuery(_cmd);
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<Documento> ListarDocumentosStatus(IDictionary<string, object> _queryParams)
        {
            DocumentoModelo _modelo = new DocumentoModelo();
            Documento _documento = new Documento();
            List<Documento> _documentos = new List<Documento>();

            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_documento_servico_usuario_sel"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                //_db.AddInParameter(_cmd, "@IdStatus", DbType.Int32, int.Parse(_queryParams["Status_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _documento = new Documento();
                        _documento.Modelo = ConverteBaseDocumento(_dr);
                        _documento.ID = int.Parse(_dr["IdDocumento"].ToString());
                        _documento.StatusDocumento = int.Parse(_dr["idStatus"].ToString());

                        _documento.Modelo.Campos = new List<CampoModelo>();

                        _documentos.Add(_documento);
                        //TODO: Criar Proc para retornar os campos e metodo para carregar esta propriedade (Ja retornar o doctocampos e o modelocapo)
                    }
                }

                if (_documento == null) { throw new Erro("Documento não localizado."); }

                return _documentos;
            }
            catch (Exception ex)
            {
                throw;
            }
        
        }
        public Documento SelecionarDocumento(IDictionary<string, object> _queryParams)
        {
            DocumentoModelo _modelo = new DocumentoModelo();
            Documento _documento = new Documento();
            List<DocumentoModelo> _documentos = new List<DocumentoModelo>();

            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_documento_servico_usuario_get"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["Documento_ID"].ToString()));
                //_db.AddInParameter(_cmd, "@IdStatus", DbType.Int32, int.Parse(_queryParams["Status_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _documento.Modelo = ConverteBaseDocumento(_dr);
                        _documento.ID = int.Parse(_dr["IdDocumento"].ToString());

                        _documento.Modelo.Campos = new List<CampoModelo>();
                    
                        //TODO: Criar Proc para retornar os campos e metodo para carregar esta propriedade (Ja retornar o doctocampos e o modelocapo)
                    }
                }


                
                if (_documento == null) { throw new Erro("Documento não localizado."); }


                return _documento;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<CampoModelo> ListarCamposModelo(IDictionary<string, object> _queryParams)
        {
            List<CampoModelo> _campos = new List<CampoModelo>();
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_campo_documento_sel"));

                _db.AddInParameter(_cmd, "@IdDocumentoModelo", DbType.Int32, int.Parse(_queryParams["DocumentoModelo_ID"].ToString()));
                //_db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["Documento_ID"].ToString()));
                
                
                //_db.AddInParameter(_cmd, "@IdStatus", DbType.Int32, int.Parse(_queryParams["Status_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _campos.Add(ConverteBaseCampo(_dr));

                    }
                }

                //if (_campos == null) { throw new Erro("Documento não localizado."); }

                return _campos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }                
        public List<CampoModelo> SelecionarDocumentoCampos(IDictionary<string, object> _queryParams)
        {
            List<CampoModelo> _campos = new List<CampoModelo>();
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_documento_campos_servico_sel"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["Documento_ID"].ToString()));
                //_db.AddInParameter(_cmd, "@IdStatus", DbType.Int32, int.Parse(_queryParams["Status_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _campos.Add(ConverteBaseCampo(_dr));
                        
                    }
                }

                //if (_campos == null) { throw new Erro("Documento não localizado."); }

                return _campos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<DocumentoImagem> SelecionarDocumentoImagens(IDictionary<string, object> _queryParams)
        {
            List<DocumentoImagem> _imagens = new List<DocumentoImagem>();
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_documento_imagens_servico_sel"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["Documento_ID"].ToString()));
                //_db.AddInParameter(_cmd, "@IdStatus", DbType.Int32, int.Parse(_queryParams["Status_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _imagens.Add(new DocumentoImagem { ID = int.Parse(_dr["IdDocumentoImagem"].ToString()), CaminhoArquivo = _dr["CaminhoArquivo"].ToString(), NumeroPagina = int.Parse(_dr["Pagina"].ToString()), Verso = bool.Parse(_dr["Verso"].ToString()) });

                    }
                }

                //if (_imagens == null) { throw new Erro("Documento não localizado."); }

                return _imagens;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<DocumentoModelo> ListarTipos(IDictionary<string, object> _queryParams)
        {
            try
            {
                DocumentoModelo _documento = new DocumentoModelo();
                List<DocumentoModelo> _documentos = new List<DocumentoModelo>();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_documentomodelo_servico_sel"));

                //_db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));

                //_db.AddInParameter(_cmd, "@IdStatus", DbType.Int32, int.Parse(_queryParams["Status_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _documentos.Add(ConverteBaseDocumento(_dr));
                    }
                }
                if (_documentos == null) { throw new Erro("Documento não localizado."); }
                return _documentos;//.Where(d => d.IdLote == int.Parse(_queryParams["Lote_ID"].ToString())).ToList<LoteItem>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private DocumentoModelo ConverteBaseDocumento(IDataReader dt)
        {
            DocumentoModelo _documento = new DocumentoModelo();
            _documento = (new DocumentoModelo
            {
                ID = int.Parse(dt["IdDocumentoModelo"].ToString()),
                Descricao = dt["Descricao"].ToString(),
                Rotulo = dt["Rotulo"].ToString(),
                //Duplex = bool.Parse(dt["Duplex"].ToString()),
                MultiPagina = bool.Parse(dt["MultiPagina"].ToString()),
                ScriptSQLTipificar = dt["ScriptSQLTipificar"].ToString(),
                ScriptSQLValidar = dt["ScriptSQLValidar"].ToString(),
                ScriptSQLConsultar = dt["ScriptSQLConsultar"].ToString(),
                DataCriacao = DateTime.Parse(dt["DataCriacao"].ToString()),
                Servico = (new Servico { ID = int.Parse(dt["IdServico"].ToString()) }),

            });
            return _documento;
        }
        private CampoModelo ConverteBaseCampo(IDataReader dt)
        {
            CampoModelo _campo = new CampoModelo();
            _campo = (new CampoModelo
            {
                ID = int.Parse(dt["idCampoModelo"].ToString()),
                IndexDoc = int.Parse(dt["IndexDoc"].ToString()),
                IndexUI = dt["IndexUI"].ToString(),
                Descricao = dt["Descricao"].ToString(),
                Rotulo = dt["Rotulo"].ToString(),
                RotuloAbreviado = dt["RotuloAbreviado"].ToString(),
                MaxLength = int.Parse(dt["MaxLength"].ToString()),
                MinLength = int.Parse(dt["MinLength"].ToString()),
                AtributosHTML = dt["AtributosHTML"].ToString(),
                ClasseCSS = dt["ClasseCSS"].ToString(),
                ControleDesk = dt["ControleDesk"].ToString(),
                ControleWEB = dt["ControleWEB"].ToString(),
                MascaraEntrada = dt["MascaraEntrada"].ToString(),
                MascaraSaida = dt["MascaraSaida"].ToString(),
                Tabulacao = int.Parse(dt["Tabulacao"].ToString()),
                MetodoValidacao = dt["MetodoValidacao"].ToString(),
                ProcSqlValidacao = dt["ProcSqlValidacao"].ToString(),
                RegexString = dt["RegexString"].ToString(),
                Requerido =  bool.Parse(dt["Requerido"].ToString()),
                Digita =  bool.Parse(dt["Digita"].ToString()),
                Reconhece =  bool.Parse(dt["Reconhece"].ToString()),
                FiltroConsulta =  bool.Parse(dt["FiltroConsulta"].ToString()),
                Valor = dt["Valor"].ToString() ?? string.Empty,
                ValorPadrao = dt["ValorPadrao"].ToString()

            });
            return _campo;
        }
        public string SalvarConsultaDocumentoModelo(IDictionary<string, object> _queryParams)
        {
            
            string _retorno = string.Empty;

            try
            {

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();

                _cmd = _db.GetStoredProcCommand(String.Format("proc_consultamodelo_ins"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                //_db.AddInParameter(_cmd, "@idDocumentoModelo", DbType.String, int.Parse(_queryParams["Documento_Modelo_ID"].ToString()));
                _db.AddInParameter(_cmd, "@Descricao", DbType.String, _queryParams["Descricao"].ToString());
                _db.AddInParameter(_cmd, "@NomeConsulta", DbType.String, _queryParams["Nome_Consulta"].ToString());
                _db.AddInParameter(_cmd, "@Compartilhado", DbType.Boolean, bool.Parse(_queryParams["Compartilhado"].ToString()));
                _db.AddInParameter(_cmd, "@Consulta", DbType.String, _queryParams["String_JSON"].ToString());

                int iret = _db.ExecuteNonQuery(_cmd);
            }
            catch (Exception ex) { _retorno = ex.Message; throw ex; }
            _retorno= "Salvo com seucesso!";
            return _retorno;
            
        }
        public string PesquisarDocumentos (IDictionary<string, object> _queryParams)
        {
            try
            {

                string scriptConsulta = _queryParams["ScriptSQLConsultar"].ToString();

                List<LoteItem> _itens = new List<LoteItem>();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format(scriptConsulta));

                _db.AddInParameter(_cmd, "@IdDocumentoModelo", DbType.Int32, int.Parse(_queryParams["DocumentoModelo_ID"].ToString()));
                _db.AddInParameter(_cmd, "@Select", DbType.String, _queryParams["CamposSQL"].ToString().Trim());
                _db.AddInParameter(_cmd, "@Where", DbType.String, _queryParams["Script_WHERE"].ToString().Trim());
                
                string _json = string.Empty;

                //SqlDataReader reader = _cmd.ExecuteReader();  //_db.ExecuteReader(_cmd);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {

                    _json= WriteDataReader(_dr);
                    
                }

                return _json;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string PesquisarMotivo(IDictionary<string, object> _queryParams)
        {
            string _mensagem = string.Empty;
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();

                int Documento_ID = int.Parse(_queryParams["idDocumento"].ToString());
                _cmd = _db.GetStoredProcCommand("proc_DocumentoMotivo_view");
                _db.AddInParameter(_cmd, "@idDocumento", DbType.Int32, Documento_ID);
                _mensagem = _db.ExecuteScalar(_cmd).ToString();

                //using (IDataReader _dr = _db.ExecuteReader(_cmd))
                //{
                //    while (_dr.Read())
                //    {
                //        _mensagem = _dr[0].ToString();
                //        //_consultas.Add(new DocumentoConsulta { ID = int.Parse(_dr["IdConsultaModelo"].ToString()), Descricao = _dr["Descricao"].ToString(), Rotulo = _dr["NomeConsulta"].ToString(), ModeloJSON = _dr["Consulta"].ToString() });
                //        //_campos.Add(ConverteBaseCampo(_dr));
                //    }
                //}



            }
            catch (Exception ex)
            {
                _mensagem = ex.Message;
            }
            return _mensagem;
        }
        //public string PesquisarDet(IDictionary<string, object> _queryParams)
        //{
        //    string _mensagem = string.Empty;
        //    try
        //    {
        //        DbCommand _cmd;
        //        Database _db = DbConn.CreateDB();

        //        int Documento_ID = int.Parse(_queryParams["idDocumento"].ToString());
        //        _cmd = _db.GetStoredProcCommand("proc_PesquisarDet");
        //        _db.AddInParameter(_cmd, "@idDocumento", DbType.Int32, Documento_ID);
        //        _mensagem = _db.ExecuteScalar(_cmd).ToString();

        //        //using (IDataReader _dr = _db.ExecuteReader(_cmd))
        //        //{
        //        //    while (_dr.Read())
        //        //    {
        //        //        _mensagem = _dr[0].ToString();
        //        //        //_consultas.Add(new DocumentoConsulta { ID = int.Parse(_dr["IdConsultaModelo"].ToString()), Descricao = _dr["Descricao"].ToString(), Rotulo = _dr["NomeConsulta"].ToString(), ModeloJSON = _dr["Consulta"].ToString() });
        //        //        //_campos.Add(ConverteBaseCampo(_dr));
        //        //    }
        //        //}



        //    }
        //    catch (Exception ex)
        //    {
        //        _mensagem = ex.Message;
        //    }
        //    return _mensagem;
        //}

        public string AtualiarDocumentoCB (IDictionary<string, object> _queryParams)
        {
            string _retorno = string.Empty;
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_documentocb_alt"));
                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdLote", DbType.Int32, int.Parse(_queryParams["Lote_ID"].ToString()));
                _db.AddInParameter(_cmd, "@Verso", DbType.Boolean, bool.Parse(_queryParams["Verso"].ToString()));
                _db.AddInParameter(_cmd, "@CB", DbType.String, _queryParams["CB"].ToString());
                _db.AddInParameter(_cmd, "@NomeImagemFim", DbType.String, _queryParams["NomeImagemFim"].ToString());
                int iret = _db.ExecuteNonQuery(_cmd);
            }
            catch (Exception ex) { _retorno = ex.Message; throw ex; }
            _retorno = "Salvo com seucesso!";
            return _retorno;
        }

        public List<DocumentoConsulta> ListarConsultasModelo(IDictionary<string, object> _queryParams)
        {
            List<DocumentoConsulta> _consultas = new List<DocumentoConsulta>();

            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_consulta_modelo_usuario_sel"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                //_db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["Documento_ID"].ToString()));


                //_db.AddInParameter(_cmd, "@IdStatus", DbType.Int32, int.Parse(_queryParams["Status_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _consultas.Add(new DocumentoConsulta { ID = int.Parse(_dr["IdConsultaModelo"].ToString()), Descricao = _dr["Descricao"].ToString(), Rotulo = _dr["NomeConsulta"].ToString(), ModeloJSON = _dr["Consulta"].ToString() });
                        //_campos.Add(ConverteBaseCampo(_dr));
                    }
                }

                //if (_campos == null) { throw new Erro("Documento não localizado."); }

                return _consultas;
            }
            catch (Exception ex)
            {
                throw;
            }
        }  

        //Walmir
        public List<Motivo> SelecionarMotivo(IDictionary<string, object> _queryParams)
        {
            List<Motivo> _Motivo = new List<Motivo>();
            //try
            //{
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_Motivo_get"));
                _db.AddInParameter(_cmd, "@IdDocumentoModelo", DbType.Int32, int.Parse(_queryParams["iddocumentomodelo"].ToString()));
                _db.AddInParameter(_cmd, "@Tipo", DbType.Int32, int.Parse(_queryParams["tipo"].ToString()));                
                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _Motivo.Add(new Motivo { ID = int.Parse(_dr["idMotivo"].ToString()), 
                                                  descricao = _dr["descricao"].ToString(),
                                                 atalho = int.Parse(_dr["Atalho"].ToString())});
                        
                    }
                }          
                return _Motivo;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
        public string GetDuplicidade(IDictionary<string, object> _queryParams)
        {
            string _ret="";
            //try
            //{
            DbCommand _cmd;
            Database _db = DbConn.CreateDB();
            _cmd = _db.GetStoredProcCommand(String.Format("proc_GetDuplicidade"));
            _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["iddocumento"].ToString()));            
            using (IDataReader _dr = _db.ExecuteReader(_cmd))
            {
                while (_dr.Read())
                {
                    _ret = _dr[0].ToString();
                }
            }
            return _ret;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
        public bool EmUso(IDictionary<string, object> _queryParams)
        {
            bool _ret=false;
            string _ret1 = "";
            //try
            //{
            DbCommand _cmd;
            Database _db = DbConn.CreateDB();
            _cmd = _db.GetStoredProcCommand(String.Format("proc_EmUso"));
            _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["idDocumento"].ToString()));
            _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["idUsuario"].ToString()));
            _db.AddInParameter(_cmd, "@Tipo", DbType.Int32, int.Parse(_queryParams["Tipo"].ToString()));
            using (IDataReader _dr = _db.ExecuteReader(_cmd))
            {
                while (_dr.Read())
                {
                    _ret1 = _dr[0].ToString();

                    if (_ret1 == "0") { _ret = false; } else { _ret = true; }

                    //_ret = bool.Parse(_dr[0].ToString());
                }
            }
            return _ret;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }






        public string GetMotivo(IDictionary<string, object> _queryParams)
        {
            string _Motivo = "";
            //try
            //{
            DbCommand _cmd;
            Database _db = DbConn.CreateDB();
            _cmd = _db.GetStoredProcCommand(String.Format("proc_DocumentoMotivo_viewId"));
            _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["iddocumento"].ToString()));            
            using (IDataReader _dr = _db.ExecuteReader(_cmd))
            {
                while (_dr.Read())
                {
                    _Motivo = _dr[0].ToString();
                }
            }
            return _Motivo;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
        public string GetStatusDocumento(IDictionary<string, object> _queryParams)
        {
            string _Ret = "";
            //try
            //{
            DbCommand _cmd;
            Database _db = DbConn.CreateDB();
            _cmd = _db.GetStoredProcCommand(String.Format("proc_GetStatusDocumento"));
            _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["iddocumento"].ToString()));
            using (IDataReader _dr = _db.ExecuteReader(_cmd))
            {
                while (_dr.Read())
                {
                    _Ret = _dr[0].ToString();
                }
            }
            return _Ret;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
        public string GetDocumentoModelo(IDictionary<string, object> _queryParams)
        {
            string _Ret = "";
            //try
            //{
            DbCommand _cmd;
            Database _db = DbConn.CreateDB();
            _cmd = _db.GetStoredProcCommand(String.Format("proc_GetDocumentoModelo"));
            _db.AddInParameter(_cmd, "@IdDocumento", DbType.Int32, int.Parse(_queryParams["iddocumento"].ToString()));
            using (IDataReader _dr = _db.ExecuteReader(_cmd))
            {
                while (_dr.Read())
                {
                    _Ret = _dr[0].ToString();
                }
            }
            return _Ret;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
        public string Executar(IDictionary<string, object> _queryParams)
        {

            string _retorno = string.Empty;

            try
            {

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                //_cmd = _db.GetStoredProcCommand(_queryParams["cmd"].ToString());
                _cmd = _db.GetSqlStringCommand(_queryParams["cmd"].ToString());
                int iret = _db.ExecuteNonQuery(_cmd);
            }
            catch (Exception ex) { _retorno = ex.Message; throw ex; }
            
            _retorno = "";
            return _retorno;

        }





        //
        #endregion IServicoRepository Members

        #region MetodosPrivados
        private string WriteDataReader(IDataReader reader)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartArray();
                while (reader.Read())
                {
                    jsonWriter.WriteStartObject();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        jsonWriter.WritePropertyName(reader.GetName(i));
                        jsonWriter.WriteValue(reader[i]);
                    }
                    jsonWriter.WriteEndObject();
                }
                jsonWriter.WriteEndArray();
            }

            //string json = JsonConvert.SerializeObject(sb.ToString(), Newtonsoft.Json.Formatting.Indented);


            return sb.ToString();
        }
        private IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                        SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
        #endregion MetodosPrivados
    }

}
