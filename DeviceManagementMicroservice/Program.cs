using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DeviceManagementMicroservice.Utilities;
using DeviceManagementMicroservice.Repositories;
using DeviceManagementMicroservice.Services;
using DeviceManagementMicroservice.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
	opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
	opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "bearer"
	});

	opt.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id="Bearer"
				}
			},
			new string[]{}
		}
	});
});

// Add services to the container.
builder.Services.AddCors(c => c.AddPolicy("AllowOrigin",
	options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
	)
);

// dependecy injection
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<DeviceRepository>();
builder.Services.AddScoped<DeviceService>();


//fusul orar
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	options.DefaultRequestCulture = new RequestCulture("ro-EU");
	options.SupportedCultures = new List<CultureInfo> { new CultureInfo("ro-EU") };
	options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("ro-EU") };
});

var connectionString = builder.Configuration.GetConnectionString("DevicesDB");
builder.Services.AddDbContext<DeviceDbContext>(options =>
{
	options.UseMySql((connectionString),
	Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(connectionString));
});

//autorizare header pt a primi token
builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = "EnergyManagementSystem.com",
		ValidAudience = "EnergyManagementSystem.com",
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("EnergyManagementSystem_admin_256"))
	};
});

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
	});

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.MaxDepth = 10; // Seteaza la valoarea potrivita
	});



builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();


//Enable CORS
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
