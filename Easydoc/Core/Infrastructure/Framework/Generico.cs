using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Net.Mime;

using ICSharpCode.SharpZipLib.Zip;
//using Tecfort.Framework.WebForm;

namespace TecFort.Framework.Generico
{
    public class Item 
    {
        string _text = string.Empty;
        object _value = null;
        object _secondaryValue = null;

        public Item() { }
        public Item(string text)
        {
            _text = text;
        }
        public Item(object value)
        {
            _value = value;
        }
        public Item(object value, string text) 
        {
            _text = text;
            _value = value;
        }
        public Item(object value, object secondaryValue, string text)
        {
            _text = text;
            _secondaryValue = secondaryValue;
            _value = value;
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public object SecondaryValue
        {
            get { return _secondaryValue; }
            set { _secondaryValue = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }


    public class Erro : Exception
    {
        public enum TipoErro { SemGravidade = 1, ComGravidade = 2 };

        public TipoErro Tipo = TipoErro.ComGravidade;
        public string NumTipoErro = "2";
        public string Mensagem = "";
        public string Projeto = "";
        public string Arquivo = "";
        public string Classe = "";
        public string Metodo = "";

        public Erro(string MensagemErro)
        {
            Tipo = TipoErro.SemGravidade;
            NumTipoErro = "1";
            Mensagem = MensagemErro.Trim();
        }

        public Erro(string MensagemErro,
                    string ProjetoErro,
                    string ArquivoErro,
                    string ClasseErro,
                    string MetodoErro,
                    string NomeArq)
        {
            Tipo = TipoErro.ComGravidade;
            NumTipoErro = "2";
            Mensagem = MensagemErro.Trim();
            Projeto = ProjetoErro.Trim();
            Arquivo = ArquivoErro.Trim();
            Classe = ClasseErro.Trim();
            Metodo = MetodoErro.Trim();

            Log oblLog = new Log();
            oblLog.GravaLog(Projeto.Trim(),
                            ArquivoErro.Trim(),
                            ClasseErro.Trim(),
                            MetodoErro.Trim(),
                            MensagemErro.Trim(),
                            NomeArq.Trim());
        }
    }

    public class Log
    {
        public void GravaLog(string Projeto,
                             string Arquivo,
                             string Classe,
                             string Funcao,
                             string Mensagem,
                             string NomeArq)
        {
            string AppPath = "";
            if (NomeArq.IndexOf("\\") <= -1)
            {
                AppPath = System.Reflection.Assembly.GetExecutingAssembly().Location.Trim();
                while (true)
                {
                    if (AppPath.Substring(AppPath.Trim().Length - 1, 1) == "\\")
                    {
                        break;
                    }
                    AppPath = AppPath.Substring(0, AppPath.Trim().Length - 1);
                }
            }

            StreamWriter objSW = new StreamWriter(AppPath.Trim() + NomeArq.Trim(), true, Encoding.Unicode);
            objSW.WriteLine(string.Concat("[",
                                          DateTime.Now.ToString("dd/MM/yyyy"),
                                          " - ",
                                          DateTime.Now.ToShortTimeString().ToString(),
                                          "]",
                                          " - Projeto:", Projeto.Trim(),
                                          " - Arquivo:", Arquivo.Trim(),
                                          " - Classe:", Classe.Trim(),
                                          " - Funcao:", Funcao.Trim(),
                                          " - Descricao Erro:", Mensagem.Trim()));
            objSW.Close();
        }
    }

    public class WebConfig
    {
        public string Ler(string Chave)
        {
            if (ConfigurationManager.AppSettings[Chave.Trim()] != null)
            {
                return ConfigurationManager.AppSettings[Chave.Trim()].Trim();
            }
            else
            {
                return "";
            }
        }

        public string[,] ListarConnectionStrings(bool Selecione)
        {
            string[,] lista = new string[ConfigurationManager.ConnectionStrings.Count + (Selecione ? 1 : 0), 2];
            if (Selecione)
            {
                lista[0, 1] = "Selecione...";
                lista[0, 0] = string.Empty;
            }
            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                lista[i + (Selecione ? 1 : 0), 1] = ConfigurationManager.ConnectionStrings[i].Name.Trim();
                lista[i + (Selecione ? 1 : 0), 0] = ConfigurationManager.ConnectionStrings[i].ToString().Trim();
            }

            return lista;
        }
    }
    public class OleDb : IDisposable
    {
        OleDbConnection _objCon = new OleDbConnection();
        WebConfig _objConfig = new WebConfig();
        private OleDbTransaction _objTrans;
        private bool _transacao = false;
        public enum TipoConexao { ConnectionString, ItemConfig }

        public OleDb(string Conexao, TipoConexao Tipo)
        {
            try
            {
                if (Tipo == TipoConexao.ConnectionString)
                {
                    _objCon.ConnectionString = Conexao;
                }
                else
                {
                    if (ConfigurationManager.ConnectionStrings[Conexao.Trim()] != null)
                    {
                        _objCon.ConnectionString = ConfigurationManager.ConnectionStrings[Conexao.Trim()].ConnectionString.Trim();
                    }
                    else
                    {
                        throw new Erro("Não foi lacalizado no arquivo de configuração a conexão '" + Conexao.Trim() + "'.");
                    }
                }
                _objCon.Open();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public OleDb(string Conexao, TipoConexao Tipo, Boolean Transacao)
        {
            try
            {
                if (Tipo == TipoConexao.ConnectionString)
                {
                    _objCon.ConnectionString = Conexao;
                }
                else
                {
                    if (ConfigurationManager.ConnectionStrings[Conexao.Trim()] != null)
                    {
                        _objCon.ConnectionString = ConfigurationManager.ConnectionStrings[Conexao.Trim()].ConnectionString.Trim();
                    }
                    else
                    {
                        throw new Erro("Não foi lacalizado no arquivo de configuração a conexão '" + Conexao.Trim() + "'.");
                    }
                }
                _objCon.Open();
                if (Transacao)
                {
                    _objTrans = _objCon.BeginTransaction();
                    _transacao = true;
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public OleDb(string Provider, string Server, string DataBase, string User, string Password)
        {
            try
            {
                _objCon.ConnectionString = "Provider=" + Provider.Trim() + ";" +
                                           "Password=" + Password.Trim() + ";" +
                                           "Persist Security Info=True;" +
                                           "User ID=" + User.Trim() + ";" +
                                           "Initial Catalog=" + DataBase.Trim() + ";" +
                                           "Data Source=" + Server.Trim() + ";";
                _objCon.Open();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public OleDb(string Provider, string Server, string DataBase, string User, string Password, Boolean Transacao)
        {
            try
            {
                _objCon.ConnectionString = "Provider=" + Provider.Trim() + ";" +
                                           "Password=" + Password.Trim() + ";" +
                                           "Persist Security Info=True;" +
                                           "User ID=" + User.Trim() + ";" +
                                           "Initial Catalog=" + DataBase.Trim() + ";" +
                                           "Data Source=" + Server.Trim() + ";";
                _objCon.Open();
                if (Transacao)
                {
                    _objTrans = _objCon.BeginTransaction();
                    _transacao = true;
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        ~OleDb()
        {
            if (_transacao)
            {
                _objTrans.Rollback();
                _transacao = false;
                _objTrans = null;
            }
        }

        private string strConexao()
        {
            try
            {
                if (ConfigurationManager.ConnectionStrings[_objConfig.Ler("ConStr").Trim()] != null)
                {
                    return ConfigurationManager.ConnectionStrings[_objConfig.Ler("ConStr").Trim()].ConnectionString.Trim();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public string ConnectionString()
        {
            try
            {
                return _objCon.ConnectionString;
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void AbrirConexao()
        {
            try
            {
                if (_objCon.State == ConnectionState.Open) { throw new Erro("Conexão já está aberta."); }
                _objCon.Open();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void AbrirConexao(Boolean Transacao)
        {
            try
            {
                if (_objCon.State == ConnectionState.Open) { throw new Erro("Conexão já está aberta."); }
                _objCon.Open();
                if (Transacao)
                {
                    if (_objTrans == null)
                    {
                        _objTrans = _objCon.BeginTransaction();
                        _transacao = true;
                    }
                    else
                    {
                        throw new Erro("Transação já está aberta.");
                    }
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void FecharConexao()
        {
            try
            {
                if (_transacao) { throw new Erro("Impossível fechar a conexão enquanto houver uma transação aberta."); }
                if (_objCon.State == ConnectionState.Closed)
                {
                    throw new Erro("Conexão está fechada.");
                }
                else
                {
                    _objCon.Close();
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void FecharConexao(Boolean Transacao)
        {
            try
            {
                if (_transacao)
                {
                    if (Transacao)
                    {
                        _objTrans.Commit();
                        _transacao = false;
                        _objTrans = null;
                    }
                    else
                    {
                        _objTrans.Rollback();
                        _transacao = false;
                        _objTrans = null;
                    }
                }
                else
                {
                    if (Transacao)
                    {
                        throw new Erro("Não existe transação aberta.");
                    }
                }

                if (_objCon.State != ConnectionState.Closed) { _objCon.Close(); }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void BeginTransaction()
        {
            try
            {
                if (_objCon.State != ConnectionState.Open) { throw new Erro("Para abrir um transação a conexação já deve estar aberta."); }
                _objTrans = _objCon.BeginTransaction();
                _transacao = true;
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void CommitTransaction()
        {
            try
            {
                if (_transacao)
                {
                    _objTrans.Commit();
                    _transacao = false;
                    _objTrans = null;
                }
                else
                {
                    throw new Erro("Não existe transação aberta.");
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void RollbackTransaction()
        {
            try
            {
                if (_transacao)
                {
                    _objTrans.Rollback();
                    _transacao = false;
                    _objTrans = null;
                }
                else
                {
                    throw new Erro("Não existe transação aberta.");
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        public DataTable DataTable(StringBuilder InstrucaoSQL) { return DataSet(InstrucaoSQL.ToString(), new List<OleDbParameter>(), string.Empty, null).Tables[0]; }
        public DataTable DataTable(StringBuilder InstrucaoSQL, OleDbParameter parametro) { return DataSet(InstrucaoSQL.ToString(), new List<OleDbParameter>(new OleDbParameter[] { parametro }), string.Empty, null).Tables[0]; }
        public DataTable DataTable(StringBuilder InstrucaoSQL, List<OleDbParameter> parametros) { return DataSet(InstrucaoSQL.ToString(), parametros, string.Empty, null).Tables[0]; }
        public DataTable DataTable(string InstrucaoSQL) { return DataSet(InstrucaoSQL, new List<OleDbParameter>(), string.Empty, null).Tables[0]; }
        public DataTable DataTable(string InstrucaoSQL, OleDbParameter parametro) { return DataSet(InstrucaoSQL, new List<OleDbParameter>(new OleDbParameter[] { parametro }), string.Empty, null).Tables[0]; }
        public DataTable DataTable(string InstrucaoSQL, List<OleDbParameter> parametros) { return DataSet(InstrucaoSQL, parametros, string.Empty, null).Tables[0]; }

        public DataSet DataSet(StringBuilder InstrucaoSQL) { return DataSet(InstrucaoSQL.ToString(), new List<OleDbParameter>(), string.Empty, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, OleDbParameter parametro) { return DataSet(InstrucaoSQL.ToString(), new List<OleDbParameter>(new OleDbParameter[] { parametro }), string.Empty, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, List<OleDbParameter> parametros) { return DataSet(InstrucaoSQL.ToString(), parametros, string.Empty, null); }
        public DataSet DataSet(string InstrucaoSQL) { return DataSet(InstrucaoSQL, new List<OleDbParameter>(), string.Empty, null); }
        public DataSet DataSet(string InstrucaoSQL, OleDbParameter parametro) { return DataSet(InstrucaoSQL, new List<OleDbParameter>(new OleDbParameter[] { parametro }), string.Empty, null); }
        public DataSet DataSet(string InstrucaoSQL, List<OleDbParameter> parametros) { return DataSet(InstrucaoSQL, parametros, string.Empty, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, string NomeDataTable) { return DataSet(InstrucaoSQL.ToString(), new List<OleDbParameter>(), NomeDataTable, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, OleDbParameter parametro, string NomeDataTable) { return DataSet(InstrucaoSQL.ToString(), new List<OleDbParameter>(new OleDbParameter[] { parametro }), NomeDataTable, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, List<OleDbParameter> parametros, string NomeDataTable) { return DataSet(InstrucaoSQL.ToString(), parametros, NomeDataTable, null); }
        public DataSet DataSet(string InstrucaoSQL, string NomeDataTable) { return DataSet(InstrucaoSQL, new List<OleDbParameter>(), NomeDataTable, null); }
        public DataSet DataSet(string InstrucaoSQL, OleDbParameter parametro, string NomeDataTable) { return DataSet(InstrucaoSQL, new List<OleDbParameter>(new OleDbParameter[] { parametro }), NomeDataTable, null); }
        public DataSet DataSet(string InstrucaoSQL, List<OleDbParameter> parametros, string NomeDataTable) { return DataSet(InstrucaoSQL, parametros, NomeDataTable, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, string NomeDataTable, DataSet DataSetOrigem) { return DataSet(InstrucaoSQL.ToString(), new List<OleDbParameter>(), NomeDataTable, DataSetOrigem); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, OleDbParameter parametro, string NomeDataTable, DataSet DataSetOrigem) { return DataSet(InstrucaoSQL.ToString(), new List<OleDbParameter>(new OleDbParameter[] { parametro }), NomeDataTable, DataSetOrigem); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, List<OleDbParameter> parametros, string NomeDataTable, DataSet DataSetOrigem) { return DataSet(InstrucaoSQL.ToString(), parametros, NomeDataTable, DataSetOrigem); }
        public DataSet DataSet(string InstrucaoSQL, string NomeDataTable, DataSet DataSetOrigem) { return DataSet(InstrucaoSQL, new List<OleDbParameter>(), NomeDataTable, DataSetOrigem); }
        public DataSet DataSet(string InstrucaoSQL, OleDbParameter parametro, string NomeDataTable, DataSet DataSetOrigem) { return DataSet(InstrucaoSQL, new List<OleDbParameter>(new OleDbParameter[] { parametro }), NomeDataTable, DataSetOrigem); }
        public DataSet DataSet(string InstrucaoSQL, List<OleDbParameter> parametros, string NomeDataTable, DataSet DataSetOrigem)
        {
            try
            {
                OleDbCommand cmdDataSet = new OleDbCommand(InstrucaoSQL.Trim(), _objCon);
                if (_transacao) { cmdDataSet.Transaction = _objTrans; }
                foreach (OleDbParameter parametro in parametros)
                {
                    cmdDataSet.Parameters.Add(parametro);
                }
                OleDbDataAdapter daDataSet = new OleDbDataAdapter(cmdDataSet);
                if (DataSetOrigem == null)
                {
                    DataSet dsDataSet = new DataSet();
                    if (string.IsNullOrEmpty(NomeDataTable.Trim()))
                    {
                        daDataSet.Fill(dsDataSet);
                    }
                    else
                    {
                        daDataSet.Fill(dsDataSet, NomeDataTable.Trim());
                    }
                    return dsDataSet;
                }
                else
                {
                    daDataSet.Fill(DataSetOrigem, NomeDataTable.Trim());
                    return DataSetOrigem;
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        public OleDbDataReader ExecuteReader(StringBuilder InstrucaoSQL) { return ExecuteReader(InstrucaoSQL.ToString(), new List<OleDbParameter>()); }
        public OleDbDataReader ExecuteReader(string InstrucaoSQL) { return ExecuteReader(InstrucaoSQL, new List<OleDbParameter>()); }
        public OleDbDataReader ExecuteReader(StringBuilder InstrucaoSQL, OleDbParameter parametro) { return ExecuteReader(InstrucaoSQL.ToString(), new List<OleDbParameter>(new OleDbParameter[] { parametro })); }
        public OleDbDataReader ExecuteReader(StringBuilder InstrucaoSQL, List<OleDbParameter> parametros) { return ExecuteReader(InstrucaoSQL.ToString(), parametros); }
        public OleDbDataReader ExecuteReader(string InstrucaoSQL, OleDbParameter parametro) { return ExecuteReader(InstrucaoSQL, new List<OleDbParameter>(new OleDbParameter[] { parametro })); }
        public OleDbDataReader ExecuteReader(string InstrucaoSQL, List<OleDbParameter> parametros)
        {
            try
            {
                OleDbCommand cmdDataReader = new OleDbCommand(InstrucaoSQL.Trim(), _objCon);
                if (_transacao) { cmdDataReader.Transaction = _objTrans; }
                foreach (OleDbParameter parametro in parametros)
                {
                    cmdDataReader.Parameters.Add(parametro);
                }
                return cmdDataReader.ExecuteReader();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        public void ExecuteNonQuery(StringBuilder InstrucaoSQL) { ExecuteNonQuery(InstrucaoSQL.ToString(), new List<OleDbParameter>()); }
        public void ExecuteNonQuery(string InstrucaoSQL) { ExecuteNonQuery(InstrucaoSQL, new List<OleDbParameter>()); }
        public void ExecuteNonQuery(StringBuilder InstrucaoSQL, OleDbParameter parametro) { ExecuteNonQuery(InstrucaoSQL.ToString(), new List<OleDbParameter>(new OleDbParameter[] { parametro })); }
        public void ExecuteNonQuery(StringBuilder InstrucaoSQL, List<OleDbParameter> parametros) { ExecuteNonQuery(InstrucaoSQL.ToString(), parametros); }
        public void ExecuteNonQuery(string InstrucaoSQL, OleDbParameter parametro) { ExecuteNonQuery(InstrucaoSQL.ToString(), new List<OleDbParameter>(new OleDbParameter[] { parametro })); }
        public void ExecuteNonQuery(string InstrucaoSQL, List<OleDbParameter> parametros)
        {
            try
            {
                OleDbCommand cmdExecuteNQuery = new OleDbCommand(InstrucaoSQL.Trim(), _objCon);
                if (_transacao) { cmdExecuteNQuery.Transaction = _objTrans; }
                foreach (OleDbParameter parametro in parametros)
                {
                    cmdExecuteNQuery.Parameters.Add(parametro);
                }
                cmdExecuteNQuery.ExecuteNonQuery();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        public object ExecuteScalar(StringBuilder InstrucaoSQL) { return ExecuteScalar(InstrucaoSQL.ToString(), new List<OleDbParameter>()); }
        public object ExecuteScalar(string InstrucaoSQL) { return ExecuteScalar(InstrucaoSQL, new List<OleDbParameter>()); }
        public object ExecuteScalar(StringBuilder InstrucaoSQL, OleDbParameter parametro) { return ExecuteScalar(InstrucaoSQL.ToString(), new List<OleDbParameter>(new OleDbParameter[] { parametro })); }
        public object ExecuteScalar(StringBuilder InstrucaoSQL, List<OleDbParameter> parametros) { return ExecuteScalar(InstrucaoSQL.ToString(), parametros); }
        public object ExecuteScalar(string InstrucaoSQL, OleDbParameter parametro) { return ExecuteScalar(InstrucaoSQL, new List<OleDbParameter>(new OleDbParameter[] { parametro })); }
        public object ExecuteScalar(string InstrucaoSQL, List<OleDbParameter> parametros)
        {
            try
            {
                OleDbCommand cmdExecuteScalar = new OleDbCommand(InstrucaoSQL.Trim(), _objCon);
                if (_transacao) { cmdExecuteScalar.Transaction = _objTrans; }
                foreach (OleDbParameter parametro in parametros)
                {
                    cmdExecuteScalar.Parameters.Add(parametro);
                }
                return cmdExecuteScalar.ExecuteScalar();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        public DataTable DataTableFile(string PathFile) { return DataSetFile(PathFile, string.Empty, null).Tables[0]; }

        public DataSet DataSetFile(string PathFile) { return DataSetFile(PathFile, string.Empty, null); }
        public DataSet DataSetFile(string PathFile, string NomeDataTable) { return DataSetFile(PathFile, NomeDataTable, null); }
        public DataSet DataSetFile(string PathFile, string NomeDataTable, DataSet DataSetOrigem)
        {
            StreamReader objSR = new StreamReader(PathFile.Trim());
            return DataSet(objSR.ReadToEnd().Trim(), new List<OleDbParameter>(), NomeDataTable.Trim(), DataSetOrigem);
        }

        public OleDbDataReader ExecuteReaderFile(string PathFile)
        {
            StreamReader objSR = new StreamReader(PathFile.Trim());
            return ExecuteReader(objSR.ReadToEnd().Trim(), new List<OleDbParameter>());
        }

        public void ExecuteNonQueryFile(string PathFile)
        {
            StreamReader objSR = new StreamReader(PathFile.Trim());
            ExecuteNonQuery(objSR.ReadToEnd().Trim(), new List<OleDbParameter>());
        }

        public object ExecuteScalarFile(string PathFile)
        {
            StreamReader objSR = new StreamReader(PathFile.Trim());
            return ExecuteScalar(objSR.ReadToEnd().Trim(), new List<OleDbParameter>());
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class SqlDb : IDisposable
    {
        SqlConnection _objCon = new SqlConnection();
        WebConfig _objConfig = new WebConfig();
        private SqlTransaction _objTrans;
        private bool _transacao = false;
        public enum TipoConexao { ConnectionString, ItemConfig }

        public SqlDb(string Conexao, TipoConexao Tipo)
        {
            try
            {
                if (Tipo == TipoConexao.ConnectionString)
                {
                    _objCon.ConnectionString = Conexao;
                }
                else
                {
                    if (ConfigurationManager.ConnectionStrings[Conexao.Trim()] != null)
                    {
                        _objCon.ConnectionString = ConfigurationManager.ConnectionStrings[Conexao.Trim()].ConnectionString.Trim();
                    }
                    else
                    {
                        throw new Erro("Não foi lacalizado no arquivo de configuração a conexão '" + Conexao.Trim() + "'.");
                    }
                }
                _objCon.Open();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public SqlDb(string Conexao, TipoConexao Tipo, Boolean Transacao)
        {
            try
            {
                if (Tipo == TipoConexao.ConnectionString)
                {
                    _objCon.ConnectionString = Conexao;
                }
                else
                {
                    if (ConfigurationManager.ConnectionStrings[Conexao.Trim()] != null)
                    {
                        _objCon.ConnectionString = ConfigurationManager.ConnectionStrings[Conexao.Trim()].ConnectionString.Trim();
                    }
                    else
                    {
                        throw new Erro("Não foi lacalizado no arquivo de configuração a conexão '" + Conexao.Trim() + "'.");
                    }
                }
                _objCon.Open();
                if (Transacao)
                {
                    _objTrans = _objCon.BeginTransaction();
                    _transacao = true;
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public SqlDb(string Provider, string Server, string DataBase, string User, string Password)
        {
            try
            {
                _objCon.ConnectionString = "Provider=" + Provider.Trim() + ";" +
                                           "Password=" + Password.Trim() + ";" +
                                           "Persist Security Info=True;" +
                                           "User ID=" + User.Trim() + ";" +
                                           "Initial Catalog=" + DataBase.Trim() + ";" +
                                           "Data Source=" + Server.Trim() + ";";
                _objCon.Open();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public SqlDb(string Provider, string Server, string DataBase, string User, string Password, Boolean Transacao)
        {
            try
            {
                _objCon.ConnectionString = "Provider=" + Provider.Trim() + ";" +
                                           "Password=" + Password.Trim() + ";" +
                                           "Persist Security Info=True;" +
                                           "User ID=" + User.Trim() + ";" +
                                           "Initial Catalog=" + DataBase.Trim() + ";" +
                                           "Data Source=" + Server.Trim() + ";";
                _objCon.Open();
                if (Transacao)
                {
                    _objTrans = _objCon.BeginTransaction();
                    _transacao = true;
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        ~SqlDb()
        {
            if (_transacao)
            {
                _objTrans.Rollback();
                _transacao = false;
                _objTrans = null;
            }
        }

        private string strConexao()
        {
            try
            {
                if (ConfigurationManager.ConnectionStrings[_objConfig.Ler("ConStr").Trim()] != null)
                {
                    return ConfigurationManager.ConnectionStrings[_objConfig.Ler("ConStr").Trim()].ConnectionString.Trim();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public string ConnectionString()
        {
            try
            {
                return _objCon.ConnectionString;
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void AbrirConexao()
        {
            try
            {
                if (_objCon.State == ConnectionState.Open) { throw new Erro("Conexão já está aberta."); }
                _objCon.Open();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void AbrirConexao(Boolean Transacao)
        {
            try
            {
                if (_objCon.State == ConnectionState.Open) { throw new Erro("Conexão já está aberta."); }
                _objCon.Open();
                if (Transacao)
                {
                    if (_objTrans == null)
                    {
                        _objTrans = _objCon.BeginTransaction();
                        _transacao = true;
                    }
                    else
                    {
                        throw new Erro("Transação já está aberta.");
                    }
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void FecharConexao()
        {
            try
            {
                if (_transacao) { throw new Erro("Impossível fechar a conexão enquanto houver uma transação aberta."); }
                if (_objCon.State == ConnectionState.Closed)
                {
                    throw new Erro("Conexão está fechada.");
                }
                else
                {
                    _objCon.Close();
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void FecharConexao(Boolean Transacao)
        {
            try
            {
                if (_transacao)
                {
                    if (Transacao)
                    {
                        _objTrans.Commit();
                        _transacao = false;
                        _objTrans = null;
                    }
                    else
                    {
                        _objTrans.Rollback();
                        _transacao = false;
                        _objTrans = null;
                    }
                }
                else
                {
                    if (Transacao)
                    {
                        throw new Erro("Não existe transação aberta.");
                    }
                }

                if (_objCon.State != ConnectionState.Closed) { _objCon.Close(); }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void BeginTransaction()
        {
            try
            {
                if (_objCon.State != ConnectionState.Open) { throw new Erro("Para abrir um transação a conexação já deve estar aberta."); }
                _objTrans = _objCon.BeginTransaction();
                _transacao = true;
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void CommitTransaction()
        {
            try
            {
                if (_transacao)
                {
                    _objTrans.Commit();
                    _transacao = false;
                    _objTrans = null;
                }
                else
                {
                    throw new Erro("Não existe transação aberta.");
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);

            }
        }

        public void RollbackTransaction()
        {
            try
            {
                if (_transacao)
                {
                    _objTrans.Rollback();
                    _transacao = false;
                    _objTrans = null;
                }
                else
                {
                    throw new Erro("Não existe transação aberta.");
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        public DataTable DataTable(StringBuilder InstrucaoSQL) { return DataSet(InstrucaoSQL.ToString(), new List<SqlParameter>(), string.Empty, null).Tables[0]; }
        public DataTable DataTable(StringBuilder InstrucaoSQL, SqlParameter parametro) { return DataSet(InstrucaoSQL.ToString(), new List<SqlParameter>(new SqlParameter[] { parametro }), string.Empty, null).Tables[0]; }
        public DataTable DataTable(StringBuilder InstrucaoSQL, List<SqlParameter> parametros) { return DataSet(InstrucaoSQL.ToString(), parametros, string.Empty, null).Tables[0]; }
        public DataTable DataTable(string InstrucaoSQL) { return DataSet(InstrucaoSQL, new List<SqlParameter>(), string.Empty, null).Tables[0]; }
        public DataTable DataTable(string InstrucaoSQL, SqlParameter parametro) { return DataSet(InstrucaoSQL, new List<SqlParameter>(new SqlParameter[] { parametro }), string.Empty, null).Tables[0]; }
        public DataTable DataTable(string InstrucaoSQL, List<SqlParameter> parametros) { return DataSet(InstrucaoSQL, parametros, string.Empty, null).Tables[0]; }

        public DataSet DataSet(StringBuilder InstrucaoSQL) { return DataSet(InstrucaoSQL.ToString(), new List<SqlParameter>(), string.Empty, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, SqlParameter parametro) { return DataSet(InstrucaoSQL.ToString(), new List<SqlParameter>(new SqlParameter[] { parametro }), string.Empty, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, List<SqlParameter> parametros) { return DataSet(InstrucaoSQL.ToString(), parametros, string.Empty, null); }
        public DataSet DataSet(string InstrucaoSQL) { return DataSet(InstrucaoSQL, new List<SqlParameter>(), string.Empty, null); }
        public DataSet DataSet(string InstrucaoSQL, SqlParameter parametro) { return DataSet(InstrucaoSQL, new List<SqlParameter>(new SqlParameter[] { parametro }), string.Empty, null); }
        public DataSet DataSet(string InstrucaoSQL, List<SqlParameter> parametros) { return DataSet(InstrucaoSQL, parametros, string.Empty, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, string NomeDataTable) { return DataSet(InstrucaoSQL.ToString(), new List<SqlParameter>(), NomeDataTable, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, SqlParameter parametro, string NomeDataTable) { return DataSet(InstrucaoSQL.ToString(), new List<SqlParameter>(new SqlParameter[] { parametro }), NomeDataTable, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, List<SqlParameter> parametros, string NomeDataTable) { return DataSet(InstrucaoSQL.ToString(), parametros, NomeDataTable, null); }
        public DataSet DataSet(string InstrucaoSQL, string NomeDataTable) { return DataSet(InstrucaoSQL, new List<SqlParameter>(), NomeDataTable, null); }
        public DataSet DataSet(string InstrucaoSQL, SqlParameter parametro, string NomeDataTable) { return DataSet(InstrucaoSQL, new List<SqlParameter>(new SqlParameter[] { parametro }), NomeDataTable, null); }
        public DataSet DataSet(string InstrucaoSQL, List<SqlParameter> parametros, string NomeDataTable) { return DataSet(InstrucaoSQL, parametros, NomeDataTable, null); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, string NomeDataTable, DataSet DataSetOrigem) { return DataSet(InstrucaoSQL.ToString(), new List<SqlParameter>(), NomeDataTable, DataSetOrigem); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, SqlParameter parametro, string NomeDataTable, DataSet DataSetOrigem) { return DataSet(InstrucaoSQL.ToString(), new List<SqlParameter>(new SqlParameter[] { parametro }), NomeDataTable, DataSetOrigem); }
        public DataSet DataSet(StringBuilder InstrucaoSQL, List<SqlParameter> parametros, string NomeDataTable, DataSet DataSetOrigem) { return DataSet(InstrucaoSQL.ToString(), parametros, NomeDataTable, DataSetOrigem); }
        public DataSet DataSet(string InstrucaoSQL, string NomeDataTable, DataSet DataSetOrigem) { return DataSet(InstrucaoSQL, new List<SqlParameter>(), NomeDataTable, DataSetOrigem); }
        public DataSet DataSet(string InstrucaoSQL, SqlParameter parametro, string NomeDataTable, DataSet DataSetOrigem) { return DataSet(InstrucaoSQL, new List<SqlParameter>(new SqlParameter[] { parametro }), NomeDataTable, DataSetOrigem); }
        public DataSet DataSet(string InstrucaoSQL, List<SqlParameter> parametros, string NomeDataTable, DataSet DataSetOrigem)
        {
            try
            {
                SqlCommand cmdDataSet = new SqlCommand(InstrucaoSQL.Trim(), _objCon);
                if (_transacao) { cmdDataSet.Transaction = _objTrans; }
                foreach (SqlParameter parametro in parametros)
                {
                    cmdDataSet.Parameters.Add(parametro);
                }
                SqlDataAdapter daDataSet = new SqlDataAdapter(cmdDataSet);
                if (DataSetOrigem == null)
                {
                    DataSet dsDataSet = new DataSet();
                    if (string.IsNullOrEmpty(NomeDataTable.Trim()))
                    {
                        daDataSet.Fill(dsDataSet);
                    }
                    else
                    {
                        daDataSet.Fill(dsDataSet, NomeDataTable.Trim());
                    }
                    return dsDataSet;
                }
                else
                {
                    daDataSet.Fill(DataSetOrigem, NomeDataTable.Trim());
                    return DataSetOrigem;
                }
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        public SqlDataReader ExecuteReader(StringBuilder InstrucaoSQL) { return ExecuteReader(InstrucaoSQL.ToString(), new List<SqlParameter>()); }
        public SqlDataReader ExecuteReader(string InstrucaoSQL) { return ExecuteReader(InstrucaoSQL, new List<SqlParameter>()); }
        public SqlDataReader ExecuteReader(StringBuilder InstrucaoSQL, SqlParameter parametro) { return ExecuteReader(InstrucaoSQL.ToString(), new List<SqlParameter>(new SqlParameter[] { parametro })); }
        public SqlDataReader ExecuteReader(StringBuilder InstrucaoSQL, List<SqlParameter> parametros) { return ExecuteReader(InstrucaoSQL.ToString(), parametros); }
        public SqlDataReader ExecuteReader(string InstrucaoSQL, SqlParameter parametro) { return ExecuteReader(InstrucaoSQL, new List<SqlParameter>(new SqlParameter[] { parametro })); }
        public SqlDataReader ExecuteReader(string InstrucaoSQL, List<SqlParameter> parametros)
        {
            try
            {
                SqlCommand cmdDataReader = new SqlCommand(InstrucaoSQL.Trim(), _objCon);
                if (_transacao) { cmdDataReader.Transaction = _objTrans; }
                foreach (SqlParameter parametro in parametros)
                {
                    cmdDataReader.Parameters.Add(parametro);
                }
                return cmdDataReader.ExecuteReader();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        public void ExecuteNonQuery(StringBuilder InstrucaoSQL) { ExecuteNonQuery(InstrucaoSQL.ToString(), new List<SqlParameter>()); }
        public void ExecuteNonQuery(string InstrucaoSQL) { ExecuteNonQuery(InstrucaoSQL, new List<SqlParameter>()); }
        public void ExecuteNonQuery(StringBuilder InstrucaoSQL, SqlParameter parametro) { ExecuteNonQuery(InstrucaoSQL.ToString(), new List<SqlParameter>(new SqlParameter[] { parametro })); }
        public void ExecuteNonQuery(StringBuilder InstrucaoSQL, List<SqlParameter> parametros) { ExecuteNonQuery(InstrucaoSQL.ToString(), parametros); }
        public void ExecuteNonQuery(string InstrucaoSQL, SqlParameter parametro) { ExecuteNonQuery(InstrucaoSQL.ToString(), new List<SqlParameter>(new SqlParameter[] { parametro })); }
        public void ExecuteNonQuery(string InstrucaoSQL, List<SqlParameter> parametros)
        {
            try
            {
                SqlCommand cmdExecuteNQuery = new SqlCommand(InstrucaoSQL.Trim(), _objCon);
                if (_transacao) { cmdExecuteNQuery.Transaction = _objTrans; }
                foreach (SqlParameter parametro in parametros)
                {
                    cmdExecuteNQuery.Parameters.Add(parametro);
                }
                cmdExecuteNQuery.ExecuteNonQuery();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        public object ExecuteScalar(StringBuilder InstrucaoSQL) { return ExecuteScalar(InstrucaoSQL.ToString(), new List<SqlParameter>()); }
        public object ExecuteScalar(string InstrucaoSQL) { return ExecuteScalar(InstrucaoSQL, new List<SqlParameter>()); }
        public object ExecuteScalar(StringBuilder InstrucaoSQL, SqlParameter parametro) { return ExecuteScalar(InstrucaoSQL.ToString(), new List<SqlParameter>(new SqlParameter[] { parametro })); }
        public object ExecuteScalar(StringBuilder InstrucaoSQL, List<SqlParameter> parametros) { return ExecuteScalar(InstrucaoSQL.ToString(), parametros); }
        public object ExecuteScalar(string InstrucaoSQL, SqlParameter parametro) { return ExecuteScalar(InstrucaoSQL, new List<SqlParameter>(new SqlParameter[] { parametro })); }
        public object ExecuteScalar(string InstrucaoSQL, List<SqlParameter> parametros)
        {
            try
            {
                SqlCommand cmdExecuteScalar = new SqlCommand(InstrucaoSQL.Trim(), _objCon);
                if (_transacao) { cmdExecuteScalar.Transaction = _objTrans; }
                foreach (SqlParameter parametro in parametros)
                {
                    cmdExecuteScalar.Parameters.Add(parametro);
                }
                return cmdExecuteScalar.ExecuteScalar();
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        public DataTable DataTableFile(string PathFile) { return DataSetFile(PathFile, string.Empty, null).Tables[0]; }

        public DataSet DataSetFile(string PathFile) { return DataSetFile(PathFile, string.Empty, null); }
        public DataSet DataSetFile(string PathFile, string NomeDataTable) { return DataSetFile(PathFile, NomeDataTable, null); }
        public DataSet DataSetFile(string PathFile, string NomeDataTable, DataSet DataSetOrigem)
        {
            StreamReader objSR = new StreamReader(PathFile.Trim());
            return DataSet(objSR.ReadToEnd().Trim(), new List<SqlParameter>(), NomeDataTable.Trim(), DataSetOrigem);
        }

        public SqlDataReader ExecuteReaderFile(string PathFile)
        {
            StreamReader objSR = new StreamReader(PathFile.Trim());
            return ExecuteReader(objSR.ReadToEnd().Trim(), new List<SqlParameter>());
        }

        public void ExecuteNonQueryFile(string PathFile)
        {
            StreamReader objSR = new StreamReader(PathFile.Trim());
            ExecuteNonQuery(objSR.ReadToEnd().Trim(), new List<SqlParameter>());
        }

        public object ExecuteScalarFile(string PathFile)
        {
            StreamReader objSR = new StreamReader(PathFile.Trim());
            return ExecuteScalar(objSR.ReadToEnd().Trim(), new List<SqlParameter>());
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class Criptografia
    {
        public enum TipoAcao { Encriptar, Decriptar }
        public enum TipoNivel { Baixo, MédioBaixo, Médio, MédioAlto, Alto }
        public enum TipoCripto { Símbolos, Números }
        string[] arrChaveOK;
        Funcoes objFunc = new Funcoes();

        public string GerarHash(string Texto)
        {
            MD5 md5 = MD5.Create();
            byte[] textoBytes = Encoding.ASCII.GetBytes(Texto);
            byte[] hash = md5.ComputeHash(textoBytes);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        public string Executar(string Texto, string Chave, TipoNivel Nivel, TipoAcao Acao, TipoCripto Cripto)
        {
            int _Nivel = 0;
            string TextoPronto = "";

            string VerOK = Verifica(Chave);
            if (VerOK.Trim() != "")
            {
                throw new Erro(VerOK.Trim());
            }

            ArrumaChave(Chave);

            switch (Nivel)
            {
                case TipoNivel.Baixo:
                    _Nivel = 1;
                    break;
                case TipoNivel.MédioBaixo:
                    _Nivel = 2;
                    break;
                case TipoNivel.Médio:
                    _Nivel = 3;
                    break;
                case TipoNivel.MédioAlto:
                    _Nivel = 4;
                    break;
                case TipoNivel.Alto:
                    _Nivel = 5;
                    break;
            }

            if (Acao == TipoAcao.Encriptar)
            {
                TextoPronto = Texto.Trim();

                for (int intFor = 1; intFor <= _Nivel; intFor++)
                {
                    TextoPronto = Encriptar(TextoPronto);
                }

                if (Cripto == TipoCripto.Símbolos)
                {
                    TextoPronto = PassaParaASCII(TextoPronto);
                }
            }
            else if (Acao == TipoAcao.Decriptar)
            {
                if (Cripto == TipoCripto.Símbolos)
                {
                    TextoPronto = PassaParaNumeros(Texto.Trim());
                }
                else
                {
                    TextoPronto = Texto.Trim();
                }

                for (int intFor = _Nivel; intFor >= 1; intFor--)
                {
                    TextoPronto = Decriptar(TextoPronto);
                }
            }

            return TextoPronto;
        }

        private string PassaParaASCII(string Texto)
        {
            string TextoPronto = "";
            string TextoAux = Texto.Trim();
            int Pedaco = 0;

            while (TextoAux.Trim() != "")
            {
                if (TextoAux.Trim().Length >= 2)
                {
                    Pedaco = int.Parse(TextoAux.Substring(0, 2));
                    TextoAux = TextoAux.Substring(2);
                }
                else
                {
                    Pedaco = int.Parse(TextoAux);
                    TextoAux = "";
                }

                if (Pedaco < 34 || Pedaco == 39)
                {
                    Pedaco = Pedaco + 100;
                }

                TextoPronto = TextoPronto.Trim() + objFunc.Chr(Pedaco).ToString().Trim();
            }

            return TextoPronto;
        }

        private string PassaParaNumeros(string Texto)
        {
            string TextoPronto = "";
            int Codigo = 0;

            for (int intFor = 0; intFor < Texto.Trim().Length; intFor++)
            {
                Codigo = objFunc.Asc(Texto.Substring(intFor, 1));
                if (Codigo > 99)
                {
                    Codigo = Codigo - 100;
                }

                if (intFor == (Texto.Trim().Length - 1))
                {
                    TextoPronto = TextoPronto.Trim() + Codigo.ToString().Trim();
                }
                else
                {
                    TextoPronto = TextoPronto.Trim() + Codigo.ToString("00").Trim();
                }
            }

            return TextoPronto;
        }

        private string Verifica(string Chave)
        {
            int TesteParse = 0;

            for (int intFor = 0; intFor < Chave.Length; intFor++)
            {
                if (!int.TryParse(Chave.Substring(intFor, 1), out TesteParse))
                {
                    return "A chave deve ser totalmente numérica";
                }
            }

            if (Chave.Trim().Length < 20)
            {
                return "A chave deve ser no mínimo 20 posições numéricas.";
            }

            return "";
        }

        private void ArrumaChave(string Chave)
        {
            string ChaveAux = Chave.Trim();
            int Pedaco = 0;
            string ChaveOK = "";

            while (ChaveAux.Trim().Length >= 3)
            {
                Pedaco = int.Parse(ChaveAux.Substring(0, 3));
                if (Pedaco >= 100 && Pedaco <= 255)
                {
                    ChaveOK = ChaveOK.Trim() + Pedaco.ToString().Trim() + "|";
                }
                ChaveAux = ChaveAux.Substring(3);
            }

            ChaveOK = ChaveOK.Substring(0, ChaveOK.Trim().Length - 1);
            arrChaveOK = ChaveOK.Split(new Char[] { '|' });
        }

        private string Encriptar(string Texto)
        {
            try
            {
                string TextoE = "";
                int intArrChaveOK = 0;

                for (int intFor = 0; intFor < Texto.Length; intFor++)
                {
                    TextoE = TextoE.Trim() + (objFunc.Asc(Texto.Substring(intFor, 1)) + int.Parse(arrChaveOK[intArrChaveOK]));

                    if (intArrChaveOK < (arrChaveOK.Length - 1))
                    {
                        intArrChaveOK++;
                    }
                    else
                    {
                        intArrChaveOK = 0;
                    }
                }

                return TextoE;
            }
            catch (Erro oErro)
            {
                throw new Erro(oErro.Mensagem);
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        private string Decriptar(string Texto)
        {
            try
            {
                string TextoD = "";
                int intArrChaveOK = 0;

                while (Texto.Trim() != "")
                {
                    TextoD = TextoD + objFunc.Chr(int.Parse(Texto.Substring(0, 3)) - int.Parse(arrChaveOK[intArrChaveOK]));

                    Texto = Texto.Substring(3);

                    if (intArrChaveOK < (arrChaveOK.Length - 1))
                    {
                        intArrChaveOK++;
                    }
                    else
                    {
                        intArrChaveOK = 0;
                    }
                }

                return TextoD.Trim();
            }
            catch (Erro oErro)
            {
                throw new Erro(oErro.Mensagem);
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }
    }

    public class Reflection
    {
        public object Executar(string AssemblyFile, string AssemblyType, string Metodo)
        {
            return Executar(AssemblyFile.Trim(), AssemblyType.Trim(), Metodo.Trim(), null);
        }

        public object Executar(string AssemblyFile, string AssemblyType, string Metodo, object[] Parametros)
        {
            return Executar(AssemblyFile.Trim(), AssemblyType.Trim(), Metodo.Trim(), null, Parametros);
        }

        public object Executar(string AssemblyFile, string AssemblyType, string Metodo, Type[] AssinaturaMetodo, object[] Parametros)
        {
            try
            {
                Assembly objAssembly = Assembly.LoadFrom(AssemblyFile.Trim());
                Type objAssemblyType = objAssembly.GetType(AssemblyType.Trim());
                Type[] objCtorParameter = Type.EmptyTypes;
                ConstructorInfo objCtor = objAssemblyType.GetConstructor(objCtorParameter);
                object objInstance = objCtor.Invoke(new object[] { });

                MethodInfo objMI;
                if (AssinaturaMetodo == null)
                {
                    objMI = objInstance.GetType().GetMethod(Metodo.Trim());
                }
                else
                {
                    objMI = objInstance.GetType().GetMethod(Metodo.Trim(), AssinaturaMetodo);
                }
                return objMI.Invoke(objInstance, Parametros);
            }
            catch (Erro oErro)
            {
                throw new Erro(oErro.Mensagem);
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }
    }

    public class Serializador
    {
        public byte[] SerializeFile(string PathFile)
        {
            FileStream fsr = new FileStream(PathFile, FileMode.OpenOrCreate, FileAccess.Read);
            byte[] MyData = new byte[fsr.Length];
            fsr.Read(MyData, 0, System.Convert.ToInt32(fsr.Length));
            fsr.Close();
            return MyData;
        }

        public void DeserializeFile(string PathFile, byte[] Binary)
        {
            FileStream fsw = new FileStream(PathFile, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fsw);
            bw.Write(Binary);
            bw.Close();
            fsw.Close();
        }

        public void DeserializeFile(string PathFile, MemoryStream Binary)
        {
            FileStream fsw = new FileStream(PathFile, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] bytes = new byte[Binary.Length];
            Binary.Read(bytes, 0, (int)Binary.Length);
            fsw.Write(bytes, 0, bytes.Length);
            fsw.Close();
        }
    }

    public class Zip
    {
        public void Compactar(string ZipFileName, string SourceDirectory, bool Recurse)
        {
            Compactar(ZipFileName, SourceDirectory, Recurse, string.Empty);
        }

        public void Compactar(string ZipFileName, string SourceDirectory, bool Recurse, string FileFilter)
        {
            try
            {
                FastZip zipFastFile = new FastZip();
                zipFastFile.CreateZip(ZipFileName, SourceDirectory, Recurse, FileFilter);
            }
            catch (Erro oErro)
            {
                throw new Erro(oErro.Mensagem);
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }

        public void Descompactar(string ZipFileName, string TargetDirectory)
        {
            Descompactar(ZipFileName, TargetDirectory, string.Empty);
        }

        public void Descompactar(string ZipFileName, string TargetDirectory, string FileFilter)
        {
            try
            {
                FastZip zipFastFile = new FastZip();
                zipFastFile.ExtractZip(ZipFileName, TargetDirectory, FileFilter);
            }
            catch (Erro oErro)
            {
                throw new Erro(oErro.Mensagem);
            }
            catch (Exception oEx)
            {
                throw new Erro(oEx.Message);
            }
        }
    }

    public class InfoSis 
    {
        public string HostName() 
        {
            return Dns.GetHostName().Trim();
        }

        public string UsuarioLogado() 
        {
            return System.Environment.UserName.Trim();
        }

        public string IP() 
        {
            return IP(HostName());
        }

        public string IP(string hostName)
        {
            string ip = string.Empty;

            try
            {
                IPHostEntry ipEntry = Dns.GetHostByName(hostName);
                IPAddress[] addr = ipEntry.AddressList;

                ip = addr[0].ToString().Trim();
            }
            catch
            {
                ip = string.Empty;
            }

            return ip;
        }
    }

    public class Imagem : IDisposable
    {
        #region | IDisposable Members

        private bool _isDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Imagem()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (disposing)
                {
                    this.Dispose();
                }
            }
        }

        #endregion | IDisposable Members

        public System.Drawing.Image Carregar(string caminhoImagem)
        {
            using (var ms = new MemoryStream(File.ReadAllBytes(caminhoImagem))) 
            {
                return System.Drawing.Image.FromStream(ms); 
            } 
        }

        public System.Drawing.Image Reduzir(string caminhoImagem, int width, int height)
        {
            using (var ms = new MemoryStream(File.ReadAllBytes(caminhoImagem)))
            {
                return System.Drawing.Image.FromStream(ms).GetThumbnailImage(width, height, null, new IntPtr());
            } 
        }

        public System.Drawing.Image Tarja(string caminhoImagemOrigem, Color cor, int top, int left, int height, int width) 
        {
            using (var ms = new MemoryStream(File.ReadAllBytes(caminhoImagemOrigem)))
            {
                return Tarja(System.Drawing.Image.FromStream(ms), cor, new Rectangle(left, top, width, height));
            }
        }

        public System.Drawing.Image Tarja(string caminhoImagemOrigem, Color cor, Rectangle retangulo)
        {
            using (var ms = new MemoryStream(File.ReadAllBytes(caminhoImagemOrigem)))
            {
                return Tarja(System.Drawing.Image.FromStream(ms), cor, retangulo);
            }
        }

        public System.Drawing.Image Tarja(System.Drawing.Image imagemOriginal, Color cor, int top, int left, int height, int width)
        {
            return Tarja(imagemOriginal, cor, new Rectangle(left, top, width, height));
        }

        public System.Drawing.Image Tarja(System.Drawing.Image imagemOriginal, Color cor, Rectangle retangulo)
        {
            Bitmap imagemClonada = new Bitmap((System.Drawing.Image)imagemOriginal.Clone());
            using (Graphics graphics = Graphics.FromImage(imagemClonada))
            {
                graphics.FillRectangle(new SolidBrush(cor), retangulo);
                return (System.Drawing.Image)imagemClonada;
            }
        }

        public System.Drawing.Image Negativo(string caminhoImagemOrigem) 
        {
            using (var ms = new MemoryStream(File.ReadAllBytes(caminhoImagemOrigem))) 
            {
                return Negativo(System.Drawing.Image.FromStream(ms));
            }
        }

        public System.Drawing.Image Negativo(System.Drawing.Image imagemOriginal)
        {
            Bitmap original = new Bitmap(imagemOriginal);
            Bitmap novo = new Bitmap(original.Width, original.Height);
            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    Color originalColor = original.GetPixel(i, j);
                    int r = 255 - originalColor.R;
                    int g = 255 - originalColor.G;
                    int b = 255 - originalColor.B;
                    Color CorEmNegativo = Color.FromArgb(r, g, b);
                    novo.SetPixel(i, j, CorEmNegativo);
                }
            }
            return (System.Drawing.Image)novo;
        }

        public System.Drawing.Image EscalaDeCinza(string caminhoImagemOrigem)
        {
            using (var ms = new MemoryStream(File.ReadAllBytes(caminhoImagemOrigem)))
            {
                return EscalaDeCinza(System.Drawing.Image.FromStream(ms));
            }
        }

        public System.Drawing.Image EscalaDeCinza(System.Drawing.Image imagemOriginal)
        {
            Bitmap original = new Bitmap(imagemOriginal);
            Bitmap novo = new Bitmap(original.Width, original.Height);
            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    Color originalColor = original.GetPixel(i, j);
                    int grayScale = (int)((originalColor.R * 0.3) + (originalColor.G * 0.59) + (originalColor.B * 0.11));
                    Color CorEmEscalaDeCinza = Color.FromArgb(grayScale, grayScale, grayScale);
                    novo.SetPixel(i, j, CorEmEscalaDeCinza);
                }
            }
            return (System.Drawing.Image)novo;
        }

        public Bitmap Recortar(string caminhoImagemOrigem, int top, int left, int width, int height)
        {
            return Recortar(caminhoImagemOrigem, string.Empty, top, left, width, height, 0, 0);
        }

        public Bitmap Recortar(string caminhoImagemOrigem, string caminhoImagemDestino, int top, int left, int width, int height) 
        {
            return Recortar(caminhoImagemOrigem, caminhoImagemDestino, top, left, width, height, 0, 0);
        }

        public Bitmap Recortar(string caminhoImagemOrigem, int top, int left, int width, int height, float xDpi, float yDpi) 
        {
            return Recortar(caminhoImagemOrigem, string.Empty, top, left, width, height, xDpi, yDpi);
        }

        public Bitmap Recortar(string caminhoImagemOrigem, string caminhoImagemDestino, int top, int left, int width, int height, float xDpi, float yDpi) 
        {
            using (var ms = new MemoryStream(File.ReadAllBytes(caminhoImagemOrigem)))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                if (xDpi != 0 && yDpi != 0) { bmp.SetResolution(xDpi, yDpi); }

                Graphics gfx = Graphics.FromImage(bmp);
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gfx.DrawImage(image, new Rectangle(0, 0, width, height), left, top, width, height, GraphicsUnit.Pixel);

                if (!string.IsNullOrEmpty(caminhoImagemDestino.Trim()))
                {
                    bmp.Save(caminhoImagemDestino, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                return bmp;
            }
        }

        public System.Drawing.Image Rotacionar(System.Drawing.Image image, RotateFlipType angulo)
        {
            image.RotateFlip(angulo);
            return image;
        }

        public System.Drawing.Image Rotacionar(string caminhoImagemOrigem, RotateFlipType angulo)
        {
            using (var ms = new MemoryStream(File.ReadAllBytes(caminhoImagemOrigem)))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                image.RotateFlip(angulo);
                return image;
            }
        }

        public void Rotacionar(string caminhoImagemOrigem, string caminhoImagemDestino, RotateFlipType angulo)
        {
            using (var ms = new MemoryStream(File.ReadAllBytes(caminhoImagemOrigem)))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms); 
                image.RotateFlip(angulo);
                image.Save(caminhoImagemDestino, ImageFormat.Jpeg);
            }
        }

        public void Rotacionar(System.Drawing.Image image, string caminhoImagemDestino, RotateFlipType angulo)
        {
            image.RotateFlip(angulo);
            image.Save(caminhoImagemDestino, ImageFormat.Jpeg);
        }
    }

    public class Base64 
    {
        public string Codificar(string origem)
        {
            FileStream sr = new FileStream(origem, FileMode.Open);
            byte[] srcBT = new byte[sr.Length];
            sr.Read(srcBT, 0, (int)sr.Length);
            sr.Close();
            return System.Convert.ToBase64String(srcBT);
        }

        public void Codificar(string origem, string destino) 
        {
            FileStream sr = new FileStream(origem, FileMode.Open);
            byte[] srcBT = new byte[sr.Length];
            sr.Read(srcBT, 0, (int)sr.Length);
            sr.Close();
            string dest = System.Convert.ToBase64String(srcBT);
            StreamWriter sw = new StreamWriter(destino, false);
            sw.Write(dest);
            sw.Close();
        }

        public System.Drawing.Image Decodificar(string origem)
        {
            using (StreamReader sr = new StreamReader(origem))
            {
                string src = sr.ReadToEnd();
                sr.Close();
                using (MemoryStream ms = new MemoryStream(System.Convert.FromBase64String(src))) 
                {
                    return System.Drawing.Image.FromStream(ms);
                }
            }
        }


        public void Decodificar(string origem, string destino) { Decodificar(origem, destino, false); }
        public void Decodificar(string origem, string destino, bool sobrescrever) 
        {
            if (!sobrescrever && File.Exists(destino)) 
            {
                throw new Exception(string.Concat("Arquivo '", destino.Trim(), "' já existe."));
            } 

            StreamReader sr = new StreamReader(origem);
            string src = sr.ReadToEnd();
            sr.Close();
            byte[] bt64 = System.Convert.FromBase64String(src);
            if (File.Exists(destino)) { File.Delete(destino); }
            FileStream sw = new FileStream(destino, FileMode.Create);
            sw.Write(bt64, 0, bt64.Length);
            sw.Close();
        }
    }

    public class Email
    {
        public void Enviar(MailAddress remetente, string senhaRemetente, string dominio, MailAddress destinatario, List<string> anexos, string assunto, string mensagem, string smtp, int porta)
        {
            List<MailAddress> destinatarios = new List<MailAddress>();
            destinatarios.Add(destinatario);
            Enviar(remetente, senhaRemetente, dominio, destinatarios, anexos, assunto, mensagem, smtp, porta);
        }
        public void Enviar(MailAddress remetente, string senhaRemetente, string dominio, List<MailAddress> destinatarios, List<string> anexos, string assunto, string mensagem, string smtp, int porta)
        {
            SmtpClient cliente = new SmtpClient(smtp.Trim(), porta);
            cliente.EnableSsl = true;

            MailMessage email = new MailMessage();
            email.From = remetente;
            foreach (MailAddress destinatario in destinatarios)
            {
                email.To.Add(destinatario);
            }
            email.Subject = assunto.Trim();
            email.Body = mensagem.Trim();

            foreach (string anexo in anexos)
            {
                if (!File.Exists(anexo.Trim()))
                {
                    throw new Erro(string.Concat("Arquivo '", anexo.Trim(), "' não localizado."));
                }
                else
                {
                    Attachment anexado = new Attachment(anexo, MediaTypeNames.Application.Octet);
                    email.Attachments.Add(anexado);
                }
            }

            NetworkCredential credenciais = new NetworkCredential(remetente.Address.Trim(), senhaRemetente.Trim(), dominio.Trim());
            cliente.Credentials = credenciais;

            cliente.Send(email);
        }
    }

    public class Funcoes
    {
        public void DuplicarPasta(string pastaOrigem, string pastaDestino)
        {
            try
            {
                DirectoryInfo diOrigem = new DirectoryInfo(pastaOrigem);

                foreach (FileInfo file in diOrigem.GetFiles())
                {
                    file.CopyTo(Path.Combine(pastaDestino.Trim(), file.Name.Trim()));
                }

                foreach (DirectoryInfo di in diOrigem.GetDirectories())
                {
                    if (!Directory.Exists(Path.Combine(pastaDestino.Trim(), di.Name.Trim()))) { Directory.CreateDirectory(Path.Combine(pastaDestino.Trim(), di.Name.Trim())); }
                    DuplicarPasta(Path.Combine(pastaOrigem.Trim(), di.Name.Trim()), Path.Combine(pastaDestino.Trim(), di.Name.Trim()));
                }

            }
            catch (Exception oEx)
            {
                throw new Erro(string.Concat("Inpossível copiar pasta.\n", oEx.Message));
            }

        }

        public string AppPath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }


        public enum TipoHora { _12horas, _24horas };

        public double Trunc(double valor, int decimais)
        {
            double retorno = valor;

            for (int i = 1; i <= decimais; i++)
            {
                retorno = retorno * 10;
            }

            retorno = Math.Truncate(retorno);

            for (int i = 1; i <= decimais; i++)
            {
                retorno = retorno / 10;
            }

            return retorno;
        }

        public decimal Trunc(decimal valor, int decimais)
        {
            decimal retorno = valor;

            for (int i = 1; i <= decimais; i++)
            {
                retorno = retorno * 10;
            }

            retorno = Math.Truncate(retorno);

            for (int i = 1; i <= decimais; i++)
            {
                retorno = retorno / 10;
            }

            return retorno;
        }

        public string[] RedimensionarArray(string[] Array, int NovoTamanho, bool PreservarDados)
        {
            string[] ArrayNEW = new string[NovoTamanho];
            if (PreservarDados)
            {
                int TamanhoLoop = Array.Length <= ArrayNEW.Length ? Array.Length : ArrayNEW.Length;
                for (int intFor = 0; intFor < TamanhoLoop; intFor++)
                {
                    ArrayNEW[intFor] = Array[intFor];
                }
            }
            return ArrayNEW;
        }

        public int[] RedimensionarArray(int[] Array, int NovoTamanho, bool PreservarDados)
        {
            int[] ArrayNEW = new int[NovoTamanho];
            if (PreservarDados)
            {
                int TamanhoLoop = Array.Length <= ArrayNEW.Length ? Array.Length : ArrayNEW.Length;
                for (int intFor = 0; intFor < TamanhoLoop; intFor++)
                {
                    ArrayNEW[intFor] = Array[intFor];
                }
            }
            return ArrayNEW;
        }

        public double[] RedimensionarArray(double[] Array, int NovoTamanho, bool PreservarDados)
        {
            double[] ArrayNEW = new double[NovoTamanho];
            if (PreservarDados)
            {
                int TamanhoLoop = Array.Length <= ArrayNEW.Length ? Array.Length : ArrayNEW.Length;
                for (int intFor = 0; intFor < TamanhoLoop; intFor++)
                {
                    ArrayNEW[intFor] = Array[intFor];
                }
            }
            return ArrayNEW;
        }

        public string NomePastaPuro(string FullPathName)
        {
            string Path = "";
            string[] arrPath = FullPathName.Split(Convert.ToChar(@"\"));

            for (int intFor = 0; intFor < arrPath.Length; intFor++)
            {
                Path = string.Concat(Path.Trim(), arrPath[intFor].Trim(), @"\");
            }

            return Path.Trim();
        }

        public string NomeArquivoPuro(string FullPathName)
        {
            string[] arrPath = FullPathName.Split(Convert.ToChar(@"\"));
            string NomeArquivo = arrPath[arrPath.Length - 1];
            return NomeArquivo;
        }

        public char Chr(int Codigo)
        {
            return (char)Codigo;
        }

        public int Asc(string Letra)
        {
            return (int)(Convert.ToChar(Letra));
        }

        public string TrataHora(int Hora) { return TrataHora((long)Hora); }
        public string TrataHora(long Hora)
        {
            string retorno = string.Empty;
            switch (Hora.ToString().Trim().Length)
            {
                case 3:
                    retorno = string.Concat(Right(string.Concat("0", Hora.ToString().Substring(0, 1)), 2), ":", Hora.ToString().Substring(1, 2), ":00");
                    break;
                case 4:
                    retorno = string.Concat(Hora.ToString().Substring(0, 2), ":", Hora.ToString().Substring(2, 2), ":00");
                    break;
                case 5:
                    retorno = string.Concat(Right(string.Concat("0", Hora.ToString().Substring(0, 1)), 2), ":", Hora.ToString().Substring(1, 2), ":", Hora.ToString().Substring(3, 2));
                    break;
                case 6:
                    retorno = string.Concat(Hora.ToString().Substring(0, 2), ":", Hora.ToString().Substring(2, 2), ":", Hora.ToString().Substring(4, 2));
                    break;
                default:
                    retorno = Hora.ToString();
                    break;
            }

            return retorno;
        }

        public long TrataHora(string Hora)
        {
            return long.Parse(Hora.Replace(":", ""));
        }

        public string TrataHora24SoMinutos(int Hora)
        {
            string hora = Right(string.Concat("0000", Hora.ToString().Trim()), 4);
            return string.Concat(hora.Substring(0, 2) + ":" + hora.Substring(2, 2));
        }

        public int TrataHora24SoMinutos(string Hora)
        {
            Hora = Hora.Replace(":", "");
            return int.Parse(Hora);
        }

        public string TrataData(int Data) { return TrataData((long)Data); }
        public string TrataData(long Data)
        {
            if (Data.ToString().Length != 8)
            {
                return string.Empty;
            }
            else
            {
                return string.Concat(Data.ToString().Trim().Substring(6, 2), "/",
                                     Data.ToString().Trim().Substring(4, 2), "/",
                                     Data.ToString().Trim().Substring(0, 4));
            }
        }

        public long TrataData(string Dia, string Mes, string Ano)
        {
            string dia = Right(string.Concat("00", Dia.Trim()), 2);
            string mes = Right(string.Concat("00", Mes.Trim()), 2);
            string ano = Right(string.Concat("0000", Ano.Trim()), 4);

            return long.Parse(string.Concat(ano.Trim(), mes.Trim(), dia.Trim()));
        }

        public long TrataData(string Data, bool invertida)
        {
            if (string.IsNullOrEmpty(Data.Trim()))
            {
                return 0;
            }
            else
            {
                Data = Data.Replace("/", "");
                if (invertida)
                {
                    return long.Parse(string.Concat(Data.Trim().Substring(0, 4),
                                                    Data.Trim().Substring(4, 2),
                                                    Data.Trim().Substring(6, 2)));
                }
                else
                {
                    return long.Parse(string.Concat(Data.Trim().Substring(4, 4),
                                                    Data.Trim().Substring(2, 2),
                                                    Data.Trim().Substring(0, 2)));
                }
            }
        }

        public enum TipoSomaData { Dia, Mes, Ano }
        public int SomaData(int Data, int quatidade, TipoSomaData tipoSomaData) { return (int)SomaData((long)Data, quatidade, tipoSomaData); }
        public long SomaData(long Data, int quatidade, TipoSomaData tipoSomaData)
        {
            if (Data.ToString().Length != 8)
            {
                return 0;
            }
            else if (quatidade == 0)
            {
                return Data;
            }
            else
            {
                DateTime data = DateTime.Parse(string.Concat(Data.ToString().Trim().Substring(0, 4), "/",
                                                             Data.ToString().Trim().Substring(4, 2), "/",
                                                             Data.ToString().Trim().Substring(6, 2)));
                switch (tipoSomaData)
                {
                    case TipoSomaData.Dia:
                        data = data.AddDays(quatidade);
                        break;
                    case TipoSomaData.Mes:
                        data = data.AddMonths(quatidade);
                        break;
                    case TipoSomaData.Ano:
                        data = data.AddYears(quatidade);
                        break;
                }
                return long.Parse(data.ToString("yyyyMMdd"));
            }
        }

        public string[] SomaData(string Dia, string Mes, string Ano, int quatidade, TipoSomaData tipoSomaData)
        {
            string dia = Right(string.Concat("00", Dia.Trim()), 2);
            string mes = Right(string.Concat("00", Mes.Trim()), 2);
            string ano = Right(string.Concat("0000", Ano.Trim()), 4);
            string data = SomaData(long.Parse(string.Concat(ano.Trim(), mes.Trim(), dia.Trim())), quatidade, tipoSomaData).ToString();

            return new string[] { data.Substring(6, 2), data.Substring(4, 2), data.Substring(0, 4) };
        }

        public string SomaData(string Data, bool invertida, int quatidade, TipoSomaData tipoSomaData)
        {
            if (string.IsNullOrEmpty(Data.Trim()))
            {
                return string.Empty;
            }
            else
            {
                long dataN = 0;
                string dataS = string.Empty;
                if (invertida)
                {
                    dataN = SomaData(long.Parse(string.Concat(Data.Trim().Substring(0, 4),
                                                              Data.Trim().Substring(5, 2),
                                                              Data.Trim().Substring(8, 2))),
                                     quatidade,
                                     tipoSomaData);
                    dataS = string.Concat(dataN.ToString().Substring(0, 4), "/",
                                          dataN.ToString().Substring(4, 2), "/",
                                          dataN.ToString().Substring(6, 2));

                }
                else
                {
                    dataN = SomaData(long.Parse(string.Concat(Data.Trim().Substring(6, 4),
                                                              Data.Trim().Substring(3, 2),
                                                              Data.Trim().Substring(0, 2))),
                                     quatidade,
                                     tipoSomaData);
                    dataS = string.Concat(dataN.ToString().Substring(6, 2), "/",
                                          dataN.ToString().Substring(4, 2), "/",
                                          dataN.ToString().Substring(0, 4));
                }
                return dataS;
            }
        }

        public string Espaco(long Quantidade)
        {
            string Resposta = "";
            for (long intFor = 1; intFor <= Quantidade; intFor++)
            {
                Resposta = Resposta + " ";
            }
            return Resposta;
        }

        public string Caracter(long Quantidade, string Texto)
        {
            string Resposta = "";
            for (long intFor = 1; intFor <= Quantidade; intFor++)
            {
                Resposta = Resposta + Texto.ToString();
            }
            return Resposta;
        }

        public string Left(long Valor, int Tamanho) { return Left(Valor.ToString(), Tamanho); }
        public string Left(int Valor, int Tamanho) { return Left(Valor.ToString(), Tamanho); }
        public string Left(double Valor, int Tamanho) { return Left(Valor.ToString(), Tamanho); }

        public string Left(string Texto, int Tamanho)
        {
            return Texto.Substring(0, Tamanho);
        }

        public string Right(long Valor, int Tamanho) { return Right(Valor.ToString(), Tamanho); }
        public string Right(int Valor, int Tamanho) { return Right(Valor.ToString(), Tamanho); }
        public string Right(double Valor, int Tamanho) { return Right(Valor.ToString(), Tamanho); }

        public string Right(string Texto, int Tamanho)
        {
            return Texto.Substring(Texto.Length - Tamanho);
        }

        public bool ValidadeData(long Data)
        {
            return ValidadeData(Data.ToString().Substring(6, 2),
                                Data.ToString().Substring(4, 2),
                                Data.ToString().Substring(0, 4));
        }

        public bool ValidadeData(string Data, bool invertida)
        {
            if (Data.Trim().Length != 10) { return false; }
            string[] data = Data.Split('/');
            if (data.Length < 3) { return false; }
            if (invertida)
            {
                return ValidadeData(data[2], data[1], data[0]);
            }
            else
            {
                return ValidadeData(data[0], data[1], data[2]);
            }
        }

        public bool ValidadeData(string Dia, string Mes, string Ano)
        {
            string dia = Right(string.Concat("00", Dia.Trim()), 2);
            string mes = Right(string.Concat("00", Mes.Trim()), 2);
            string ano = Right(string.Concat("0000", Ano.Trim()), 4);

            long dataValidade = 0;
            if (!long.TryParse(string.Concat(ano, mes, dia), out dataValidade))
            {
                return false;
            }

            DateTime validade = DateTime.MinValue;
            if (!DateTime.TryParse(string.Concat(ano, "/", mes, ",", dia), out validade))
            {
                return false;
            }

            return true;
        }

        public bool ValidadeHora(int Hora, TipoHora tipo)
        {
            return ValidadeHora((long)Hora, tipo);
        }

        public bool ValidadeHora(long Hora, TipoHora tipo)
        {
            if (Hora.ToString().Length != 3 && Hora.ToString().Length != 4 && Hora.ToString().Length != 6) { return false; }

            long hora = 0;
            long minuto = 0;
            long segundo = 0;

            if (Hora.ToString().Length == 3)
            {
                hora = long.Parse(Hora.ToString().Substring(0, 1));
                minuto = long.Parse(Hora.ToString().Substring(1, 2));
            }
            else if (Hora.ToString().Length == 4)
            {
                if (long.Parse(Hora.ToString().Substring(0, 2)) < 60)
                {
                    hora = long.Parse(Hora.ToString().Substring(0, 2));
                    minuto = long.Parse(Hora.ToString().Substring(2, 2));
                }
                else
                {
                    minuto = long.Parse(Hora.ToString().Substring(0, 2));
                    segundo = long.Parse(Hora.ToString().Substring(2, 2));
                }
            }
            else if (Hora.ToString().Length == 6)
            {
                hora = long.Parse(Hora.ToString().Substring(0, 2));
                minuto = long.Parse(Hora.ToString().Substring(2, 2));
                segundo = long.Parse(Hora.ToString().Substring(4, 2));
            }

            return ValidadeHora(hora, minuto, segundo, tipo);
        }

        public bool ValidadeHora(string Hora, TipoHora tipo)
        {
            string[] arrHora = Hora.Split(':');
            long hora = long.Parse(arrHora[0]);
            long minuto = long.Parse(arrHora[1]);
            long segundo = long.Parse(arrHora[2]);

            return ValidadeHora(hora, minuto, segundo, tipo);
        }

        public bool ValidadeHora(long Hora, long Minuto, long Segundo, TipoHora tipo)
        {
            if (tipo == TipoHora._12horas)
            {
                if (Hora < 0 || Hora > 12) { return false; }
            }
            else if (tipo == TipoHora._24horas)
            {
                if (Hora < 0 || Hora > 23) { return false; }
            }
            if (Minuto < 0 || Minuto > 59) { return false; }
            if (Segundo < 0 || Segundo > 59) { return false; }

            return true;
        }

        public bool ValidaCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(".", string.Empty).Replace(",", string.Empty).Replace("-", string.Empty).Trim();

            if (valor.Length != 11)
            {
                return false;
            }

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
            {
                if (valor[i] != valor[0])
                {
                    igual = false;
                }
            }

            if (igual || valor == "12345678909")
            {
                return false;
            }

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
            {
                numeros[i] = int.Parse(valor[i].ToString());
            }

            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += (10 - i) * numeros[i];
            }

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                {
                    return false;
                }
            }
            else if (numeros[9] != 11 - resultado)
            {
                return false;
            }

            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += (11 - i) * numeros[i];
            }

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                {
                    return false;
                }
            }
            else
            {
                if (numeros[10] != 11 - resultado)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ValidaCNPJ(string vrCNPJ)
        {
            string CNPJ = vrCNPJ.Replace(".", "").Replace(",", "").Replace(",", "").Replace("/", "").Replace("-", "").Trim();

            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;

            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11)
                    {
                        soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));
                    }
                    if (nrDig <= 12)
                    {
                        soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
                    }
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                    {
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
                    }
                    else
                    {
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                    }
                }
                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
        }

        public string FormatarCPF(string cpf)
        {
            cpf = Right(string.Concat("00000000000" + cpf.Trim().Replace(".", "").Replace("-", "").Replace("/", "")), 11);
            return string.Concat(cpf.Substring(0, 3), ".", cpf.Substring(3, 3), ".", cpf.Substring(6, 3), "-", cpf.Substring(9, 2));
        }

        public string FormatarCNPJ(string cnpj)
        {
            cnpj = Right(string.Concat("00000000000000" + cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "")), 14);
            return string.Concat(cnpj.Substring(0, 2), ".", cnpj.Substring(2, 3), ".", cnpj.Substring(5, 3), "/", cnpj.Substring(8, 4), "-", cnpj.Substring(12, 2));
        }
    }

    public class LerArquivo : IDisposable 
    {
        public List<string> Ler(string caminhoCompleto) 
        {
            return Ler(new FileInfo(caminhoCompleto));
        }

        public List<string> Ler(FileInfo arquivo) 
        {
            List<string> retorno = new List<string>();

            if (!arquivo.Exists) { throw new Erro(string.Concat("Arquivo '", arquivo.FullName, "' não localizado.")); }

            using (StreamReader streamReader = new StreamReader(arquivo.FullName))
            {
                string linha = string.Empty;
                while ((linha = streamReader.ReadLine()) != null)
                {
                    retorno.Add(linha);
                }
            }

            return retorno;
        }


        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class GerarArquivo : IDisposable
    {
        public void Txt(string caminhoCompleto, List<string> arquivo, bool sobreEscrever)
        {
            Verifica(caminhoCompleto, sobreEscrever);
            using (StreamWriter writer = new StreamWriter(caminhoCompleto.Trim(), true, Encoding.Unicode))
            {
                Gravar(writer, arquivo);
            }
        }

        public void Cvs(string caminhoCompleto, List<string> arquivo, bool sobreEscrever)
        {
            Verifica(caminhoCompleto, sobreEscrever);
            using (StreamWriter writer = new StreamWriter(caminhoCompleto.Trim(), false, Encoding.GetEncoding("iso-8859-15")))
            {
                Gravar(writer, arquivo);
            }
        }

        private void Verifica(string caminhoCompleto, bool sobreEscrever)
        {
            if (File.Exists(caminhoCompleto))
            {
                if (sobreEscrever)
                {
                    File.Delete(caminhoCompleto);
                }
                else
                {
                    throw new Erro(string.Concat("Arquivo '", caminhoCompleto.Trim(), "' já existe."));
                }
            }
        }

        private void Gravar(StreamWriter writer, List<string> arquivo)
        {
            foreach (string linha in arquivo)
            {
                writer.WriteLine(linha);
            }
            writer.Close();
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
