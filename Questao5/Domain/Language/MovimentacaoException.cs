namespace Questao5.Domain.Language
{
    public class MovimentacaoException : Exception
    {
        public string TipoErro { get; private set; }

        public MovimentacaoException(string message, string tipoErro)
            : base(message)
        {
            TipoErro = tipoErro;
        }
    }

}
