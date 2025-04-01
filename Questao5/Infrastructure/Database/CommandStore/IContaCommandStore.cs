using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public interface IContaCommandStore
    {
        Task<ContaCorrente> ObterContaCorrentePorIdAsync(string idContaCorrente);
        Task<string> InserirMovimentoAsync(Movimento movimento);
    }

}
