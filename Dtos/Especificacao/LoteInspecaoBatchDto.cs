using System.Collections.Generic;

namespace GrupoTecnofix_Api.Dtos.Especificacao
{
    public class LoteInspecaoBatchDto
    {
        public List<LoteInspecaoDto> Items { get; set; } = new();
        public string? StatusLote { get; set; }
    }
}
