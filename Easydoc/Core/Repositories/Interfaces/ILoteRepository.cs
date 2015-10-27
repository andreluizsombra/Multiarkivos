using System;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;
namespace MK.Easydoc.Core.Repositories.Interfaces
{
    public interface ILoteRepository
    {
        Lote AtualizarLote(IDictionary<string, object> _queryParams);
        Lote CriarLote(IDictionary<string, object> _queryParams);
        int ApagarLote(IDictionary<string, object> _queryParams);
        int VerificaLoteJaExiste(IDictionary<string, object> _queryParams);//walmir
        bool UsaArquivoDados(IDictionary<string, object> _queryParams);//walmir
        List<Lote> ListarLotesStatus(IDictionary<string, object> _queryParams);
        List<Lote> ListarLotes(IDictionary<string, object> _queryParams);
        List<LoteItem> ListarItensLote(IDictionary<string, object> _queryParams);
        int InserirItemLote(IDictionary<string, object> _queryParams);
        bool TipificarItem(IDictionary<string, object> _queryParams);
        //bool ExcluirLote(IDictionary<string, object> _queryParams);
    }

}
