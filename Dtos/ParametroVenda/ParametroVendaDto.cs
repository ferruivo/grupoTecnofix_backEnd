namespace GrupoTecnofix_Api.Dtos.ParametroVenda
{
    public class ParametroVendaDto
    {
        public List<LoteParametroVendaDto> Lotes { get; set; } = new();
        public List<PedidoCompraDisponivelDto> PedidosCompra { get; set; } = new();
        public List<PedidoVendaDisponivelDto> PedidosVenda { get; set; } = new();
    }

    public class LoteParametroVendaDto
    {
        public long Lote { get; set; }
        public string Entrada { get; set; }
        public string Fornecedor { get; set; }
        public decimal Preco { get; set; }
        public decimal? Disponivel { get; set; }
        public string? Status { get; set; }
    }

    public class PedidoCompraDisponivelDto
    {
        public string Data { get; set; }
        public string Fornecedor { get; set; }
        public int Pedido { get; set; }
        public int Item { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Recebido { get; set; }
        public decimal Diferenca { get; set; }
    }

    public class PedidoVendaDisponivelDto
    {
        public string Data { get; set; }
        public string Cliente { get; set; }
        public int Pedido { get; set; }
        public int Item { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Entregue { get; set; }
        public decimal Diferenca { get; set; }
    }
}


