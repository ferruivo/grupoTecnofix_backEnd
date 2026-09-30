using AcbrApi_integration;
using AcbrApi_integration.Interfaces;
using AcbrApi_integration.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AcbrApiOptions>(
    builder.Configuration.GetSection("AcbrApi"));

builder.Services.AddHttpClient<IAcbrAuthService, AcbrAuthService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(60);
});

builder.Services.AddHttpClient<IAcbrNfeService, AcbrNfeService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(60);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();