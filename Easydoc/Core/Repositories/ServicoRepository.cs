using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using TecFort.Framework.Generico;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.Core.Repositories.Interfaces;

namespace MK.Easydoc.Core.Repositories
{
    public sealed class ServicoRepository : IServicoRepository
    {
        #region Constructors

        //public ServicoRepository(ILog logger, IConsumer consumer)
        public ServicoRepository()
        {

            //_servico = DependencyResolver.Current.GetService<IServicoService>();

            //this._logger = logger;
            //this._consumer = consumer;
        }

        #endregion

        private Servico ConverteBaseObjeto(IDataReader dt) {
            Servico _servico = new Servico();
            _servico = (new Servico
            {
                ID = int.Parse(dt["IdServico"].ToString()),
                Descricao = dt["Descricao"].ToString(),
                Default = bool.Parse(dt["ServicoDefault"].ToString()),
                Documentos = new List<DocumentoModelo>(),
                Modulos = new List<Modulo>(),
                IdCliente = int.Parse(dt["IdCliente"].ToString()),
                ScriptSQLDashboard_Captura = dt["ScriptSQLDashboard_Captura"].ToString(),
                ScriptSQLDashboard_Pendencias = dt["ScriptSQLDashboard_Pendencias"].ToString(),
                ScriptSQLDashboard_Doc_Modulo = dt["ScriptSQLDashboard_Doc_Modulo"].ToString()
            });
            return _servico;
        
        }

        #region IServicoRepository Members

        public List<Graficos> ExibirDashboard_Doc_Modulo(IDictionary<string, object> _queryParams)
        {
            try
            {
                string scriptConsulta = GetServico(_queryParams).ScriptSQLDashboard_Doc_Modulo;

                List<Graficos> _itens = new List<Graficos>();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format(scriptConsulta));

                _db.AddInParameter(_cmd, "@PeriodoInicial", DbType.Int32, int.Parse(_queryParams["periodoInicial"].ToString()));
                _db.AddInParameter(_cmd, "@PeriodoFinal", DbType.Int32, int.Parse(_queryParams["periodoFinal"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _itens.Add(new Graficos() { label = _dr["Descricao"].ToString(), value = int.Parse(_dr["Quantidade"].ToString()) });
                    }
                }
                

                return _itens;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar o método ExibirDashboard_Doc_Modulo, detalhes: " + ex.Message);
            }
        }

        public List<GraficoAreaChart> ExibirDashboard_Captura(IDictionary<string, object> _queryParams)
        {
            try
            {
                string scriptConsulta = GetServico(_queryParams).ScriptSQLDashboard_Captura;

                List<GraficoAreaChart> _itens = new List<GraficoAreaChart>();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format(scriptConsulta));

                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                _db.AddInParameter(_cmd, "@PeriodoInicial", DbType.Int32, int.Parse(_queryParams["periodoInicial"].ToString()));
                _db.AddInParameter(_cmd, "@PeriodoFinal", DbType.Int32, int.Parse(_queryParams["periodoFinal"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _itens.Add(new GraficoAreaChart() { Data_Captura = _dr["Data_Captura"].ToString(), Quantidade_Capturada = int.Parse(_dr["Quantidade_Capturada"].ToString()) });
                    }
                }

                return _itens;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar o método ExibirDashboard_Doc_Captura, detalhes: " + ex.Message);
            }
        }

        //TODO: AndreSombra 19/11/2015
        public List<GraficoPendencias> ExibirDashboard_Pendencias(IDictionary<string, object> _queryParams)
        {
            try
            {
                string scriptConsulta = GetServico(_queryParams).ScriptSQLDashboard_Pendencias;

                List<GraficoPendencias> _itens = new List<GraficoPendencias>();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format(scriptConsulta));

                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, int.Parse(_queryParams["Servico_ID"].ToString()));
                _db.AddInParameter(_cmd, "@PeriodoInicial", DbType.Int32, int.Parse(_queryParams["periodoInicial"].ToString()));
                _db.AddInParameter(_cmd, "@PeriodoFinal", DbType.Int32, int.Parse(_queryParams["periodoFinal"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _itens.Add(new GraficoPendencias() { Descricao = _dr["Descricao"].ToString(), Quantidade = int.Parse(_dr["Quantidade"].ToString()) });
                    }
                }

                return _itens;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar o método ExibirDashboard_Doc_Pendencias, detalhes: " + ex.Message);
            }
        }


        public List<Servico> ListarServicosUsuario(IDictionary<string, object> _queryParams)
        {
            try
            {
                List<Servico> _servicos = new List<Servico>();
                
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_servicos_usuario_sel"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _servicos.Add(ConverteBaseObjeto(_dr));
                    }
                }
                if (_servicos == null) { throw new Erro("Servico não localizado."); }
                return _servicos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool GetControleAtencao(int _idServico)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Proc_Get_ControleAtencao"));

                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, _idServico);

                int flag = 0;
                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        flag = int.Parse(_dr[0].ToString());
                    }
                }
                ///if (_servicos == null) { throw new Erro("Servico não localizado."); }
                return flag==1;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no método GetControleAtencao, "+ex.Message);
            }
        }

        public Servico GetServico(IDictionary<string, object> _queryParams)
        { 
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                Servico _servico = new Servico();

                _cmd = _db.GetStoredProcCommand(String.Format("proc_servico_get"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int16, _queryParams["Servico_ID"]);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _servico = ConverteBaseObjeto(_dr);
                    }
                }
                if (_servico == null) { throw new Erro("Servico não localizado."); }
                return _servico;
            }
            catch (Exception ex) { throw new Erro(ex.Message);  }        
        }

        public Servico GetServico(int _usuarioID, int _servicoID)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                Servico _servico = new Servico();

                _cmd = _db.GetStoredProcCommand(String.Format("proc_servico_get"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, _usuarioID);
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int16, _servicoID);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _servico = ConverteBaseObjeto(_dr);
                    }
                }
                if (_servico == null) { throw new Erro("Servico não localizado."); }
                return _servico;
            }
            catch (Exception ex) { throw new Erro(ex.Message); }
        }		

        #endregion IServicoRepository Members

    }
}
