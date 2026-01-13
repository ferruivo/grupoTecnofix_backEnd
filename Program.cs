using AutoMapper;
using GrupoTecnofix_Api.Auth;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.BLL.Services;
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Data.Repositories;
using GrupoTecnofix_Api.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AuthService = GrupoTecnofix_Api.Auth.AuthService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

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
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();

// ===================== AutoMapper =====================
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UsuariosProfile>();
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GrupoTecnofix API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("CorsDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
