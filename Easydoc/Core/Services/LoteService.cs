using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Repositories.Interfaces;
using MK.Easydoc.Core.Services.Interfaces;

namespace MK.Easydoc.Core.Services
{
    public sealed class LoteService : ILoteService
    {
        #region Private Fields

        //private readonly ILog _logger;
        private ILoteRepository _repository;
        private IDocumentoService _documento;
        private IDictionary<string, object> _queryParams;

        #endregion
        #region Public Constructors

        //public UsuarioService(ILog logger, IUsuarioRepository repository)
        public LoteService(ILoteRepository repository, IDocumentoService documento)
        {
            //this._logger = logger;
            this._repository = repository;
            this._queryParams = new Dictionary<string, object>();
            _documento = documento;
        }

        #endregion
        #region IUserService Members
        public Lote CriarLote(int idUsuario, int idOrigem, int idServico, string path)
        {
            try
            {
                this._queryParams.Clear();
                
                _queryParams["QtdeImagem"] = 0;
                _queryParams["Duplex"] = true;
                _queryParams["PathCaptura"] = path;
                _queryParams["IdUsuarioCaptura"] = idUsuario;
                _queryParams["IdServico"] = idServico;
                _queryParams["IdStatus"] = 1000;
                _queryParams["IdOrigem"] = idOrigem;

                return this._repository.CriarLote(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }
        public int InserirItensLote(Lote lote, int idServico) {

            try
            {
                int _qtdItens = 0;
                foreach (LoteItem item in lote.Itens.OrderBy(x=> x.DataCriacaoArqCap))
                {
                    int _id = 0;
                    this._queryParams.Clear();

                    _queryParams["IdLote"] = lote.ID;
                    _queryParams["IdOrigem"] = item.OrigemID;
                    _queryParams["IdDocumentoModelo"] = item.DocumentoModelo.ID;
                    _queryParams["IdUsuarioCaptura"] = item.UsuarioCaptura.ID;
                    _queryParams["ImgNomeOriginal"] = item.NomeOriginal;
                    _queryParams["ImgNomeFinal"] = item.NomeFinal;
                    _queryParams["SequenciaCaptura"] = item.SequenciaCaptura;
                    _queryParams["Verso"] = item.Verso;
                    _queryParams["StatusImagem"] = item.StatusImagem;
                    _queryParams["idservico"] = idServico;

                    _id = _repository.InserirItemLote(_queryParams);
                    if (_id != 0) _qtdItens++;

                }
                return lote.QtdeImagem - _qtdItens;
            }
            catch (Exception ex) { throw ex; }
        }
        public List<Lote> ListarLotesPendente(int idUsuario, int idOrigem, int idServico)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                //this._queryParams["Origem_ID"] = idOrigem;
                this._queryParams["Servico_ID"] = idServico;
                //this._queryParams["Status_Lote"] = 1000;

                return this._repository.ListarLotesStatus(this._queryParams).Where(l => l.StatusLote == 1010).ToList<Lote>();
            }
            catch (Exception ex) { throw ex; }
        }
        public List<Lote> ListarLotesTipificar(int idUsuario, int idOrigem, int idServico)
        {
            try
            {

                Lote _lote = new Lote();
                List<Lote> _lotes = new List<Lote>();

                List<Lote> _lotes_temp = new List<Lote>();


                LoteItem _item = new LoteItem();
                List<LoteItem> _itens = new List<LoteItem>();

                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Origem_ID"] = idOrigem;
                this._queryParams["Servico_ID"] = idServico;
                

                _lotes = _repository.ListarLotes(this._queryParams).Where(l => l.StatusLote == 1010).ToList<Lote>();

                foreach (Lote lote in _lotes)
                {
                    lote.Itens.AddRange(ListarItensLote(lote.ID, idUsuario, idServico, true, 1000));
                
                }
                
                
               /* 
                int n=0;
                foreach (var tmp_lote in _lote.Itens)
                {
                    if (tmp_lote.StatusImagem == 2000)
                    {
                        
                        tmp_lote.Itens.RemoveRange(n, 1);
                    }
                    n++;
                }

                foreach (var _lt in _lotes[].Itens.){
                    _lt.Itens.
                    _lotes_temp.AddRange(_lt.Itens.Where(x => x.StatusImagem == 1000).ToList<Lote>());

                }*/

                return _lotes;
            }
            catch (Exception ex) { throw ex; }
        }
        public List<LoteItem> ListarItensLote(int idLote, int idUsuario, int idServico, bool SemTipo, int Statusimagem=0)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Servico_ID"] = idServico;
                this._queryParams["Lote_ID"] = idLote;
                this._queryParams["SemTipo"] = SemTipo;
                this._queryParams["Statusimagem"] = Statusimagem;

