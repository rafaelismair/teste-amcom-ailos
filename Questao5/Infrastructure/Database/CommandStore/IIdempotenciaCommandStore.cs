using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public interface IIdempotenciaCommandStore
    {
        Task<Idempotencia> ObterPorChaveAsync(string chave);
        Task InserirAsync(Idempotencia idempotencia);
    }

}
