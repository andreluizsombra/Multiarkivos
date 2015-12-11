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
                Descricao = dt["Descricao"].ToString(),
                UrlCSS = dt["UrlEstilo"].ToString(),
                Servicos = new List<Servico>()
            });
            return _cliente;
        }

        #region ICliente Members

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
        public void PrimeiroClienteServicoPadrao(string nomeUsuario)
        {
            string cmd = string.Format("exec Proc_GET_ServicoDefault '{0}'", nomeUsuario);
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
            cli.Add(new Cliente() { ID = 0, Descricao = "Selecione" });
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

        #endregion IClienteRepository Members




    }
}
