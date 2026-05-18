using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.PedidoCompra;
using GrupoTecnofix_Api.BLL.Templates;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.PedidoVenda;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class PdfService : IPdfService
    {
        // removed hardcoded relative path; resolve at runtime to support publish environments

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

        public async Task<string> GeneratePurchaseOrderPdfBase64Async(EmpresaDto empresa, UsuarioDto usuario, PedidoCompraDto model)
        {
            var logo = ResolveLogoPath();
            var bytes = PedidoCompraPdfBuilder.Build(empresa, usuario, model, logo);
            var base64 = Convert.ToBase64String(bytes);

            await Task.CompletedTask;
            return base64;
        }

        public async Task<string> GenerateSalesOrderPdfBase64Async(EmpresaDto empresa, UsuarioDto usuario, PedidoVendaDto model)
        {
            var logo = ResolveLogoPath();
            var bytes = PedidoVendaPdfBuilder.Build(empresa, usuario, model, logo);
            var base64 = Convert.ToBase64String(bytes);

            await Task.CompletedTask;
            return base64;
        }

        private static string? ResolveLogoPath()
        {
            // try several likely locations (preserve casing variations) relative to app base and current dir
            var baseDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var current = Directory.GetCurrentDirectory();

            var candidates = new[]
            {
                Path.Combine(baseDir, "Utils", "Logo.png"),
                Path.Combine(baseDir, "Utils", "logo.png"),
                Path.Combine(baseDir, "wwwroot", "images", "logo.png"),
                Path.Combine(baseDir, "wwwroot", "logo.png"),
                Path.Combine(current, "Utils", "Logo.png"),
                Path.Combine(current, "Utils", "logo.png"),
                Path.Combine(current, "wwwroot", "images", "logo.png"),
                Path.Combine(current, "wwwroot", "logo.png")
            };

            foreach (var c in candidates)
            {
                try
                {
                    if (File.Exists(c)) return c;
                }
                catch { }
            }

            return null;
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength - 3) + "...";
        }
    }
}
