namespace MK.Easydoc.Core.Entities
{
    public class UserDataEntity
    {
        #region Public Properties

        public int IdClienteAtual { get; set; }
        public Cliente ClienteAtual { get; set; }
        public Servico ServicoAtual { get; set; }
        public string UrlCSSClienteAtual { get; set; }
        public Usuario UsuarioAtual { get; set; }

        #endregion 

        #region Constructors

        public UserDataEntity()
        {
            //UrlCSSClienteAtual = "./assets/themes/tecfort-theme.css";
        }

        //public UserDataEntity()
        //{
        //    IdClienteAtual = 0;
        //    ClienteAtual = new Cliente();
        //    ServicoAtual = new Servico();
        //    UsuarioAtual = new Usuario();
        //    UrlCSSClienteAtual = "./assets/themes/tecfort-theme.css";

        //}

        public UserDataEntity(int idClienteAtual, Cliente clienteAtual, Servico servicoAtual, Usuario usuarioAtual,string urlCSSClienteAtual)
        {
            this.IdClienteAtual = idClienteAtual;
            this.ClienteAtual = clienteAtual;
            this.UrlCSSClienteAtual = urlCSSClienteAtual;
            this.ServicoAtual = servicoAtual;
            this.UsuarioAtual = usuarioAtual;
        }

        #endregion
    }
}
