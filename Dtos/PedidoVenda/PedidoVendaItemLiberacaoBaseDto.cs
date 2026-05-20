namespace GrupoTecnofix_Api.Dtos.PedidoVenda
{
    public class PedidoVendaItemLiberacaoBaseDto
    {
        public int IdPedidoVenda { get; set; }
        public int Item { get; set; }
        public int IdProduto { get; set; }
        public decimal Quantidade { get; set; }
    }
}
