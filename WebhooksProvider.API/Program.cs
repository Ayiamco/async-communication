using DapperHelper;
using MediatR;
using Webhooks.App.Api.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


//Add services to the container.
services.AddConnectionStrings((options) =>
{
    options.SqlServerConnectionString = builder.Configuration["ConnectionStrings:SqlServerConnection"];
});
services.AddRepositories();
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

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
