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
                    _lstPerfil.Add(new Perfil() { ID = 0, Descricao = "Selecione" });
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
    }
}
