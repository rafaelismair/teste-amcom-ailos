using NSubstitute;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
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
        public async Task Handle_ShouldSucceed_WhenContaValida()
        {
            // Arrange
            var contaCommandStore = Substitute.For<IContaCommandStore>();
            var idempotenciaCommandStore = Substitute.For<IIdempotenciaCommandStore>();

            // Simula a conta "abc" existente e ativa
            contaCommandStore.ObterContaCorrentePorIdAsync("abc").Returns(new ContaCorrente
            {
                IdContaCorrente = "abc",
                Nome = "Conta Teste",
                Numero = 123,
                Ativo = true
            });

            idempotenciaCommandStore.ObterPorChaveAsync("REQ-123").Returns((Idempotencia)null);

            
            contaCommandStore.InserirMovimentoAsync(Arg.Any<Movimento>())
                             .Returns("MOV-999");

            var handler = new CreateMovimentacaoCommandHandler(
                contaCommandStore,
                idempotenciaCommandStore
            );

            // Cria o comando válido
            var command = new CreateMovimentacaoCommand
            {
                IdRequisicao = "REQ-123",
                IdContaCorrente = "abc",
                Valor = 100m,
                TipoMovimento = TipoMovimentoEnum.C 
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("MOV-999", result.IdMovimentoGerado);

            // Opcionalmente, você pode verificar se as chamadas foram feitas
            await contaCommandStore.Received(1).ObterContaCorrentePorIdAsync("abc");
            await contaCommandStore.Received(1).InserirMovimentoAsync(Arg.Any<Movimento>());
            await idempotenciaCommandStore.Received(1).ObterPorChaveAsync("REQ-123");
        }
    

        [Fact]
        public async Task Handle_ShouldThrowException_WhenContaNaoExiste()
        {
            var contaCommandStore = Substitute.For<IContaCommandStore>();
            var idempotenciaCommandStore = Substitute.For<IIdempotenciaCommandStore>();

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

            var ex = await Assert.ThrowsAsync<MovimentacaoException>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.Equal("Conta não encontrada.", ex.Message);
            Assert.Equal("INVALID_ACCOUNT", ex.TipoErro);
        }
    }
}
