using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class CreateMovimentacaoCommand : IRequest<CreateMovimentacaoCommandResponse>
    {
        public string IdRequisicao { get; set; }  // Chave de idempotência
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public TipoMovimentoEnum TipoMovimento { get; set; } 
    }
}
