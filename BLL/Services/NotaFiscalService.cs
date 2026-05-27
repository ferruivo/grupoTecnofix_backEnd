using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Acbr;
using GrupoTecnofix_Api.Dtos.NotaFiscal;
using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class NotaFiscalService : INotaFiscalService
    {
        private readonly INotaFiscalRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        private readonly INotaFiscalCalculoService _calculoService;
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _configuration;
        private readonly IEmpresaService _empresaService;
        private readonly IClientesService _clientesService;
        private readonly IFornecedoresService _fornecedoresService;
        private readonly IMunicipiosRepository _municipiosRepo;
        private readonly IProdutosService _produtosService;
        private readonly ICondicoesPagamentoService _condicoesPagamentoService;

        public NotaFiscalService(
            INotaFiscalRepository repo,
            ICurrentUserService currentUser,
            IMapper mapper,
            INotaFiscalCalculoService calculoService,
            IHttpClientFactory httpFactory,
            IConfiguration configuration,
            IEmpresaService empresaService,
            IClientesService clientesService,
            IFornecedoresService fornecedoresService,
            IMunicipiosRepository municipiosRepo,
            IProdutosService produtosService,
            ICondicoesPagamentoService condicoesPagamentoService)
        {
            _repo = repo;
            _currentUser = currentUser;
            _mapper = mapper;
            _calculoService = calculoService;
            _httpFactory = httpFactory;
            _configuration = configuration;
            _empresaService = empresaService;
            _clientesService = clientesService;
            _fornecedoresService = fornecedoresService;
            _municipiosRepo = municipiosRepo;
            _produtosService = produtosService;
            _condicoesPagamentoService = condicoesPagamentoService;
        }

        public async Task<List<NotaFiscalEventoResponseDto>> GetEventosAsync(long idNotaFiscal, CancellationToken ct)
        {
            var nota = await _repo.GetByIdAsync(idNotaFiscal, ct);
            if (nota is null) throw new KeyNotFoundException("Nota fiscal não encontrada.");

            var result = new List<NotaFiscalEventoResponseDto>();

            foreach (var ev in nota.NotaFiscalEventos.OrderBy(e => e.DataEvento))
            {
                var dto = new NotaFiscalEventoResponseDto
                {
                    IdNotaFiscalEvento = ev.IdNotaFiscalEvento,
                    DataEvento = ev.DataEvento,
                    TipoEvento = MapTipoEvento(ev.TipoEvento),
                    Usuario = ev.Usuario,
                    XmlRetorno = ev.XmlRetorno
                };


                // try parse XmlRetorno as JSON returned by ACBr integration
                if (!string.IsNullOrWhiteSpace(ev.XmlRetorno))
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(ev.XmlRetorno);
                        var root = doc.RootElement;

                        // collect details
                        var details = new Dictionary<string, string?>();
                        var errors = new List<string>();

                        // If contains top-level error object
                        if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("error", out var err))
                        {
                            // err may contain message and errors[]
                            if (err.ValueKind == JsonValueKind.Object)
                            {
                                if (err.TryGetProperty("message", out var m) && m.ValueKind == JsonValueKind.String)
                                    errors.Add(m.GetString()!);

                                if (err.TryGetProperty("errors", out var errs) && errs.ValueKind == JsonValueKind.Array)
                                {
                                    foreach (var item in errs.EnumerateArray())
                                    {
                                        if (item.ValueKind == JsonValueKind.Object && item.TryGetProperty("message", out var im) && im.ValueKind == JsonValueKind.String)
                                            errors.Add(im.GetString()!);
                                    }
                                }
                                if (err.TryGetProperty("code", out var c) && c.ValueKind == JsonValueKind.String)
                                    details["error_code"] = c.GetString();
                            }

                            // also expose http_status or raw if present
                            if (root.TryGetProperty("http_status", out var hs) && hs.ValueKind == JsonValueKind.Number)
                                details["http_status"] = hs.GetRawText();

                            if (root.TryGetProperty("raw", out var raw) && raw.ValueKind == JsonValueKind.String)
                                details["raw"] = raw.GetString();
                        }

                        // Extrair motivo_status de autorizacao, mesmo sem error no topo
                        if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("autorizacao", out var autorizacao) && autorizacao.ValueKind == JsonValueKind.Object)
                        {
                            if (autorizacao.TryGetProperty("motivo_status", out var motivoStatus) && motivoStatus.ValueKind == JsonValueKind.String)
                            {
                                var motivo = motivoStatus.GetString();
                                if (!string.IsNullOrWhiteSpace(motivo))
                                    errors.Add(motivo);
                            }
                        }

                        // Extrair campos principais
                        if (root.ValueKind == JsonValueKind.Object)
                        {
                            if (root.TryGetProperty("chave", out var ch) && ch.ValueKind == JsonValueKind.String)
                                details["chave"] = ch.GetString();

                            if (root.TryGetProperty("numero", out var num) && (num.ValueKind == JsonValueKind.Number || num.ValueKind == JsonValueKind.String))
                                details["numero"] = num.GetRawText().Trim('"');

                            if (root.TryGetProperty("serie", out var ser) && (ser.ValueKind == JsonValueKind.Number || ser.ValueKind == JsonValueKind.String))
                                details["serie"] = ser.GetRawText().Trim('"');

                            if (root.TryGetProperty("valor_total", out var vt) && (vt.ValueKind == JsonValueKind.Number || vt.ValueKind == JsonValueKind.String))
                                details["valor_total"] = vt.GetRawText().Trim('"');

                            if (root.TryGetProperty("http_status", out var hs2) && hs2.ValueKind == JsonValueKind.Number)
                                details["http_status"] = hs2.GetRawText();
                        }

                        dto.Details = details.Count > 0 ? details : null;
                        dto.Errors = errors.Count > 0 ? errors : null;

                        // summary for human
                        if (dto.Errors != null && dto.Errors.Count > 0)
                        {
                            dto.Summary = string.Join("; ", dto.Errors);
                        }
                        else if (dto.Details != null && dto.Details.Count > 0)
                        {
                            // build small summary from details
                            var parts = new List<string>();
                            if (dto.Details.TryGetValue("chave", out var chv) && !string.IsNullOrWhiteSpace(chv)) parts.Add($"chave: {chv}");
                            if (dto.Details.TryGetValue("numero", out var nv) && !string.IsNullOrWhiteSpace(nv)) parts.Add($"numero: {nv}");
                            if (dto.Details.TryGetValue("serie", out var sv) && !string.IsNullOrWhiteSpace(sv)) parts.Add($"serie: {sv}");
                            if (dto.Details.TryGetValue("valor_total", out var vv) && !string.IsNullOrWhiteSpace(vv)) parts.Add($"valor_total: {vv}");
                            dto.Summary = parts.Count > 0 ? string.Join(", ", parts) : null;
                        }
                    }
                    catch
                    {
                        // not JSON: leave XmlRetorno as-is and set summary as first 200 chars
                        var raw = ev.XmlRetorno.Length > 200 ? ev.XmlRetorno.Substring(0, 200) + "..." : ev.XmlRetorno;
                        dto.Summary = raw;
                    }
                }

                result.Add(dto);
            }

            return result;
        }

        private static string MapTipoEvento(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;

            var v = raw.Trim();

            // if numeric code provided, map accordingly
            if (v == "1") return "AUTORIZADA";
            if (v == "2") return "REJEITADA";
            if (v == "3") return "CANCELADA";
            if (v == "4") return "CANCELAMENTO REJEITADO";
            if (v == "5") return "CARTA CORRECAO";

            // normalize textual values
            var up = v.ToUpperInvariant();
            if (up.Contains("AUTORIZ")) return "AUTORIZADA";
            if (up.Contains("REJEIT")) return "REJEITADA";
            if (up.Contains("CANCELAD")) return "CANCELADA";
            if (up.Contains("CANCELAMENTO")) return "CANCELAMENTO REJEITADO"; // assume cancelamento reject if mentions cancelamento
            if (up.Contains("CARTA")) return "CARTA CORRECAO";

            return v;
        }

        public async Task<PagedResult<NotaFiscalListDto>> GetPagedAsync(
            int page,
            int pageSize,
            string? search,
            CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListPagedAsync(page, pageSize, search, ct);
        }

        public async Task<NotaFiscal?> GetByIdAsync(
            long idNotaFiscal,
            CancellationToken ct)
        {
            var nota = await _repo.GetByIdAsync(idNotaFiscal, ct);

            if (nota is null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            return nota;
        }

        public async Task<long> CreateAsync(
            NotaFiscalCreateDto dto,
            CancellationToken ct)
        {
            var nota = _mapper.Map<NotaFiscal>(dto);

            nota.Status = string.IsNullOrWhiteSpace(nota.Status)
                ? "DIGITADA"
                : nota.Status;

            nota.DataCadastro = DateTime.Now;

            nota.EnsureCreationAudit(_currentUser);

            await _calculoService.CalcularAsync(nota, ct);

            await _repo.AddAsync(nota, ct);
            await _repo.SaveAsync(ct);

            return nota.IdNotaFiscal;
        }

        public async Task UpdateAsync(
            long idNotaFiscal,
            NotaFiscalUpdateDto dto,
            CancellationToken ct)
        {
            var nota = await _repo.GetByIdForUpdateAsync(idNotaFiscal, ct);

            if (nota is null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            _mapper.Map(dto, nota);

            nota.DataAlteracao = DateTime.Now;
            nota.EnsureUpdateAudit(_currentUser);

            await _calculoService.CalcularAsync(nota, ct);

            await _repo.SaveAsync(ct);
        }

        public async Task DeleteAsync(
            long idNotaFiscal,
            CancellationToken ct)
        {
            var nota = await _repo.GetByIdForUpdateAsync(idNotaFiscal, ct);

            if (nota is null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            await _repo.DeleteAsync(nota, ct);
            await _repo.SaveAsync(ct);
        }

        public async Task AddEventoAsync(
            NotaFiscalEventoCreateDto dto,
            CancellationToken ct)
        {
            var nota = await _repo.GetByIdForUpdateAsync(dto.IdNotaFiscal, ct);

            if (nota is null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            var evento = _mapper.Map<NotaFiscalEvento>(dto);

            evento.DataEvento = DateTime.Now;

            await _repo.AddEventoAsync(evento, ct);

            if (evento.TipoEvento.Equals("CANCELAMENTO", StringComparison.OrdinalIgnoreCase))
            {
                nota.Status = "CANCELADA";
                nota.DataCancelamento = DateTime.Now;
                nota.UsuarioCancelamento = dto.Usuario;
                nota.MotivoCancelamento = dto.Justificativa;
                nota.DataAlteracao = DateTime.Now;

                nota.EnsureUpdateAudit(_currentUser);
            }

            await _repo.SaveAsync(ct);
        }

        public async Task EmitirAsync(
    long idNotaFiscal,
    CancellationToken ct)
        {
            var nota = await _repo.GetByIdForUpdateAsync(idNotaFiscal, ct);

            if (nota is null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            await _calculoService.CalcularAsync(nota, ct);

            // Build EmitirNfeRequest
            var request = await BuildEmitirRequestAsync(nota, ct);

            // Call Acbr API
            var client = _httpFactory.CreateClient("AcbrApi");

            var empresaKey = _configuration.GetSection("AcbrApi")["EmpresaKey"] ?? string.Empty;

            var response = await client.PostAsJsonAsync($"api/nfe/{empresaKey}/emitir", request, ct);

            var responseBody = await response.Content.ReadAsStringAsync(ct);

            // default: consider error when HTTP status != 200
            var httpStatus = (int)response.StatusCode;
            var isError = httpStatus != 200;

            // attempt to parse fields from JSON body
            string id = null;
            string chave = null;
            string protocolo = null;
            DateTime? dataAutorizacao = null;
            string motivoRejeicao = null;

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Object)
                {
                    // if response includes http_status, prefer it
                    if (root.TryGetProperty("http_status", out var hs) && hs.ValueKind == JsonValueKind.Number)
                    {
                        if (hs.TryGetInt32(out var v))
                        {
                            httpStatus = v;
                            isError = httpStatus != 200;
                        }
                    }

                    if (root.TryGetProperty("error", out var _))
                        isError = true;

                    // Check for status: "rejeitado" at root
                    if (root.TryGetProperty("status", out var statusProp) && statusProp.ValueKind == JsonValueKind.String)
                    {
                        var statusVal = statusProp.GetString();
                        if (!string.IsNullOrWhiteSpace(statusVal) && statusVal.Equals("rejeitado", StringComparison.OrdinalIgnoreCase))
                        {
                            isError = true;
                        }
                    }

                    if (root.TryGetProperty("id", out var idProp) && idProp.ValueKind == JsonValueKind.String)
                        id = idProp.GetString();

                    if (root.TryGetProperty("chave", out var chaveProp) && chaveProp.ValueKind == JsonValueKind.String)
                        chave = chaveProp.GetString();

                    if (root.TryGetProperty("autorizacao", out var auth) && auth.ValueKind == JsonValueKind.Object)
                    {
                        // Check for status: "rejeitado" in autorizacao
                        if (auth.TryGetProperty("status", out var authStatus) && authStatus.ValueKind == JsonValueKind.String)
                        {
                            var authStatusVal = authStatus.GetString();
                            if (!string.IsNullOrWhiteSpace(authStatusVal) && authStatusVal.Equals("rejeitado", StringComparison.OrdinalIgnoreCase))
                            {
                                isError = true;
                            }
                        }
                        if (auth.TryGetProperty("motivo_status", out var motivoStatus) && motivoStatus.ValueKind == JsonValueKind.String)
                        {
                            motivoRejeicao = motivoStatus.GetString();
                        }
                        if (auth.TryGetProperty("numero_protocolo", out var np) && np.ValueKind == JsonValueKind.String)
                            protocolo = np.GetString();

                        if (auth.TryGetProperty("data_evento", out var dt) && dt.ValueKind == JsonValueKind.String)
                        {
                            if (DateTime.TryParse(dt.GetString(), out var dtv))
                                dataAutorizacao = dtv;
                        }
                    }
                }
            }
            catch
            {
                // ignore parse errors
            }

            // Map event type codes
            // 1 - AUTORIZADA, 2 - REJEITADA, 3 - CANCELADA, 4 - CANCELAMENTO REJEITADO, 5 - CARTA CORRECAO
            string tipoEvento = isError ? "2" : "1";

            // update nota
            nota.Status = isError ? "REJEITADA" : "AUTORIZADA";
            nota.DataAlteracao = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(id)) nota.IdNfe = id;
            if (!string.IsNullOrWhiteSpace(chave)) nota.ChaveAcesso = chave;
            if (!string.IsNullOrWhiteSpace(protocolo)) nota.ProtocoloAutorizacao = protocolo;
            if (dataAutorizacao.HasValue) nota.DataAutorizacao = dataAutorizacao.Value;
            nota.EnsureUpdateAudit(_currentUser);

            // add event (safe: current user may be unavailable in some contexts)
            string usuario = null;
            try
            {
                usuario = _currentUser.GetUsuarioLogadoId().ToString();
            }
            catch (UnauthorizedAccessException)
            {
                // no user in context (e.g., called from background). leave usuario null.
                usuario = null;
            }

            var evento = new NotaFiscalEvento
            {
                IdNotaFiscal = nota.IdNotaFiscal,
                DataEvento = DateTime.Now,
                TipoEvento = tipoEvento,
                XmlRetorno = responseBody,
                Usuario = usuario
            };
            // Se rejeitado, incluir motivo no evento (se disponível)
            if (isError && !string.IsNullOrWhiteSpace(motivoRejeicao))
            {
                // Se houver campo Justificativa ou similar, pode ser salvo aqui
                // evento.Justificativa = motivoRejeicao; // descomente se existir o campo
            }

            await _repo.AddEventoAsync(evento, ct);
            await _repo.SaveAsync(ct);
        }

        public Task<NotaFiscalImportacaoPreviewDto> ImportarItensPreviewAsync(
            IFormFile arquivo,
            CancellationToken ct)
        {
            var result = new NotaFiscalImportacaoPreviewDto();

            // Aqui depois entra a leitura real do Excel.
            // Por enquanto retorna estrutura pronta para o front integrar.

            return Task.FromResult(result);
        }

        private async Task<EmitirNfeRequestLocal> BuildEmitirRequestAsync(NotaFiscal nota, CancellationToken ct)
        {
            var req = new EmitirNfeRequestLocal();

            req.Ambiente = _configuration.GetSection("AcbrApi")["Ambiente"] ?? "homologacao"; // ou "PRODUCAO" dependendo do ambiente configurado
            req.Referencia = $"NF-{nota.NumeroNota}";

            // InfNFe
            req.InfNFe.Ide.nNF = (int)nota.NumeroNota;
            req.InfNFe.Ide.serie = nota.Serie;
            if (int.TryParse(nota.Modelo, out var modelo)) req.InfNFe.Ide.mod = modelo;

            // Empresa (emitente)
            var empresa = _empresaService.GetAsync(null, CancellationToken.None).GetAwaiter().GetResult();
            if (empresa != null)
            {
                var munEmp = _municipiosRepo.GetByIdAsync(empresa.IdMunicipio, CancellationToken.None).GetAwaiter().GetResult();
                if (munEmp != null)
                {
                    // cUF is UF code (use CodIbgeUf)
                    req.InfNFe.Ide.cUF = munEmp.CodIbgeUf ?? 0;
                    req.InfNFe.Ide.cMunFG = (munEmp.CodIbge ?? 0).ToString().PadLeft(7, '0');
                }

                // emit
                req.InfNFe.Emit.CNPJ = Regex.Replace(empresa.Cnpj ?? string.Empty, "\\D", string.Empty);
                req.InfNFe.Emit.xNome = empresa.RazaoSocial ?? string.Empty;
                req.InfNFe.Emit.xFant = empresa.NomeFantasia ?? string.Empty;
                req.InfNFe.Emit.IE = empresa.InscricaoEstadual ?? "";
                if (!int.TryParse(empresa.Regime, out var crt)) crt = 1;
                req.InfNFe.Emit.CRT = crt;
                if (munEmp != null)
                {
                    req.InfNFe.Emit.enderEmit.xLgr = empresa.Endereco ?? string.Empty;
                    req.InfNFe.Emit.enderEmit.nro = empresa.Numero.ToString();
                    req.InfNFe.Emit.enderEmit.xBairro = empresa.Bairro ?? string.Empty;
                    req.InfNFe.Emit.enderEmit.cMun = (munEmp.CodIbge ?? 0).ToString().PadLeft(7, '0');
                    req.InfNFe.Emit.enderEmit.xMun = munEmp.Nome ?? string.Empty;
                    req.InfNFe.Emit.enderEmit.UF = munEmp.Uf ?? string.Empty;
                    req.InfNFe.Emit.enderEmit.CEP = Regex.Replace(empresa.Cep ?? string.Empty, "\\D", string.Empty);
                }
            }

            // required fields to satisfy ACBr validation
            req.InfNFe.Ide.cNF = new Random().Next(10000000, 99999999).ToString();
            req.InfNFe.Ide.natOp = nota.TipoOperacao ?? "VENDA";
            req.InfNFe.Ide.dhEmi = DateTimeOffset.Now;
            req.InfNFe.Ide.tpNF = 1; // saída
            req.InfNFe.Ide.idDest = 1; // interna
            req.InfNFe.Ide.tpEmis = 1;
            req.InfNFe.Ide.tpAmb = req.Ambiente.Equals("producao", StringComparison.OrdinalIgnoreCase) ? 1 : 2;
            req.InfNFe.Ide.finNFe = (int)(nota.Finalidade == 0 ? 1 : nota.Finalidade);
            
            req.InfNFe.Ide.indPres = 0;
            req.InfNFe.Ide.procEmi = 0;

            // Totais
            req.InfNFe.Total.ICMSTot.vProd = nota.ValorProdutos;
            req.InfNFe.Total.ICMSTot.vFrete = nota.ValorFrete;
            req.InfNFe.Total.ICMSTot.vSeg = nota.ValorSeguro;
            req.InfNFe.Total.ICMSTot.vDesc = nota.ValorDesconto;
            req.InfNFe.Total.ICMSTot.vIPI = nota.ValorIpi;
            req.InfNFe.Total.ICMSTot.vOutro = nota.ValorDespesasAcessorias;
            // Calcular vNF conforme regra SEFAZ: vNF = vProd + vFrete + vSeg + vOutro - vDesc
            req.InfNFe.Total.ICMSTot.vNF = Math.Round(
                (nota.ValorProdutos + nota.ValorIpi + nota.ValorFrete + nota.ValorSeguro + nota.ValorDespesasAcessorias) - nota.ValorDesconto,
                2
            );

            // Pagamentos (obter da condição de pagamento ligada à nota)
            try
            {
                if (nota.IdPagamento.HasValue)
                {
                    var cond = await _condicoesPagamentoService.GetByIdAsync(nota.IdPagamento.Value, ct);
                    if (cond != null)
                    {
                        var valores = new List<DetPag>();
                        // Se tPag for 99, preencher xPag obrigatoriamente
                        valores.Add(new DetPag { indPag = 0, tPag = "99", vPag = nota.ValorNota, xPag = "OUTROS" });
                        req.InfNFe.Pag.detPag = valores;
                    }
                }
            }
            catch
            {
                // ignore errors retrieving condição pagamento
            }

            // Destinatario
            if (nota.IdDestinatario.HasValue)
            {
                // TipoDestinatario pode ser 'C' cliente ou 'F' fornecedor
                if ((nota.TipoDestinatario ?? "C").Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    var cliente = _clientesService.GetByIdAsync(nota.IdDestinatario.Value, CancellationToken.None).GetAwaiter().GetResult();
                    if (cliente != null)
                    {
                        req.InfNFe.Dest.xNome = cliente.Nome;
                        if (cliente.IdTipodocumento == 1) // CPF
                        {
                            req.InfNFe.Dest.CPF = cliente.Cpf;
                            req.InfNFe.Ide.indFinal = 1;
                        }
                        else
                        {
                            req.InfNFe.Dest.CNPJ = cliente.Cnpj;
                            req.InfNFe.Ide.indFinal = 0;
                        }

                        req.InfNFe.Dest.indIEDest = cliente.IdTipodocumento == 2 && cliente.InscricaoEstadual.ToUpper() != "ISENTO" ? 1 : cliente.IdTipodocumento == 2 && cliente.InscricaoEstadual.ToUpper() == "ISENTO" ? 2 : 9; // assume contribuinte com IE por padrão
                        if (cliente.Municipio != null)
                        {
                            var mun = cliente.Municipio;
                            // buscar codigo IBGE do municipio
                            var munEntity = _municipiosRepo.GetByIdAsync(cliente.IdMunicipio, CancellationToken.None).GetAwaiter().GetResult();
                            req.InfNFe.Dest.enderDest.xLgr = cliente.Endereco ?? string.Empty;
                            req.InfNFe.Dest.enderDest.nro = cliente.Numero.ToString();
                            req.InfNFe.Dest.enderDest.xBairro = cliente.Bairro ?? string.Empty;
                            req.InfNFe.Dest.enderDest.cMun = (munEntity?.CodIbge ?? 0).ToString().PadLeft(7, '0');
                            req.InfNFe.Dest.enderDest.xMun = mun.Nome;
                            req.InfNFe.Dest.enderDest.UF = mun.UF;
                            req.InfNFe.Dest.enderDest.CEP = Regex.Replace(cliente.Cep ?? string.Empty, "\\D", string.Empty);
                        }
                    }
                }
                else
                {
                    var forn = _fornecedoresService.GetByIdAsync(nota.IdDestinatario.Value, CancellationToken.None).GetAwaiter().GetResult();
                    if (forn != null)
                    {
                        req.InfNFe.Ide.indFinal = 0;
                        req.InfNFe.Dest.xNome = forn.Fantasia ?? forn.RazaoSocial ?? string.Empty;
                        req.InfNFe.Dest.CNPJ = forn.CpfCnpj;
                        req.InfNFe.Dest.indIEDest = forn.Ie.ToUpper() != "ISENTO" ? 1 : 2; // assume contribuinte com IE por padrão
                        var munEntity = _municipiosRepo.GetByIdAsync(forn.IdMunicipio, CancellationToken.None).GetAwaiter().GetResult();
                        req.InfNFe.Dest.enderDest.xLgr = forn.Endereco ?? string.Empty;
                        req.InfNFe.Dest.enderDest.nro = forn.Numero ?? string.Empty;
                        req.InfNFe.Dest.enderDest.xBairro = forn.Bairro ?? string.Empty;
                        req.InfNFe.Dest.enderDest.cMun = (munEntity?.CodIbge ?? 0).ToString().PadLeft(7, '0');
                        req.InfNFe.Dest.enderDest.xMun = munEntity?.Nome ?? string.Empty;
                        req.InfNFe.Dest.enderDest.UF = munEntity?.Uf ?? string.Empty;
                        req.InfNFe.Dest.enderDest.CEP = Regex.Replace(forn.Cep ?? string.Empty, "\\D", string.Empty);
                    }
                }
            }

            // Items
            foreach (var it in nota.NotaFiscalItems)
            {
                var prodDto = _produtosService.GetByIdAsync(it.IdProduto, CancellationToken.None).GetAwaiter().GetResult();

                // Definir unidade tributável igual à unidade comercial, a menos que haja diferença
                var unidade = prodDto?.Unidade ?? it.Unidade ?? string.Empty;
                var qCom = it.Quantidade;
                var vUnCom = it.PrecoUnitario;
                var qTrib = qCom;
                var vUnTrib = vUnCom;
                // vProd deve ser qTrib * vUnTrib, arredondado para 2 casas decimais
                var vProd = Math.Round(qTrib * vUnTrib, 2);

                var det = new Det
                {
                    nItem = it.Item,
                    prod = new Prod
                    {
                        cProd = it.IdProduto.ToString(),
                        xProd = prodDto?.Descricao ?? it.DescricaoProduto ?? string.Empty,
                        NCM = prodDto?.Ncm ?? it.Ncm ?? string.Empty,
                        CFOP = it.Cfop ?? string.Empty,
                        uCom = unidade,
                        qCom = qCom,
                        vUnCom = vUnCom,
                        vProd = vProd,
                        uTrib = unidade,
                        qTrib = qTrib,
                        vUnTrib = vUnTrib
                    },
                    imposto = new Imposto()
                };

                // map tributos (simple mapping)
                if (it.NotaFiscalItemTributos != null)
                {
                    foreach (var tr in it.NotaFiscalItemTributos)
                    {
                        var tipo = tr.TipoTributo?.ToUpperInvariant();
                        if (tipo == "PIS")
                        {
                            det.imposto.PIS.PISNT = new PISNT { CST = "07" };
                        }
                        else if (tipo == "COFINS")
                        {
                            det.imposto.COFINS.COFINSNT = new COFINSNT { CST = "07" };
                        }
                        else if (tipo == "ICMS")
                        {
                            det.imposto.ICMS.ICMSSN102 = new ICMSSN102 { orig = 0, CSOSN = "102" };
                        }
                        else if (tipo == "IPI")
                        {
                            det.imposto.IPI = new IPI
                            {
                                cEnq = "999",
                                IPITrib = new IPITrib
                                {
                                    CST = "50",
                                    vBC = tr.BaseCalculo,
                                    pIPI = tr.Aliquota,
                                    vIPI = tr.Valor
                                }
                            };
                        }
                    }
                }

                req.InfNFe.Det.Add(det);
            }
            Console.WriteLine(JsonSerializer.Serialize(req));
            return req;
        }

        public async Task<NotaFiscalEventoResponseDto> CancelarAsync(long idNotaFiscal, object request, CancellationToken ct)
        {
            var nota = await _repo.GetByIdForUpdateAsync(idNotaFiscal, ct);
            if (nota is null || string.IsNullOrEmpty(nota.IdNfe))
                throw new KeyNotFoundException("Nota fiscal não encontrada ou sem IdNfe.");

            var empresaKey = _configuration.GetSection("AcbrApi")["EmpresaKey"] ?? string.Empty;
            var client = _httpFactory.CreateClient("AcbrApi");
            var response = await client.PostAsJsonAsync($"api/nfe/{empresaKey}/{nota.IdNfe}/cancelamento", request, ct);
            var responseBody = await response.Content.ReadAsStringAsync(ct);

            bool isSuccess = false;
            string tipoEvento = "4"; // 3 = CANCELADA, 4 = CANCELAMENTO REJEITADO
            string? usuario = null;
            string? motivo = null;
            try { usuario = _currentUser.GetUsuarioLogadoId().ToString(); } catch { }

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;
                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("status", out var st) && st.ValueKind == JsonValueKind.String &&
                        st.GetString()?.Equals("registrado", StringComparison.OrdinalIgnoreCase) == true &&
                        root.TryGetProperty("tipo_evento", out var te) && te.ValueKind == JsonValueKind.String &&
                        te.GetString() == "110111")
                    {
                        isSuccess = true;
                        tipoEvento = "3"; // CANCELADA
                        motivo = root.TryGetProperty("motivo_status", out var ms) && ms.ValueKind == JsonValueKind.String ? ms.GetString() : null;
                    }
                    if (root.TryGetProperty("error", out var err))
                    {
                        motivo = err.TryGetProperty("message", out var m) && m.ValueKind == JsonValueKind.String ? m.GetString() : null;
                        tipoEvento = "4"; // CANCELAMENTO REJEITADO
                    }
                }
            }
            catch { }

            // Só altera status se sucesso
            if (isSuccess)
            {
                nota.Status = "CANCELADA";
                nota.DataCancelamento = DateTime.Now;
                nota.UsuarioCancelamento = usuario;
                nota.MotivoCancelamento = motivo;
                nota.DataAlteracao = DateTime.Now;
                nota.EnsureUpdateAudit(_currentUser);
            }
            else
            {
                nota.DataAlteracao = DateTime.Now;
                nota.EnsureUpdateAudit(_currentUser);
            }

            var evento = new NotaFiscalEvento
            {
                IdNotaFiscal = nota.IdNotaFiscal,
                DataEvento = DateTime.Now,
                TipoEvento = tipoEvento,
                XmlRetorno = responseBody,
                Usuario = usuario
            };
            await _repo.AddEventoAsync(evento, ct);
            await _repo.SaveAsync(ct);

            var dto = new NotaFiscalEventoResponseDto
            {
                IdNotaFiscalEvento = evento.IdNotaFiscalEvento,
                DataEvento = evento.DataEvento,
                TipoEvento = MapTipoEvento(tipoEvento),
                Usuario = usuario,
                XmlRetorno = responseBody
            };
            if (!string.IsNullOrWhiteSpace(motivo))
            {
                dto.Summary = motivo;
                dto.Errors = new List<string> { motivo };
            }
            return dto;
        }

    }
}