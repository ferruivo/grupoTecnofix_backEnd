using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class NotaFiscalCalculoService : INotaFiscalCalculoService
    {
        public Task CalcularAsync(NotaFiscal nota, CancellationToken ct = default)
        {
            ZerarTotais(nota);

            foreach (var item in nota.NotaFiscalItems)
            {
                CalcularItem(item);

                // Ensure tributos exist (if caller didn't send them) with sensible defaults
                EnsureTributos(item);

                CalcularTributosItem(item);
                SomarTotaisNota(nota, item);
            }

            CalcularTotalNota(nota);

            return Task.CompletedTask;
        }

        private static void ZerarTotais(NotaFiscal nota)
        {
            nota.ValorProdutos = 0;
            nota.ValorDesconto = 0;
            nota.ValorFrete = 0;
            nota.ValorSeguro = 0;
            nota.ValorDespesasAcessorias = 0;

            nota.BaseIcms = 0;
            nota.ValorIcms = 0;
            nota.BaseIcmsSt = 0;
            nota.ValorIcmsSt = 0;
            nota.ValorIpi = 0;
            nota.ValorPis = 0;
            nota.ValorCofins = 0;
            nota.ValorImportacao = 0;

            nota.BaseCbs = 0;
            nota.ValorCbs = 0;
            nota.BaseIbsUf = 0;
            nota.ValorIbsUf = 0;
            nota.BaseIbsMunicipio = 0;
            nota.ValorIbsMunicipio = 0;
            nota.ValorIs = 0;

            nota.ValorNota = 0;
        }

        private static void CalcularItem(NotaFiscalItem item)
        {
            item.ValorProduto = Round2(item.Quantidade * item.PrecoUnitario);

            item.ValorDesconto = Round2(item.ValorDesconto);
            item.ValorFrete = Round2(item.ValorFrete);
            item.ValorSeguro = Round2(item.ValorSeguro);
            item.ValorOutrasDespesas = Round2(item.ValorOutrasDespesas);

            item.ValorTotal = Round2(
                item.ValorProduto
                - item.ValorDesconto
                + item.ValorFrete
                + item.ValorSeguro
                + item.ValorOutrasDespesas
            );
        }

        private static void CalcularTributosItem(NotaFiscalItem item)
        {
            if (item.NotaFiscalItemTributos == null) return;

            foreach (var tributo in item.NotaFiscalItemTributos)
            {
                tributo.BaseCalculo = Round2(tributo.BaseCalculo);

                tributo.Valor = Round2(
                    tributo.BaseCalculo * tributo.Aliquota / 100
                );
            }
        }

        private static void EnsureTributos(NotaFiscalItem item)
        {
            if (item.NotaFiscalItemTributos == null)
                item.NotaFiscalItemTributos = new System.Collections.Generic.List<NotaFiscalItemTributo>();

            // If there are no tributos, add default PIS/COFINS/IPI/ICMS based on simple percentages
            if (item.NotaFiscalItemTributos.Count == 0)
            {
                var baseCalc = item.ValorProduto;

                item.NotaFiscalItemTributos.Add(new NotaFiscalItemTributo
                {
                    TipoTributo = "PIS",
                    BaseCalculo = baseCalc,
                    Aliquota = 0.65m
                });

                item.NotaFiscalItemTributos.Add(new NotaFiscalItemTributo
                {
                    TipoTributo = "COFINS",
                    BaseCalculo = baseCalc,
                    Aliquota = 3.00m
                });

                // add IPI if any percentual (simple heuristic: skip if zero)
                item.NotaFiscalItemTributos.Add(new NotaFiscalItemTributo
                {
                    TipoTributo = "IPI",
                    BaseCalculo = baseCalc,
                    Aliquota = item.PercentualIpi
                });

                // ICMS placeholder
                item.NotaFiscalItemTributos.Add(new NotaFiscalItemTributo
                {
                    TipoTributo = "ICMS",
                    BaseCalculo = baseCalc,
                    Aliquota = item.PercentualIcms
                });
            }
        }

        private static void SomarTotaisNota(NotaFiscal nota, NotaFiscalItem item)
        {
            nota.ValorProdutos += item.ValorProduto;
            nota.ValorDesconto += item.ValorDesconto;
            nota.ValorFrete += item.ValorFrete;
            nota.ValorSeguro += item.ValorSeguro;
            nota.ValorDespesasAcessorias += item.ValorOutrasDespesas;

            foreach (var tributo in item.NotaFiscalItemTributos)
            {
                var tipo = tributo.TipoTributo.ToUpper().Trim();

                switch (tipo)
                {
                    case "ICMS":
                        nota.BaseIcms += tributo.BaseCalculo;
                        nota.ValorIcms += tributo.Valor;
                        break;

                    case "ICMS_ST":
                        nota.BaseIcmsSt += tributo.BaseCalculo;
                        nota.ValorIcmsSt += tributo.Valor;
                        break;

                    case "IPI":
                        nota.ValorIpi += tributo.Valor;
                        break;

                    case "PIS":
                        nota.ValorPis += tributo.Valor;
                        break;

                    case "COFINS":
                        nota.ValorCofins += tributo.Valor;
                        break;

                    case "II":
                    case "IMPORTACAO":
                        nota.ValorImportacao += tributo.Valor;
                        break;

                    case "CBS":
                        nota.BaseCbs += tributo.BaseCalculo;
                        nota.ValorCbs += tributo.Valor;
                        break;

                    case "IBS_UF":
                        nota.BaseIbsUf += tributo.BaseCalculo;
                        nota.ValorIbsUf += tributo.Valor;
                        break;

                    case "IBS_MUN":
                    case "IBS_MUNICIPIO":
                        nota.BaseIbsMunicipio += tributo.BaseCalculo;
                        nota.ValorIbsMunicipio += tributo.Valor;
                        break;

                    case "IS":
                        nota.ValorIs += tributo.Valor;
                        break;
                }
            }
        }

        private static void CalcularTotalNota(NotaFiscal nota)
        {
            nota.ValorProdutos = Round2(nota.ValorProdutos);
            nota.ValorDesconto = Round2(nota.ValorDesconto);
            nota.ValorFrete = Round2(nota.ValorFrete);
            nota.ValorSeguro = Round2(nota.ValorSeguro);
            nota.ValorDespesasAcessorias = Round2(nota.ValorDespesasAcessorias);

            nota.BaseIcms = Round2(nota.BaseIcms);
            nota.ValorIcms = Round2(nota.ValorIcms);
            nota.BaseIcmsSt = Round2(nota.BaseIcmsSt);
            nota.ValorIcmsSt = Round2(nota.ValorIcmsSt);
            nota.ValorIpi = Round2(nota.ValorIpi);
            nota.ValorPis = Round2(nota.ValorPis);
            nota.ValorCofins = Round2(nota.ValorCofins);
            nota.ValorImportacao = Round2(nota.ValorImportacao);

            nota.BaseCbs = Round2(nota.BaseCbs);
            nota.ValorCbs = Round2(nota.ValorCbs);
            nota.BaseIbsUf = Round2(nota.BaseIbsUf);
            nota.ValorIbsUf = Round2(nota.ValorIbsUf);
            nota.BaseIbsMunicipio = Round2(nota.BaseIbsMunicipio);
            nota.ValorIbsMunicipio = Round2(nota.ValorIbsMunicipio);
            nota.ValorIs = Round2(nota.ValorIs);

            nota.ValorNota = Round2(
                nota.ValorProdutos
                - nota.ValorDesconto
                + nota.ValorFrete
                + nota.ValorSeguro
                + nota.ValorDespesasAcessorias
                + nota.ValorIpi
                + nota.ValorIcmsSt
                + nota.ValorImportacao
                + nota.ValorCbs
                + nota.ValorIbsUf
                + nota.ValorIbsMunicipio
                + nota.ValorIs
            );
        }

        private static decimal Round2(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
