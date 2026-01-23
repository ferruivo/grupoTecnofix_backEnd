using DocumentFormat.OpenXml.Drawing.Charts;
using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.PedidoCompra;
using GrupoTecnofix_Api.Dtos.Usuario;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace GrupoTecnofix_Api.BLL.Templates
{
    public static class PedidoCompraPdfBuilder
    {
        public static byte[] Build(EmpresaDto empresa, UsuarioDto usuario, PedidoCompraDto model, string? logoPath = null)
        {
            var culture = new CultureInfo("pt-BR");
            var items = model.Itens ?? new List<PedidoCompraItemDto>();

            var tipoFrete = (model as dynamic)?.TipoFrete as string ?? "";
            var valorFrete = model.ValorFrete;

            var compradorNome = usuario.NomeCompleto;
            var compradorEmail = usuario.Email;
            var compradorDepto = "Depto. Compras";

            var textoPadrao =
                "\"Não recebemos materiais sem certificado de qualidade eletrônico, ou anexo a NF/Vale.\n" +
                " Favor confirmar o recebimento do pedido e data de entrega.\n" +
                " Email para envio de NFE : nfetecnofix@grupotecnofix.com.br.\"";

            var dataPedido = model.DataPedido;
            var horaImpressao = DateTime.Now;

            var totalSemImpostos = model.TotalProdutos;
            var totalIpi = model.TotalIpi;
            var totalGeral = model.TotalPedido;

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(18);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(t => t.FontSize(9).FontColor(Colors.Black));

                    // =========================
                    // HEADER
                    // =========================
                    page.Header().Column(h =>
                    {
                        h.Item().Row(row =>
                        {
                            row.RelativeColumn().Column(col =>
                            {
                                col.Item().Text(empresa.RazaoSocial).SemiBold().FontSize(10);
                                col.Item().Text(empresa.Endereco);

                                col.Item().Row(r =>
                                {
                                    r.RelativeColumn().Text($"Cep : {empresa.Cep}");
                                    r.RelativeColumn().Text($"Fone : {empresa.Telefone}");
                                });

                                col.Item().Row(r =>
                                {
                                    r.RelativeColumn().Text($"Cnpj : {empresa.Cnpj}");
                                    r.RelativeColumn().Text($"Inscrição : {empresa.InscricaoEstadual}");
                                });

                                col.Item().Row(r =>
                                {
                                    r.RelativeColumn().Text("Site : www.grupotecnofix.com.br");
                                });
                            });

                            row.ConstantColumn(175).AlignRight().Column(col =>
                            {
                                if (!string.IsNullOrWhiteSpace(logoPath) && File.Exists(logoPath))
                                {
                                    col.Item().AlignRight().Height(34).Element(e => e.Image(logoPath, ImageScaling.FitArea));
                                    col.Item().PaddingBottom(4);
                                }

                                col.Item().Element(e => RightInfoBox(e, dataPedido, model.IdPedidoCompra, horaImpressao));
                            });
                        });

                        h.Item().PaddingTop(6).AlignCenter().Text("PEDIDO DE COMPRA").FontSize(12).SemiBold();
                        h.Item().PaddingTop(2).LineHorizontal(1).LineColor(Colors.Black);

                        // Quadro fornecedor
                        h.Item().PaddingTop(6)
                            .Border(1).BorderColor(Colors.Black)
                            .Padding(6)
                            .Column(col =>
                            {
                                col.Item().Row(r =>
                                {
                                    r.ConstantColumn(75).Text("Fornecedor :").SemiBold();
                                    r.RelativeColumn().Text(model.Fornecedor.RazaoSocial);
                                });

                                col.Item().Row(r =>
                                {
                                    r.ConstantColumn(75).Text("Endereço :").SemiBold();
                                    r.RelativeColumn().Text(model.Fornecedor.Endereco);
                                });

                                col.Item().Row(r =>
                                {
                                    r.ConstantColumn(75).Text("Bairro :").SemiBold();
                                    r.RelativeColumn().Text(model.Fornecedor.Bairro);
                                });

                                col.Item().Row(r =>
                                {
                                    r.ConstantColumn(75).Text("Fone / Fax :").SemiBold();
                                    r.RelativeColumn().Text(model.Fornecedor.Telefone);
                                });

                                col.Item().Row(r =>
                                {
                                    r.ConstantColumn(75).Text("A / C :").SemiBold();
                                    r.RelativeColumn().Text(model.Fornecedor.Contato);
                                });

                                col.Item().PaddingTop(4).Row(r =>
                                {
                                    r.RelativeColumn().Text($"CEP : {model.Fornecedor.Cep}");
                                    r.RelativeColumn().Text($"CNPJ : {model.Fornecedor.CpfCnpj}");
                                    r.RelativeColumn().Text($"Cidade / UF : {model.Fornecedor.Municipio.Nome}/{model.Fornecedor.Municipio.UF}");
                                    r.RelativeColumn().Text($"I.E : {model.Fornecedor.Ie}");
                                });
                            });
                    });

                    // =========================
                    // CONTENT
                    // =========================
                    page.Content().PaddingTop(10).Column(content =>
                    {
                        content.Item().Element(e => ItemsTable(e, items, culture));

                        content.Item().PaddingTop(10).Row(row =>
                        {
                            row.RelativeColumn().Column(left =>
                            {
                                left.Item().Text($"Pagamento : {model.CondicaoPagamento?.Descricao ?? ""}");

                                if (!string.IsNullOrWhiteSpace(tipoFrete))
                                    left.Item().Text($"Frete : {tipoFrete}");

                                left.Item().Text("R$ " + valorFrete.ToString("N2", culture));
                            });

                            row.ConstantColumn(240).Element(e =>
                            {
                                TotalsBox(
                                    e,
                                    totalSemImpostos.ToString("N2", culture),
                                    totalIpi.ToString("N2", culture),
                                    valorFrete.ToString("N2", culture),
                                    totalGeral.ToString("N2", culture)
                                );
                            });
                        });

                        content.Item().PaddingTop(8).Element(box =>
                        {
                            box.Border(1).BorderColor(Colors.Black).Padding(6).Column(c =>
                            {
                                c.Item()
                                    .Background(Colors.Grey.Lighten3)
                                    .PaddingVertical(3).PaddingHorizontal(4)
                                    .Text("Observações").SemiBold();

                                c.Item().PaddingTop(4).Text(textoPadrao ?? "");
                            });
                        });

                        content.Item().PaddingTop(12).Column(col =>
                        {
                            col.Item().Text("No aguardo de seu retorno,");
                            col.Item().PaddingTop(6).Text(compradorNome).SemiBold();
                            col.Item().Text(compradorEmail);
                            col.Item().Text(compradorDepto);
                        });
                    });

                    // =========================
                    // FOOTER
                    // =========================
                    page.Footer().AlignRight().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            using var ms = new MemoryStream();
            doc.GeneratePdf(ms);
            return ms.ToArray();
        }

        // =========================
        // TABELA ITENS:
        // - sem borda vertical
        // - borda externa
        // - linha horizontal no header e por item
        // - zebra light
        // - descrição longa quebra e mantém colunas alinhadas
        // =========================
        private static void ItemsTable(IContainer container, List<PedidoCompraItemDto> items, CultureInfo culture)
        {
            var zebraColor = Colors.Grey.Lighten4;

            container
                .Border(1).BorderColor(Colors.Black)
                .Padding(0)
                .Column(box =>
                {
                    // Header (ROW, sem borda de coluna)
                    box.Item()
                        .Background(Colors.Grey.Lighten3)
                        .PaddingVertical(4)
                        .PaddingHorizontal(6)
                        .Row(r =>
                        {
                            HeaderText(r.RelativeColumn(6), "Produto");
                            HeaderText(r.RelativeColumn(2), "Entrega");
                            HeaderText(r.RelativeColumn(2).AlignRight(), "Quantidade");
                            HeaderText(r.RelativeColumn(2).AlignRight(), "Preço / UN");
                            HeaderText(r.RelativeColumn(2).AlignRight(), "Total");
                            HeaderText(r.RelativeColumn(1).AlignRight(), "IPI");
                            HeaderText(r.RelativeColumn(2).AlignRight(), "Total + IPI");
                        });

                    box.Item().LineHorizontal(1).LineColor(Colors.Black);

                    for (int i = 0; i < items.Count; i++)
                    {
                        var item = items[i];
                        var isZebra = i % 2 == 1;

                        var entrega = (item as dynamic)?.DataEntrega is DateTime de
                            ? de.ToString("dd/MM/yyyy")
                            : ((item as dynamic)?.DataEntrega as string ?? "");

                        decimal aliquotaIpi = 0m;
                        try { aliquotaIpi = (decimal)((item as dynamic)?.AliquotaIpi ?? 0m); } catch { }

                        var totalItem = item.TotalItem;
                        var valorIpiItem = aliquotaIpi > 0 ? Math.Round(totalItem * (aliquotaIpi / 100m), 2) : 0m;
                        var totalComIpi = totalItem + valorIpiItem;

                        box.Item()
                            .Background(isZebra ? zebraColor : Colors.White)
                            .PaddingVertical(3)
                            .PaddingHorizontal(6)
                            .Row(r =>
                            {
                                // Produto: quebra (inclusive em códigos grandes) usando “soft break”
                                BodyTextProduto(r.RelativeColumn(6), $"{item.ProdutoCodigo ?? ""} - {item.ProdutoDescricao ?? ""}");

                                // Outras colunas: normalmente não quebram; mantemos alinhamento e fontes fixas
                                BodyText(r.RelativeColumn(2), entrega);
                                BodyText(r.RelativeColumn(2).AlignRight(), item.Quantidade.ToString("N0", culture));
                                BodyText(r.RelativeColumn(2).AlignRight(), item.PrecoUnitario.ToString("N5", culture));
                                BodyText(r.RelativeColumn(2).AlignRight(), totalItem.ToString("N2", culture));
                                BodyText(r.RelativeColumn(1).AlignRight(), aliquotaIpi > 0 ? aliquotaIpi.ToString("N1", culture) : "");
                                BodyText(r.RelativeColumn(2).AlignRight(), totalComIpi.ToString("N2", culture));
                            });

                        box.Item().LineHorizontal(1).LineColor(Colors.Black);
                    }
                });
        }

        // =========================
        // TOTALS BOX
        // =========================
        private static void TotalsBox(IContainer container, string totalSemImpostos, string totalIpi, string totalFrete, string totalGeral)
        {
            container.Border(1).BorderColor(Colors.Black).Padding(0).Column(col =>
            {
                col.Item()
                    .Background(Colors.Grey.Lighten3)
                    .PaddingVertical(4).PaddingHorizontal(6)
                    .Text("TOTAIS").SemiBold().FontSize(10);

                col.Item().Padding(6).Column(c =>
                {
                    TotLineStrong(c, "Total sem impostos :", totalSemImpostos);
                    TotLineStrong(c, "Total IPI :", totalIpi);
                    TotLineStrong(c, "Total frete :", totalFrete);

                    c.Item().PaddingTop(4).LineHorizontal(1).LineColor(Colors.Black);

                    c.Item()
                        .PaddingTop(6)
                        .Background(Colors.Grey.Lighten2)
                        .Padding(6)
                        .Row(r =>
                        {
                            r.RelativeColumn().Text("TOTAL GERAL :").SemiBold().FontSize(11);
                            r.ConstantColumn(90).AlignRight().Text(totalGeral).SemiBold().FontSize(11);
                        });
                });
            });
        }

        private static void TotLineStrong(ColumnDescriptor col, string label, string value)
        {
            col.Item().Row(r =>
            {
                r.RelativeColumn().Text(label).SemiBold();
                r.ConstantColumn(90).AlignRight().Text(value).SemiBold();
            });
        }

        // =========================
        // Right Info Box
        // =========================
        private static void RightInfoBox(IContainer container, DateTime dataPedido, int idPedido, DateTime horaImpressao)
        {
            container.Border(1).BorderColor(Colors.Black).Padding(6).Table(t =>
            {
                t.ColumnsDefinition(c =>
                {
                    c.RelativeColumn(2);
                    c.RelativeColumn(3);
                });

                InfoRow(t, "DATA :", dataPedido.ToString("dd/MM/yyyy"));
                InfoRow(t, "PEDIDO Nº :", idPedido.ToString());
                InfoRow(t, "HORA :", horaImpressao.ToString("HH:mm"));
            });
        }

        private static void InfoRow(TableDescriptor t, string label, string value)
        {
            t.Cell().Text(label).SemiBold();
            t.Cell().AlignRight().Text(value);
        }

        // =========================
        // HELPERS
        // =========================
        private static void HeaderText(IContainer container, string text)
        {
            container.Text(text).SemiBold().FontSize(9);
        }

        private static void BodyText(IContainer container, string text)
        {
            container.Text(text ?? "").FontSize(9);
        }

        // Produto: forçar quebra "segura" em códigos longos sem espaço
        // (insere zero-width space após '.', '-', '/', '_' para permitir wrap sem estourar layout)
        private static void BodyTextProduto(IContainer container, string text)
        {
            container.Text(t =>
            {
                t.Span(InsertSoftBreaks(text ?? ""));
            });
        }

        private static string InsertSoftBreaks(string s)
        {
            // Zero-width space: permite quebra sem aparecer caractere visível
            const string ZWSP = "\u200B";

            // após separadores comuns de código
            return s
                .Replace(".", "." + ZWSP)
                .Replace("-", "-" + ZWSP)
                .Replace("/", "/" + ZWSP)
                .Replace("_", "_" + ZWSP);
        }
    }
}
