using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Language;
using System;
using System.Net;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Cria uma nova movimentação na conta corrente (crédito ou débito).
        /// </summary>
        /// <param name="command">Dados da movimentação (IdRequisicao, IdContaCorrente, Valor, TipoMovimento)</param>
        /// <returns>Retorna o Id do movimento gerado.</returns>
        /// <response code="200">Movimentação realizada com sucesso</response>
        /// <response code="400">Dados inválidos (ex: conta não encontrada, inativa, valor inválido)</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateMovimentacaoCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErroReponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateMovimentacaoCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
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
