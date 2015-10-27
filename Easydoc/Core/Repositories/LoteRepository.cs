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

namespace MK.Easydoc.Core.Repositories
{
    public class LoteRepository : ILoteRepository
    {
        #region Constructors
        public LoteRepository() { }
        #endregion Constructors
        
        #region ILoteRepository Members
        public Lote CriarLote(IDictionary<string, object> _queryParams)
        {

            try
            {
                Lote _lote = new Lote();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_lote_ins"));

                _db.AddInParameter(_cmd, "@QtdeImagem", DbType.Int32, int.Parse(_queryParams["QtdeImagem"].ToString()));
                _db.AddInParameter(_cmd, "@Duplex", DbType.Boolean, Boolean.Parse(_queryParams["Duplex"].ToString()));
                _db.AddInParameter(_cmd, "@PathCaptura", DbType.String, _queryParams["PathCaptura"].ToString());
                _db.AddInParameter(_cmd, "@IdUsuarioCaptura", DbType.Int32, int.Parse(_queryParams["IdUsuarioCaptura"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["IdServico"].ToString()));
                _db.AddInParameter(_cmd, "@IdStatus", DbType.Int32, int.Parse(_queryParams["IdStatus"].ToString()));
                _db.AddInParameter(_cmd, "@IdOrigem", DbType.Int32, int.Parse(_queryParams["IdOrigem"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _lote = (new Lote
                        {
                            ID = int.Parse(_dr["IdLote"].ToString()),
                            QtdeImagem = int.Parse(_dr["QtdeImagem"].ToString()),
                            Duplex = Boolean.Parse(_queryParams["Duplex"].ToString()),
                            PathCaptura = _dr["PathCaptura"].ToString(),
                            UsuarioCaptura = (new Usuario { ID = int.Parse(_dr["IdUsuarioCaptura"].ToString()) }),
                            ServicoCaptura = (new Servico { ID = int.Parse(_dr["IdServico"].ToString()) }),
                            StatusLote = int.Parse(_dr["IdStatus"].ToString()),
                            OrigemID = int.Parse(_dr["IdOrigem"].ToString()),
                        });
                    }
                }
                return _lote;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int InserirItemLote(IDictionary<string, object> _queryParams)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_loteitem_ins"));
                _db.AddInParameter(_cmd, "@IdLote", DbType.Int32, int.Parse(_queryParams["IdLote"].ToString()));
                _db.AddInParameter(_cmd, "@IdOrigem", DbType.Int32, int.Parse(_queryParams["IdOrigem"].ToString()));
                _db.AddInParameter(_cmd, "@IdDocumentoModelo", DbType.Int32, int.Parse(_queryParams["IdDocumentoModelo"].ToString()));
                _db.AddInParameter(_cmd, "@IdUsuarioCaptura", DbType.Int32, int.Parse(_queryParams["IdUsuarioCaptura"].ToString()));
                _db.AddInParameter(_cmd, "@ImgNomeOriginal", DbType.String, _queryParams["ImgNomeOriginal"].ToString());
                _db.AddInParameter(_cmd, "@ImgNomeFinal", DbType.String, _queryParams["ImgNomeFinal"].ToString());
                _db.AddInParameter(_cmd, "@SequenciaCaptura", DbType.Int32, int.Parse(_queryParams["SequenciaCaptura"].ToString()));
                _db.AddInParameter(_cmd, "@Verso", DbType.Boolean, Boolean.Parse(_queryParams["Verso"].ToString()));
                _db.AddInParameter(_cmd, "@StatusImagem", DbType.Int32, int.Parse(_queryParams["StatusImagem"].ToString()));

                int _ID = _db.ExecuteNonQuery(_cmd); 
                return _ID;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Lote AtualizarLote(IDictionary<string, object> _queryParams)
        {
            try
            {
                Lote _lote = new Lote();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_lote_alt"));

                _db.AddInParameter(_cmd, "@IdLote", DbType.Int32, int.Parse(_queryParams["IdLote"].ToString()));

                if (!string.IsNullOrEmpty(_queryParams["QtdeImagem"].ToString()))
                    _db.AddInParameter(_cmd, "@QtdeImagem", DbType.Int32, int.Parse(_queryParams["QtdeImagem"].ToString()));

                if (!string.IsNullOrEmpty(_queryParams["PathCaptura"].ToString()))
                    _db.AddInParameter(_cmd, "@PathCaptura", DbType.String, _queryParams["PathCaptura"].ToString());

                if (!string.IsNullOrEmpty(_queryParams["IdUsuarioCaptura"].ToString()))
                    _db.AddInParameter(_cmd, "@IdUsuarioCaptura", DbType.Int32, int.Parse(_queryParams["IdUsuarioCaptura"].ToString()));

                if (!string.IsNullOrEmpty(_queryParams["IdServico"].ToString()))
                    _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["IdServico"].ToString()));

                if (!string.IsNullOrEmpty(_queryParams["IdStatus"].ToString()))
                    _db.AddInParameter(_cmd, "@IdStatus", DbType.Int32, int.Parse(_queryParams["IdStatus"].ToString()));

                if (!string.IsNullOrEmpty(_queryParams["IdOrigem"].ToString()))
                    _db.AddInParameter(_cmd, "@IdOrigem", DbType.Int32, int.Parse(_queryParams["IdOrigem"].ToString()));


                if (!string.IsNullOrEmpty(_queryParams["NomeLote"].ToString())) 
                    _db.AddInParameter(_cmd, "@NomeLote", DbType.String, _queryParams["NomeLote"].ToString());

                if (!string.IsNullOrEmpty(_queryParams["DataAbertura"].ToString()))
                    _db.AddInParameter(_cmd, "@DataAbertura", DbType.DateTime, _queryParams["DataAbertura"].ToString());

                if (!string.IsNullOrEmpty(_queryParams["DataFim"].ToString()))
                    _db.AddInParameter(_cmd, "@DataFim", DbType.DateTime, _queryParams["DataFim"].ToString());

                if (!string.IsNullOrEmpty(_queryParams["Login"].ToString()))
                    _db.AddInParameter(_cmd, "@Login", DbType.String, _queryParams["Login"].ToString());

                if (!string.IsNullOrEmpty(_queryParams["Scanner"].ToString()))
                    _db.AddInParameter(_cmd, "@Scanner", DbType.String, _queryParams["Scanner"].ToString());

                if (!string.IsNullOrEmpty(_queryParams["NumeroPaginas"].ToString()) || int.Parse(_queryParams["NumeroPaginas"].ToString()) > 0)
                    _db.AddInParameter(_cmd, "@NumeroPaginas", DbType.Int32, int.Parse(_queryParams["NumeroPaginas"].ToString()));

                if (!string.IsNullOrEmpty(_queryParams["Dados"].ToString()))
                    _db.AddInParameter(_cmd, "@Dados", DbType.String, _queryParams["Dados"].ToString());

                if (!string.IsNullOrEmpty(_queryParams["Versao"].ToString()))
                    _db.AddInParameter(_cmd, "@Versao", DbType.String, _queryParams["Versao"].ToString());

                if (!string.IsNullOrEmpty(_queryParams["IDPerfil"].ToString()) || int.Parse(_queryParams["IDPerfil"].ToString()) > 0)
                    _db.AddInParameter(_cmd, "@IDPerfil", DbType.Int32, int.Parse(_queryParams["IDPerfil"].ToString()));

                //if (_queryParams["Arquivos"] != null)
                 //   _db.AddInParameter(_cmd, "@Arquivos", DbType.String, _queryParams["Arquivos"].ToString());

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _lote = (new Lote
                        {
                            ID = int.Parse(_dr["IdLote"].ToString()),
                            QtdeImagem = int.Parse(_dr["QtdeImagem"].ToString()),
                            PathCaptura = _dr["PathCaptura"].ToString(),
                            UsuarioCaptura = (new Usuario { ID = int.Parse(_dr["IdUsuarioCaptura"].ToString()) }),
                            ServicoCaptura = (new Servico { ID = int.Parse(_dr["IdServico"].ToString()) }),
                            StatusLote = int.Parse(_dr["IdStatus"].ToString()),
                            OrigemID = int.Parse(_dr["IdOrigem"].ToString()),
                        });
                    }
                }
                return _lote;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int ApagarLote(IDictionary<string, object> _queryParams)
        {
            try
            {
                Lote _lote = new Lote();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_lote_del"));
                _db.AddInParameter(_cmd, "@IdLote", DbType.Int32, int.Parse(_queryParams["IdLote"].ToString()));
                int _ret = _db.ExecuteNonQuery(_cmd); 
                return _ret;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int VerificaLoteJaExiste(IDictionary<string, object> _queryParams)//Walmir
        {
            try
            {
                Lote _lote = new Lote();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_lote_seleciona"));
                _db.AddInParameter(_cmd, "@idLote", DbType.Int32, (int.Parse(_queryParams["idLote"].ToString())));
                _db.AddInParameter(_cmd, "@NomeLote", DbType.String, (_queryParams["NomeLote"].ToString()));

                int _ret=0;
                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _ret = int.Parse(_dr[0].ToString());
                    }
                }

                //int _ret = _db.ExecuteNonQuery(_cmd);

                return _ret;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool UsaArquivoDados(IDictionary<string, object> _queryParams)//Walmir
        {
            //try
            //{  
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_UsaArquivoDados"));
                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, (int.Parse(_queryParams["idServico"].ToString())));
             
                bool _ret = true ;
                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _ret = (_dr[0] as bool?) ?? false;
                        //_ret = (bool)_dr[0]; 
                    }
                }
                return _ret;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
        public bool TipificarItem(IDictionary<string, object> _queryParams) {
            try
            {

                string scriptTipificacao = _queryParams["ScriptTipificacao"].ToString();

                List<LoteItem> _itens = new List<LoteItem>();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format(scriptTipificacao));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdLote", DbType.Int32, int.Parse(_queryParams["Lote_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdLoteItem", DbType.Int32, int.Parse(_queryParams["LoteItem_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdDocumentoModelo", DbType.Int32, int.Parse(_queryParams["DocumentoModelo_ID"].ToString()));


                _db.ExecuteNonQuery(_cmd);

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public void ExcluirLote(IDictionary<string, object> _queryParams)
        //{
        //    //try
        //    //{
        //        DbCommand _cmd;
        //        Database _db = DbConn.CreateDB();
        //        _cmd = _db.GetStoredProcCommand("proc_ExcluirLote");
        //        _db.AddInParameter(_cmd, "@IdLote", DbType.Int32, int.Parse(_queryParams["Lote_ID"].ToString()));
        //        _db.ExecuteNonQuery(_cmd);
                
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw;
        //    //}
        //}
        public List<LoteItem> ListarItensLote(IDictionary<string, object> _queryParams) {

            List<LoteItem> _itens = new List<LoteItem>();
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_loteitens_servico_usuario_sel"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                
                //_db.AddInParameter(_cmd, "@IdStatus", DbType.Int32, int.Parse(_queryParams["Status_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _itens.Add(ConverteBaseLoteItens(_dr));
                    }
                }
                if (_itens == null) { throw new Erro("Lote não localizado."); }
                return _itens.Where(i => i.IdLote == int.Parse(_queryParams["Lote_ID"].ToString())).ToList<LoteItem>();
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        public List<Lote> ListarLotesStatus(IDictionary<string, object> _queryParams)
        {
            Lote _lote = new Lote();
            List<Lote> _lotes = new List<Lote>();
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_lotes_servico_usuario_sel"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                //_db.AddInParameter(_cmd, "@IdStatus", DbType.Int32, int.Parse(_queryParams["Status_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _lotes.Add(ConverteBaseObjeto(_dr));
                    }
                }
                if (_lotes == null) { throw new Erro("Lote não localizado."); }
                return _lotes;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<Lote> ListarLotes(IDictionary<string, object> _queryParams)
        {
            Lote _lote = new Lote();
            List<Lote> _lotes = new List<Lote>();
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_lotes_servico_usuario_sel"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _lotes.Add(ConverteBaseObjeto(_dr));
                    }
                }
                if (_lotes == null) { throw new Erro("Servico não localizado."); }
                return _lotes;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion ILoteRepository Members

