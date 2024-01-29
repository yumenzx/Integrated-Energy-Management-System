using ChatMicroservice.Data;
using ChatMicroservice.Handlers;
using ChatMicroservice.Services;
using ChatMicroservice.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(c => c.AddPolicy("AllowOrigin",
	options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
	)
);

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


builder.Services.Configure<WebSocketOptions>(new Action<WebSocketOptions>((options) =>
{
	options.KeepAliveInterval = TimeSpan.FromSeconds(120); // timeout
}));

builder.Services.AddSingleton<WebSocketConnectionManager>();
builder.Services.AddSingleton<MessagesContext>();
builder.Services.AddScoped<WebSocketHandler>();

builder.Services.AddScoped<TokenValidationService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

var webSocketOptions = new WebSocketOptions
{
	KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//Enable CORS
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.Run();
