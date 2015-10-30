﻿using System;
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
                _cmd = _db.GetSqlStringCommand(String.Format("SELECT * FROM Usuario where UserName = @UserName;"));
                _db.AddInParameter(_cmd, "@UserName", DbType.String, usuario.NomeUsuario);
                _db.AddInParameter(_cmd, "@Senha", DbType.String, usuario.Senha);

                List<Usuario> _usuarios = new List<Usuario>();
                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        //_usuarios.Add(new Usuario { ID = Guid.Empty, Senha = _dr["Senha"].ToString() });  
                        _usuarios.Add(new Usuario { ID = int.Parse(_dr["UserId"].ToString()), Senha = _dr["Senha"].ToString() });  
                    }
                }


                Criptografia _cripto = new Criptografia();
                Util _utils = new Util();

                if (_usuarios.Count == 0) { throw new Erro("Usuário não localizado."); }
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
            catch (Exception ex) {throw ex; }
            //catch (Exception ex) { this._logger.Error(ex.Message, ex); throw ex; }
        }

        public Usuario GetUsuario(IDictionary<string, object> _queryParams)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetSqlStringCommand(String.Format("SELECT * FROM Usuario where UserName = @UserName;"));
                _db.AddInParameter(_cmd, "@UserName", DbType.String, _queryParams["nomeUsuario"]);

                List<Usuario> _usuarios = new List<Usuario>();

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        //_usuarios.Add(new Usuario { ID = Guid.Empty, NomeUsuario = _dr["UserName"].ToString(), NomeCompleto = _dr["UserName"].ToString(), Perfil = _dr["UserName"].ToString(), Senha = _dr["Senha"].ToString() });
                        _usuarios.Add(new Usuario { ID = int.Parse(_dr["UserId"].ToString())
                            , NomeUsuario = _dr["UserName"].ToString()
                            , NomeCompleto = _dr["UserName"].ToString()
                            , Perfil = _dr["UserName"].ToString()
                            , Senha = _dr["Senha"].ToString()
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

        #endregion

    }
}
