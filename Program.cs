using AutoMapper;
using GrupoTecnofix_Api.Auth;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.BLL.Services;
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Data.Repositories;
using GrupoTecnofix_Api.Mappings;
using GrupoTecnofix_Api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF;
using QuestPDF.Infrastructure;
using System.Text;
using AuthService = GrupoTecnofix_Api.Auth.AuthService;

var builder = WebApplication.CreateBuilder(args);

// QuestPDF license configuration to avoid runtime exception
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddControllers();

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
        sql =>
        {
            sql.CommandTimeout(120); // 120s
            sql.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        });
});


builder.Services.AddHttpContextAccessor();

// ===================== JWT =====================
var jwt = builder.Configuration.GetSection("Jwt");
var key = jwt["Key"]!;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

// ===================== CORS =====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsDev", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ===================== Authorization =====================
// Policy base (você usa PermissionPolicyProvider/Handler para criar as policies por nome)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Permission", policy =>
        policy.RequireAssertion(ctx =>
            ctx.User.Claims.Any(c => c.Type == "permission")));
});

// Helper: criar policy por permissão automaticamente
builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, PermissionHandler>();

// ===================== DI =====================

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<ITransportadorasRepository, TransportadorasRepository>();
builder.Services.AddScoped<ITransportadorasService, TransportadorasService>();
builder.Services.AddScoped<IVendedoresRepository, VendedoresRepository>();
builder.Services.AddScoped<IVendedoresService, VendedoresService>();
builder.Services.AddScoped<IMunicipiosRepository, MunicipiosRepository>();
builder.Services.AddScoped<IMunicipiosService, MunicipiosService>();
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IPerfisRepository, PerfisRepository>();
builder.Services.AddScoped<IUsuariosAclRepository, UsuariosAclRepository>();
builder.Services.AddScoped<IPerfisService, PerfisService>();
builder.Services.AddScoped<IUsuariosAclService, UsuariosAclService>();
builder.Services.AddScoped<IPermissoesRepository, PermissoesRepository>();
builder.Services.AddScoped<IPermissoesService, PermissoesService>();
builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IClientesRepository, ClientesRepository>();
builder.Services.AddScoped<ICondicoesPagamentoRepository, CondicoesPagamentoRepository>();
builder.Services.AddScoped<ICondicoesPagamentoService, CondicoesPagamentoService>();
builder.Services.AddScoped<IFornecedoresRepository, FornecedoresRepository>();
builder.Services.AddScoped<IFornecedoresService, FornecedoresService>();
builder.Services.AddScoped<IProdutosRepository, ProdutosRepository>();
builder.Services.AddScoped<IProdutosService, ProdutosService>();
builder.Services.AddScoped<IPrateleirasService, PrateleirasService>();
builder.Services.AddScoped<IPrateleirasRepository, PrateleirasRepository>();
builder.Services.AddScoped<IPedidoCompraRepository, PedidoCompraRepository>();
builder.Services.AddScoped<IPedidoCompraService, PedidoCompraService>();
// register pedido venda
builder.Services.AddScoped<IPedidoVendaRepository, PedidoVendaRepository>();
builder.Services.AddScoped<IPedidoVendaService, PedidoVendaService>();
// register PDF service
builder.Services.AddScoped<IPdfService, PdfService>();
// ===================== AutoMapper =====================
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UsuariosProfile>();
    cfg.AddProfile<TransportadorasProfile>();
    cfg.AddProfile<VendedoresProfile>();
    cfg.AddProfile<EmpresaProfile>();
    cfg.AddProfile<PerfisProfile>();
    cfg.AddProfile<MunicipioProfile>();
    cfg.AddProfile<ClienteProfile>();
    cfg.AddProfile<TipoDocumentoProfile>();
    cfg.AddProfile<CondicaoPagamentoProfile>();
    cfg.AddProfile<FornecedoresProfile>();
    cfg.AddProfile<ProdutosProfile>();
    cfg.AddProfile<PrecoVendaProfile>();
    cfg.AddProfile<PrecoCompraProfile>();
    cfg.AddProfile<ProdutoKitItenProfile>();
    cfg.AddProfile<PedidoCompraProfile>();
    cfg.AddProfile<PedidoVendaProfile>();

});

// ===================== Swagger =====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GrupoTecnofix API",
        Version = "v1"
    });

    // 🔐 Bearer JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe: Bearer {seu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ===================== Seed =====================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeed.SeedAsync(db);
}

// ===================== Pipeline =====================
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "GrupoTecnofix API v1"); // <- sem barra no começo
    c.RoutePrefix = "swagger";
});
//}

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        context.Response.ContentType = "application/json";

        if (exception is ConflictException)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new
            {
                message = exception.Message
            });
            return;
        }
        if (exception is KeyNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            await context.Response.WriteAsJsonAsync(new
            {
                message = exception.Message
            });
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            message = "Erro interno."
        });
    });
});

app.UseHttpsRedirection();

app.UseCors("CorsDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
