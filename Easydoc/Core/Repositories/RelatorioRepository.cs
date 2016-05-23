using Microsoft.Practices.EnterpriseLibrary.Data;
using Newtonsoft.Json;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using TecFort.Framework.Generico;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.Core.Repositories.Interfaces;


namespace MK.Easydoc.Core.Repositories
{
    public class RelatorioRepository
    {

        public RelatorioRepository()
        {
        }

        public List<Relatorio> Listar(int idServico)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("Get_Relatorio_Servico");
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, idServico);
                var lista = new List<Relatorio>();
                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        lista.Add(new Relatorio()
                        {
                            idRelatorio = int.Parse(_dr["idRelatorio"].ToString()),
                            Titulo = _dr["Titulo"].ToString(),
                            Descricao = _dr["Descricao"].ToString(),
                            ScriptSQLRelatorio = _dr["ScriptSQLRelatorio"].ToString(),
                            ListaRelatorioParametro = ListarParametros(idServico, int.Parse(_dr["idRelatorio"].ToString()))
                        });
                    }
                }
                return lista;
            }
            catch (Exception ex) { throw new Exception("Erro ao listar Relatorio, "+ex.Message); }
        }

        public List<RelatorioParametros> ListarParametros(int idServico, int idRelatorio)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("Get_Parametro_Relatorio");
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, idServico);
                _db.AddInParameter(_cmd, "@idRelatorio", DbType.Int16, idRelatorio);
                var lista = new List<RelatorioParametros>();
                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        lista.Add(new RelatorioParametros()
                        {
                            Descricao = _dr["Descricao"].ToString(), 
                            MascaraEntrada = _dr["MascaraEntrada"].ToString(),
                            ProcSqlValidacao = _dr["ProcSqlValidacao"].ToString(),
                            ControleWEB = _dr["ControleWEB"].ToString(),
                            TipoSQL = _dr["TipoSQL"].ToString(),
                            RotuloAbreviado = _dr["RotuloAbreviado"].ToString(),
                            MaxLength = int.Parse(_dr["MaxLength"].ToString()),
                            MascaraSaida = _dr["MascaraSaida"].ToString()
                        });
                    }
                }
                return lista;
            }
            catch (Exception ex) { throw new Exception("Erro ao ListarParametros do Relatorio, " + ex.Message); }
        }

        /* public DataTable ListarDocsVinculoPai(int _idServico, int _idDocumentoModelo, int _tipo)
         {
             //DocumentoModelo _modelo = new DocumentoModelo();
             //Documento _documento = new Documento();
             DataTable _lista = new DataTable();
             try
             {
                 DbCommand _cmd;
                 Database _db = DbConn.CreateDB();
                 _cmd = _db.GetStoredProcCommand(String.Format("proc_listar_docto_pais"));
                 _db.AddInParameter(_cmd, "@idServico", DbType.Int32, _idServico);
                 _db.AddInParameter(_cmd, "@idDocumentoModelo", DbType.Int32, _idDocumentoModelo);
                 _db.AddInParameter(_cmd, "@tipo", DbType.Int32, _tipo);

                 using (IDataReader _dr = _db.ExecuteReader(_cmd))
                 {
                     _lista.Load(_dr);
                     //while (_dr.Read())
                     //{
                        
                     //}
                 }

                 if (_lista == null) { throw new Erro("Documento não localizado."); }
                 return _lista;
             }
             catch (Exception ex)
             {
                 throw;
             }
         } */
    }
}
