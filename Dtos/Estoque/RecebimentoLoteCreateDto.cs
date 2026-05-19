namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class RecebimentoLoteCreateDto
    {
        public int IdPedidoCompra { get; set; }
        public int IdPedidoCompraItem { get; set; }
        public DateOnly Data { get; set; }

        public string? Nf { get; set; }
        public string? Certificado { get; set; }

        public decimal QuantidadeRecebida { get; set; }
        public decimal? QuantidadePecas { get; set; }

        public bool AspectoVisualOk { get; set; }
        public bool EmbalagemOk { get; set; }

        public string? Observacao { get; set; }
    }
}
