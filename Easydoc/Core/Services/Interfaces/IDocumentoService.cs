using System;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;
namespace MK.Easydoc.Core.Services.Interfaces
{
    public interface IDocumentoService
    {
        void AtualizarDocumento(Documento Documento);
        void IncluirMotivo(int IdDocumento, int Atalho, int UserID, int tipo);
        void AlteraDuplicidade(int IdDocumento, int ID);
        void AtualizarDocumentoCampo(CampoModelo campoModelo);
        void ExcluirDocumento(int idDocumento);
        void MudaStatusDocumento(int idDocumento, int idUsuario,int idStatus);
        string ValidarCamposDocumento(int idDocumentoModelo, CampoModelo campoModelo);
        void FinalizarDigitacao(int idDocumento);
        DocumentoModelo CriarDocumento(int idUsuario, int idOrigem, int idServico);
        List<Documento> ListarDocumentosDigitar(int idUsuario, int idOrigem, int idServico);
        List<Documento> ListarDocumentosTipificar(int idUsuario, int idOrigem, int idServico);
        List<Documento> ListarDocumentosSupervisao(int idUsuario, int idOrigem, int idServico);
        List<DocumentoModelo> ListarTipos(int idServico);
        Documento GetDocumentoDigitar(int idUsuatio, int idServico);
        Documento SelecionaDocumentoDigitar(int idUsuatio, int idServico, int idDocumento);
        List<Motivo> GetMotivoDigitar(int idServico, int Tipo);
        bool EmUso(int idDocumento, int idUsuario, int Tipo);
        string GetDuplicidade(int idDocumento);
        string GetMotivo(int idDocumento);
        string GetStatusDocumento(int idDocumento);
        string GetDocumentoModelo(int idDocumento);
        string Executar(string cmd);
        List<CampoModelo> ListarCamposModelo(int idDocumentoModelo);
        string PesquisarDocumentosConsulta(int idServico, int idDocumentoModelo, string campos, string scriptWhere);
        string PesquisarDocumentosModulo(int idServico, int idDocumentoModelo, string campos, string scriptWhere);
        string PesquisarMotivo(int idDocumentoModelo);
        //string PesquisarDet(int idDocumentoModelo);
        string ValidarDocumento(int idDocumento, int idDocumentoModelo, int idServico);
        string SalvarConsultaDocumentoModelo(int idUsuario, int idServico, int idDocumentoModelo, string nomeConsulta, string descricao, bool compartilhado, string stringJSON);
        void AtualiarDocumentoCB(int idUsuario, int idServico, int idLote, bool verso, string CB, string NomeImagemFim);
        List<DocumentoConsulta> ListarConsultasModelo(int idUsuario, int idServico);
    }
}
