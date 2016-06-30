using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.WebApp.ViewModels
{
    public class TipificarViewModel
    {
        public DocumentoModelo TipoDocumentoSelecionado { get; set; }
        public int Lote_ID { get; set; }
        public int QtdeItensLote { get; set; }
        public int QtdeItensLotePendente { get; set; }
        public string PathImagem { get; set; }
        public string CaminhoImg { get; set; }
        public int Nuvem { get; set; }
        
        public  TipificarViewModel(){
            TipoDocumentoSelecionado = new DocumentoModelo();
            Lote_ID = 0;
            QtdeItensLote = 0;
            QtdeItensLotePendente = 0;
            PathImagem = string.Empty;
            CaminhoImg = string.Empty;
            Nuvem = 0;
        }
    }
}