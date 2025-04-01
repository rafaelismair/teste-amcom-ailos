using MediatR;

namespace Questao5.Application.Commands
{
    public class CreateMovimentacaoCommand : IRequest<CreateMovimentacaoCommandResponse>
    {
        public string IdRequisicao { get; set; }  // Chave de idempotência
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; } // 'C' ou 'D'
    }

    public class CreateMovimentacaoCommandResponse
    {
        public string IdMovimentoGerado { get; set; }
    }

}
