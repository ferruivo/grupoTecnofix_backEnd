using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class PedidosCompra
{
    public int IdPedidoCompra { get; set; }

    public DateOnly DataEmissao { get; set; }

    public int IdFornecedor { get; set; }

    public int? IdCondPagamento { get; set; }

    public int? IdTransportadora { get; set; }

    public string TipoFrete { get; set; } = null!;

    public decimal ValorFrete { get; set; }

    public decimal TotalProdutos { get; set; }

    public decimal TotalIpi { get; set; }

    public decimal TotalIcms { get; set; }

    public decimal TotalPedido { get; set; }

    public string? Observacao { get; set; }

    public string? ObservacaoCompl { get; set; }

    public int IdUsuarioCadastro { get; set; }

    public DateTime DataCadastro { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public DateTime? DataAlteracao { get; set; }
}
