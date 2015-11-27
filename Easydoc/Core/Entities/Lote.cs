using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Entities
{
    public class Lote
    {
        public int ID { get; set; }
        public DateTime DataCriacao { get; set; }
        public int QtdeImagem { get; set; }
        public bool Duplex { get; set; }
        public string PathCaptura { get; set; }
        public Usuario UsuarioCaptura { get; set; }
        public Servico ServicoCaptura { get; set; }
        public int StatusLote { get; set; }
        public int OrigemID { get; set; }
        public List<LoteItem> Itens { get; set; }
        public LogLote Log { get; set; }

        public Lote() { 
            ID =0;
            DataCriacao = DateTime.MinValue;
            QtdeImagem = 0;
            Duplex = false;
            PathCaptura = string.Empty;
            UsuarioCaptura = new Usuario();
            ServicoCaptura = new Servico();
            StatusLote =0;
            OrigemID =0;
            Itens = new List<LoteItem>();
            Log = new LogLote();
        }

        public Lote(Lote _lote)
        {
            ID = _lote.ID;
            
            DataCriacao = _lote.DataCriacao;
            QtdeImagem = _lote.QtdeImagem;
            Duplex = _lote.Duplex;
            PathCaptura = _lote.PathCaptura;
            UsuarioCaptura = _lote.UsuarioCaptura;
            ServicoCaptura = _lote.ServicoCaptura;
            StatusLote = _lote.StatusLote;
            OrigemID = _lote.OrigemID;
            Itens = _lote.Itens;
        }

    }
    public class LoteItem
    {
        public int ID { get; set; }
        public int IdLote { get; set; }
        public string NomeOriginal { get; set; }
        public string NomeFinal { get; set; }
        public int OrigemID { get; set; }
        public int SequenciaCaptura { get; set; }
        public bool Verso{ get; set; }

        public DateTime DataCriacaoArqCap { get; set; }

        public DateTime DataCaptura { get; set; }
        public Usuario UsuarioCaptura { get; set; }
        public int StatusImagem { get; set; }
        public DocumentoModelo DocumentoModelo { get; set; }

        public LoteItem() { 
            ID=0;
            IdLote = 0;
            NomeOriginal = string.Empty;
            NomeFinal = string.Empty;
            OrigemID = 0;
            SequenciaCaptura = 0;
            Verso = false;
            DataCaptura = DateTime.MinValue;
            DataCriacaoArqCap = DateTime.MinValue;

            UsuarioCaptura = new Usuario();
            StatusImagem = 0;
            DocumentoModelo = new DocumentoModelo();
        }
    }

    public class LogLote {
        public string NomeLote { get; set; }
        public string DataAbertura { get; set; }
        public string DataFim { get; set; }
        public string Login { get; set; }
        public string Estacao { get; set; }
        public string Scanner { get; set; }
        public int NumeroPaginas { get; set; }
        public string[] Dados { get; set; }
        public string Versao { get; set; }
        public int IdPerfil { get; set; }
        public List<Arquivo> Arquivos { get; set; }
        public LogLote() { Arquivos = new List<Arquivo>(); }
    }

    public class Arquivo {
        public string Nome { get; set; }
        public bool Verso { get; set; }
        public string CB { get; set; }
        public Arquivo() { Nome = string.Empty; Verso = false; CB = string.Empty; }
    }

}
