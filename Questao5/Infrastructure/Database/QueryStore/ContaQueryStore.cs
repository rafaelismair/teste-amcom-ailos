using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class ContaQueryStore : IContaQueryStore
    {
        private readonly IDatabaseConfig _dbConfig;

        public ContaQueryStore(IDatabaseConfig dbConfig)
        {
            _dbConfig = dbConfig;
        }

        public async Task<ContaCorrente> ObterContaCorrentePorIdAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(_dbConfig.Name);
            var sql = "SELECT * FROM contacorrente WHERE idcontacorrente = @id";
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { id = idContaCorrente });
        }

        public async Task<decimal> ObterSaldoAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(_dbConfig.Name);

            // Soma dos créditos
            var sqlCreditos = @"
                SELECT IFNULL(SUM(valor), 0) 
                FROM movimento 
                WHERE idcontacorrente = @id
                  AND tipomovimento = 'C';
            ";
            decimal totalCreditos = await connection.ExecuteScalarAsync<decimal>(sqlCreditos, new { id = idContaCorrente });

            // Soma dos débitos
            var sqlDebitos = @"
                SELECT IFNULL(SUM(valor), 0) 
                FROM movimento 
                WHERE idcontacorrente = @id
                  AND tipomovimento = 'D';
            ";
            decimal totalDebitos = await connection.ExecuteScalarAsync<decimal>(sqlDebitos, new { id = idContaCorrente });

            return totalCreditos - totalDebitos;
        }
    }

}
