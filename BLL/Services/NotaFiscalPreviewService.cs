using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.NotaFiscal;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class NotaFiscalPreviewService : INotaFiscalPreviewService
    {
        private readonly INotaFiscalPreviewRepository _repo;

        public NotaFiscalPreviewService(INotaFiscalPreviewRepository repo)
        {
            _repo = repo;
        }

        public async Task<NotaFiscalPreviewResponseDto> GerarPreviewAsync(
            NotaFiscalPreviewRequestDto dto,
            CancellationToken ct)
        {
            var result = new NotaFiscalPreviewResponseDto();

            if (dto.Itens == null || dto.Itens.Count == 0)
                throw new InvalidOperationException("Informe ao menos um item para gerar o preview.");

            var natureza = await _repo.GetNaturezaOperacaoAsync(
                dto.IdNaturezaOperacao,
                ct);

            if (natureza == null)
                throw new KeyNotFoundException("Natureza de operação não encontrada.");

            var itemNumero = 1;

            foreach (var itemDto in dto.Itens)
            {
                var produto = await _repo.GetProdutoAsync(
                    itemDto.IdProduto,
                    ct);

                if (produto == null)
                    throw new KeyNotFoundException(
                        $"Produto {itemDto.IdProduto} não encontrado.");

                var item = new NotaFiscalPreviewItemResponseDto
                {
                    Item = itemNumero++,
                    OrigemItem = itemDto.OrigemItem,
                    IdProduto = itemDto.IdProduto,
                    CodigoProduto = produto.Codigo,
                    DescricaoProduto = produto.Descricao,
                    Ncm = produto.Ncm,
                    Cfop = string.IsNullOrWhiteSpace(itemDto.Cfop)
                        ? natureza.Cfop1
                        : itemDto.Cfop,

                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = itemDto.PrecoUnitario,
                    ValorDesconto = Round2(itemDto.ValorDesconto),
                    PercentualIcms = itemDto.PercentualIcms,
                    PercentualIpi = itemDto.PercentualIpi
                };

                item.ValorProduto = Round2(
                    item.Quantidade * item.PrecoUnitario);

                item.ValorTotal = Round2(
                    item.ValorProduto
                    - item.ValorDesconto
                    + itemDto.ValorFrete
                    + itemDto.ValorSeguro
                    + itemDto.ValorOutrasDespesas
                );

                CalcularIcms(item, produto, natureza);
                CalcularIpi(item, produto);
                CalcularPisCofins(item);
                CalcularIbsCbs(item);

                result.Itens.Add(item);

                SomarTotais(result, item);
            }

            result.ValorFrete = Round2(dto.ValorFrete);
            result.ValorSeguro = Round2(dto.ValorSeguro);
            result.ValorDesconto += Round2(dto.ValorDesconto);
            result.ValorDespesasAcessorias = Round2(dto.ValorDespesasAcessorias);

            result.ValorNota = Round2(
                result.ValorProdutos
                - result.ValorDesconto
                + result.ValorFrete
                + result.ValorSeguro
                + result.ValorDespesasAcessorias
                + result.ValorIpi
                + result.ValorIcmsSt
                + result.ValorCbs
                + result.ValorIbsUf
                + result.ValorIbsMunicipio
                + result.ValorIs
            );

            return result;
        }

        private static void CalcularIcms(
    NotaFiscalPreviewItemResponseDto item,
    dynamic produto,
    dynamic natureza)
        {
            var cfop = item.Cfop ?? string.Empty;

            decimal aliquota = 0;

            //if (produto.Icms != null)
                aliquota = Convert.ToDecimal(item.PercentualIcms);

            var baseCalculo = item.ValorProduto;

            if (cfop is "5949" or "6949" or "5901" or "6923")
            {
                baseCalculo = 0;
                aliquota = 0;
            }

            item.PercentualIcms = Round4(aliquota);

            item.Tributos.Add(new NotaFiscalPreviewTributoDto
            {
                TipoTributo = "ICMS",
                Cst = produto.CstIcms?.ToString(),
                BaseCalculo = Round2(baseCalculo),
                Aliquota = Round4(aliquota),
                Valor = Round2(baseCalculo * aliquota / 100)
            });
        }

        private static void CalcularIpi(
    NotaFiscalPreviewItemResponseDto item,
    dynamic produto)
        {
            decimal aliquota = 0;

           
                aliquota = Convert.ToDecimal(item.PercentualIpi);

            var baseCalculo = item.ValorProduto;

            item.PercentualIpi = Round4(aliquota);

            item.Tributos.Add(new NotaFiscalPreviewTributoDto
            {
                TipoTributo = "IPI",
                Cst = produto.CstIpi?.ToString(),
                BaseCalculo = Round2(baseCalculo),
                Aliquota = Round4(aliquota),
                Valor = Round2(baseCalculo * aliquota / 100)
            });
        }

        private static void CalcularPisCofins(
            NotaFiscalPreviewItemResponseDto item)
        {
            var baseCalculo = item.ValorProduto;

            item.Tributos.Add(new NotaFiscalPreviewTributoDto
            {
                TipoTributo = "PIS",
                Cst = "01",
                BaseCalculo = Round2(baseCalculo),
                Aliquota = 0.65m,
                Valor = Round2(baseCalculo * 0.65m / 100)
            });

            item.Tributos.Add(new NotaFiscalPreviewTributoDto
            {
                TipoTributo = "COFINS",
                Cst = "01",
                BaseCalculo = Round2(baseCalculo),
                Aliquota = 3.00m,
                Valor = Round2(baseCalculo * 3.00m / 100)
            });
        }

        private static void CalcularIbsCbs(
            NotaFiscalPreviewItemResponseDto item)
        {
            var baseCalculo = item.ValorTotal;

            item.Tributos.Add(new NotaFiscalPreviewTributoDto
            {
                TipoTributo = "IBS_UF",
                Cst = "000",
                CClassTrib = "000001",
                BaseCalculo = Round2(baseCalculo),
                Aliquota = 0.10m,
                Valor = Round2(baseCalculo * 0.10m / 100)
            });

            item.Tributos.Add(new NotaFiscalPreviewTributoDto
            {
                TipoTributo = "IBS_MUN",
                Cst = "000",
                CClassTrib = "000001",
                BaseCalculo = Round2(baseCalculo),
                Aliquota = 0.00m,
                Valor = 0
            });

            item.Tributos.Add(new NotaFiscalPreviewTributoDto
            {
                TipoTributo = "CBS",
                Cst = "000",
                CClassTrib = "000001",
                BaseCalculo = Round2(baseCalculo),
                Aliquota = 0.90m,
                Valor = Round2(baseCalculo * 0.90m / 100)
            });
        }

        private static void SomarTotais(
            NotaFiscalPreviewResponseDto result,
            NotaFiscalPreviewItemResponseDto item)
        {
            result.ValorProdutos += item.ValorProduto;
            result.ValorDesconto += item.ValorDesconto;

            foreach (var tributo in item.Tributos)
            {
                switch (tributo.TipoTributo)
                {
                    case "ICMS":
                        result.BaseIcms += tributo.BaseCalculo;
                        result.ValorIcms += tributo.Valor;
                        break;

                    case "ICMS_ST":
                        result.BaseIcmsSt += tributo.BaseCalculo;
                        result.ValorIcmsSt += tributo.Valor;
                        break;

                    case "IPI":
                        result.ValorIpi += tributo.Valor;
                        break;

                    case "PIS":
                        result.ValorPis += tributo.Valor;
                        break;

                    case "COFINS":
                        result.ValorCofins += tributo.Valor;
                        break;

                    case "CBS":
                        result.BaseCbs += tributo.BaseCalculo;
                        result.ValorCbs += tributo.Valor;
                        break;

                    case "IBS_UF":
                        result.BaseIbsUf += tributo.BaseCalculo;
                        result.ValorIbsUf += tributo.Valor;
                        break;

                    case "IBS_MUN":
                    case "IBS_MUNICIPIO":
                        result.BaseIbsMunicipio += tributo.BaseCalculo;
                        result.ValorIbsMunicipio += tributo.Valor;
                        break;

                    case "IS":
                        result.ValorIs += tributo.Valor;
                        break;
                }
            }
        }

        private static decimal Round2(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private static decimal Round4(decimal value)
        {
            return Math.Round(value, 4, MidpointRounding.AwayFromZero);
        }
    }
}