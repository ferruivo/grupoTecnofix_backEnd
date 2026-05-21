using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Utils;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class RecebimentoService : IRecebimentoService
    {
        private readonly IRecebimentoRepository _repo;
        private readonly IEmpresaRepository _empresaRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<RecebimentoService> _logger;
        public RecebimentoService(IRecebimentoRepository repo, IEmpresaRepository empresaRepo, ICurrentUserService currentUser, ILogger<RecebimentoService> logger)
        {
            _repo = repo;
            _empresaRepo = empresaRepo;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<RecebimentoPedidoDto?> GetPedidoByNumeroAsync(int numero, CancellationToken ct)
        {
            return await _repo.GetPedidoByNumeroAsync(numero, ct);
        }

        public async Task<RecebimentoLoteDto> CriarLoteAsync(RecebimentoLoteCreateDto dto, CancellationToken ct)
        {
            var idUsuario = _currentUser.GetUsuarioLogadoId();

            return await _repo.CriarLoteAsync(dto, idUsuario, ct);
        }

        public async Task<List<RecebimentoLoteDto>> GetLotesByPedidoAsync(int idPedidoCompra, CancellationToken ct)
        {
            return await _repo.GetLotesByPedidoAsync(idPedidoCompra, ct);
        }

        public async Task<byte[]> GetEtiquetasByLoteAsync(long idLote, CancellationToken ct)
        {
            var etquetaDto = await _repo.GetLoteEtiquetaByIdAsync(idLote, ct);
            var empresa = await _empresaRepo.GetAsync(null, ct);
            etquetaDto.Empresa = empresa.NomeFantasia;
            return GerarEtiquetasPdf(etquetaDto);
        }

        public byte[] GerarEtiquetasPdf(EtiquetaProdutoDto dto)
        {
            var etiquetas = new List<int>();

            var restante = dto.QuantidadeTotal;

            while (restante > 0)
            {
                var qtd = restante >= dto.Fator ? dto.Fator : restante;
                etiquetas.Add((int)qtd);
                restante -= qtd;
            }

            var logoPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "logo.png");

            var logoBytes = File.Exists(logoPath)
                ? File.ReadAllBytes(logoPath)
                : null;

            return Document.Create(container =>
            {
                for (int i = 0; i < etiquetas.Count; i++)
                {
                    var numeroEtiqueta = i + 1;
                    var quantidadeEtiqueta = etiquetas[i];

                    var codigoBarras =
                        dto.Lote.ToString().PadLeft(6, '0') +
                        numeroEtiqueta.ToString().PadLeft(4, '0') +
                        quantidadeEtiqueta.ToString().PadLeft(4, '0');

                    var barcodeBytes = GenerateBarcode(codigoBarras);

                    container.Page(page =>
                    {
                        page.Size(283.46f, 141.73f); // 100mm x 50mm
                        page.Margin(0);

                        page.Content()
                            .PaddingLeft(6)
                            .PaddingRight(6)
                            .PaddingTop(4)
                            .PaddingBottom(4)
                            .Border(1)
                            .Padding(4)
                            .Column(col =>
                            {
                                col.Spacing(1);

                                col.Item().Row(row =>
                                {
                                    row.ConstantItem(38)
                                        .Height(24)
                                        .Element(e =>
                                        {
                                            if (logoBytes != null)
                                                e.Image(logoBytes).FitArea();
                                            else
                                                e.Text("");
                                        });

                                    row.RelativeItem().Column(title =>
                                    {
                                        title.Item()
                                            .AlignCenter()
                                            .Text(dto.Empresa)
                                            .FontSize(7)
                                            .Bold();

                                        title.Item()
                                            .AlignCenter()
                                            .Text("ISO 9001")
                                            .FontSize(6);
                                    });
                                });

                                col.Item()
                                    .PaddingTop(2)
                                    .Text(text =>
                                    {
                                        text.DefaultTextStyle(x => x.FontSize(6).Bold());
                                        text.Span("PRODUTO ");
                                        text.Span($"{dto.ProdutoCodigo} - {dto.ProdutoDescricao}");
                                    });

                                col.Item()
                                    .PaddingTop(2)
                                    .Row(row =>
                                    {
                                        row.RelativeItem()
                                            .Text($"QUANTIDADE {quantidadeEtiqueta}")
                                            .FontSize(8)
                                            .Bold();

                                        row.ConstantItem(80)
                                            .AlignRight()
                                            .Text($"DEM: {dto.Lote} / {numeroEtiqueta}")
                                            .FontSize(8)
                                            .Bold();
                                    });

                                col.Item()
                                .PaddingTop(2)
                                .AlignCenter()
                                .Width(220)
                                .Height(48)
                                .Image(barcodeBytes)
                                .FitArea();

                                col.Item()
                                .AlignCenter()
                                .Text(codigoBarras)
                                .FontSize(7);
                            });
                    });
                }
            }).GeneratePdf();
        }

        private byte[] GenerateBarcode(string content)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 520,
                    Height = 120,
                    Margin = 10,
                    PureBarcode = true
                }
            };

            var pixelData = writer.Write(content);

            using var bitmap = new Bitmap(
                pixelData.Width,
                pixelData.Height,
                PixelFormat.Format32bppRgb);

            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb);

            try
            {
                Marshal.Copy(
                    pixelData.Pixels,
                    0,
                    bitmapData.Scan0,
                    pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            using var ms = new MemoryStream();

            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return ms.ToArray();
        }
    }
}
