using Demo.Api.Services;
using Hangfire;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMathService, MathService>();
builder.Services.AddSingleton<ITextService, TextService>();
builder.Services.AddSingleton<IPrimeService, PrimeService>();
builder.Services.AddSingleton<IMathBackgroundService, MathBackgroundService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire((provider, options) =>
{
    options.UseMemoryStorage(new MemoryStorageOptions());

    options
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings();

    options.UseFilter(
        new AutomaticRetryAttribute
        {
            Attempts = 0,
        }
    );
});

builder.Services.AddHangfireServer((provider, options) =>
{
    options.SchedulePollingInterval = TimeSpan.FromSeconds(0.1);
});

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

public partial class Program
{
}