                return this._repository.ListarItensLote(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }
        public Lote GetLote(int idLote, int idUsuario,int idServico, int numItens)
        {
            try
            {
                Lote _lote = new Lote();
                this._queryParams.Clear();
                this._queryParams["Lote_ID"] = idLote;
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Servico_ID"] = idServico;
                this._queryParams["Statusimagem"] = 1000;                


                _lote =  _repository.ListarLotes(_queryParams).Where(l => l.ID==idLote).FirstOrDefault();
                if (numItens==0)
                {
                    _lote.Itens.AddRange(_repository.ListarItensLote(_queryParams).ToList<LoteItem>());
                }
                else
                {
                    _lote.Itens.AddRange(_repository.ListarItensLote(_queryParams).Take(numItens).ToList<LoteItem>());
                }
                

                return _lote;
            }
            catch (Exception ex) { throw ex; }
        }
        //public void ExcluirLote(int idLote)
        //{
        //    try
        //    {
                
        //        this._queryParams.Clear();
        //        this._queryParams["idLote"] = idLote;
        //        _repository.ListarLotes(_queryParams).Where(l => l.ID == idLote).FirstOrDefault();                
        //    }
        //    catch (Exception ex) { throw ex; }
        //}
        public Lote AtualizarLote(Lote lote)
        {
            try
            {
                this._queryParams.Clear();
                
                _queryParams["IdLote"] = lote.ID;
                _queryParams["QtdeImagem"] = lote.QtdeImagem;
                _queryParams["Duplex"] = lote.Duplex;
                _queryParams["PathCaptura"] = lote.PathCaptura;
                _queryParams["IdUsuarioCaptura"] = lote.UsuarioCaptura.ID;
                _queryParams["IdServico"] = lote.ServicoCaptura.ID;
                _queryParams["IdStatus"] = lote.StatusLote;
                _queryParams["IdOrigem"] = lote.OrigemID;

                _queryParams["NomeLote"] = lote.Log.NomeLote ?? string.Empty;
                _queryParams["DataAbertura"] = lote.Log.DataAbertura ?? string.Empty;
                _queryParams["DataFim"] = lote.Log.DataFim ?? string.Empty;
                _queryParams["Login"] = lote.Log.Login ?? string.Empty;
                _queryParams["Versao"] = lote.Log.Versao ?? string.Empty;
                _queryParams["Estacao"] = lote.Log.Estacao ?? string.Empty;
                _queryParams["Scanner"] = lote.Log.Scanner ?? string.Empty;
                _queryParams["NumeroPaginas"] = lote.Log.NumeroPaginas;
                _queryParams["IDPerfil"] = lote.Log.IdPerfil;
                _queryParams["Arquivos"] = lote.Log.Arquivos;

                _queryParams["Dados"] = string.Empty;

                if (lote.Log.Dados != null)
                {
                    foreach (string _dado in lote.Log.Dados)
                    {
                        string _dataConvertida = string.Empty;
                        if (_dado.Contains("dtInicial"))
                        {
                            //_dataConvertida = _dado.Replace("dtInicial=", "").Replace("-", "");
                            if (_dado.Replace("dtInicial=", "").Replace("-", "").Length > 8)
                            {
                                _dataConvertida = _dado.Replace("dtInicial=", "").Replace("-", "");
                                _dataConvertida = "dtInicial=" + _dataConvertida.Replace("dtInicial=", "").Replace("-", "").Substring(0, 8);
                            }
                        }

                        if (_dado.Contains("dtFinal"))
                        {
                            if (_dado.Replace("dtFinal=", "").Replace("-", "").Length > 8)
                            {
                                _dataConvertida = _dado.Replace("dtFinal=", "").Replace("-", "");
                                _dataConvertida = "dtFinal=" + _dataConvertida.Replace("dtFinal=", "").Replace("-", "").Substring(0, 8);
                            }
                        }
                        if (string.IsNullOrEmpty(_dataConvertida))
                        {
                            _queryParams["Dados"] += _dado + "; ";
                        }
                        else
                        {
                            _queryParams["Dados"] += _dataConvertida + "; ";
                        }

                    }
                    _queryParams["Dados"] = _queryParams["Dados"].ToString().Trim().Substring(0, _queryParams["Dados"].ToString().Trim().Length - 1);

                    _queryParams["Versao"] = lote.Log.Versao;
                    _queryParams["IDPerfil"] = lote.Log.IdPerfil;

                    _queryParams["Arquivos"] = string.Empty;
                   /* foreach (string _arquivo in lote.Log.Arquivos)
                    {
                        _queryParams["Arquivos"] += _arquivo + "; ";
                    }
                    _queryParams["Arquivos"] = _queryParams["Arquivos"].ToString().Trim().Substring(0, _queryParams["Arquivos"].ToString().Trim().Length - 1);
                    */
                    _queryParams["Arquivos"] = lote.Log.Arquivos;
                }
                else
                {

                }

                Lote _lote = this._repository.AtualizarLote(this._queryParams);

                return _lote;
            }
            catch (Exception ex) { throw ex; }        
        }
        public bool ApagarLote(int idLote)
        {
            try
            {
                this._queryParams.Clear();
                _queryParams["IdLote"] = idLote;
                return this._repository.ApagarLote(this._queryParams) == 0;
            }
            catch (Exception ex) { throw ex; }
        }
        public int VerificaLoteJaExiste(int idLote, string NomeLote)//walmir
        {
            try
            {
                this._queryParams.Clear();
                _queryParams["idLote"] = idLote;
                _queryParams["NomeLote"] = NomeLote;
                return this._repository.VerificaLoteJaExiste(this._queryParams) ;
            }
            catch (Exception ex) { throw ex; }
        }
        public bool UsaArquivoDados(int idServico)
        {
            try
            {
                this._queryParams.Clear();
                _queryParams["idServico"] = idServico;               
                return this._repository.UsaArquivoDados(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }        
        public bool TipificarItem(int idUsuario, int idServico, int idLote, int idLoteItem, int idDocumentoModelo)
        {
            try
            {
                this._queryParams.Clear();

                string _scriptSQLTipificar = _documento.ListarTipos(idServico).Where(d => d.ID == idDocumentoModelo).FirstOrDefault().ScriptSQLTipificar.Trim();

                _queryParams["ScriptTipificacao"] = string.Format(_scriptSQLTipificar);
                _queryParams["Usuario_ID"] = idUsuario;
                _queryParams["Servico_ID"]=idServico;
                _queryParams["Lote_ID"]=idLote;
                _queryParams["LoteItem_ID"]=idLoteItem;
                _queryParams["DocumentoModelo_ID"]=idDocumentoModelo;

                return this._repository.TipificarItem(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }
        public bool TipificarItemDescricao(int idUsuario, int idServico, int idLote, int idLoteItem, string descricaoDocumentoModelo)
        {
            try
            {
                this._queryParams.Clear();

                DocumentoModelo _modelo = new DocumentoModelo();
                List<DocumentoModelo> _modelos = new List<DocumentoModelo>();

                _modelos = _documento.ListarTipos(idServico);
                _modelo = _modelos.Where(d => d.Descricao.Trim().ToUpper() == descricaoDocumentoModelo.Trim().ToUpper()).FirstOrDefault();

                if (_modelo == null) return false;

                string _scriptSQLTipificar = _modelo.ScriptSQLTipificar.Trim();

                _queryParams["ScriptTipificacao"] = string.Format(_scriptSQLTipificar);
                _queryParams["Usuario_ID"] = idUsuario;
                _queryParams["Servico_ID"] = idServico;
                _queryParams["Lote_ID"] = idLote;
                _queryParams["LoteItem_ID"] = idLoteItem;
                _queryParams["DocumentoModelo_ID"] = _modelo.ID;

                return this._repository.TipificarItem(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion IUserService Members
    }
}
