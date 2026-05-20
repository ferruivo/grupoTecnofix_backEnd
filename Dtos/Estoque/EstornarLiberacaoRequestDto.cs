namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class EstornarLiberacaoRequestDto
    {
        public int IdPedidoVenda { get; set; }
        public int Item { get; set; }
        public int IdLote { get; set; }
        public decimal? Quantidade { get; set; }
    }
}
