using System;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;
using System.Data;
namespace MK.Easydoc.Core.Repositories.Interfaces
{
    public interface IDocumentoRepository
    {
        DataTable ListarDocsVinculoPai(int _idServico, int _idDocumentoModelo, int _tipo);
        //DocumentoModelo AtualizarDocumento(IDictionary<string, object> _queryParams);
        DocumentoModelo CriarDocumento(IDictionary<string, object> _queryParams);
        List<Documento> ListarDocumentosStatus(IDictionary<string, object> _queryParams);
        Documento SelecionarDocumento(IDictionary<string, object> _queryParams);
        List<CampoModelo> SelecionarDocumentoCampos(IDictionary<string, object> _queryParams);
        List<DocumentoImagem> SelecionarDocumentoImagens(IDictionary<string, object> _queryParams);
        List<DocumentoModelo> ListarTipos(IDictionary<string, object> _queryParams);
        List<DocumentoModelo> ListarTiposConsulta(int idServico);
        List<CampoModelo> ListarCamposModelo(IDictionary<string, object> _queryParams);
        List<Perguntas> ListarPerguntas(int _idServico, int _idDocumentoModelo);
        
        //Walmir
        //List<Motivo> SelecionarMotivo(IDictionary<string, object> _queryParams);
        string GetDuplicidade(IDictionary<string, object> _queryParams);
        bool EmUso(IDictionary<string, object> _queryParams);
        string GetMotivo(IDictionary<string, object> _queryParams);
        string GetStatusDocumento(IDictionary<string, object> _queryParams);
        string GetDocumentoModelo(IDictionary<string, object> _queryParams);
        string Executar(IDictionary<string, object> _queryParams);
        //void AtualizarDocumento(Documento Documento);
        void IncluirMotivo(IDictionary<string, object> _queryParams);
        void AlteraDuplicidade(IDictionary<string, object> _queryParams);
        void AtualizarDocumento(IDictionary<string, object> _queryParams);
        void AtualizarDocumentoCampo(IDictionary<string, object> _queryParams);
        void ExcluirDocumento(IDictionary<string, object> _queryParams);
        void MudaStatusDocumento(IDictionary<string, object> _queryParams);
        string PesquisarDocumentosConsulta(IDictionary<string, object> _queryParams);
        string PesquisarDocumentosModulo(IDictionary<string, object> _queryParams);
        string PesquisarMotivo(IDictionary<string, object> _queryParams);
        string ValidarCamposDocumento(IDictionary<string, object> _queryParams);
        string ValidarDocumento(IDictionary<string, object> _queryParams);
        string SalvarConsultaDocumentoModelo(IDictionary<string, object> _queryParams);
        string AtualiarDocumentoCB(IDictionary<string, object> dictionary);
        List<DocumentoConsulta> ListarConsultasModelo(IDictionary<string, object> dictionary);
        List<ConsultaDetalhe> ListarConsultaDetalhe(int idServico, int idDocumento, int idLote);
        List<Ocorrencia> ListaOcorrencia(int _idServico, int _tipo);
        //List<Ocorrencia> GetMotivoDigitar(int idServico, int Tipo);
    }
}