        private Lote  ConverteBaseObjeto(IDataReader dt) {
            Lote _lote = new Lote();
            _lote = (new Lote
            {
                ID = int.Parse(dt["IdLote"].ToString()),
                OrigemID = int.Parse(dt["IdOrigem"].ToString()),
                PathCaptura = dt["PathCaptura"].ToString(),
                QtdeImagem = int.Parse(dt["QtdeImagem"].ToString()),
                Duplex = Boolean.Parse(dt["Duplex"].ToString()),
                StatusLote = int.Parse(dt["IdStatus"].ToString()),
                DataCriacao = DateTime.Parse(dt["DataCriacao"].ToString()),

                UsuarioCaptura = (new Usuario { ID= int.Parse(dt["IdUsuarioCaptura"].ToString())}),
                Itens = new List<LoteItem>(),
                ServicoCaptura = (new Servico { ID = int.Parse(dt["IdServico"].ToString()) })
            });
            return _lote;
        }
        private LoteItem ConverteBaseLoteItens(IDataReader dt)
        {
            LoteItem _item = new LoteItem();
            _item = (new LoteItem
            {
                ID = int.Parse(dt["IdLoteItem"].ToString()),
                IdLote = int.Parse(dt["IdLote"].ToString()),
                NomeFinal = dt["ImgNomeFinal"].ToString(),
                NomeOriginal = dt["ImgNomeOriginal"].ToString(),
                SequenciaCaptura = int.Parse(dt["SequenciaCaptura"].ToString()),
                Verso = Boolean.Parse(dt["Verso"].ToString()),
                DataCaptura = DateTime.Parse(dt["DataCriacao"].ToString()),
                StatusImagem = int.Parse(dt["StatusImagem"].ToString()),
                DocumentoModelo = (new DocumentoModelo { ID = int.Parse(dt["IdDocumentoModelo"].ToString()) }),
                UsuarioCaptura = (new Usuario { ID = int.Parse(dt["IdUsuarioCaptura"].ToString()) })

            });
            return _item;
        }
    }
}
