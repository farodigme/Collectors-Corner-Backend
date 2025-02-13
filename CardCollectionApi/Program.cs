using CardCollectionApi.DataBase;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery();
builder.Services.AddDbContext<ApplicationContext>(options =>
options.UseMySql(
	builder.Configuration.GetConnectionString("DefaultConnection"),
	new MySqlServerVersion("8.0.40"))
);
var app = builder.Build();	

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
