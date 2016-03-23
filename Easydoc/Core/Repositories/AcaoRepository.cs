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


namespace MK.Easydoc.Core.Repositories
{
    public class AcaoRepository
    {
        public AcaoRepository() { }

        public List<Acao> Listar(int idServico, int Oculta = 0)
        {
            var _lst = new List<Acao>();
            
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("GET_LogAcao");
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, idServico);
                _db.AddInParameter(_cmd, "@Oculta", DbType.Int16, Oculta);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {

                        if (int.Parse(_dr[0].ToString()) < 0)
                        {
                            throw new Exception("Erro, " + _dr[1].ToString());
                        }
                        else
                        {
                            _lst.Add(new Acao()
                            {
                                ID = int.Parse(_dr["idAcao"].ToString()),
                                Descricao = _dr["Descricao"].ToString()
                            });
                        }

                    }
                }

                return _lst;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
