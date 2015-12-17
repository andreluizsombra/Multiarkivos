using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Repositories.Interfaces;
using MK.Easydoc.Core.Entities;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MK.Easydoc.Core.Infrastructure;
using System.Data;
using TecFort.Framework.Generico;
using MK.Easydoc.Core.Infrastructure.Framework;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.Core.Services;

namespace MK.Easydoc.Core.Repositories
{
    public sealed class UsuarioRepository : IUsuarioRepository
    {
        #region Private Fields

        //private readonly IServicoService _servico;
        //private readonly ILog _logger;
        //private readonly IConsumer _consumer;

        #endregion

        #region Constructors

        //public UsuarioRepository(ILog logger, IConsumer consumer)
        public UsuarioRepository()
        {

            //_servico = DependencyResolver.Current.GetService<IServicoService>();

            //this._logger = logger;
            //this._consumer = consumer;
        }

        #endregion

        #region IUsuarioRepository Members

        public bool ValidarUsuario(Usuario usuario)
        {
            var resourceUri = default(string);
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                //_cmd = _db.GetSqlStringCommand(String.Format("SELECT * FROM Usuario where UserName = @UserName;"));
                _cmd = _db.GetStoredProcCommand("Get_Usuario");
                _db.AddInParameter(_cmd, "@UserName", DbType.String, usuario.NomeUsuario);
                _db.AddInParameter(_cmd, "@Senha", DbType.String, usuario.Senha);

                List<Usuario> _usuarios = new List<Usuario>();
                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        //_usuarios.Add(new Usuario { ID = Guid.Empty, Senha = _dr["Senha"].ToString() });  
                        _usuarios.Add(new Usuario { ID = int.Parse(_dr["UserId"].ToString()), Senha = _dr["Senha"].ToString(), Bloqueado = int.Parse(_dr["Bloqueado"].ToString()) });  
                    }
                }


                Criptografia _cripto = new Criptografia();
                Util _utils = new Util();

                if (_usuarios.Count == 0) { throw new Exception("Login ou senha inválida. Favor verificar!."); }

                if (_usuarios[0].Bloqueado == 1) { throw new Exception("Usuário Bloqueado, contate o administrador."); }

                usuario.Senha = _cripto.Executar(usuario.Senha.Trim(), _utils.ChaveCripto, Criptografia.TipoNivel.Baixo, Criptografia.TipoAcao.Encriptar, Criptografia.TipoCripto.Números);

                if (usuario.Senha.Trim() == _usuarios[0].Senha.Trim())
                {
                    return true;
                }
                else
                {
                    { throw new Erro("Usuário não localizado."); }
                }

                //return usuario.Senha.Trim() == _usuarios[0].Senha.Trim();

