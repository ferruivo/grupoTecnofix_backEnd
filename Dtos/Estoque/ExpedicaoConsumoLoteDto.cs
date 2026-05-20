namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class ExpedicaoConsumoLoteDto
    {
        public string NomeCliente { get; set; } = string.Empty;
        public string? Contato { get; set; }
        public string Municipio { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string? ObservacaoOrdemExpedicao { get; set; }
        public string? TransportadoraNome { get; set; }
        public string? TransportadoraTelefone { get; set; }

        public List<ExpedicaoConsumoLoteItemDto> Itens { get; set; } = new();
    }

    public class ExpedicaoConsumoLoteItemDto
    {
        public string ProdutoCodigo { get; set; } = string.Empty;
        public string ProdutoDescricao { get; set; } = string.Empty;
        public string? ProdutoCliente { get; set; }

        public decimal Quantidade { get; set; }
        public int IdLote { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Total { get; set; }
    }
}
