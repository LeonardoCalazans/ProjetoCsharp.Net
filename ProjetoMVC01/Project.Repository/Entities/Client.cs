using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repository.Entities
{
    public class Cliente
    {
        #region Propriedades

        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }

        #endregion

        #region Construtores

        public Cliente()
        {
        }
        public Cliente(int idCliente, string nome, string email, string cpf)
        {
            IdCliente = idCliente;
            Nome = nome;
            Email = email;
            Cpf = cpf;
        }

        #endregion

        #region Relacionamentos

        public List<Dependente> Dependentes { get; set; }

        #endregion
    }
}
