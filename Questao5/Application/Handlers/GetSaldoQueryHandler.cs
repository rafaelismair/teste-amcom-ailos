using MediatR;
using Questao5.Application.Queries;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.QueryStore;

namespace Questao5.Application.Handlers
{
    public class GetSaldoQueryHandler : IRequestHandler<GetSaldoQuery, GetSaldoQueryResponse>
    {
        private readonly IContaQueryStore _contaQueryStore;

        public GetSaldoQueryHandler(IContaQueryStore contaQueryStore)
        {
            _contaQueryStore = contaQueryStore;
        }

        public async Task<GetSaldoQueryResponse> Handle(GetSaldoQuery request, CancellationToken cancellationToken)
        {
            // 1) Valida conta
            var conta = await _contaQueryStore.ObterContaCorrentePorIdAsync(request.IdContaCorrente);
            if (conta == null)
            {
                throw new MovimentacaoException("Conta não encontrada.", "INVALID_ACCOUNT");
            }
            if (!conta.Ativo)
            {
                throw new MovimentacaoException("Conta inativa.", "INACTIVE_ACCOUNT");
            }

            // 2) Calcula saldo
            var saldo = await _contaQueryStore.ObterSaldoAsync(request.IdContaCorrente);

            // 3) Retorna resposta
            return new GetSaldoQueryResponse
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Nome,
                DataHoraConsulta = DateTime.Now,
                Saldo = saldo
            };
        }
    }

}
