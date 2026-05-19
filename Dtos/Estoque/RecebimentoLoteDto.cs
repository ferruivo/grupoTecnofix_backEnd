namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class RecebimentoLoteDto
    {
        public int Id { get; set; }
        public int IdLote { get; set; }

        public int IdPedidoCompra { get; set; }
        public int IdPedidoCompraItem { get; set; }

        public int? Item { get; set; }

        public long? IdProduto { get; set; }
        public string? ProdutoCodigo { get; set; }
        public string? ProdutoDescricao { get; set; }

        public DateOnly Data { get; set; }

        public string? Nf { get; set; }
        public string? Certificado { get; set; }

        public decimal QuantidadeRecebida { get; set; }
        public string? Status { get; set; }
        public string? Observacao { get; set; }
    }
}
