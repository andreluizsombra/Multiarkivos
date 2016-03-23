using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.Core.Repositories.Interfaces;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace MK.Easydoc.Core.Repositories
{
    public class OcorrenciaRepository
    {
        public List<Ocorrencia> PesquisaServicoCliente(Filtro flt)
        {
            try
            {
                List<Ocorrencia> _ocorrencia = new List<Ocorrencia>();

                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Get_MANU_Ocorrencia"));
                
                _db.AddInParameter(_cmd, "@IdCliente", DbType.Int32, flt.IdCliente);
                _db.AddInParameter(_cmd, "@IdServico", DbType.Int32, flt.IdServico);
                _db.AddInParameter(_cmd, "@IdUsuario", DbType.Int32, flt.IdUsuarioAtual);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        _ocorrencia.Add( new Ocorrencia(){ 
                            IdOcorrencia = int.Parse(_dr["idocorrencia"].ToString()),
                            descOcorrencia = _dr["descocorrencia"].ToString(),
                            idCliente = int.Parse(_dr["idcliente"].ToString()),
                            nomeCliente = _dr["nomecliente"].ToString(),
                            idServico = int.Parse(_dr["idservico"].ToString()),
                            nomeServico = _dr["nomeservico"].ToString()
                        });
                    }
                }
                if (_ocorrencia == null) { throw new Exception("Pesquisa servico ou cliente não localizado."); }
                return _ocorrencia;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Retorno Incluir(Ocorrencia _ocor)
        {
            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand("proc_Manutencao_Ocorrencia");
                _db.AddInParameter(_cmd, "@TipoAcao", DbType.Int16, _ocor.TipoAcao);
                _db.AddInParameter(_cmd, "@Descricao", DbType.String, _ocor.descOcorrencia);
                _db.AddInParameter(_cmd, "@idServico", DbType.Int16, _ocor.idServico);
                _db.AddInParameter(_cmd, "@idOcorrencia", DbType.Int16, _ocor.IdOcorrencia);
                _db.AddInParameter(_cmd, "@idUsuario", DbType.Int16, _ocor.idUsuario);

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
    }
}
