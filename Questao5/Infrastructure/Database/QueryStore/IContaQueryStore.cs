using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public interface IContaQueryStore
    {
        Task<ContaCorrente> ObterContaCorrentePorIdAsync(string idContaCorrente);
        Task<decimal> ObterSaldoAsync(string idContaCorrente);
    }

}
