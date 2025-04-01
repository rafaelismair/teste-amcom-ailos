using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application;
using Questao5.Application.Queries;
using Questao5.Domain.Language;
using System.Net;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaldoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaldoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Consulta o saldo atual da conta corrente.
        /// </summary>
        /// <param name="idContaCorrente">Identificação da conta corrente</param>
        /// <returns>Dados do saldo (número da conta, nome, data/hora e valor).</returns>
        /// <response code="200">Saldo retornado com sucesso</response>
        /// <response code="400">Conta não encontrada ou inativa</response>
        [HttpGet("{idContaCorrente}")]
        [ProducesResponseType(typeof(GetSaldoQueryResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErroReponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetSaldo(string idContaCorrente)
        {
            var query = new GetSaldoQuery { IdContaCorrente = idContaCorrente };

            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (MovimentacaoException ex)
            {
                return BadRequest(new ErroReponse
                {
                    Mensagem = ex.Message,
                    Tipo = ex.TipoErro
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }
        }
    }
}
