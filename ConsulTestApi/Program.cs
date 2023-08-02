using ConsulTestApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConsulService, ConsulService>();
builder.Services.AddHealthChecks();

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

app.MapHealthChecks("/healthz");

app.Lifetime.ApplicationStarted.Register(() =>
{
    var cs = app.Services.GetService<IConsulService>();
    cs?.StartService().Wait();
});
app.Lifetime.ApplicationStopping.Register(() =>
{
    var cs = app.Services.GetService<IConsulService>();
    cs?.StopService().Wait();
});

app.Run();
