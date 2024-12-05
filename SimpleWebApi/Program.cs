using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load InfluxDB configuration
var influxDbConfig = builder.Configuration.GetSection("InfluxDB");
var influxUrl = influxDbConfig["Url"];
var influxToken = influxDbConfig["Token"];
var influxOrg = influxDbConfig["Org"];
var influxBucket = influxDbConfig["Bucket"];

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.InfluxDB(
        applicationName: "SimpleWebApi",
        uri: new Uri(influxUrl),
        organizationId: influxOrg,
        bucketName: influxBucket,
        token: influxToken
    )
    .CreateLogger();

builder.Host.UseSerilog();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();