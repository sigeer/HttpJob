using Grpc.Net.Client;
using SpiderRemoteServiceClient;
using SpiderRemoteServiceClient.Services;
using SpiderService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging();
builder.Services.AddSpiderService(builder.Configuration["gRPCServer"]);
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", v =>
    {
        var allowedHost = builder.Configuration.GetValue<string>("AllowedHosts");
        if (allowedHost == "*")
            v.SetIsOriginAllowed(_ => true);
        else
            v.WithOrigins(allowedHost.Split(","));

        v.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithExposedHeaders("Content-Disposition");
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("cors");
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
