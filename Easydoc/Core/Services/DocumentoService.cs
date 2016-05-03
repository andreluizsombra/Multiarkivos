using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Repositories.Interfaces;
using MK.Easydoc.Core.Services.Interfaces;
using System.Data;

namespace MK.Easydoc.Core.Services
{
    public class DocumentoService : IDocumentoService
    {
        #region Private Fields

        //private readonly ILog _logger;
        private IDocumentoRepository _repository;
        private IDictionary<string, object> _queryParams;
        
        
        #endregion

        #region Public Constructors

        //public UsuarioService(ILog logger, IUsuarioRepository repository)
        public DocumentoService(IDocumentoRepository repository)
        {
            //this._logger = logger;
            this._repository = repository;
            this._queryParams = new Dictionary<string, object>();
        }

        #endregion
        #region IUserService Members
        public DocumentoModelo CriarDocumento(int idUsuario, int idOrigem, int idServico)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Origem_ID"] = idOrigem;
                this._queryParams["Servico_ID"] = idServico;

                return this._repository.CriarDocumento(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }


        public List<DocumentoConsulta> ListarConsultasModelo(int idUsuario, int idServico)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Servico_ID"] = idServico;

                return this._repository.ListarConsultasModelo(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }


        public List<Documento> ListarDocumentosDigitar(int idUsuario, int idOrigem, int idServico)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Origem_ID"] = idOrigem;
                this._queryParams["Servico_ID"] = idServico;


