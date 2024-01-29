using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MonitoringAndCommunicationMicroservice.DataAccess;
using MonitoringAndCommunicationMicroservice.Handlers;
using MonitoringAndCommunicationMicroservice.RabbitMQService;
using MonitoringAndCommunicationMicroservice.Repositories;
using MonitoringAndCommunicationMicroservice.Services;
using MonitoringAndCommunicationMicroservice.Utilities;
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

builder.Services.AddSingleton<RabbitMQConsumerDeviceMeasurements>();
builder.Services.AddSingleton<RabbitMQConsumerDeviceChanges>();
builder.Services.AddSingleton<WebSocketConnectionManager>();
builder.Services.AddSingleton<WebSocketHandler>();
//builder.Services.AddScoped<WebSocketHandler>();

// dependecy injection
builder.Services.AddScoped<MonitoringRepository>();
builder.Services.AddScoped<MonitoringService>();
builder.Services.AddScoped<TokenValidationService>();
builder.Services.AddScoped<ConsumptionService>();
builder.Services.AddScoped<ConsumptionRepository>();


// db context
var connectionString = builder.Configuration.GetConnectionString("MonitoringDB");
builder.Services.AddDbContext<MonitoringDbContext>(options =>
{
	options.UseMySql((connectionString),
	Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

var rabbitMQConsumerDeviceMeasurements = new RabbitMQConsumerDeviceMeasurements();
var rabbitMQConsumerDeviceChanges = new RabbitMQConsumerDeviceChanges();
// configure stopping callback
var lifetime = app.Services.GetService<IHostApplicationLifetime>();
if (lifetime != null)
{
	lifetime.ApplicationStopping.Register(() =>
	{
		rabbitMQConsumerDeviceMeasurements.StopConsuming();
		rabbitMQConsumerDeviceChanges.StopConsuming();
		Console.WriteLine("MonitoringMicroservice:: OK RabbitMq stopped consuming");
	});
}
else
{
	Console.WriteLine("ERROR: variabila lifetime este null. Registrarea functiei callback nu se poate face");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//var cc = new RabbitMQConsumer(); RabbitMQConsumerDeviceMeasurements()
rabbitMQConsumerDeviceMeasurements = app.Services.GetRequiredService<RabbitMQConsumerDeviceMeasurements>();
rabbitMQConsumerDeviceMeasurements.StartConsuming();

rabbitMQConsumerDeviceChanges = app.Services.GetRequiredService<RabbitMQConsumerDeviceChanges>();
rabbitMQConsumerDeviceChanges.StartConsuming();

//app.UseMiddleware<WebSocketMiddleware>();

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
/*
app.Run(async (context) =>
{
    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
    var socketFinishedTcs = new TaskCompletionSource<object>();

    BackgroundSocketProcessor.AddSocket(webSocket, socketFinishedTcs);

    await socketFinishedTcs.Task;
});*/

app.Run();
