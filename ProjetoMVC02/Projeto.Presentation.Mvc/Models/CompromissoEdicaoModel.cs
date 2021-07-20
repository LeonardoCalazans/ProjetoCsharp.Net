using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //validações
using Projeto.Infra.Data.Enums;

namespace Projeto.Presentation.Mvc.Models
{
    public class CompromissoEdicaoModel
    {
        public int IdCompromisso { get; set; } //campo oculto

        [Required(ErrorMessage = "Por favor, informe o título do compromisso.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Por favor, informe a descrição do compromisso.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de início do compromisso.")]
        public string DataInicio { get; set; }

        [Required(ErrorMessage = "Por favor, informe a hora de início do compromisso.")]
        public string HoraInicio { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de fim do compromisso.")]
        public string DataFim { get; set; }

        [Required(ErrorMessage = "Por favor, informe a hora de fim do compromisso.")]
        public string HoraFim { get; set; }

        [Required(ErrorMessage = "Por favor, selecione a categoria.")]
        public Categoria Categoria { get; set; } //ENUM
    }
}
