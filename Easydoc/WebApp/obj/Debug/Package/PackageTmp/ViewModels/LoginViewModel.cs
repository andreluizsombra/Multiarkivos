
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MK.Easydoc.WebApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O nome de usuário é de preenchimento obrigatório.")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "A senha é de preenchimento obrigatório.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DisplayName("Mantenha-me conectado")]
        public bool ManterConectado { get; set; }

    }
}