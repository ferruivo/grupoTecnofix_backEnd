using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using GrupoTecnofix_Api.BLL.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class PdfService : IPdfService
    {
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
    }
}
