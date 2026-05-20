namespace GrupoTecnofix_Api.Models;

public partial class ProdutoEspecificacao
{
    public int IdProduto { get; set; }
    public int IdEspecificacao { get; set; }
    public string? Minimo { get; set; }
    public string? Maximo { get; set; }
    public string? Aproximado { get; set; }
    public string? Observacao { get; set; }
}
