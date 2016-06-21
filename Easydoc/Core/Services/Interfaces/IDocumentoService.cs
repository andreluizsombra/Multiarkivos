using System;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;
using System.Data;
namespace MK.Easydoc.Core.Services.Interfaces
{
    public interface IDocumentoService
    {
        DataTable ListarDocumentosVinculoPai(int idServico, int idDocModelo, int tipo);
        void AtualizarDocumento(Documento Documento, int idServico);
        //void IncluirMotivo(int IdDocumento, int Atalho, int UserID, int tipo);
        void IncluirMotivo(int idServico, int idDocumento, int idOcorrencia, int tipo, int UserID);
        void AlteraDuplicidade(int IdDocumento, int ID);
        void AtualizarDocumentoCampo(CampoModelo campoModelo);
        void ExcluirDocumento(int idDocumento);
        void MudaStatusDocumento(int idDocumento, int idUsuario,int idStatus, int idServico);
        string ValidarCamposDocumento(int idDocumentoModelo, CampoModelo campoModelo, int idServico);
        void FinalizarDigitacao(int idDocumento, int id_Servico);
        DocumentoModelo CriarDocumento(int idUsuario, int idOrigem, int idServico);
        List<Documento> ListarDocumentosDigitar(int idUsuario, int idOrigem, int idServico);
        List<Documento> ListarDocumentosTipificar(int idUsuario, int idOrigem, int idServico);
        List<Documento> ListarDocumentosSupervisao(int idUsuario, int idOrigem, int idServico);
        List<Documento> ListarDocumentosFormalizar(int idUsuario, int idOrigem, int idServico);
        List<Documento> ListarDocumentosVincular(int idUsuario, int idOrigem, int idServico);
        List<DocumentoModelo> ListarTipos(int idServico);
        List<DocumentoModelo> ListarTiposConsulta(int idServico);
        Documento GetDocumentoDigitar(int idUsuatio, int idServico);
        Documento SelecionaDocumentoDigitar(int idUsuatio, int idServico, int idDocumento);
        //List<Motivo> GetMotivoDigitar(int idServico, int Tipo);
        bool EmUso(int idDocumento, int idUsuario, int Tipo, int idServico);
        string GetDuplicidade(int idDocumento, int idservico);
        string GetMotivo(int idDocumento);
        string GetStatusDocumento(int idDocumento, int idServico);
        string GetDocumentoModelo(int idDocumento);
        string Executar(string cmd);
        List<CampoModelo> ListarCamposModelo(int idDocumentoModelo, int idServico);
        string PesquisarDocumentosConsulta(int idServico, int idDocumentoModelo, string campos, string scriptWhere);
        string PesquisarDocumentosModulo(int idServico, int idDocumentoModelo, string campos, string scriptWhere);
        string PesquisarMotivo(int idDocumentoModelo, int idServico);
        //string PesquisarDet(int idDocumentoModelo);
        string ValidarDocumento(int idDocumento, int idDocumentoModelo, int idServico);
        string SalvarConsultaDocumentoModelo(int idUsuario, int idServico, int idDocumentoModelo, string nomeConsulta, string descricao, bool compartilhado, string stringJSON);
        void AtualiarDocumentoCB(int idUsuario, int idServico, int idLote, bool verso, string CB, string NomeImagemFim);
        List<DocumentoConsulta> ListarConsultasModelo(int idUsuario, int idServico);
        List<ConsultaDetalhe> ListarConsultaDetalhe(int idServico, int idDocumento, int idLote);

        Documento GetDocumentoFormalizar(int idUsuatio, int idServico);
        Documento GetDocumentoVincular(int idUsuario, int idServico);
        List<Ocorrencia> GetMotivoDigitar(int idServico, int Tipo);
    }
}
