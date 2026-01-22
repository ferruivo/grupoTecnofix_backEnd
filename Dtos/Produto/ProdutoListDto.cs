namespace GrupoTecnofix_Api.Dtos.Produto
{
    public class ProdutoListDto
    {
        public int IdProduto { get; set; }
        public string Codigo { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public string Ncm { get; set; } = null!;
        public bool? IS_KIT { get; set; } = null!;
    }
}
