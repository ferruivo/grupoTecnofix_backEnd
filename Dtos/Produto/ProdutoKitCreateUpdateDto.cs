namespace GrupoTecnofix_Api.Dtos.Produto
{
    public class ProdutoKitCreateUpdateDto
    {
        public int IdProdutoKit { get; set; }
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
    }
}
