using MediatR;
using Questao5.Application.Commands;
using Questao5.Domain.Entities;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.CommandStore;


namespace Questao5.Application.Handlers
{
    public class CreateMovimentacaoCommandHandler
        : IRequestHandler<CreateMovimentacaoCommand, CreateMovimentacaoCommandResponse>
    {
        private readonly IContaCommandStore _contaCommandStore;
        private readonly IIdempotenciaCommandStore _idempotenciaCommandStore;

        public CreateMovimentacaoCommandHandler(
            IContaCommandStore contaCommandStore,
            IIdempotenciaCommandStore idempotenciaCommandStore)
        {
            _contaCommandStore = contaCommandStore;
            _idempotenciaCommandStore = idempotenciaCommandStore;
        }

        public async Task<CreateMovimentacaoCommandResponse> Handle(
            CreateMovimentacaoCommand request,
            CancellationToken cancellationToken)
        {
            // 1) Verifica se já existe registro de idempotência com a mesma chave
            //    Se existir, retorna o resultado que já foi gerado anteriormente.
            var idempotenciaExistente = await _idempotenciaCommandStore
                .ObterPorChaveAsync(request.IdRequisicao);

            if (idempotenciaExistente != null)
            {
                // Podemos desserializar o campo Resultado e devolver 
                return new CreateMovimentacaoCommandResponse
                {
                    IdMovimentoGerado = idempotenciaExistente.Resultado
                };
            }

            // 2) Validações:
            var conta = await _contaCommandStore.ObterContaCorrentePorIdAsync(request.IdContaCorrente);
            if (conta == null)
            {
                throw new MovimentacaoException("Conta não encontrada.", "INVALID_ACCOUNT");
            }
            if (!conta.Ativo)
            {
                throw new MovimentacaoException("Conta inativa.", "INACTIVE_ACCOUNT");
            }
            if (request.Valor <= 0)
            {
                throw new MovimentacaoException("Valor deve ser positivo.", "INVALID_VALUE");
            }
            if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
            {
                throw new MovimentacaoException("Tipo de movimento inválido.", "INVALID_TYPE");
            }

            // 3) Insere movimento
            var idMovimentoGerado = await _contaCommandStore.InserirMovimentoAsync(
                new Movimento
                {
                    IdMovimento = Guid.NewGuid().ToString().ToUpper(),
                    IdContaCorrente = request.IdContaCorrente,
                    DataMovimento = DateTime.Now.ToString("dd/MM/yyyy"),
                    TipoMovimento = request.TipoMovimento,
                    Valor = request.Valor
                }
            );

            // 4) Grava a idempotência para evitar duplicações
            await _idempotenciaCommandStore.InserirAsync(new Idempotencia
            {
                ChaveIdempotencia = request.IdRequisicao,
                Requisicao = $"Conta={request.IdContaCorrente}; Valor={request.Valor}; Tipo={request.TipoMovimento}",
                Resultado = idMovimentoGerado
            });

            return new CreateMovimentacaoCommandResponse
            {
                IdMovimentoGerado = idMovimentoGerado
            };
        }
    }
}
