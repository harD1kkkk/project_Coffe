using AI_Coffee_Advisor.Data;
using AI_Coffee_Advisor.Model.ModelInterface;
using AI_Coffee_Advisor.Model.ModelRealization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddHttpClient<ICoffeeRecommendation, CoffeeRecommendationService>();
builder.Services.AddHostedService<CoffeeRecommendationBackgroundService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
