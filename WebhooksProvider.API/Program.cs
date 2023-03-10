using Dapper.BaseRepository.Config;
using MediatR;
using Webhooks.App.Api.Infrastructure.Extensions;
using WebHooks.SharedKernel.Services;
using WebHooks.SharedKernel.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


//Add services to the container.
services.AddBaseRepostiorySetup((options) =>
{
    options.DefaultSqlServerConnectionString = builder.Configuration["ConnectionStrings:SqlServerConnection"];
});
services.AddSingleton<ITransferCashTopicProducer, TransferCashTopicProducer>();
services.AddRepositories();
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

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
