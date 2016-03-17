using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MK.Easydoc.Core.Entities
{
    public class TipoTipificacao {
        public int ID { get; set; }
        public string Descicao { get; set; }
    }
    public class DocumentoModelo
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public string Rotulo { get; set; }
        public bool MultiPagina { get; set; }
        public bool Duplex { get; set; }
        public int NumeroPagina { get; set; }
        public string ScriptSQLTipificar { get; set; }
        public string ScriptSQLValidar { get; set; }
        public string ScriptSQLModulo { get; set; }
        public string ScriptSQLConsulta { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataExclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public Servico Servico { get; set; }
        public List<CampoModelo> Campos { get; set; }
        public int Tipificalote { get; set; }

        public DocumentoModelo (){ 
            ID = 0;
            Descricao = string.Empty;
            Rotulo = string.Empty;
            MultiPagina = false;
            Duplex = false;
            NumeroPagina = 0;
            ScriptSQLTipificar = string.Empty;
            ScriptSQLValidar = string.Empty;
            ScriptSQLModulo = string.Empty;
            ScriptSQLConsulta = string.Empty;
            Campos = new List<CampoModelo>();
            Servico = new Servico();
            Tipificalote = 0;
        }

    }
    public class CampoModelo
    {
        public int ID { get; set; }
        public int IndexDoc { get; set; }
        public string IndexUI { get; set; }
        public string Descricao { get; set; }
        public string Rotulo { get; set; }
        public string RotuloAbreviado { get; set; }
        public string MascaraEntrada { get; set; }
        public string MascaraSaida { get; set; }
        public string ProcSqlValidacao { get; set; }
        public string MetodoValidacao { get; set; }
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public int Tabulacao { get; set; }
        public string TipoSQL { get; set; }
        public bool Requerido { get; set; }
        public bool Digita { get; set; }
        public bool Reconhece { get; set; }
        public bool FiltroConsulta { get; set; }
        public string RegexString { get; set; }
        public string ControleWEB { get; set; }
        public string ControleDesk { get; set; }
        public string ClasseCSS { get; set; }
        public string AtributosHTML { get; set; }
        public string ValorPadrao { get; set; }
        public string Valor { get; set; }
        public int Movecampo { get; set; }
        public int Maiuscula { get; set; }

        public CampoModelo()
        {
            ID = 0;
            IndexDoc = 0;
            IndexUI = string.Empty;
            Descricao = string.Empty;
            Rotulo = string.Empty;
            RotuloAbreviado = string.Empty;
            MascaraEntrada = string.Empty;
            MascaraSaida = string.Empty;
            ProcSqlValidacao = string.Empty;
            MetodoValidacao = string.Empty;
            MaxLength = 0;
            MinLength = 0;
            Tabulacao = 0;

            Requerido = false;
            Digita = true;
            Reconhece = false;
            FiltroConsulta = true;

            TipoSQL = string.Empty;
            RegexString = string.Empty;
            ControleWEB = string.Empty;
            ControleDesk = string.Empty;
            ClasseCSS = string.Empty;
            AtributosHTML = string.Empty;
            ValorPadrao = string.Empty;
            Valor = string.Empty;
            
            Movecampo = 0;
            Maiuscula = 0;
        }
    }


    public class DocumentoImagem
    {
        public int ID { get; set; }
        public string CaminhoArquivo { get; set; }
        public bool Verso { get; set; }
        public int NumeroPagina { get; set; }

    }

    public class Documento {
        public int ID { get; set; }
        public int StatusDocumento { get; set; }
        public DocumentoModelo Modelo { get; set; }
        public List<DocumentoImagem> Arquivos { get; set; }
        public List<Motivo> Motivos { get; set; }//walmir

        public Documento()
        {
            ID = 0;
            StatusDocumento = 0;
            Modelo = new DocumentoModelo();
            Arquivos = new List<DocumentoImagem>();
        }    
    }
    
    public class Motivo
    {
        public int ID { get; set; }
        public string descricao { get; set; }
        public int atalho { get; set; }
        
        public Motivo()
        {
            ID = 0;
            descricao = "";
            atalho = 0;            
        }
    }

    public class Ocorrencia
    {
        //TODO: 16/03/2016
        public int IdOcorrencia { get; set; }
        public string descOcorrencia  { get; set; }
        public int idCliente { get; set; }
        public int idServico { get; set; }
        public string nomeCliente { get; set; }
        public string nomeServico { get; set; }
        public int TipoAcao { get; set; }
        public int idUsuario { get; set; }
        public Ocorrencia()
        {
            IdOcorrencia = 0;
            descOcorrencia = "";
            idCliente = 0;
            idServico = 0;
            nomeCliente = "";
            nomeServico = "";
            TipoAcao = 0;
            idUsuario = 0;
        }
    }

    public class DocumentoConsulta {

        public int ID { get; set; }
        public DocumentoModelo Documento { get; set; }
        public Usuario Usuario { get; set; }
        public Servico Servico { get; set; }
        public string Rotulo { get; set; }
        public string Descricao { get; set; }
        public bool Compartilhado { get; set; }
        public string ModeloJSON { get; set; }
    
    }

    public class ConsultaDetalhe
    {
        public string Descricao { get; set; }
        public string PathArquivo { get; set; }
    }
}