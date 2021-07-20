using Dapper;
using Project.Repository.Contracts;
using Project.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace Projeto.Repository.Repositories
{
    public class DependenteRepository : IDependenteRepository
    {
        //atributo
        private readonly string connectionString;

        //método construtor para receber e inicializar o atributo
        public DependenteRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void Create(Dependente entity)
        {
            var query = "insert into Dependente(Nome, DataNascimento, IdCliente) "
                        + "values(@Nome, @DataNascimento, @IdCliente)";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }
        public void Update(Dependente entity)
        {
            var query = "update Dependente set Nome = @Nome, DataNascimento = @DataNascimento, "
                        + "IdCliente = @IdCliente where IdDependente = @IdDependente";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }
        public void Delete(Dependente entity)
        {
            var query = "delete from Dependente where IdDependente = @IdDependente";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }
        public List<Dependente> GetAll()
        {
            var query = "select * from Dependente d "
                + "inner join Cliente c "
                + "on c.IdCliente = d.IdCliente "
                + "order by d.Nome asc";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query
                    (query, (Dependente d, Cliente c) =>
                {
                    d.Cliente = c;
                    return d;
                }, splitOn: "IdCliente")
                    .ToList();
            }
        }

        public Dependente GetById(int id)
        {
            var query = "select * from Dependente where IdDependente = @IdDependente";
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<Dependente>
                (query, new { IdDependente = id });
            }
        }

        public List<Dependente> GetByNome(string nome)
        {
            var query = "select * from Dependente d "
                        + "inner join Cliente c "
                        + "on d.IdCliente = c.IdCliente "
                        + "where d.Nome like @Nome "
                        + "order by d.Nome asc";
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query
                (query, (Dependente d, Cliente c) =>
                {
                    d.Cliente = c;
                    return d;
                },
               new { Nome = ("%" + nome + "%") },
                splitOn: "IdCliente"
               ).ToList();
            }
        }
    }
}