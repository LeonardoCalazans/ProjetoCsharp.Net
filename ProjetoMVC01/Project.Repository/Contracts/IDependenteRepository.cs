using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Repository.Entities;

namespace Project.Repository.Contracts
{
    interface IDependenteRepository : IBaseRepository<Dependente>
    {
        List<Dependente> GetByNome(string nome);
    }
}
