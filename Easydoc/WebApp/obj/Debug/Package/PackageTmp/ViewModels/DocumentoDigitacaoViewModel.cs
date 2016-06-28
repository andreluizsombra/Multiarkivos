using System.Collections.Generic;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.WebApp.ViewModels
{
    public class DocumentoDigitacaoViewModel
    {
        public int IdDocumento { get; set; }
        public List<CampoModelo> Campos { get; set; }
    }    
}