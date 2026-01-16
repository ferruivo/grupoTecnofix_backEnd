namespace GrupoTecnofix_Api.Dtos.Condições_Pagamento
{
    public class CondicaoPagamentoDto
    {
        public int IdCondicoespagamento { get; set; }
        public string Descricao { get; set; } = "";
        public string Venc01 { get; set; } = "";
        public string? Venc02 { get; set; }
        public string? Venc03 { get; set; }
        public string? Venc04 { get; set; }
        public string? Venc05 { get; set; }
        public string? Venc06 { get; set; }
        public string? Forames { get; set; } // 'N'/'S'
    }
}
