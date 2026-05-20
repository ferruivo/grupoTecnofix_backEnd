namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class LiberacaoAlocacoesDto
    {
        public int IdPedidoVenda { get; set; }
        public int Item { get; set; }
        public int IdProduto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Liberado { get; set; }
        public List<LiberacaoAlocacaoDto> Alocacoes { get; set; } = new();
    }

    public class LiberacaoAlocacaoDto
    {
        public int IdLote { get; set; }
        public string NumeroLote { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public string FornecedorNome { get; set; } = string.Empty;
    }
}
