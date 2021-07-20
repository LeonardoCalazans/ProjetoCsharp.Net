using Dapper;
using Projeto.Infra.Data.Contracts;
using Projeto.Infra.Data.Dtos;
using Projeto.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Projeto.Infra.Data.Repositories
{
    public class CompromissoRepository : ICompromissoRepository
    {
        //atributo
        private readonly string connectionString;

        //construtor para inicializar o atributo
        public CompromissoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Create(Compromisso entity)
        {
            var query = "insert into Compromisso(Titulo, Descricao, DataInicio, HoraInicio, DataFim, HoraFim, IdUsuario, Categoria) "
                      + "values(@Titulo, @Descricao, @DataInicio, @HoraInicio, @DataFim, @HoraFim, @IdUsuario, @Categoria)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Update(Compromisso entity)
        {
            var query = "update Compromisso set Titulo = @Titulo, Descricao = @Descricao, DataInicio = @DataInicio, "
                      + "HoraInicio = @HoraInicio, DataFim = @DataFim, HoraFim = @HoraFim, Categoria = @Categoria "
                      + "where IdCompromisso = @IdCompromisso";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Delete(Compromisso entity)
        {
            var query = "delete from Compromisso where IdCompromisso = @IdCompromisso";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public List<Compromisso> GetAll()
        {
            var query = "select * from Compromisso order by DataInicio desc";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Compromisso>(query).ToList();
            }
        }

        public Compromisso GetById(int id)
        {
            var query = "select * from Compromisso where IdCompromisso = @IdCompromisso";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<Compromisso>
                    (query, new { IdCompromisso = id });
            }
        }

        public List<Compromisso> GetByDatas(DateTime dataMin, DateTime dataMax, int idUsuario)
        {
            var query = "select * from Compromisso where IdUsuario = @IdUsuario and DataInicio between @DataMin and @DataMax";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Compromisso>
                    (query, new 
                    { 
                        IdUsuario = idUsuario,
                        DataMin = dataMin, 
                        DataMax = dataMax 
                    }).ToList();
            }
        }

        public List<ResumoCategoriaDto> GetResumoCategoriaDto(int idUsuario)
        {
            var query = "select Categoria, count(*) as Quantidade from Compromisso "
                      + "where IdUsuario = @idUsuario group by Categoria";
            using(var connection = new SqlConnection(connectionString))
            {
                return connection.Query<ResumoCategoriaDto>
                    (query, new
                    {
                        IdUsuario = idUsuario
                    }).ToList();
            }
        }

        public List<Compromisso> GetAll(int idUsuario)
        {
            var query = "select * from Compromisso where IdUsuario = @IdUsuario";
            using( var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Compromisso>(query, new { IdUsuario = idUsuario } ).ToList();
            }
        }
    }
}
