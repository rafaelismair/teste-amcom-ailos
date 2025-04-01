using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database
{
    public class IdempotenciaCommandStore : IIdempotenciaCommandStore
    {
        private readonly IDatabaseConfig _dbConfig;

        public IdempotenciaCommandStore(IDatabaseConfig dbConfig)
        {
            _dbConfig = dbConfig;
        }

        public async Task<Idempotencia> ObterPorChaveAsync(string chave)
        {
            using var connection = new SqliteConnection(_dbConfig.Name);
            var sql = "SELECT * FROM idempotencia WHERE chave_idempotencia = @chave";
            return await connection.QueryFirstOrDefaultAsync<Idempotencia>(sql, new { chave });
        }

        public async Task InserirAsync(Idempotencia idempotencia)
        {
            using var connection = new SqliteConnection(_dbConfig.Name);
            var sql = @"
                INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";
            await connection.ExecuteAsync(sql, idempotencia);
        }
    }

}
