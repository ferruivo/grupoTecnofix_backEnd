namespace GrupoTecnofix_Api.Dtos.PedidoVenda
{
    public class PedidoVendaListItemDto
    {
        public int IdPedidoVenda { get; set; }
        public DateTime DataPedido { get; set; }
        public string? NomeCliente { get; set; }
        public string? NomeVendedor { get; set; }
        public decimal TotalPedido { get; set; }
    }
}