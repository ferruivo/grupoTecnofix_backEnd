namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class AlocacaoMaterialSalvarDto
    {
        public int IdLote { get; set; }
        public List<AlocacaoMaterialItemDto> Alocacoes { get; set; } = new();
    }

    public class AlocacaoMaterialItemDto
    {
        public int Etq { get; set; }
        public int IdPrateleira { get; set; } 
        public int Qtd { get; set; }
        //public int? Reservado { get; set; }
        public DateTime? DataAlocacao { get; set; }
    }
}
