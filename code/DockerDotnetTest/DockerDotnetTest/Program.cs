using DockerDotnetTest.Scenario.Memory;
using DockerDotnetTest.Scenario.Stackoverflow;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IMemoryConsumption, MemoryConsumption>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

if (Environment.GetEnvironmentVariable("TriggerError") != null)
    throw new Exception();

app.Run();


