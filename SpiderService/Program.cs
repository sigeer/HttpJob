using SpiderService.Mapper.Spiders;
using SpiderService.Services;
using SpiderTool.SqlSugar;
using SqlSugar;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
var sqlClient = new SqlSugarClient(new ConnectionConfig
{
    ConnectionString = "data source=database.db",
    DbType = DbType.Sqlite,
    ConfigureExternalServices = ExternalServiceDefaultBuilder.Build()
});
builder.Services.AddSpiderService(sqlClient, ServiceLifetime.Scoped);
// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(x =>
{
    x.AddProfile<SpiderProfile>();
});

var app = builder.Build();
// Configure the HTTP request pipeline.
app.MapGrpcService<SpiderWorkService>();
app.MapGet("/health", () => "web health check");

app.Run();
