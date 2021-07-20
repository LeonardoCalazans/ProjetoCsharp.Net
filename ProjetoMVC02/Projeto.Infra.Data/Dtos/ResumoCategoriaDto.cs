using Projeto.Infra.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Infra.Data.Dtos
{
    public class ResumoCategoriaDto
    {
        public Categoria Categoria { get; set; }
        public int Quantidade { get; set; }
    }
}
