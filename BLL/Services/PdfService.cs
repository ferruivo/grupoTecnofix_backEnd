using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.PedidoCompra;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class PdfService : IPdfService
    {
        private const string LogoPath = "Utils/logo.png"; // ajustar se necessário
        private const string TemplatePath = "Utils/PdfTemplates/PedidoCompra.pdf";

        public async Task<string> GeneratePdfBase64Async<T>(T model)
        {
            // Serializa o objeto para exibir no PDF de forma genérica
            var json = JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true });

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text("Documento gerado").SemiBold().FontSize(16).AlignCenter();

                    page.Content().PaddingVertical(10).Column(column =>
                    {
                        column.Item().Text(text =>
                        {
                            text.Line(json);
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
                });
            });

            using var ms = new MemoryStream();
            doc.GeneratePdf(ms);
            var bytes = ms.ToArray();
            var base64 = Convert.ToBase64String(bytes);

            await Task.CompletedTask; // manter assinatura async
            return base64;
        }

        public async Task<string> GeneratePurchaseOrderPdfBase64Async(PedidoCompraDto model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            // If template exists, stamp it using PdfSharpCore
            if (File.Exists(TemplatePath))
            {
                using var templateStream = File.OpenRead(TemplatePath);
                // Open the PDF template for modification
                var document = PdfReader.Open(templateStream, PdfDocumentOpenMode.Modify);

                // Use first page as base (if multiple pages needed, you can extend)
                var page = document.PageCount > 0 ? document.Pages[0] : document.AddPage();

                var gfx = XGraphics.FromPdfPage(page);

                // Fonts (system dependent)
                var fontRegular = new XFont("Arial", 10, XFontStyle.Regular);
                var fontBold = new XFont("Arial", 11, XFontStyle.Bold);

                // Draw header fields (coordinates may need adjustment to match template)
                gfx.DrawString($"Pedido: {model.IdPedidoCompra}", fontBold, XBrushes.Black, new XPoint(420, 80));
                gfx.DrawString($"Data: {model.DataPedido:dd/MM/yyyy}", fontRegular, XBrushes.Black, new XPoint(420, 100));
                gfx.DrawString($"Fornecedor: {model.FornecedorNome}", fontRegular, XBrushes.Black, new XPoint(40, 130));

                // Draw items table starting position
                double startX = 40;
                double startY = 230; // ajustar conforme template
                double rowHeight = 18;

                // Column positions (adjust to match template layout)
                double colCodigo = startX;
                double colDescricao = startX + 60;
                double colQtd = startX + 360;
                double colUnit = startX + 420;
                double colTotal = startX + 490;

                // Header for items (optional, if template already has table headers)
                gfx.DrawString("Cód", fontBold, XBrushes.Black, new XPoint(colCodigo, startY - 12));
                gfx.DrawString("Descrição", fontBold, XBrushes.Black, new XPoint(colDescricao, startY - 12));
                gfx.DrawString("Qtd", fontBold, XBrushes.Black, new XPoint(colQtd, startY - 12));
                gfx.DrawString("Unit", fontBold, XBrushes.Black, new XPoint(colUnit, startY - 12));
                gfx.DrawString("Total", fontBold, XBrushes.Black, new XPoint(colTotal, startY - 12));

                // Render each item
                var items = model.Itens ?? new System.Collections.Generic.List<PedidoCompraItemDto>();
                double y = startY;
                foreach (var item in items)
                {
                    gfx.DrawString(item.ProdutoCodigo ?? "", fontRegular, XBrushes.Black, new XPoint(colCodigo, y));
                    gfx.DrawString(Truncate(item.ProdutoDescricao ?? "", 60), fontRegular, XBrushes.Black, new XPoint(colDescricao, y));
                    gfx.DrawString(item.Quantidade.ToString("N2"), fontRegular, XBrushes.Black, new XPoint(colQtd, y));
                    gfx.DrawString(item.PrecoUnitario.ToString("N2"), fontRegular, XBrushes.Black, new XPoint(colUnit, y));
                    gfx.DrawString(item.TotalItem.ToString("N2"), fontRegular, XBrushes.Black, new XPoint(colTotal, y));

                    y += rowHeight;

                    // If page is full, you could add logic to create/append a new page and continue
                }

                // Totals area (adjust positions)
                double totalsX = 420;
                double totalsY = Math.Max(y + 10, 420);

                gfx.DrawString($"Total Produtos: {model.TotalProdutos:N2}", fontBold, XBrushes.Black, new XPoint(totalsX, totalsY));
                gfx.DrawString($"Total IPI: {model.TotalIpi:N2}", fontRegular, XBrushes.Black, new XPoint(totalsX, totalsY + 15));
                gfx.DrawString($"Total ICMS: {model.TotalIcms:N2}", fontRegular, XBrushes.Black, new XPoint(totalsX, totalsY + 30));
                gfx.DrawString($"Frete: {model.ValorFrete:N2}", fontRegular, XBrushes.Black, new XPoint(totalsX, totalsY + 45));
                gfx.DrawString($"Total Pedido: {model.TotalPedido:N2}", fontBold, XBrushes.Black, new XPoint(totalsX, totalsY + 65));

                // Observations
                if (!string.IsNullOrWhiteSpace(model.Observacao))
                {
                    gfx.DrawString("Observações:", fontBold, XBrushes.Black, new XPoint(40, totalsY + 90));
                    gfx.DrawString(Truncate(model.Observacao, 200), fontRegular, XBrushes.Black, new XPoint(40, totalsY + 105));
                }

                // Save to memory stream
                using var outStream = new MemoryStream();
                document.Save(outStream, false);
                var bytes = outStream.ToArray();
                var base64 = Convert.ToBase64String(bytes);

                await Task.CompletedTask;
                return base64;
            }

            // Fallback: generate with QuestPDF if template not found
            return await GeneratePdfBase64Async(model);
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength - 3) + "...";
        }
    }
}
