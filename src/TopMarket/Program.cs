using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Service.Helpers;
using TopMarket.Extentions;
using TopMarket.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSwagger();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration
        .GetConnectionString("DefaultConnection"));
});

builder.Services.AddService();

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

var app = builder.Build();

PathHepler.WebRootPath = Path.GetFullPath("wwwroot");
PathHepler.CountryPath = Path.GetFullPath(builder.Configuration.GetValue<string>(("FilePath:CountryFilePaths")));
PathHepler.RegionPath = Path.GetFullPath(builder.Configuration.GetValue<string>(("FilePath:RegionFilePaths")));
PathHepler.DistrictPath = Path.GetFullPath(builder.Configuration.GetValue<string>(("FilePath:DictrictsFilePaths")));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
