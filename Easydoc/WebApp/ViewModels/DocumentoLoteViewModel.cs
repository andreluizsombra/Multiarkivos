using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.WebApp.ViewModels
{
    public class DocumentoLoteViewModel
    {
        public Lote Lote { get; set; }
        public Documento Documento { get; set; }
        public string CamposDocumentoJSON { get; set; }
        public string CamposDocumentoDetalhe { get; set; }
    }
}