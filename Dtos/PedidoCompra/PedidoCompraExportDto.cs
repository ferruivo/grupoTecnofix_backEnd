using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Dtos.PedidoCompra
{
    public class PedidoCompraExportDto
    {
        public int IdPedidoCompra { get; set; }
        public DateTime DataPedido { get; set; }
        public string? FornecedorNome { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal TotalIpi { get; set; }
        public decimal TotalIcms { get; set; }
        public decimal TotalPedido { get; set; }
        public string? Observacao { get; set; }

        public List<PedidoCompraItemExportDto> Itens { get; set; } = new();
    }
}
