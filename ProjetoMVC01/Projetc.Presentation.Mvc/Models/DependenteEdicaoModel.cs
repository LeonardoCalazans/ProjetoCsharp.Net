using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projetc.Presentation.Mvc.Models
{
    public class DependenteEdicaoModel
    {
        public int IdDependente { get; set; }

        [MinLength(6, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome do dependente.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Por favor, informe a data de nascimento do dependente")]
        public string DataNascimento { get; set; }
        [Required(ErrorMessage = "Por favor, selecione um cliente.")]
        public string IdCliente { get; set; }
        //listagem de clientes para exibir no formulário
        public List<SelectListItem> ListamgeDeClientes { get; set; }

    }
}
