using Projeto.Infra.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Infra.Data.Entities
{
    public class Compromisso
    {
        //propriedades -> prop + 2x[tab]
        public int IdCompromisso { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public DateTime DataFim { get; set; }
        public TimeSpan HoraFim { get; set; }
        public int IdUsuario { get; set; }

        //construtor default -> ctor + 2x[tab]
        public Compromisso()
        {
            //vazio
        }

        //sobrecarga de construtores (overloading)
        public Compromisso(int idCompromisso, string titulo, string descricao, DateTime dataInicio, TimeSpan horaInicio, DateTime dataFim, TimeSpan horaFim, int idUsuario)
        {
            IdCompromisso = idCompromisso;
            Titulo = titulo;
            Descricao = descricao;
            DataInicio = dataInicio;
            HoraInicio = horaInicio;
            DataFim = dataFim;
            HoraFim = horaFim;
            IdUsuario = idUsuario;
        }

        //Relacionamento com Usuario
        //Associação TER-1
        public Usuario Usuario { get; set; }

        //Relacionamento com Categoria
        //Associação TER 1 (ENUM)
        public Categoria Categoria { get; set; }
    }
}
