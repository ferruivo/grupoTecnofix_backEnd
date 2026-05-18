using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrupoTecnofix_Api.Dtos.Produto
{
    public class ProdutoCreateUpdate
    {
        public string Codigo { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public string Ncm { get; set; } = null!;
        public string Unidade { get; set; } = null!;
        public string CstIpi { get; set; } = null!;
        public string CstIcms { get; set; } = null!;
        public int? Minimo { get; set; }
        public decimal PesoItem { get; set; }
        public decimal ValorKg { get; set; }
        public string Tipo { get; set; } = null!;
        public string? Obs { get; set; }
        public string? ObsNf { get; set; }
        public string? ObsEntrada { get; set; }
        public bool? IS_KIT { get; set; }
    }
}
