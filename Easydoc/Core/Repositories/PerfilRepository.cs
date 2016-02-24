using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.Core.Repositories.Interfaces;
using System.Data.Common;
using System.Data;

namespace MK.Easydoc.Core.Repositories
{
    public class PerfilRepository:IPerfilRepository
    {
        public Retorno Incluir(Perfil p)
        {
            
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("proc_Manutencao_Perfil");
                _db.AddInParameter(_cmd, "@TipoAcao", DbType.Int16, p.TipoAcao);
                _db.AddInParameter(_cmd, "@Descricao", DbType.String, p.Descricao);
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, p.idServico);
                _db.AddInParameter(_cmd, "@idPerfil", DbType.Int16, p.idPerfil);
                _db.AddInParameter(_cmd, "@idModulo", DbType.String, p.idModulo);
                _db.AddInParameter(_cmd, "@QtdeModulo", DbType.Int16, p.qtdeModulo);

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

        public Retorno Excluir(Perfil p)
        {

            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("proc_Manutencao_Perfil");
                _db.AddInParameter(_cmd, "@TipoAcao", DbType.Int16, p.TipoAcao);
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, p.idServico);
                _db.AddInParameter(_cmd, "@idPerfil", DbType.Int16, p.idPerfil);

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

        public List<Perfil> ListaPerfil(int idServico)
        {
            try
            {
                List<Perfil> _lstPerfil = new List<Perfil>();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Proc_Get_Perfil"));

                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, idServico);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    _lstPerfil.Add(new Perfil() { ID = -1, Descricao = "Selecione" });
                    while (_dr.Read())
                    {
                        _lstPerfil.Add(new Perfil() { ID = int.Parse(_dr[0].ToString()), Descricao = _dr[0].ToString() });
                    }
                }
                if (_lstPerfil == null) { throw new Exception("Perfil não localizado."); }
                return _lstPerfil;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Perfil> ListaPerfilDescricao(int idServico)
        {
            try
            {
                List<Perfil> _lstPerfil = new List<Perfil>();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Proc_Get_Perfil"));

                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, idServico);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    _lstPerfil.Add(new Perfil() { ID = -1, Descricao = "Selecione" });
                    while (_dr.Read())
                    {
                        _lstPerfil.Add(new Perfil() { ID = int.Parse(_dr[0].ToString()), Descricao = _dr[1].ToString() });
                    }
                }
                if (_lstPerfil == null) { throw new Exception("Perfil não localizado."); }
                return _lstPerfil;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Perfil> ListaPerfil(int idCliente, int idServico)
        {
            string cmd = string.Format("exec Proc_Get_Perfil_Servico {0},{1}", idCliente,idServico);
            var lista = new DbConn().RetornaDados(cmd);

            var prf = new List<Perfil>();
            prf.Add(new Perfil() { ID = -1, Descricao = "Selecione" });
            foreach (DataRow item in lista.Rows)
            {
                prf.Add(new Perfil() { ID = int.Parse(item[0].ToString()), Descricao = item[1].ToString() });
            }
            return prf;
        }

        public List<AcessoModulo> ListarModulo(IDictionary<string, object> _queryParams)
        {
            try
            {
                List<AcessoModulo> _itens = new List<AcessoModulo>();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("VerificaPermissaoModulo"));

                _db.AddInParameter(_cmd, "@idUsuario", DbType.Int32, int.Parse(_queryParams["idUsuario"].ToString()));
                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, int.Parse(_queryParams["idServico"].ToString()));
                _db.AddInParameter(_cmd, "@idModulo", DbType.Int32, int.Parse(_queryParams["idModulo"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _itens.Add(new AcessoModulo() { idModulo = int.Parse(_dr["idModulo"].ToString()), Habilitado = int.Parse(_dr["Habilitado"].ToString()) });
                    }
                }


                return _itens;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar o método ExibirDashboard_Doc_Modulo, detalhes: " + ex.Message);
            }
        }

        public List<AcessoModulo> ListarModuloPrincipal(IDictionary<string, object> _queryParams)
        {
            try
            {
                List<AcessoModulo> _itens = new List<AcessoModulo>();
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("VerificaPermissaoModuloPAI"));

                _db.AddInParameter(_cmd, "@idUsuario", DbType.Int32, int.Parse(_queryParams["idUsuario"].ToString()));
                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, int.Parse(_queryParams["idServico"].ToString()));
                //_db.AddInParameter(_cmd, "@idModulo", DbType.Int32, int.Parse(_queryParams["idModulo"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _itens.Add(new AcessoModulo() { idModulo = int.Parse(_dr["idModulo"].ToString()), Habilitado = int.Parse(_dr["Habilitado"].ToString()) });
                    }
                }


                return _itens;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar o método ExibirDashboard_Doc_Modulo, detalhes: " + ex.Message);
            }
        }
    }
}
