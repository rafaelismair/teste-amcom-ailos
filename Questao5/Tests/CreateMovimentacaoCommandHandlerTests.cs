using NSubstitute;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.CommandStore;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Questao5.Tests
{
    public class CreateMovimentacaoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldThrowException_WhenContaNaoExiste()
        {
            // Arrange
            var contaCommandStore = Substitute.For<IContaCommandStore>();
            var idempotenciaCommandStore = Substitute.For<IIdempotenciaCommandStore>();

            // Retorna null ao buscar a conta => conta inexistente
            contaCommandStore.ObterContaCorrentePorIdAsync("abc").Returns((ContaCorrente)null);

            var handler = new CreateMovimentacaoCommandHandler(
                contaCommandStore, idempotenciaCommandStore);

            var command = new CreateMovimentacaoCommand
            {
                IdRequisicao = "REQ-123",
                IdContaCorrente = "abc",
                Valor = 100,
                TipoMovimento = Domain.Enumerators.TipoMovimentoEnum.C
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<MovimentacaoException>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.Equal("Conta não encontrada.", ex.Message);
            Assert.Equal("INVALID_ACCOUNT", ex.TipoErro);
        }
    }
}
