using CoffeeShopAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_Coffe.Data;
<<<<<<< HEAD
using Project_Coffe.Entities;
=======
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
using Project_Coffe.Models.ModelInterface;
using Project_Coffe.Models.ModelRealization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD

// ������ ������������ ��� �'������� � ����� �����
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
=======
// ������ ������������ ��� �'������� � ����� �����
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ������ ������ ��� �����������
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();

<<<<<<< HEAD
// ������ ��������� AuthenticationService
builder.Services.AddScoped<AuthenticationService>();

// ������������ �������������� �� ��������� JWT
var jwtKey = builder.Configuration["Jwt:SecretKey"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
{
    throw new ArgumentNullException("JWT configuration values are missing.");
}
=======
// ������������ �������������� �� ��������� JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// ������ ����������
builder.Services.AddControllers();

// ������ Swagger ��� ������������ API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ������������ ����������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
