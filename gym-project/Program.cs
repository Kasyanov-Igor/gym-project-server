using gym_project_business_logic.Services;
using gym_project_business_logic.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация зависимостей
builder.Services.AddScoped<ADatabaseConnection, SqliteConnection>();
builder.Services.AddScoped<ICoachService, CoachService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<MapperConfig, MapperConfig>();
//builder.Services.AddScoped<ITokenService, TokenService>();


var app = builder.Build();

app.UseCors(x => x
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowCredentials()
	.SetIsOriginAllowed(origin => true));

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
