namespace GrupoTecnofix_Api.Dtos.Produto
{
    public class PrecoCompraDto
    {
        public int IdPrecocompra { get; set; }
        public int IdProduto { get; set; }
        public int IdFornecedor { get; set; }
        public decimal Preco { get; set; }
        public decimal? Precoantigo { get; set; }
        public DateTime? Vigencia { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? Obs { get; set; }
        public ProdutoListDto? Produto { get; set; }
    }
}
