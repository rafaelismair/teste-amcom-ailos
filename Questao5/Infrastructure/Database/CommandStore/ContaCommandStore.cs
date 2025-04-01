using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class ContaCommandStore : IContaCommandStore
    {
        private readonly IDatabaseConfig _dbConfig;

        public ContaCommandStore(IDatabaseConfig dbConfig)
        {
            _dbConfig = dbConfig;
        }

        public async Task<ContaCorrente> ObterContaCorrentePorIdAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(_dbConfig.Name);
            var sql = "SELECT * FROM contacorrente WHERE idcontacorrente = @id";
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { id = idContaCorrente });
        }

        public async Task<string> InserirMovimentoAsync(Movimento movimento)
        {
            using var connection = new SqliteConnection(_dbConfig.Name);
            var sql = @"
                INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

            await connection.ExecuteAsync(sql, movimento);
            return movimento.IdMovimento; // Retorna o id recém inserido
        }
    }

}
