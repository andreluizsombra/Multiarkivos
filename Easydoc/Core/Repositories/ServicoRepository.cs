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
            //Servico _servico = new Servico();
            var _servico = new Servico(){
                ID = int.Parse(dt["IdServico"].ToString()),
                Descricao = dt["Descricao"].ToString(),
                Default = bool.Parse(dt["ServicoDefault"].ToString()),
                Documentos = new List<DocumentoModelo>(),
                Modulos = new List<Modulo>(),
                IdCliente = int.Parse(dt["IdCliente"].ToString()),

                NomeCliente = dt["NomeCliente"].ToString(),

                ServicoDefault = bool.Parse(dt["ServicoDefault"].ToString()),
                ArquivoDados = bool.Parse(dt["ArquivoDados"].ToString()),
                ControleAtencao = bool.Parse(dt["ControleAtencao"].ToString()),

                ScriptSQLDashboard_Captura = dt["ScriptSQLDashboard_Captura"].ToString(),
                ScriptSQLDashboard_Pendencias = dt["ScriptSQLDashboard_Pendencias"].ToString(),
                ScriptSQLDashboard_Doc_Modulo = dt["ScriptSQLDashboard_Doc_Modulo"].ToString()
            };
            return _servico;
        
        }

        #region IServicoRepository Members

        public Dictionary<int, string> ListaSituacao()
        {
            Dictionary<int, string> mDic = new Dictionary<int, string>();
            mDic.Add(9, "Selecione");
            mDic.Add(0, "Ativo");
            mDic.Add(1, "Bloqueado");
            return mDic;
        }

        public List<Graficos> ExibirDashboard_Doc_Modulo(IDictionary<string, object> _queryParams)
        {
            try
            {
                string scriptConsulta = GetServico(_queryParams).ScriptSQLDashboard_Doc_Modulo;

                List<Graficos> _itens = new List<Graficos>();
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
                        _itens.Add(new Graficos() { label = _dr["Descricao"].ToString(), value = int.Parse(_dr["Quantidade"].ToString()), Header = _dr["Header"].ToString() });
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
                        _itens.Add(new GraficoAreaChart() { Data_Captura = _dr["Data_Captura"].ToString(), Quantidade_Capturada = int.Parse(_dr["Quantidade_Capturada"].ToString()), Header = _dr["Header"].ToString() });
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
                        _itens.Add(new GraficoPendencias() { Descricao = _dr["Descricao"].ToString(), Quantidade = int.Parse(_dr["Quantidade"].ToString()), Header = _dr["Header"].ToString() });
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

        public List<Modulo> ListaModulos(int _idServico)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Get_MenuModuloPorServico"));

                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, _idServico);

                var _Mod = new List<Modulo>();
                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _Mod.Add(new Modulo() { CodRetorno = int.Parse(_dr[0].ToString()), Mensagem = _dr[1].ToString(), ID = int.Parse(_dr[2].ToString()), Descricao = _dr[3].ToString() });
                    }
                }

                return _Mod;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no método ListaModulos, " + ex.Message);
            }
        }


        public List<Modulo> BuscarCheckModulos(int _idServico, int _idPerfil)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Proc_GetPerfilModuloPorServico"));

                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, _idServico);
                _db.AddInParameter(_cmd, "@idPerfil", DbType.Int32, _idPerfil);

                var _Mod = new List<Modulo>();
                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _Mod.Add(new Modulo() { CodRetorno = int.Parse(_dr[0].ToString()) });
                    }
                }

                return _Mod;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no método ListaModulos, " + ex.Message);
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

        public Retorno Incluir(Servico ser) 
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("proc_Manutencao_Servico");
                _db.AddInParameter(_cmd, "@TipoAcao", DbType.Int16, ser.TipoAcao); //(1)Criar, (2)Alterar, (3)Excluir
                _db.AddInParameter(_cmd, "@idCliente", DbType.Int16, ser.IdCliente);
                _db.AddInParameter(_cmd, "@Descricao", DbType.String, ser.Descricao);
                _db.AddInParameter(_cmd, "@ServicoDefault", DbType.Boolean, ser.ServicoDefault);
                _db.AddInParameter(_cmd, "@ArquivoDAdos", DbType.Boolean, ser.ArquivoDados);
                _db.AddInParameter(_cmd, "@ControleAtencao", DbType.Boolean, ser.ControleAtencao);
                _db.AddInParameter(_cmd, "@DataCriacao", DbType.Int16, 0);
                _db.AddInParameter(_cmd, "@DataExclusao", DbType.Int16, 0);
                _db.AddInParameter(_cmd, "@DataAlteracao", DbType.Int16, 0);
                _db.AddInParameter(_cmd, "@idUsuarioAtual", DbType.Int16, ser.IdUsuarioAtual);

                var _Ret = new Retorno();

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _Ret.CodigoRetorno = int.Parse(_dr[0].ToString());
                        _Ret.Mensagem = _dr[1].ToString();
                    }
                }
                return _Ret;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<Servico> ListaServicoCliente(int _idUsuarioAtual)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                List<Servico> _servico = new List<Servico>();

                _cmd = _db.GetStoredProcCommand(String.Format("proc_servicos_usuario_sel"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, _idUsuarioAtual);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        var _serv = new Servico();
                        _serv = ConverteBaseObjeto(_dr);
                        _servico.Add(_serv);
                    }
                }
                if (_servico == null) { throw new Erro("Servico não localizado."); }
                return _servico;
            }
            catch (Exception ex) { throw new Erro(ex.Message); }
        }

        public List<Servico> PesquisaServicoCliente(int tipoConsulta, int condicao, int idCliente, string localizador, int idUsuario)
        {
            try
            {
                List<Servico> _servico = new List<Servico>();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Get_MANU_Servico"));

                _db.AddInParameter(_cmd, "@TipoConsulta", DbType.Int32, tipoConsulta);
                _db.AddInParameter(_cmd, "@Condicao", DbType.Int32, condicao);
                _db.AddInParameter(_cmd, "@IdCliente", DbType.Int32, idCliente);
                _db.AddInParameter(_cmd, "@Localizador", DbType.String, localizador);
                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, idUsuario);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _servico.Add(ConverteBaseObjeto(_dr));
                    }
                }
                if (_servico == null) { throw new Erro("Pesquisa servico ou cliente não localizado."); }
                return _servico;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion IServicoRepository Members
    }

}
