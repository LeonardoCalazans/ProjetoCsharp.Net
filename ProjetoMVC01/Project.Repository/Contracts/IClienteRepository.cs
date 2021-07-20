using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Repository.Entities;

namespace Project.Repository.Contracts
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        List<Cliente> GetByNome(string nome);
        Cliente GetByEmail(string email);
        Cliente GetByCpf(string cpf);
        int CountDependentes(int idCliente);
    }
}
