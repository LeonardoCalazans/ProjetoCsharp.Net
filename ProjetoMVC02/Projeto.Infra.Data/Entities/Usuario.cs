using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Infra.Data.Entities
{
    public class Usuario
    {
        //propriedades -> prop + 2x[tab]
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataCriacao { get; set; }

        //construtor default -> ctor + 2x[tab]
        public Usuario()
        {
            //vazio
        }
        
        //sobrecarga de método construtor (overloading)
        public Usuario(int idUsuario, string nome, string email, string senha, DateTime dataCriacao)
        {
            IdUsuario = idUsuario;
            Nome = nome;
            Email = email;
            Senha = senha;
            DataCriacao = dataCriacao;
        }

        //Relacionamento com a classe Compromisso..
        //Associação TER-MUITOS
        public List<Compromisso> Compromissos { get; set; }
    }
}
