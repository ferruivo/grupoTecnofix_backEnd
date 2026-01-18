namespace GrupoTecnofix_Api.Dtos.Produto
{
    public class ProdutoListDto
    {
        public int IdProduto { get; set; }
        public string Codigo { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        
    }
}
