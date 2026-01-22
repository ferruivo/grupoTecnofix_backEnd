namespace GrupoTecnofix_Api.Dtos.Produto
{
    public class ProdutoKitDto
    {
        public int IdProdutoKit { get; set; }
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        public DateTime? DataCadastro { get; set; }

        public int? IdUsuarioCadastro { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public int? IdUsuarioAlteracao { get; set; }
        public ProdutoListDto Produto { get; set; }
    }
}
