using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Dapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Optional;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Infrastructure.Database.SharedContext.Repository.Dapper
{
    public abstract class RepositoryBaseDapper<T, In> : IRepositoryBaseDapper<T, In>
        where T : class
        where In : class
    {
        private readonly IConfiguration _configuration;

        public RepositoryBaseDapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual async Task<Option<IEnumerable<T>>> ListarAsync(In filtros)
        {
            return Option.None<IEnumerable<T>>();
        }

        public async Task<Option<IEnumerable<T>>> RetornarTodosAsync(string instrucaoSQL)
        {
            return await RetornarTodosWrapper(connection =>
            {
                return connection.QueryAsync<T>(instrucaoSQL);
            });            
        }

        public async Task<int> RetornarInteiroAsync(string instrucaoSQL)
        {
            var conexao = ConexaoBanco();

            using (var db = new SqlConnection(conexao))
            {
                var consulta = await db.QueryAsync<int>(instrucaoSQL);
                return consulta.FirstOrDefault();
            }
        }

        public async Task<Option<IEnumerable<T>>> RetornarTodosWrapper(Func<IDbConnection, Task<IEnumerable<T>>> acao)
        {
            var conexao = ConexaoBanco();

            using (var db = new SqlConnection(conexao))
            {
                await db.OpenAsync();
                var retorno = await acao.Invoke(db);
                return retorno == null ? Option.None<IEnumerable<T>>() : Option.Some<IEnumerable<T>>(retorno);                
            }
        }
        public async Task<bool> ExecuteWrapper(Func<IDbConnection, Task<bool>> acao)
        {
            try
            {
                var conexao = ConexaoBanco();

                using (var db = new SqlConnection(conexao))
                {
                    await db.OpenAsync();
                    var retorno = await acao.Invoke(db);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string ConexaoBanco()
        {
            return _configuration.GetSection("AppSettings").GetSection("ConnectionString").Value;
        }       
    }
}
