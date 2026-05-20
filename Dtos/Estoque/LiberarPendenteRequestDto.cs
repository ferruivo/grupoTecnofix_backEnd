namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class LiberarPendenteRequestDto
    {
        public int IdPedidoVenda { get; set; }
        public int Item { get; set; }
        public int IdProduto { get; set; }
        public List<LiberarPendenteAlocacaoRequestDto> Alocacoes { get; set; } = new();
    }

    public class LiberarPendenteAlocacaoRequestDto
    {
        public int IdLote { get; set; }
        public decimal Quantidade { get; set; }
    }
}
