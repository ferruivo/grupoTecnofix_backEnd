namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class LoteDisponivelDto
    {
        public int IdLote { get; set; }
        public string NumeroLote { get; set; } = string.Empty;

        public int IdFornecedor { get; set; }
        public string FornecedorCodigo { get; set; } = string.Empty;
        public string FornecedorNome { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public decimal Saldo { get; set; }
        public decimal Reservado { get; set; }
        public decimal Disponivel { get; set; }

        public DateOnly? DataEntrada { get; set; }
        public DateOnly? DataValidade { get; set; }
    }
}
