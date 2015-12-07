using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.Core.Services.Interfaces
{
    public interface ILoteService
    {
        #region Methods
        Lote CriarLote(int idUsuario, int idOrigem, int idServico, string path);
        bool ApagarLote(int idLote);
        //bool ExcluirLote(int idLote);
        bool UsaArquivoDados(int idServico);
        int VerificaLoteJaExiste(int idLote,string NomeLote);//walmir
        List<Lote> ListarLotesPendente(int idUsuario, int idOrigem, int idServico);
        List<Lote> ListarLotesTipificar(int idUsuario, int idOrigem, int idServico);
        List<LoteItem> ListarItensLote(int idLote, int idUsuario, int idServico, bool SemTipo, int StatusImagem);
        Lote GetLote(int idLote, int idUsuario, int idServico, int numItem);
        Lote AtualizarLote(Lote lote);
        int InserirItensLote(Lote lote);
        bool TipificarItem(int idUsuario, int idServico, int idLote, int idLoteItem, int idDocumentoModelo);
        bool TipificarItemDescricao(int idUsuario, int idServico, int idLote, int idLoteItem, string descricaoDocumentoModelo);
        
        #endregion Methods
    }
}