                return this._repository.ListarDocumentosStatus(this._queryParams).Where(d=>d.StatusDocumento==1000).ToList<Documento>();
               
            }
            catch (Exception ex) { throw ex; }
        }
        public List<Documento> ListarDocumentosTipificar(int idUsuario, int idOrigem, int idServico)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Origem_ID"] = idOrigem;
                this._queryParams["Servico_ID"] = idServico;

            var tmp = this._repository.ListarDocumentosStatus(this._queryParams).Where(d => d.StatusDocumento == 1000).ToList<Documento>();

            return tmp;

            }
            catch (Exception ex) { throw ex; }
        }
        public List<Documento> ListarDocumentosSupervisao(int idUsuario, int idOrigem, int idServico)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Origem_ID"] = idOrigem;
                this._queryParams["Servico_ID"] = idServico;

                return this._repository.ListarDocumentosStatus(this._queryParams).Where(d => d.StatusDocumento == 1020).ToList<Documento>();
            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable ListarDocumentosVinculoPai(int idServico, int idDocModelo, int tipo)
        {
            try
            {
                return this._repository.ListarDocsVinculoPai(idServico, idDocModelo, tipo);  
            }
            catch (Exception ex) { throw ex; }
        }
        public List<Documento> ListarDocumentosFormalizar(int idUsuario, int idOrigem, int idServico)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Origem_ID"] = idOrigem;
                this._queryParams["Servico_ID"] = idServico;

                return this._repository.ListarDocumentosStatus(this._queryParams).Where(d => d.StatusDocumento == 2000).ToList<Documento>();
            }
            catch (Exception ex) { throw ex; }
        }
        public string PesquisarDocumentosModulo(int idServico, int idDocumentoModelo, string campos, string scriptWhere)
        {

            try
            {
                this._queryParams.Clear();
                this._queryParams["Servico_ID"] = idServico;
                string _scriptSQLModulo = _repository.ListarTipos(_queryParams).Where(d => d.ID == idDocumentoModelo).FirstOrDefault().ScriptSQLModulo.Trim();        //ListarTipos(this._queryParams["Servico_ID"]).Where(d => d.ID == idDocumentoModelo).FirstOrDefault().ScriptSQLTipificar.Trim();

                this._queryParams.Clear();
                _queryParams["CamposSQL"] = campos;
                _queryParams["ScriptSQLModulo"] = string.Format(_scriptSQLModulo);
                _queryParams["DocumentoModelo_ID"] = idDocumentoModelo;
                _queryParams["Script_WHERE"] = scriptWhere;

                return this._repository.PesquisarDocumentosModulo(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }
        public string PesquisarDocumentosConsulta(int idServico, int idDocumentoModelo, string campos, string scriptWhere) {

            try
            {
                this._queryParams.Clear();
                this._queryParams["Servico_ID"] = idServico;
                string _scriptSQLConsulta = _repository.ListarTipos(_queryParams).Where(d => d.ID == idDocumentoModelo).FirstOrDefault().ScriptSQLConsulta.Trim();        //ListarTipos(this._queryParams["Servico_ID"]).Where(d => d.ID == idDocumentoModelo).FirstOrDefault().ScriptSQLTipificar.Trim();
                
                this._queryParams.Clear();
                _queryParams["CamposSQL"] = campos;
                _queryParams["ScriptSQLConsulta"] = string.Format(_scriptSQLConsulta);
                _queryParams["DocumentoModelo_ID"] = idDocumentoModelo;
                _queryParams["Script_WHERE"] = scriptWhere;

                return this._repository.PesquisarDocumentosConsulta(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }
        public string PesquisarMotivo(int idDocumento, int idServico)
        {
            try
            { 
                this._queryParams.Clear();
                this._queryParams["idDocumento"] = idDocumento;
                this._queryParams["idServico"] = idServico;
                return this._repository.PesquisarMotivo(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }
        
        public List<DocumentoModelo> ListarTipos(int idServico)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Servico_ID"] = idServico;

                return this._repository.ListarTipos( this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }

        public List<DocumentoModelo> ListarTiposConsulta(int idServico)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Servico_ID"] = idServico;

                return this._repository.ListarTiposConsulta(idServico);
            }
            catch (Exception ex) { throw ex; }
        }
        public void AtualizarDocumento(Documento documento, int idServico)
        {
            try
            {
                this._queryParams.Clear();

                this._queryParams["Servico_ID"] = idServico;
                this._queryParams["Documento"] = documento;

                this._repository.AtualizarDocumento(this._queryParams);
            }
            catch (Exception ex) { throw ex; }        
        }
        //walmir
        //public void  IncluirMotivo(int IdDocumento,int Atalho,int UserID, int tipo)
        public void IncluirMotivo(int idServico, int idDocumento, int idOcorrencia, int tipo, int UserID)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["idServico"] = idServico;
                this._queryParams["idDocumento"] = idDocumento;
                this._queryParams["idOcorrencia"] = idOcorrencia;
                this._queryParams["tipo"] = tipo;
                this._queryParams["idUsuario"] = UserID;
                

                this._repository.IncluirMotivo(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }
        public void AlteraDuplicidade(int IdDocumento, int ID )
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["IdDocumento"] = IdDocumento;
                this._queryParams["ID"] = ID;
                this._repository.AlteraDuplicidade(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }






        public void ExcluirDocumento(int idDocumento)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Documento_ID"] = idDocumento;
                this._repository.ExcluirDocumento(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }
        public void MudaStatusDocumento(int idDocumento,int idUsuario, int idStatus)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["Documento_ID"] = idDocumento;
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Status_ID"] = idStatus;
                this._repository.MudaStatusDocumento(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }




        public void FinalizarDigitacao(int idDocumento, int id_Servico)
        {
            try
            {

                Documento _documento = (new Documento { ID = idDocumento, StatusDocumento = 1010 });

                this._queryParams.Clear();
                this._queryParams["Servico_ID"] = id_Servico;
                this._queryParams["Documento"] = _documento;

                this._repository.AtualizarDocumento(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }
        public void AtualizarDocumentoCampo(CampoModelo campoModelo)
        {
            try
            {
                this._queryParams.Clear();
                this._queryParams["DocumentoCampo"] = campoModelo;

                this._repository.AtualizarDocumentoCampo(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }

        public string GetStatusDocumento(int idDocumento)
        {
            //try
            //{
                string _ret;
                this._queryParams.Clear();
                this._queryParams["iddocumento"] = idDocumento;
                _ret = this._repository.GetStatusDocumento(_queryParams);
                return _ret;;
            //}
            //catch (Exception ex) { throw ex; }
        }
        public string GetDocumentoModelo(int idDocumento)
        {
            //try
            //{
            string _ret;
            this._queryParams.Clear();
            this._queryParams["iddocumento"] = idDocumento;
            _ret = this._repository.GetDocumentoModelo(_queryParams);
            return _ret; ;
            //}
            //catch (Exception ex) { throw ex; }
        }
        public string Executar(string cmd)
        {
            //try
            //{
            string _ret;
            this._queryParams.Clear();
            this._queryParams["cmd"] = cmd;
            _ret = this._repository.Executar(_queryParams);
            return _ret; ;
            //}
            //catch (Exception ex) { throw ex; }
        }





        public string ValidarCamposDocumento(int idDocumentoModelo, CampoModelo campoModelo)
        {
            try
            {
                CampoModelo _cm = new CampoModelo();
                List<CampoModelo> _cms = new List<CampoModelo>();

                this._queryParams.Clear();

                _cms.AddRange(ListarCamposModelo(idDocumentoModelo));
                _cm = _cms.Where(c => c.ID == campoModelo.ID).FirstOrDefault();

                if (_cm == null) return string.Empty;

                if (!string.IsNullOrEmpty(_cm.ProcSqlValidacao))
	            {
	                campoModelo.ProcSqlValidacao = _cm.ProcSqlValidacao;
                    this._queryParams["DocumentoCampo"] = campoModelo;
                    return this._repository.ValidarCamposDocumento(this._queryParams);
                }else{
                    return string.Empty;
	            }
            }
            catch (Exception ex) { throw ex; }
        }
        public string ValidarDocumento(int idDocumento, int idDocumentoModelo, int idServico )
        {
            //_procValidacao = "proc_valida_doc01_serv01";
            try
            {
                string _procValidacao = string.Empty;
                _procValidacao = ListarTipos(idServico).Where(d => d.ID == idDocumentoModelo).FirstOrDefault().ScriptSQLValidar;

                if (!string.IsNullOrEmpty(_procValidacao))
                {
                    this._queryParams.Clear();
                    this._queryParams["ProcSqlValidacao"] = _procValidacao;
                    this._queryParams["Documento_ID"] = idDocumento;
                    this._queryParams["DocumentoModelo_ID"] = idDocumentoModelo;
                    return this._repository.ValidarDocumento(this._queryParams);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        public string SalvarConsultaDocumentoModelo(int idUsuario, int idServico, int idDocumentoModelo, string nomeConsulta, string descricao,bool compartilhado , string stringJSON) {

            string _retorno = string.Empty;

            try
            {
                this._queryParams.Clear();

                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Servico_ID"] = idServico;
                this._queryParams["Documento_Modelo_ID"] = idDocumentoModelo;
                this._queryParams["Nome_Consulta"] = nomeConsulta;
                this._queryParams["String_JSON"] = stringJSON;


                this._queryParams["Descricao"] = descricao;
                this._queryParams["Compartilhado"] = compartilhado;

                _retorno = this._repository.SalvarConsultaDocumentoModelo(this._queryParams);

            }
            catch (Exception ex) { _retorno = ex.Message; throw ex; }
            return _retorno;
        }
        public Documento SelecionaDocumentoDigitar(int idUsuario, int idServico, int idDocumento)
        {
            try
            {
                Documento _documento = new Documento();

                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                //this._queryParams["Origem_ID"] = idOrigem;
                this._queryParams["Servico_ID"] = idServico;

                //TODO: Atualizar a data de atualização do doc e pegar apenas os que estiverem com data de atualização maior que 10 minutos
                _documento = this._repository.ListarDocumentosStatus(this._queryParams).Where(d => (d.StatusDocumento == 1000 || d.StatusDocumento == 1020 || d.StatusDocumento == 1010) && d.ID == idDocumento).FirstOrDefault();

                if (_documento != null)
                {
                    this._queryParams.Clear();
                    this._queryParams["Usuario_ID"] = idUsuario;
                    this._queryParams["Documento_ID"] = _documento.ID;
                    this._queryParams["Servico_ID"] = idServico;

                    _documento.Modelo.Campos.AddRange(_repository.SelecionarDocumentoCampos(_queryParams).Where(c => c.Digita).ToList<CampoModelo>());
                    _documento.Arquivos.AddRange(_repository.SelecionarDocumentoImagens(_queryParams).ToList<DocumentoImagem>());

                    this._queryParams.Clear();
                    this._queryParams["Servico_ID"] = idServico;
                    this._queryParams["Documento"] = _documento;
                    _repository.AtualizarDocumento(_queryParams);
                }
                else
                {
                    _documento = new Documento();
                }

                return _documento;

            }
            catch (Exception ex) { throw ex; }

        }
        //walmir
        public bool EmUso(int idDocumento,int idUsuario ,int Tipo)
        {
            //try
            //{

            bool _ret;
            this._queryParams.Clear();
            this._queryParams["idDocumento"] = idDocumento;
            this._queryParams["idUsuario"] = idUsuario;
            this._queryParams["Tipo"] = Tipo;
            _ret = _repository.EmUso(_queryParams);            
            return _ret;
            //}
            //catch (Exception ex) { throw ex; }
        }

        /*public List<Motivo> GetMotivoDigitar_Antigo(int idServico, int Tipo)
        {
            //try
            //{
                List<Motivo> _Motivo = new List<Motivo>();
                this._queryParams.Clear();
                this._queryParams["iddocumentomodelo"] = idServico;
                this._queryParams["tipo"] = Tipo;
                _Motivo.AddRange(_repository.SelecionarMotivo(_queryParams));                
                return _Motivo;
            //}
            //catch (Exception ex) { throw ex; }
        }*/

        public List<Ocorrencia> GetMotivoDigitar(int idServico, int Tipo)
        {
            try
            {
            //List<Ocorrencia> _Motivo = new List<Ocorrencia>();
            //this._queryParams.Clear();
            //this._queryParams["iddocumentomodelo"] = idServico;
            //this._queryParams["tipo"] = Tipo;
            // _Motivo.AddRange(_repository.ListaOcorrencia(idServico,Tipo));
                return _repository.ListaOcorrencia(idServico, Tipo); //_Motivo;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public string GetDuplicidade(int idDocumento)
        {
            //try
            //{
            string _ret ;
            this._queryParams.Clear();
            this._queryParams["iddocumento"] = idDocumento;
            _ret = _repository.GetDuplicidade(_queryParams);
            return _ret;
            //}
            //catch (Exception ex) { throw ex; }
        }
        public string GetMotivo(int idDocumento)
        {
            //try
            //{
            string _Motivo ;
            this._queryParams.Clear();
            this._queryParams["iddocumento"] = idDocumento;
            _Motivo = this._repository.GetMotivo(_queryParams);            
            return _Motivo;
            //}
            //catch (Exception ex) { throw ex; }
        }



        public Documento GetDocumentoDigitar(int idUsuario, int idServico) {
            try
            {
                Documento _documento = new Documento();

                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                //this._queryParams["Origem_ID"] = idOrigem;
                this._queryParams["Servico_ID"] = idServico;

                //TODO: Atualizar a data de atualização do doc e pegar apenas os que estiverem com data de atualização maior que 10 minutos
                _documento =  this._repository.ListarDocumentosStatus(this._queryParams).Where(d=>d.StatusDocumento==1000).FirstOrDefault();

                if (_documento != null)
                {
                    this._queryParams.Clear();
                    this._queryParams["Usuario_ID"] = idUsuario;
                    this._queryParams["Documento_ID"] = _documento.ID;
                    this._queryParams["Servico_ID"] = idServico;

                    _documento.Modelo.Campos.AddRange(_repository.SelecionarDocumentoCampos(_queryParams).Where(c=>c.Digita).ToList<CampoModelo>());
                    _documento.Arquivos.AddRange(_repository.SelecionarDocumentoImagens(_queryParams).ToList<DocumentoImagem>());

                    this._queryParams.Clear();
                    this._queryParams["Documento"] = _documento;
                    this._queryParams["Servico_ID"] = idServico;
                    _repository.AtualizarDocumento(_queryParams);
                }
                else
                {
                    _documento = new Documento();
                }

                return _documento;

            }
            catch (Exception ex) { throw ex; }

        }

        public Documento GetDocumentoFormalizar(int idUsuario, int idServico)
        {
            try
            {
                Documento _documento = new Documento();

                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Servico_ID"] = idServico;

                //TODO: Atualizar a data de atualização do doc e pegar apenas os que estiverem com data de atualização maior que 10 minutos
                _documento = this._repository.ListarDocumentosStatus(this._queryParams).Where(d => d.StatusDocumento == 2000).FirstOrDefault();

                if (_documento != null)
                {
                    this._queryParams.Clear();
                    this._queryParams["Usuario_ID"] = idUsuario;
                    this._queryParams["Documento_ID"] = _documento.ID;
                    this._queryParams["Servico_ID"] = idServico;

                    _documento.Perguntas = this._repository.ListarPerguntas(int.Parse(_queryParams["Servico_ID"].ToString()), _documento.Modelo.ID); 
                    
                    _documento.Modelo.Campos.AddRange(_repository.SelecionarDocumentoCampos(_queryParams).Where(c => c.Digita).ToList<CampoModelo>());
                    _documento.Arquivos.AddRange(_repository.SelecionarDocumentoImagens(_queryParams).ToList<DocumentoImagem>());

                    this._queryParams.Clear();
                    this._queryParams["Documento"] = _documento;
                    this._queryParams["Servico_ID"] = idServico;
                    _repository.AtualizarDocumento(_queryParams);
                }
                else
                {
                    _documento = new Documento();
                }

                return _documento;

            }
            catch (Exception ex) { throw ex; }

        }

        public Documento GetDocumentoVincular(int idUsuario, int idServico)
        {
            try
            {
                Documento _documento = new Documento();

                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Servico_ID"] = idServico;

                //TODO: Atualizar a data de atualização do doc e pegar apenas os que estiverem com data de atualização maior que 10 minutos
                _documento = this._repository.ListarDocumentosStatus(this._queryParams).Where(d => d.StatusDocumento == 3000).FirstOrDefault();

                if (_documento != null)
                {
                    this._queryParams.Clear();
                    this._queryParams["Usuario_ID"] = idUsuario;
                    this._queryParams["Documento_ID"] = _documento.ID;
                    this._queryParams["Servico_ID"] = idServico;

                    //_documento.Perguntas = this._repository.ListarPerguntas(int.Parse(_queryParams["Servico_ID"].ToString()), _documento.Modelo.ID);

                    _documento.Modelo.Campos.AddRange(_repository.SelecionarDocumentoCampos(_queryParams).Where(c => c.Digita).ToList<CampoModelo>());
                    _documento.Arquivos.AddRange(_repository.SelecionarDocumentoImagens(_queryParams).ToList<DocumentoImagem>());

                    this._queryParams.Clear();
                    this._queryParams["Documento"] = _documento;
                    this._queryParams["Servico_ID"] = idServico;
                    _repository.AtualizarDocumento(_queryParams);
                }
                else
                {
                    _documento = new Documento();
                }

                return _documento;

            }
            catch (Exception ex) { throw ex; }

        }

        //proc_campo_documento_sel
        public List<CampoModelo> ListarCamposModelo(int idDocumentoModelo)
        {
            try
            {

                List<CampoModelo> _campos = new List<CampoModelo>();
                this._queryParams.Clear();
                this._queryParams["DocumentoModelo_ID"] = idDocumentoModelo;
                _campos.AddRange(_repository.ListarCamposModelo(_queryParams).ToList<CampoModelo>());
                
                return _campos;

            }
            catch (Exception ex) { throw ex; }
        }
        public void AtualiarDocumentoCB(int idUsuario, int idServico, int idLote, bool verso, string CB, string NomeImagemFim)
        {
            try
            {
                string _retorno = string.Empty;
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = idUsuario;
                this._queryParams["Servico_ID"] = idServico;
                this._queryParams["Lote_ID"] = idLote;
                this._queryParams["Verso"] = verso;
                this._queryParams["CB"] = CB;
                this._queryParams["NomeImagemFim"] = NomeImagemFim;
                _retorno = this._repository.AtualiarDocumentoCB(this._queryParams);
             }
            catch (Exception ex) { throw ex; }        
        }

        public List<ConsultaDetalhe> ListarConsultaDetalhe(int idServico, int idDocumento, int idLote)
        {
            try{
                    return this._repository.ListarConsultaDetalhe(idServico, idDocumento, idLote);
            }
            catch (Exception ex) { throw new Exception("Erro em ListarConsultaDetalhe: "+ex.Message); }        
        }

        #endregion IUserService Members
    }
      
}
