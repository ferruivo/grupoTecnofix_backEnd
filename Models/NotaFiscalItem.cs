using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrupoTecnofix_Api.Models;

public partial class NotaFiscalItem
{
    public long IdNotaFiscalItem { get; set; }

    public long IdNotaFiscal { get; set; }

    public int Item { get; set; }

    public int IdProduto { get; set; }

    public string? CodigoProduto { get; set; }

    public string? DescricaoProduto { get; set; }

    public string? Ncm { get; set; }

    public string? Cest { get; set; }

    public string? Cfop { get; set; }

    public string? Unidade { get; set; }

    public decimal Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }

    public decimal ValorProduto { get; set; }

    public decimal ValorDesconto { get; set; }

    public decimal ValorFrete { get; set; }

    public decimal ValorSeguro { get; set; }

    public decimal ValorOutrasDespesas { get; set; }

    public decimal ValorTotal { get; set; }

    public string? PedidoCliente { get; set; }

    public string? ItemPedidoCliente { get; set; }

    public int? IdConsumo { get; set; }
    [NotMapped]
    public decimal PercentualIpi { get; set; }
    [NotMapped]
    public decimal PercentualIcms { get; set; }

    public virtual NotaFiscal IdNotaFiscalNavigation { get; set; } = null!;

    public virtual ICollection<NotaFiscalItemTributo> NotaFiscalItemTributos { get; set; } = new List<NotaFiscalItemTributo>();
}
