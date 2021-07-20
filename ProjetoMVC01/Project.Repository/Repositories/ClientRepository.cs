using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Repository.Entities;
using Project.Repository.Contracts;
using System.Data.SqlClient;
using Dapper;

namespace Project.Repository.Repositories
{
    public class ClientRepository : IClienteRepository
    {
        //atributo
        private readonly string connectionString;

        //construtor com entrada de argumentos
        public ClientRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Create(Cliente entity)
        {
            var query = "insert into Cliente(Nome, Email, Cpf)"
                + "values(@Nome, @Email, @Cpf)";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }
        public void Update(Cliente entity)
        {
            var query = "update Cliente set Nome = @Nome, Email = @Email, Cpf = @Cpf "
                        + "where IdCliente = @IdCliente";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }
        public void Delete(Cliente entity)
        {
            var query = "delete from Cliente where IdCliente = @IdCliente";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }
        public List<Cliente> GetAll()
        {
            /*
            var query = "select * from Cliente order by Nome asc";
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Cliente>(query).ToList();
            }
            */
            var query = "select * from Cliente c "
                + "inner join Dependente d "
                + "on d.IdCliente = c.IdCliente "
                + "order by c.Nome asc";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query
                    (query, (Cliente c, Dependente d) =>
                    {
                        //tem que ser um foreach e não pode retornar nulo
                        c.Dependentes.Add(d);
                        return c;
                    }, splitOn: "IdCliente")
                    .ToList();
            }
        }
        public Cliente GetById(int id)
        {
            var query = "select * from Cliente where IdCliente = @IdCliente";
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<Cliente>
                (query, new { IdCliente = id });
            }
        }

        public Cliente GetByEmail(string email)
        {
            var query = "select * from Cliente where Email = @Email";
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<Cliente>
                (query, new { Email = email });
            }

        }

        public Cliente GetByCpf(string cpf)
        {
            var query = "select * from Cliente where Cpf = @Cpf";
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<Cliente>
                (query, new { Cpf = cpf });
            }

        }

        public List<Cliente> GetByNome(string nome)
        {
            /*
            var query = "select * from Cliente where Nome like @Nome order by Nome asc";
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Cliente>(query, new { Nome = ("%" + nome + "%") }).ToList();
            }
            */
            var query = "select * from Cliente c "
                        + "inner join Dependente d "
                        + "on d.IdCliente = c.IdCliente "
                        + "where c.Nome like @Nome "
                        + "order by c.Nome asc";
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query
                (query, (Cliente c, Dependente d) =>
                {
                    c.Dependentes.Add(d);
                    return c;
                },
               new { Nome = ("%" + nome + "%") },
                splitOn: "IdCliente"
               ).ToList();
            }
        }

        public int CountDependentes(int idCliente)
        {
            var query = "select cont(*) from Dependentes where IdCliente = @IdCliente";
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<int>
                    (query, new { IdCliente = idCliente });
            }
        }
    }
}
