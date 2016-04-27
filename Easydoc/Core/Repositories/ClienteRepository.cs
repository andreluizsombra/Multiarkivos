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
using MK.Easydoc.Core.Repositories;
using MK.Easydoc.Core.Infrastructure.Framework;

namespace MK.Easydoc.Core.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        public Cliente TCliente { get; set; }
        public int idServico { get; set; }
        public string Servico { get; set; }

        #region Constructors

        //public ServicoRepository(ILog logger, IConsumer consumer)
        public ClienteRepository()
        {
            TCliente = new Cliente();
        }

        #endregion

        private Cliente ConverteBaseObjeto(IDataReader dt)
        {
            Cliente _cliente = new Cliente();
            _cliente = (new Cliente
            {
                ID = int.Parse(dt["IdCliente"].ToString()),
                CPF_CNPJ = Decimal.Parse(dt["CPF_CNPJ"].ToString()),
                Descricao = dt["Descricao"].ToString(),
                UrlCSS = dt["UrlEstilo"].ToString(),
                Servicos = new List<Servico>(),
                EmailPrincipal = dt["Email_Principal"].ToString(),
                Status = int.Parse(dt["Status"].ToString()),
                QtdeUsuario = int.Parse(dt["QtdeUsuario"].ToString())
            });
            return _cliente;
        }

        #region ICliente Members

        public Retorno Incluir(Cliente usu)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("proc_Manutencao_Cliente");
                _db.AddInParameter(_cmd, "@TipoAcao", DbType.Int16, usu.TipoAcao);
                _db.AddInParameter(_cmd, "@CPF_CNPJ", DbType.Decimal, usu.CPF_CNPJ);
                _db.AddInParameter(_cmd, "@Descricao", DbType.String, usu.Descricao);
                _db.AddInParameter(_cmd, "@UrlEstilo", DbType.String, "URL.CSS");
                _db.AddInParameter(_cmd, "@DataCriacao", DbType.Int16, 0);
                _db.AddInParameter(_cmd, "@DataExclusao", DbType.Int16, 0);
                _db.AddInParameter(_cmd, "@DataAlteracao", DbType.Int16, 0);
                _db.AddInParameter(_cmd, "@Status", DbType.Int16, usu.Status);
                _db.AddInParameter(_cmd, "@QtdeUsuario", DbType.Int16, usu.QtdeUsuario);
                _db.AddInParameter(_cmd, "@Email_principal", DbType.String, usu.EmailPrincipal);
                _db.AddInParameter(_cmd, "@idUsuarioAtual", DbType.Int16, usu.idUsuarioAtual);

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

        public List<Cliente> ListarClientesUsuario(IDictionary<string, object> _queryParams)
        {
            try
            {
                List<Cliente> _clientes = new List<Cliente>();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_clientes_usuario_sel"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _clientes.Add(ConverteBaseObjeto(_dr));
                    }
                }
                if (_clientes == null) { throw new Erro("Cliente não localizado."); }
                return _clientes;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Cliente> ListarClientesUsuario(int idUsuario)
        {
            try
            {
                List<Cliente> _clientes = new List<Cliente>();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("proc_clientes_usuario_sel"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, idUsuario);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _clientes.Add(ConverteBaseObjeto(_dr));
                    }
                }
                if (_clientes == null) { throw new Erro("Cliente não localizado."); }
                return _clientes;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Cliente> PesquisaCliente(int tipoConsulta, int condicao, int idCliente, string localizador, int idUsuario)
        {
            try
            {
                List<Cliente> _clientes = new List<Cliente>();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Get_MANU_Cliente"));

                _db.AddInParameter(_cmd, "@TipoConsulta", DbType.Int32, tipoConsulta);
                _db.AddInParameter(_cmd, "@Condicao", DbType.Int32, condicao);
                _db.AddInParameter(_cmd, "@IdCliente", DbType.Int32, idCliente);
                _db.AddInParameter(_cmd, "@Localizador", DbType.String, localizador);
                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, idUsuario);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _clientes.Add(ConverteBaseObjeto(_dr));
                    }
                }
                if (_clientes == null) { throw new Erro("Cliente não localizado."); }
                return _clientes;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void PrimeiroCliente(string nomeUsuario)
        {
            string cmd = string.Format("exec Proc_GetClientePorLogin '{0}'", nomeUsuario);
            var lista = new DbConn().RetornaDados(cmd);
            string nomeCliente = "";
            foreach (DataRow item in lista.Rows)
            {
                nomeCliente = item[1].ToString();
                TCliente.ID=int.Parse(item[0].ToString());
                TCliente.Descricao = item[1].ToString();
            }

            PrimeiroServicoPorCliente(nomeUsuario, TCliente.ID.ToString());
        }

        /// <summary>
        /// Retorna dados o serviço padrão
        /// </summary>
        /// <Autor>AndreSombra</Autor>
        /// <DataInicio>16/11/2015</DataInicio>
        /// <param name="nomeUsuario"></param>
        public void PrimeiroClienteServicoPadrao(string nomeUsuario, string senha)
        {
            var _cripto = new Criptografia();
            var _utils = new Util();

            string _senha = _cripto.Executar(senha, _utils.ChaveCripto, Criptografia.TipoNivel.Baixo, Criptografia.TipoAcao.Encriptar, Criptografia.TipoCripto.Números);

            string cmd = string.Format("exec Proc_GET_ServicoDefault '{0}','{1}'", nomeUsuario, _senha);
            var lista = new DbConn().RetornaDados(cmd);
            foreach (DataRow item in lista.Rows)
            {
                TCliente.ID = int.Parse(item["IdCliente"].ToString());
                TCliente.Descricao = item["CLiente"].ToString();
                this.idServico = int.Parse(item["IdServico"].ToString());
                this.Servico = item["Servico"].ToString();
            }
        }

        public List<Cliente> ListaClientePorUsuario(string nomeUsuario)
        {
            string cmd = string.Format("exec Proc_GetClientePorLogin '{0}'", nomeUsuario);
            var lista = new DbConn().RetornaDados(cmd);

            var cli = new List<Cliente>();
            cli.Add(new Cliente() { ID = -1, Descricao = "Selecione" });
            foreach (DataRow item in lista.Rows)
            {
                cli.Add(new Cliente() { ID = int.Parse(item[0].ToString()), Descricao = item[1].ToString() });
            }
            return cli;
        }

        public List<Servico> ListaServicoPorCliente(string nomeUsuario, string idCliente)
        {
            string cmd = string.Format("exec Proc_GetServicoPorLogin '{0}',{1}", nomeUsuario, idCliente);
            var lista = new DbConn().RetornaDados(cmd);

            var srv = new List<Servico>();
            srv.Add(new Servico() { ID = 0, Descricao = "Selecione" });
            foreach (DataRow item in lista.Rows)
            {
                srv.Add(new Servico() { ID = int.Parse(item[0].ToString()), Descricao = item[1].ToString() });
                this.Servico = item[1].ToString();
            }
            return srv;
        }

        public void PrimeiroServicoPorCliente(string nomeUsuario, string idCliente)
        {
            string cmd = string.Format("exec Proc_GetServicoPorLogin '{0}',{1}", nomeUsuario, idCliente);
            var lista = new DbConn().RetornaDados(cmd);

            var srv = new List<Servico>();
            
            foreach (DataRow item in lista.Rows)
            {
                srv.Add(new Servico() { ID = int.Parse(item[0].ToString()), Descricao = item[1].ToString() });
                this.idServico = int.Parse(item[0].ToString());
                this.Servico = item[1].ToString();
            }
            
        }

        public Cliente GetCliente(IDictionary<string, object> _queryParams)
        { 
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                Cliente _cliente = new Cliente();

                _cmd = _db.GetStoredProcCommand(String.Format("proc_cliente_get"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, int.Parse(_queryParams["Usuario_ID"].ToString()));
                _db.AddInParameter(_cmd, "@IdCliente", DbType.Int32, int.Parse(_queryParams["Cliente_ID"].ToString()));
                //_db.AddInParameter(_cmd, "@IdServico", DbType.Int16, _queryParams["Servico_ID"]);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _cliente = ConverteBaseObjeto(_dr);
                    }
                }
                if (_cliente == null) { throw new Erro("Cliente não localizado."); }
                return _cliente;
            }
            catch (Exception ex) { throw new Erro(ex.Message);  }        
        }
        public Cliente GetCliente(int idUsuario, int idCliente)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                Cliente _cliente = new Cliente();

                _cmd = _db.GetStoredProcCommand(String.Format("proc_cliente_get"));

                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, idUsuario);
                _db.AddInParameter(_cmd, "@IdCliente", DbType.Int32, idCliente);
                //_db.AddInParameter(_cmd, "@IdServico", DbType.Int16, _queryParams["Servico_ID"]);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _cliente = ConverteBaseObjeto(_dr);
                    }
                }
                if (_cliente == null) { throw new Erro("Cliente não localizado."); }
                return _cliente;
            }
            catch (Exception ex) { throw new Erro(ex.Message); }
        }

        public Cliente GetClienteCPFCNPJ(string cpfcnpj, int idUsuarioAtual)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                Cliente _cliente = new Cliente();

                _cmd = _db.GetStoredProcCommand(String.Format("Proc_Cliente_Por_CPF_CNPJ"));

                _db.AddInParameter(_cmd, "@CPF_CNPJ", DbType.Decimal,decimal.Parse(cpfcnpj));
                _db.AddInParameter(_cmd, "@idUsuario", DbType.Int32, idUsuarioAtual);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _cliente = ConverteBaseObjeto(_dr);
                    }
                }
                if (_cliente == null) { throw new Erro("Cliente não localizado."); }
                return _cliente;
            }
            catch (Exception ex) { throw new Erro(ex.Message); }
        }

        
        public Cliente GetClienteServicoPorNome(string nomeServico, string nomeCliente)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                Cliente _cliente = new Cliente();

                _cmd = _db.GetStoredProcCommand(String.Format("Get_ClienteSRVPorNome"));

                _db.AddInParameter(_cmd, "@NomeServico", DbType.String, nomeServico);
                _db.AddInParameter(_cmd, "@NomeCliente", DbType.String, nomeCliente);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        //_cliente = ConverteBaseObjeto(_dr);
                        _cliente.IdServico = int.Parse(_dr["idServico"].ToString());
                        _cliente.IdCliente = int.Parse(_dr["idCliente"].ToString());
                    }
                }
                if (_cliente == null) { throw new Erro("Cliente não localizado."); }
                return _cliente;
            }
            catch (Exception ex) { throw new Erro(ex.Message); }
        }


        #endregion IClienteRepository Members




    }
}