                //this._consumer.Post<Usuario>("ValidarUser_Post", usuario, out resourceUri);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            //catch (Exception ex) {throw ex; }
            //catch (Exception ex) { this._logger.Error(ex.Message, ex); throw ex; }
        }

        public List<ClienteServico> ListaClienteServicos(int idUsuario)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();

                _cmd = _db.GetStoredProcCommand("Proc_Listar_CadastroNovo");
                _db.AddInParameter(_cmd, "@idUsuario", DbType.Int16, idUsuario);

                List<ClienteServico> _usuarios = new List<ClienteServico>();

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _usuarios.Add(new ClienteServico
                        {
                            ClienteID   = int.Parse(_dr["IdCliente"].ToString()), 
                            NomeCliente = _dr["Descricao_Cliente"].ToString(),
                            ServicoID = int.Parse(_dr["IdServico"].ToString()),
                            NomeServico = _dr["Descricao_Servico"].ToString(),
                            PerfilID = int.Parse(_dr["Perfil"].ToString()),
                            Bloqueado = int.Parse(_dr["Usuario_Bloqueado"].ToString())
                        });
                    }
                }

                if (_usuarios == null) { throw new Erro("ListaClienteServicos não localizado."); }

                return _usuarios.ToList();
            }
            catch (Exception ex) { throw ex; }

        }

        public Usuario GetUsuario(IDictionary<string, object> _queryParams)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                //_cmd = _db.GetSqlStringCommand(String.Format("SELECT * FROM Usuario where UserName = @UserName;"));
                _cmd = _db.GetStoredProcCommand("Get_Usuario");
                _db.AddInParameter(_cmd, "@UserName", DbType.String, _queryParams["nomeUsuario"]);
                _db.AddInParameter(_cmd, "@Senha", DbType.String, "");

                List<Usuario> _usuarios = new List<Usuario>();

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        //_usuarios.Add(new Usuario { ID = Guid.Empty, NomeUsuario = _dr["UserName"].ToString(), NomeCompleto = _dr["UserName"].ToString(), Perfil = _dr["UserName"].ToString(), Senha = _dr["Senha"].ToString() });
                        _usuarios.Add(new Usuario { ID = int.Parse(_dr["UserId"].ToString())
                            , NomeUsuario = _dr["UserName"].ToString()
                            , NomeCompleto = _dr["Nome"].ToString()
                            , Perfil = _dr["Perfil"].ToString()
                            , Senha = _dr["Senha"].ToString()
                            , Bloqueado = int.Parse(_dr["Bloqueado"].ToString())

                            //, Servicos = _servico.ListarServicosUsuario(int.Parse(_dr["UserId"].ToString())) 
                        });
                    }
                }

                //Criptografia _cripto = new Criptografia();
                //Util _utils = new Util();

                if (_usuarios == null) { throw new Erro("Usuário não localizado."); }

                return _usuarios[0];
            }
            catch (Exception ex) { throw ex; }
            
        }

        public Usuario GetUsuario(string UserName)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                
                _cmd = _db.GetStoredProcCommand("Get_Usuario");
                _db.AddInParameter(_cmd, "@UserName", DbType.String, UserName);
                _db.AddInParameter(_cmd, "@Senha", DbType.String, "");

                List<Usuario> _usuarios = new List<Usuario>();

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        //_usuarios.Add(new Usuario { ID = Guid.Empty, NomeUsuario = _dr["UserName"].ToString(), NomeCompleto = _dr["UserName"].ToString(), Perfil = _dr["UserName"].ToString(), Senha = _dr["Senha"].ToString() });
                        _usuarios.Add(new Usuario
                        {
                            ID = int.Parse(_dr["UserId"].ToString())
                            ,
                            NomeUsuario = _dr["UserName"].ToString()
                            ,
                            NomeCompleto = _dr["Nome"].ToString()
                            ,
                            Perfil = _dr["Perfil"].ToString()
                            ,
                            Senha = _dr["Senha"].ToString()
                            ,
                            Bloqueado = int.Parse(_dr["Bloqueado"].ToString())
                            ,
                            CPF = _dr["Cpf"].ToString()
                            ,
                            Email = _dr["Email"].ToString()
                            //, Servicos = _servico.ListarServicosUsuario(int.Parse(_dr["UserId"].ToString())) 
                        });
                    }
                }

                //Criptografia _cripto = new Criptografia();
                //Util _utils = new Util();

                if (_usuarios == null) { throw new Erro("Usuário não localizado."); }

                return _usuarios[0];
            }
            catch (Exception ex) { throw ex; }

        }

        public int IncluirUsuario(Usuario usu)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("proc_Manutencao_Usuario");
                _db.AddInParameter(_cmd, "@CPF", DbType.Decimal, Decimal.Parse(usu.CPF));
                _db.AddInParameter(_cmd, "@Nome", DbType.String, usu.NomeCompleto);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, usu.NomeUsuario);
                _db.AddInParameter(_cmd, "@Senha", DbType.String, usu.Senha);
                _db.AddInParameter(_cmd, "@TipoAcao", DbType.Int16, usu.TipoAcao);
                _db.AddInParameter(_cmd, "@idCliente", DbType.Int16, usu.ClienteID);
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, usu.ServicoID);
                _db.AddInParameter(_cmd, "@Perfil", DbType.Int16, usu.PerfilID);
                _db.AddInParameter(_cmd, "@Situacao", DbType.Int16, usu.Situacao);
                _db.AddInParameter(_cmd, "@Email", DbType.String, usu.Email);

                int ID_Gravou = 0;

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        ID_Gravou = int.Parse(_dr[0].ToString());
                    }
                }
                return ID_Gravou;
            }
            catch (Exception ex) { throw ex; }
        }
        public void BloquearUsuario(int idServico, int idUsuarioBloqueado, int idUsuario)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("UPD_BloqueioUsuario");
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, idServico);
                _db.AddInParameter(_cmd, "@idUsuarioBloqueio", DbType.Int16, idUsuarioBloqueado);
                _db.AddInParameter(_cmd, "@idUsuario", DbType.Int16, idUsuario);

                _db.ExecuteNonQuery(_cmd);
            }
            catch (Exception ex) { throw ex; }
        }

        public List<Usuario> GetUsuarioCadastro(int tipoConsulta, int Condicao, int idCliente, string txtPesq, int idUsuario)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                
                _cmd = _db.GetStoredProcCommand("Get_UsuarioCad");
                _db.AddInParameter(_cmd, "@TipoConsulta", DbType.Int16, tipoConsulta);
                _db.AddInParameter(_cmd, "@Condicao", DbType.Int16, Condicao);
                _db.AddInParameter(_cmd, "@idCliente", DbType.Int16, idCliente);
                _db.AddInParameter(_cmd, "@Localizador", DbType.String, txtPesq);
                _db.AddInParameter(_cmd, "@idUsuario", DbType.Int16, idUsuario);  

                List<Usuario> _usuarios = new List<Usuario>();

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        //_usuarios.Add(new Usuario { ID = Guid.Empty, NomeUsuario = _dr["UserName"].ToString(), NomeCompleto = _dr["UserName"].ToString(), Perfil = _dr["UserName"].ToString(), Senha = _dr["Senha"].ToString() });
                        _usuarios.Add(new Usuario
                        {
                            ID = int.Parse(_dr["UserId"].ToString())
                            ,
                            NomeUsuario = _dr["UserName"].ToString()
                            ,
                            NomeCompleto = _dr["Nome"].ToString()
                            ,
                            CPF = _dr["CPF"].ToString()
                            ,
                            Bloqueado = int.Parse(_dr["Bloqueado"].ToString())
                            //,
                            //Perfil = _dr["UserName"].ToString()
                            //,
                            //Senha = _dr["Senha"].ToString()
                            //, Servicos = _servico.ListarServicosUsuario(int.Parse(_dr["UserId"].ToString())) 
                        });
                    }
                }

                //Criptografia _cripto = new Criptografia();
                //Util _utils = new Util();

                if (_usuarios == null) { throw new Erro("Usuário não localizado."); }

                return _usuarios.ToList();
            }
            catch (Exception ex) { throw ex; }

        }

        public Retorno VerificaLoginDisponivel(int idUsuario, string novoUserName)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();

                _cmd = _db.GetStoredProcCommand("Proc_verificaLogin_Disponivel");
                _db.AddInParameter(_cmd, "@idUsuario", DbType.Int16, idUsuario);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, novoUserName);

                var _retorno = new Retorno();

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {

                        _retorno.CodigoRetorno = int.Parse(_dr[0].ToString());
                        _retorno.Mensagem = _dr[1].ToString();
                    }
                }


                if (_retorno == null) { throw new Erro("Retorno não localizado."); }

                return _retorno;
            }
            catch (Exception ex) { throw ex; }

        }


        #endregion

    }
}
