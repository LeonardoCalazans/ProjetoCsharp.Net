using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projetc.Presentation.Mvc.Models
{
    public class ClienteEdicaoModel
    {
        public int IdCliente { get; set; } //campo oculto

        [MinLength(6, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome do cliente.")]
        public string Nome { get; set; }
        [EmailAddress(ErrorMessage = "Por favor, informe um endereço de e-mail válido.")]
        [Required(ErrorMessage = "Por favor, informe o nome do e-mail.")]
        public string Email { get; set; }
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Por favor, preencha o CPF com 11 digitos numéricos sem pontos e traços")]
        [Required(ErrorMessage = "Por favor, informe o nome do CPF.")]
        public string Cpf { get; set; }
    }
}
