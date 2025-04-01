using MediatR;

namespace Questao5.Application.Queries
{
    public class GetSaldoQuery : IRequest<GetSaldoQueryResponse>
    {
        public string IdContaCorrente { get; set; }
    }

    public class GetSaldoQueryResponse
    {
        public int NumeroConta { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public decimal Saldo { get; set; }
    }

}
