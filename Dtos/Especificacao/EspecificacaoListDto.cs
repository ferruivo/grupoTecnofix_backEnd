namespace GrupoTecnofix_Api.Dtos.Especificacao
{
    public class EspecificacaoListDto
    {
        public int IdEspecificacao { get; set; }
        public string Descricao { get; set; } = "";
        public string Minimo { get; set; } = "";
        public string Maximo { get; set; } = "";
        public string Aproximado { get; set; } = "";
        public string Observacao { get; set; } = "";

    }
}
